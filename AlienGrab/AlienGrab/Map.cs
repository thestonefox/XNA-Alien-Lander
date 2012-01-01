﻿using System;
using System.Collections.Generic;
using System.Collections;
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
        protected float playAreaCeiling;
        protected BoundingBox playArea;

        protected Vector3 blockDimensions = new Vector3(165,60,165);

        public Map(Game _game, Vector3 coordinates)
        {
            game = _game;
            playAreaCeiling = (coordinates.Y * blockDimensions.Y) * 1.2f;
            layout = new int[(int)coordinates.Z, (int)coordinates.X];
            GenerateMap((int)coordinates.Y);            
        }

        public void LoadContent()
        {
            foreach (Base3DObject block in blocks)
            {
                block.LoadContent();
            }
        }

        public void Update(GameTime gameTime)
        {
        }

        public void Draw(GameTime gameTime, BaseCamera camera, ref RenderTarget2D shadowRenderTarget)
        {
            DrawMap(gameTime, camera, ref shadowRenderTarget);            
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

        protected void DrawMap(GameTime gameTime, BaseCamera camera, ref RenderTarget2D shadowRenderTarget)
        {
            int blockCounter = 0;
            for (int d = 0;  d < layout.GetLength(0); d++)
            {
                for (int w = 0; w < layout.GetLength(1); w++)
                {
                    blockCounter = DrawBuilding(gameTime, blockCounter, new Vector2(w, d), camera, ref shadowRenderTarget);
                }
            }
        }

        protected Vector3 CalculatePosition(Vector3 coordinates)
        {
            return new Vector3(coordinates.X * blockDimensions.X, coordinates.Y * blockDimensions.Y, coordinates.Z * blockDimensions.Z);
        }

        protected int DrawBuilding(GameTime gameTime, int blockCounter, Vector2 coordinate, BaseCamera camera, ref RenderTarget2D shadowRenderTarget)
        {
            for (int h = 0; h < layout[(int)coordinate.Y, (int)coordinate.X]; h++)
            {             
                blocks[blockCounter].Update(gameTime);
                blocks[blockCounter].Draw(camera, ref shadowRenderTarget);
                blockCounter++;
            }
            return blockCounter;
        }

        protected void GenerateMap(int maxHeight)
        {
            int blockCounter = 0;
            Random height = new Random();
            ArrayList positions = new ArrayList();
            
            for (int d = 0; d < layout.GetLength(0); d++)
            {
                for (int w = 0; w < layout.GetLength(1); w++)
                {                    
                    layout[d, w] =  height.Next((layout.GetLength(0)-d), maxHeight);
                    blockCounter += layout[d, w];
                    for (int h = 0; h < layout[d, w]; h++)
                    {
                        int[] coords = new int[3] { w, h, d };
                        positions.Add(coords);
                    }
                }
            }
            blocks = new Base3DObject[blockCounter];
            Vector3 minPlayArea = Vector3.Zero;
            Vector3 maxPlayArea = Vector3.Zero;
            for (int c = 0; c < blocks.Length; c++)
            {
                int[] coords = (int[])positions[c];
                blocks[c] = new Base3DObject(game, "cube");
                blocks[c].Position = CalculatePosition(new Vector3(coords[0], coords[1], coords[2]));
                blocks[c].Initialize();

                //Console.WriteLine(coords[0] + "," + coords[1] + ","+coords[2]+","+  " - " + (layout.GetLength(1)-1) + " , " + (layout.GetLength(0)-1));

                //top left                
                if (coords[0] == 0 && coords[1] == 0 && coords[2] == 0)
                {
                    minPlayArea = blocks[c].bounds.Min;
                }

                //bottom right                
                if (c == blocks.Length-1)
                {
                    maxPlayArea = blocks[c].bounds.Max;
                    maxPlayArea.Y = playAreaCeiling;
                }
            }
            playArea = new BoundingBox(minPlayArea, maxPlayArea);
        }

        public BoundingBox GetPlayArea()
        {
            return playArea;
        }
    }
}
