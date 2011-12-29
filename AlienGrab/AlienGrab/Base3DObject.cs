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

        protected Model mesh;
        protected String modelName;

        public Matrix ViewMatrix;
        public Matrix ProjectionMatrix;

        protected Matrix World;
        protected BasicEffect BEffect;
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
            BEffect = new BasicEffect(Game.GraphicsDevice);

            ViewMatrix = Matrix.Identity;
            ProjectionMatrix = Matrix.Identity;
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
            World = Matrix.CreateScale(Scale) *
                      Matrix.CreateFromQuaternion(Rotation) *
                      Matrix.CreateTranslation(Position);
            bounds = new BoundingBox(Vector3.Transform(volume.Min, World), Vector3.Transform(volume.Max, World));
        }

        public bool Collided(Base3DObject bob)
        {
            bool retVal = false;

            if (bounds.Intersects(bob.bounds))
            {
                bob.DiffuseColor = Color.DarkRed;                
                retVal = true;
            }
            else
            {
                bob.DiffuseColor = Color.White;
            }

            return retVal;
        }

        public void Draw(GameTime gameTime, BaseCamera camera)
        {
            Game.GraphicsDevice.BlendState = BlendState.Opaque;
            Game.GraphicsDevice.DepthStencilState = DepthStencilState.Default;
            Game.GraphicsDevice.RasterizerState = RasterizerState.CullCounterClockwise;
            Game.GraphicsDevice.SamplerStates[0] = SamplerState.PointWrap;

            //BEffect.AmbientLightColor = AmbientColor.ToVector3();
            BEffect.DiffuseColor = DiffuseColor.ToVector3();
            //BEffect.DirectionalLight0.Direction = Vector3.Normalize(Position - LightPosition);
            //BEffect.LightingEnabled = true;
            BEffect.EnableDefaultLighting();
            BEffect.Projection = camera.GetProjectionMatrix();
            //BEffect.PreferPerPixelLighting = true;
            BEffect.View = camera.GetViewMatrix();
            
            foreach (ModelMesh meshM in mesh.Meshes)
            {
                meshWorld = transforms[meshM.ParentBone.Index] * World;
                meshWVP = meshWorld * camera.GetViewMatrix() * camera.GetProjectionMatrix();
                BEffect.World = meshWorld;

                BEffect.CurrentTechnique.Passes[0].Apply();

                foreach (ModelMeshPart meshPart in meshM.MeshParts)
                {
                    Game.GraphicsDevice.SetVertexBuffer(meshPart.VertexBuffer);
                    Game.GraphicsDevice.Indices = meshPart.IndexBuffer;
                    Game.GraphicsDevice.DrawIndexedPrimitives(PrimitiveType.TriangleList, meshPart.VertexOffset, 0, meshPart.NumVertices, meshPart.StartIndex, meshPart.PrimitiveCount);
                }
            }

        }
    }
}
