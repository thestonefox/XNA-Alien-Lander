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
        private Sprite3D[] blocks;

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
            foreach (Sprite3D block in blocks)
            {
                block.LoadContent(content);
                block.Initalize(Vector3.Zero, 0.0f, cameraView, cameraPosition);
            }
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
            int blockCounter = 0;
            for (int d = 0;  d < layout.GetLength(0); d++)
            {
                for (int w = layout.GetLength(1) - 1; w >= 0;  w--)
                {
                    blockCounter = DrawBuilding(device, blockCounter, new Vector2(w,d));
                }
            }
        }

        protected int DrawBuilding(GraphicsDevice device, int blockCounter, Vector2 coordinate)
        {
            for (int h = 0; h < layout[(int)coordinate.Y, (int)coordinate.X]; h++)
            {
                Vector3 position = new Vector3(coordinate.X * 125, h * 60, coordinate.Y * 125);
                blocks[blockCounter].Initalize(position, 0.0f, cameraView, cameraPosition);
                blocks[blockCounter].Draw(device);
                blockCounter++;
            }
            return blockCounter;
        }

        protected void GenerateMap(int maxHeight)
        {
            int blockCounter = 0;
            Random height = new Random();
            for (int d = 0; d < layout.GetLength(0); d++)
            {
                for (int w = 0; w < layout.GetLength(1); w++)
                {
                    layout[d, w] = height.Next((layout.GetLength(0)) - d, maxHeight);
                    blockCounter += layout[d, w];
                }
            }
            blocks = new Sprite3D[blockCounter];
            for (int c = 0; c < blocks.Length; c++)
            {
                blocks[c] = new Sprite3D("cube");
            }
        }
    }
}
