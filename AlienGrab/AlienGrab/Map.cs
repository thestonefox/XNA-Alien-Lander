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
        private Texture2D block;
        private Rectangle blockSource;
        private const int clipWidth = 99;
        private const int clipDepth = 30;
        private const int clipHeight = 50;  

        public Map(int depth, int width, int maxHeight)
        {
            layout = new int[depth, width];
            GenerateMap(maxHeight);
        }

        public void LoadContent(ContentManager content)
        {
            block = content.Load<Texture2D>("block");
            blockSource = new Rectangle(0,0, block.Width, block.Height);
        }

        public void Update(GameTime gameTime)
        {
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            DrawMap(spriteBatch);
        }

        protected void DrawMap(SpriteBatch spriteBatch)
        {
            for (int d = 0; d <= layout.GetUpperBound(0); d++)
            {
                for (int w = 0; w <= layout.GetUpperBound(1); w++)
                {
                    DrawBuilding(spriteBatch, d, w);
                }
            }
        }

        protected void DrawBuilding(SpriteBatch spriteBatch, int depth, int width)
        {
            int height = layout[depth, width];
            float drawDepth = (float)(((width * 1) + (depth * 10)) + 1.0f)/100;
            for (int h = 0; h < height; h++)
            {
                Vector2 position = new Vector2((150 - (clipDepth * depth)) + clipWidth * width, (350 + (clipDepth * depth)) - (clipHeight * h));
                spriteBatch.Draw(block, position, blockSource, Color.White, 0.0f, new Vector2(blockSource.Width / 2, blockSource.Height / 2), 1.0f, SpriteEffects.None, drawDepth);
            }
        }

        protected void GenerateMap(int maxHeight)
        {
            Random height = new Random();
            for (int d = 0; d <= layout.GetUpperBound(0); d++)
            {
                for (int w = 0; w <= layout.GetUpperBound(1); w++)
                {
                    layout[d, w] = 3; // height.Next((layout.GetUpperBound(0) + 1) - d, maxHeight - d);
                }
            }
        }
    }
}
