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
        public Matrix[] elementsWorldMatrix;
        public BoundingBox Bounds;
        public bool HitTest;

        public bool OverrideFirstCollision;

        protected LightSource light;

        protected bool firstPassCollision;

        protected Model mesh;
        protected String modelName;

        protected Matrix world;
        protected Matrix lastWorld;
        protected Matrix[] transforms;
        protected Matrix meshWorld;

        protected BoundingBox volume;
        protected bool hasBounds;

        public bool Active;

        public Base3DObject(Game game, String modelAssetName, LightSource _light)
            : base(game)
        {
            Position = Vector3.Zero;
            Scale = Vector3.One;
            Rotation = Vector3.Zero;
            modelName = modelAssetName;
            light = _light;
            firstPassCollision = false;
            Active = true;
            elementsWorldMatrix = new Matrix[0];
            hasBounds = false;
        }

        public void LoadContent(bool withBounds)
        {
            mesh = Game.Content.Load<Model>(modelName);
            transforms = new Matrix[mesh.Bones.Count];
            mesh.CopyAbsoluteBoneTransformsTo(transforms);
            if (withBounds)
            {
                CreateBounds();
            }
        }

        protected void CreateBounds()
        {
            Dictionary<string, object> data = (Dictionary<string, object>)mesh.Tag;
            for (int i = 0; i < ((List<BoundingBox>)data["BoundingBoxs"]).Count; i++)
            {
                if (i == 0)
                {
                    volume = ((List<BoundingBox>)data["BoundingBoxs"])[i];
                }
                else
                {
                    if (((List<BoundingBox>)data["BoundingBoxs"])[i].Min.X < volume.Min.X)
                    {
                        volume.Min.X = ((List<BoundingBox>)data["BoundingBoxs"])[i].Min.X;
                    }
                    if (((List<BoundingBox>)data["BoundingBoxs"])[i].Max.X > volume.Max.X)
                    {
                        volume.Max.X = ((List<BoundingBox>)data["BoundingBoxs"])[i].Max.X;
                    }

                    if (((List<BoundingBox>)data["BoundingBoxs"])[i].Min.Y < volume.Min.Y)
                    {
                        volume.Min.Y = ((List<BoundingBox>)data["BoundingBoxs"])[i].Min.Y;
                    }
                    if (((List<BoundingBox>)data["BoundingBoxs"])[i].Max.Y > volume.Max.Y)
                    {
                        volume.Max.Y = ((List<BoundingBox>)data["BoundingBoxs"])[i].Max.Y;
                    }

                    if (((List<BoundingBox>)data["BoundingBoxs"])[i].Min.Z < volume.Min.Z)
                    {
                        volume.Min.Z = ((List<BoundingBox>)data["BoundingBoxs"])[i].Min.Z;
                    }
                    if (((List<BoundingBox>)data["BoundingBoxs"])[i].Max.Y > volume.Max.Z)
                    {
                        volume.Max.Z = ((List<BoundingBox>)data["BoundingBoxs"])[i].Max.Z;
                    }
                }
            }
            hasBounds = true;
        }

        public override void Initialize()
        {
            SetPositions();
            base.Initialize();
        }

        public override void Update(GameTime gameTime)
        {
            if (Active == true)
            {
                if (lastWorld != world)
                {
                    lastWorld = world;
                }
                SetPositions();
            }
        }

        protected void SetPositions()
        {
            if (hasBounds)
            {
                Matrix boundsWorld = Matrix.CreateScale(Scale) * Matrix.CreateTranslation(Position);
                Bounds = new BoundingBox(Vector3.Transform(volume.Min, boundsWorld), Vector3.Transform(volume.Max, boundsWorld));
            }
            world = Matrix.CreateScale(Scale) * (Matrix.CreateRotationX(Rotation.X) * Matrix.CreateRotationY(Rotation.Y) * Matrix.CreateRotationZ(Rotation.Z)) * Matrix.CreateTranslation(Position);
        }

        protected void DrawModel(BaseCamera camera, bool createShadowMap, ref RenderTarget2D shadowRenderTarget)
        {
            Matrix lightViewProjection = light.CreateLightViewProjectionMatrix();
            String techniqueName = createShadowMap ? "CreateShadowMap" : "DrawWithShadowMap";
   
            for(int meshIndex = 0; meshIndex < mesh.Meshes.Count; meshIndex++)
            {                
                ModelMesh meshM = mesh.Meshes[meshIndex];
                // Do the world stuff. 
                // Scale * transform * pos * rotation
                if (elementsWorldMatrix.Length == 0)
                {
                    meshWorld = transforms[meshM.ParentBone.Index] * world;
                }
                else
                {
                    meshWorld = transforms[meshM.ParentBone.Index] * elementsWorldMatrix[meshIndex] * world;
                }

                foreach (Effect effect in meshM.Effects)
                {
                    effect.CurrentTechnique = effect.Techniques[techniqueName];
                    effect.Parameters["World"].SetValue(meshWorld);
                    effect.Parameters["View"].SetValue(camera.GetViewMatrix());
                    effect.Parameters["Projection"].SetValue(camera.GetProjectionMatrix());
                    effect.Parameters["LightDirection"].SetValue(light.LightDirection);
                    effect.Parameters["LightViewProj"].SetValue(lightViewProjection);
                    if (createShadowMap == false)
                    {
                        effect.Parameters["ShadowMap"].SetValue(shadowRenderTarget);
                    }
                }
                
                meshM.Draw();
            }

        }

        public void Draw(BaseCamera camera, ref RenderTarget2D shadowRenderTarget)
        {
            if (Active)
            {
                if (HitTest)
                {
                    world = lastWorld;
                }
                DrawModel(camera, false, ref shadowRenderTarget);
            }            
        }

        public void Draw(BaseCamera camera)
        {
            if (Active)
            {
                if (HitTest)
                {
                    world = lastWorld;
                }
                // Copy any parent transforms.

                // Draw the model. A model can have multiple meshes, so loop.
                for(int meshIndex = 0; meshIndex < mesh.Meshes.Count; meshIndex++)
                {                
                    ModelMesh meshM = mesh.Meshes[meshIndex];
                    // This is where the mesh orientation is set, as well 
                    // as our camera and projection.
                    foreach (BasicEffect effect in meshM.Effects)
                    {
                        effect.EnableDefaultLighting();
                        if (elementsWorldMatrix.Length == 0)
                        {
                            effect.World = transforms[meshM.ParentBone.Index] * world;
                        }
                        else
                        {
                            effect.World = transforms[meshM.ParentBone.Index] * elementsWorldMatrix[meshIndex] * world;
                        }
                        effect.View = camera.GetViewMatrix();
                        effect.Projection = camera.GetProjectionMatrix();
                    }
                    // Draw the mesh, using the effects set above.
                    meshM.Draw();
                }
            }
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
            if (Active == true)
            {
                if (Bounds.Intersects(bob.Bounds) && (firstPassCollision == true || OverrideFirstCollision == true))
                {
                    return true;
                }
                firstPassCollision = true;
                return false;
            }
            return false;
        }
    }
}
