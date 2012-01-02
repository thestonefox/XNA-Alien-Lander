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
        public Vector3 lightDirection = new Vector3(-0.3f, 500.0f, 0.5f);

        Matrix lightViewProjection;

        protected bool firstPassCollision;
        protected bool hitTest;
        protected bool safeHit;

        protected Model mesh;
        protected String modelName;

        protected Matrix world;
        protected Matrix lastWorld;
        protected Matrix[] transforms;
        protected Matrix meshWorld;

        protected BoundingBox volume;
        public BoundingBox bounds;

        public Base3DObject(Game game, String modelAssetName)
            : base(game)
        {
            Position = Vector3.Zero;
            Scale = Vector3.One;
            Rotation = Vector3.Zero;
            modelName = modelAssetName;
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
            world = Matrix.CreateScale(Scale) * (Matrix.CreateRotationX(Rotation.X) * Matrix.CreateRotationY(Rotation.Y) * Matrix.CreateRotationZ(Rotation.Z)) * Matrix.CreateTranslation(Position);
            bounds = new BoundingBox(Vector3.Transform(volume.Min, world), Vector3.Transform(volume.Max, world));            
        }

        protected Matrix CreateLightViewProjectionMatrix(BaseCamera camera)
        {
            // Matrix with that will rotate in points the direction of the light
            Matrix lightRotation = Matrix.CreateLookAt(Vector3.Zero, -lightDirection, Vector3.Up);

            // Get the corners of the frustum
            Vector3[] frustumCorners = camera.cameraFrustum.GetCorners();

            // Transform the positions of the corners into the direction of the light
            for (int i = 0; i < frustumCorners.Length; i++)
            {
                frustumCorners[i] = Vector3.Transform(frustumCorners[i], lightRotation);
            }

            // Find the smallest box around the points
            BoundingBox lightBox = BoundingBox.CreateFromPoints(frustumCorners);

            Vector3 boxSize = lightBox.Max - lightBox.Min;
            Vector3 halfBoxSize = boxSize * 0.5f;

            // The position of the light should be in the center of the back
            // pannel of the box. 
            Vector3 lightPosition = lightBox.Min + halfBoxSize;
            lightPosition.Z = lightBox.Min.Z;

            // We need the position back in world coordinates so we transform 
            // the light position by the inverse of the lights rotation
            lightPosition = Vector3.Transform(lightPosition,
                                              Matrix.Invert(lightRotation));

            // Create the view matrix for the light
            Matrix lightView = Matrix.CreateLookAt(lightPosition, lightPosition - lightDirection, Vector3.Up);

            // Create the projection matrix for the light
            // The projection is orthographic since we are using a directional light
            Matrix lightProjection = Matrix.CreateOrthographic(boxSize.X, boxSize.Y, -boxSize.Z, boxSize.Z);

            return lightView * lightProjection;
        }

        protected void DrawModel(BaseCamera camera, bool createShadowMap, ref RenderTarget2D shadowRenderTarget)
        {
            lightViewProjection = CreateLightViewProjectionMatrix(camera);
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
                    effect.Parameters["LightDirection"].SetValue(lightDirection);
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
            if (bounds.Intersects(bob.bounds) && firstPassCollision == true)
            {
                if (bob.bounds.Min.Y > (bounds.Max.Y - 1.0f) && (bob.bounds.Min.X > bounds.Min.X && bob.bounds.Max.X < bounds.Max.X && bob.bounds.Min.Z > bounds.Min.Z && bob.bounds.Max.Z < bounds.Max.Z))
                {
                    safeHit = true;
                    bob.safeHit = true;
                }
                else
                {
                    safeHit = false;
                    bob.safeHit = false;
                }
                hitTest = true;
                bob.hitTest = true;
            }
            firstPassCollision = true;
            return hitTest;
        }
    }
}
