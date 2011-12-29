using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace AlienGrab
{
    class Sprite3D : DrawableGameComponent
    {
        protected Model model;
        protected Vector3 position;
        protected String assetName;
        protected Vector3 diffuseColor;
        public BasicEffect Effect;
        protected float rotation;
        protected Model mesh;
        protected Matrix[] transforms;
        protected BoundingBox volume;
        public BoundingBox bounds;
        public Matrix world;

        public Sprite3D(Game game, String asset) : base(game)
        {
            assetName = asset;
            position = Vector3.Zero;
            rotation = 0.0f;
            diffuseColor = Vector3.Zero;
        }

        public void LoadContent(ContentManager content)
        {
            model = LoadModel(content);
        }

        protected Model LoadModel(ContentManager content)
        {
            Model newModel = content.Load<Model>(assetName);
            return newModel;
        }

        public void Initialize(Vector3 _position, float _rotation)
        {
            diffuseColor = new Vector3(position.X / 1000, position.Y / 1000, position.Z / 1000);
            Update(_position, _rotation);
        }

        public void Update(Vector3 _position, float _rotation)
        {
            position = _position;
            rotation = _rotation;
            world = Matrix.CreateRotationY(rotation) * Matrix.CreateTranslation(position);

            if (mesh == null && assetName != string.Empty)
            {
                mesh = Game.Content.Load<Model>(assetName);

                transforms = new Matrix[mesh.Bones.Count];
                mesh.CopyAbsoluteBoneTransformsTo(transforms);

                Dictionary<string, object> data = (Dictionary<string, object>)mesh.Tag;

                volume = ((List<BoundingBox>)data["BoundingBoxs"])[0];
            }

            if (Effect == null)
                Effect = new BasicEffect(Game.GraphicsDevice);

            bounds = new BoundingBox(Vector3.Transform(volume.Min, world), Vector3.Transform(volume.Max, world));
        }

        public void Draw(GraphicsDevice device, Matrix viewMatrix, Matrix projectionMatrix)
        {            
            // Copy any parent transforms.
            Matrix[] transforms = new Matrix[model.Bones.Count];
            model.CopyAbsoluteBoneTransformsTo(transforms);

            // Draw the model. A model can have multiple meshes, so loop.
            foreach (ModelMesh mesh in model.Meshes)
            {
                // This is where the mesh orientation is set, as well 
                // as our camera and projection.
                foreach (BasicEffect effect in mesh.Effects)
                {
                    effect.EnableDefaultLighting();
                    effect.DiffuseColor = diffuseColor;
                    effect.World = transforms[mesh.ParentBone.Index] * world;
                    effect.View = viewMatrix;
                    effect.Projection = projectionMatrix;
                }
                // Draw the mesh, using the effects set above.
                mesh.Draw();
            }
        }

        public bool Collision(Sprite3D bob)
        {
            bool retVal = false;

            if (bounds.Intersects(bob.bounds))
            {
                retVal = true;
            }

            return retVal;
        }

        public bool CollisionDetect(ref Sprite3D sprite)
        {
            for (int i = 0; i < model.Meshes.Count; i++)
            {
                // Check whether the bounding boxes of the two cubes intersect.
                BoundingSphere c1BoundingSphere = model.Meshes[i].BoundingSphere;
                c1BoundingSphere.Center += position;

                for (int j = 0; j < sprite.model.Meshes.Count; j++)
                {
                    BoundingSphere c2BoundingSphere = sprite.model.Meshes[j].BoundingSphere;
                    c2BoundingSphere.Center += sprite.position;

                    if (c1BoundingSphere.Intersects(c2BoundingSphere))
                    {
                        return true;
                    }
                }
            }
            return false;
        }
    }
}
