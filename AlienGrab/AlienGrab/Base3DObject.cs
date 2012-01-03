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
        public Vector3 Rotation;
        public BoundingBox Bounds;

        protected LightSource light;

        protected bool firstPassCollision;
        public bool hitTest;

        protected Model mesh;
        protected String modelName;

        protected Matrix world;
        protected Matrix lastWorld;
        protected Matrix[] transforms;
        protected Matrix meshWorld;

        protected BoundingBox volume;

        public Base3DObject(Game game, String modelAssetName, LightSource _light)
            : base(game)
        {
            Position = Vector3.Zero;
            Scale = Vector3.One;
            Rotation = Vector3.Zero;
            modelName = modelAssetName;
            light = _light;
            firstPassCollision = false;            
        }

        public new void LoadContent()
        {
            mesh = Game.Content.Load<Model>(modelName);
            transforms = new Matrix[mesh.Bones.Count];
            mesh.CopyAbsoluteBoneTransformsTo(transforms);
            Dictionary<string, object> data = (Dictionary<string, object>)mesh.Tag;
            volume = ((List<BoundingBox>)data["BoundingBoxs"])[0];
        }

        public override void Initialize()
        {
            SetPositions();
            base.Initialize();
        }

        public override void Update(GameTime gameTime)
        {
            if (lastWorld != world)
            {
                lastWorld = world;
            }
            SetPositions();
        }

        protected void SetPositions()
        {
            Matrix boundsWorld = Matrix.CreateScale(Scale) * Matrix.CreateTranslation(Position);
            Bounds = new BoundingBox(Vector3.Transform(volume.Min, boundsWorld), Vector3.Transform(volume.Max, boundsWorld));
            world = Matrix.CreateScale(Scale) * (Matrix.CreateRotationX(Rotation.X) * Matrix.CreateRotationY(Rotation.Y) * Matrix.CreateRotationZ(Rotation.Z)) * Matrix.CreateTranslation(Position);
        }

        protected void DrawModel(BaseCamera camera, bool createShadowMap, ref RenderTarget2D shadowRenderTarget)
        {
            Matrix lightViewProjection = light.CreateLightViewProjectionMatrix();
            String techniqueName = createShadowMap ? "CreateShadowMap" : "DrawWithShadowMap";

            if (hitTest)
            {
                world = lastWorld;
            }
            foreach (ModelMesh meshM in mesh.Meshes)
            {                
                // Do the world stuff. 
                // Scale * transform * pos * rotation
                meshWorld = transforms[meshM.ParentBone.Index] * world;

                foreach (Effect effect in meshM.Effects)
                {
                    effect.CurrentTechnique = effect.Techniques[techniqueName];
                    effect.Parameters["World"].SetValue(meshWorld);
                    effect.Parameters["View"].SetValue(camera.GetViewMatrix());
                    effect.Parameters["Projection"].SetValue(camera.GetProjectionMatrix());
                    effect.Parameters["LightDirection"].SetValue(light.LightDirection);
                    effect.Parameters["LightViewProj"].SetValue(lightViewProjection);
                    if (!createShadowMap)
                    {
                        effect.Parameters["ShadowMap"].SetValue(shadowRenderTarget);
                    }
                }
                
                meshM.Draw();
            }

        }

        public void Draw(BaseCamera camera, ref RenderTarget2D shadowRenderTarget)
        {
            DrawModel(camera, false, ref shadowRenderTarget);
        }

        public void DrawShadow(BaseCamera camera, ref RenderTarget2D shadowRenderTarget)
        {
            Game.GraphicsDevice.SetRenderTarget(shadowRenderTarget);
            Game.GraphicsDevice.Clear(Color.White);
            DrawModel(camera, true, ref shadowRenderTarget);
            Game.GraphicsDevice.SetRenderTarget(null);
        }

        public bool Collided(Base3DObject bob)
        {
            hitTest = false;
            bob.hitTest = false;
            if (Bounds.Intersects(bob.Bounds) && firstPassCollision == true)
            {
                hitTest = true;
                bob.hitTest = true;
            }            
            firstPassCollision = true;
            return bob.hitTest;
        }
    }
}
