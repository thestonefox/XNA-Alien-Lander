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
        protected Game game;
        protected int[,] layout;
        protected Base3DObject[] blocks;

        public Map(Game _game, Vector3 coordinates)
        {
            game = _game;
            layout = new int[(int)coordinates.Z, (int)coordinates.X];
            GenerateMap((int)coordinates.Y);
        }

        public void LoadContent(ContentManager content)
        {
            foreach (Base3DObject block in blocks)
            {
                block.LoadContent(content);
            }
        }

        public void Update(GameTime gameTime)
        {
        }

        public void Draw(GameTime gameTime, BaseCamera camera)
        {
            DrawMap(gameTime, camera);            
        }

        public bool CheckCollision(Base3DObject foreignObject)
        {
            bool collisionDetected = false;
            int blockCounter = 0;
            for (int d = 0; d < layout.GetLength(0); d++)
            {
                for (int w = 0; w < layout.GetLength(1); w++)
                {
                    for (int h = 0; h < layout[d, w]; h++)
                    {
                        if (blocks[blockCounter].Collided(foreignObject))
                        {
                            return true;
                        }
                        blockCounter++;
                    }
                }
            }
            return collisionDetected;
        }

        protected void DrawMap(GameTime gameTime, BaseCamera camera)
        {
            int blockCounter = 0;
            for (int d = 0;  d < layout.GetLength(0); d++)
            {
                for (int w = 0; w < layout.GetLength(1); w++)
                {
                    blockCounter = DrawBuilding(gameTime, blockCounter, new Vector2(w, d), camera);
                }
            }
        }

        protected Vector3 CalculatePosition(Vector3 coordinates)
        {
            return new Vector3(coordinates.X * 125, coordinates.Y * 60, coordinates.Z * 125);
        }

        protected int DrawBuilding(GameTime gameTime, int blockCounter, Vector2 coordinate, BaseCamera camera)
        {
            for (int h = 0; h < layout[(int)coordinate.Y, (int)coordinate.X]; h++)
            {
                blocks[blockCounter].Position = CalculatePosition(new Vector3(coordinate.X, h, coordinate.Y));
                blocks[blockCounter].Update(gameTime);
                blocks[blockCounter].Draw(gameTime, camera);
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
            blocks = new Base3DObject[blockCounter];
            for (int c = 0; c < blocks.Length; c++)
            {
                blocks[c] = new Base3DObject(game, "cube");
            }
        }
    }
}
