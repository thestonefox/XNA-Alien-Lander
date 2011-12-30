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
    public class Base3DObject : DrawableGameComponent
    {
        public Vector3 Position;
        public Vector3 Scale;
        public Quaternion Rotation;
        public Vector3 LightPosition = new Vector3(10, 10, 0);
        public Color AmbientColor = Color.White;
        public Color DiffuseColor = Color.White;
        public Matrix ViewMatrix;
        public Matrix ProjectionMatrix;

        protected bool firstPassCollision;
        protected bool hitTest;
        protected bool safeHit;

        protected Model mesh;
        protected String modelName;

        protected Matrix world;
        protected Matrix lastWorld;
        protected BasicEffect bEffect;
        protected Matrix[] transforms;
        protected Matrix meshWorld;
        protected Matrix meshWVP;        

        protected BoundingBox volume;
        protected BoundingBox bounds;

        public Base3DObject(Game game, String modelAssetName)
            : base(game)
        {
            Position = Vector3.Zero;
            Scale = Vector3.One;
            Rotation = Quaternion.Identity;
            modelName = modelAssetName;
            bEffect = new BasicEffect(Game.GraphicsDevice);

            ViewMatrix = Matrix.Identity;
            ProjectionMatrix = Matrix.Identity;
            firstPassCollision = false;
        }

        public void LoadContent(ContentManager content)
        {
            mesh = Game.Content.Load<Model>(modelName);
            transforms = new Matrix[mesh.Bones.Count];
            mesh.CopyAbsoluteBoneTransformsTo(transforms);
            Dictionary<string, object> data = (Dictionary<string, object>)mesh.Tag;
            volume = ((List<BoundingBox>)data["BoundingBoxs"])[0];     
        }

        public override void Initialize()
        {
            base.Initialize();
        }

        public override void Update(GameTime gameTime)
        {

            if (lastWorld != world)
            {
                lastWorld = world;
            }
            world =     Matrix.CreateScale(Scale) *
                        Matrix.CreateFromQuaternion(Rotation) *
                        Matrix.CreateTranslation(Position);
            bounds = new BoundingBox(Vector3.Transform(volume.Min, world), Vector3.Transform(volume.Max, world));
        }

        public bool Collided(Base3DObject bob)
        {
            hitTest = false;
            bob.hitTest = false;

            if (bounds.Intersects(bob.bounds) && firstPassCollision == true)
            {
                if (bob.bounds.Min.Y > (bounds.Max.Y - 4.0f) && (bob.bounds.Min.X > bounds.Min.X && bob.bounds.Max.X < bounds.Max.X && bob.bounds.Min.Z > bounds.Min.Z && bob.bounds.Max.Z < bounds.Max.Z))
                {
                    safeHit = true;
                    bob.safeHit = true;
                    DiffuseColor = Color.DarkRed;
                }
                else
                {
                    safeHit = false;
                    bob.safeHit = false;
                    DiffuseColor = Color.White;
                }
                hitTest = true;
                bob.hitTest = true;
            }
            firstPassCollision = true;
            return hitTest;
        }

        protected void DrawModel(GameTime gameTime)
        {
            Game.GraphicsDevice.BlendState = BlendState.Opaque;
            Game.GraphicsDevice.DepthStencilState = DepthStencilState.Default;
            Game.GraphicsDevice.RasterizerState = RasterizerState.CullCounterClockwise;
            Game.GraphicsDevice.SamplerStates[0] = SamplerState.PointWrap;

            bEffect.AmbientLightColor = AmbientColor.ToVector3();
            bEffect.DiffuseColor = DiffuseColor.ToVector3();
            bEffect.DirectionalLight0.Direction = Vector3.Normalize(Position - LightPosition);
            bEffect.LightingEnabled = true;
            bEffect.PreferPerPixelLighting = true;
            bEffect.EnableDefaultLighting();
            bEffect.Projection = ProjectionMatrix;            
            bEffect.View = ViewMatrix;

            if (hitTest)
            {
                world = lastWorld;
            }
            foreach (ModelMesh meshM in mesh.Meshes)
            {                
                // Do the world stuff. 
                // Scale * transform * pos * rotation
                meshWorld = transforms[meshM.ParentBone.Index] * world;
                meshWVP = meshWorld * ViewMatrix * ProjectionMatrix;

                bEffect.World = meshWorld;

                bEffect.CurrentTechnique.Passes[0].Apply();

                foreach (ModelMeshPart meshPart in meshM.MeshParts)
                {
                    Game.GraphicsDevice.SetVertexBuffer(meshPart.VertexBuffer);
                    Game.GraphicsDevice.Indices = meshPart.IndexBuffer;
                    Game.GraphicsDevice.DrawIndexedPrimitives(PrimitiveType.TriangleList, meshPart.VertexOffset, 0, meshPart.NumVertices, meshPart.StartIndex, meshPart.PrimitiveCount);
                }
            }

        }

        public override void Draw(GameTime gameTime)
        {
            DrawModel(gameTime);            
        }
    }
}
