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
    class Map
    {
        private int[,] layout;
        private Model block;
        private Vector3 cameraView;
        private Vector3 cameraPosition;

        public Map(Vector3 coordinates)
        {
            cameraView = Vector3.Zero;
            cameraPosition = Vector3.Zero;
            layout = new int[(int)coordinates.Z, (int)coordinates.X];
            GenerateMap((int)coordinates.Y);
        }

        public void LoadContent(ContentManager content)
        {
            block = LoadModel(content, "cube");
        }

        protected Model LoadModel(ContentManager content, String assetName)
        {
            Model newModel = content.Load<Model>(assetName);
            return newModel;
        }

        public void Update(GameTime gameTime, Vector3 _cameraPosition, Vector3 _cameraView)
        {
            cameraPosition = _cameraPosition;
            cameraView = _cameraView;
        }

        public void Draw(GraphicsDevice device, GameTime gameTime, SpriteBatch spriteBatch)
        {
            DrawMap(device);            
        }

        protected void DrawMap(GraphicsDevice device)
        {
            for (int d = 0;  d < layout.GetLength(0); d++)
            {
                for (int w = layout.GetLength(1) - 1; w >= 0;  w--)
                {
                    DrawBuilding(device, new Vector2(w, d));
                }
            }
        }

        protected void DrawBuilding(GraphicsDevice device, Vector2 coordinate)
        {
            for (int h = 0; h < layout[(int)coordinate.Y, (int)coordinate.X]; h++)
            {
                Vector3 position = new Vector3(coordinate.X * 125, h * 60, coordinate.Y * 125);
                DrawBlock(device, block, position);
            }
        }

        protected void DrawBlock(GraphicsDevice device, Model model, Vector3 position)
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
                    effect.DiffuseColor = new Vector3(position.X / 1000, position.Y / 1000, position.Z / 1000);
                    effect.World = transforms[mesh.ParentBone.Index] *
                                    Matrix.CreateRotationY(0.0f) *
                                    Matrix.CreateTranslation(position);
                    effect.View = Matrix.CreateLookAt(cameraPosition, cameraView, Vector3.Up);
                    effect.Projection = Matrix.CreatePerspectiveFieldOfView(MathHelper.ToRadians(45.0f), device.Viewport.AspectRatio, 10f, 10000.0f);
                }
                // Draw the mesh, using the effects set above.
                mesh.Draw();
            }
        }

        protected void GenerateMap(int maxHeight)
        {
            Random height = new Random();
            for (int d = 0; d < layout.GetLength(0); d++)
            {
                for (int w = 0; w < layout.GetLength(1); w++)
                {
                    layout[d, w] = height.Next((layout.GetLength(0)) - d, maxHeight);
                }
            }
        }
    }
}
