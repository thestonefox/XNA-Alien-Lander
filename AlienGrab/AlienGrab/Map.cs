using System;
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
        public Person[] peeps;
        protected float playAreaCeiling;
        protected BoundingBox playArea;
        protected Vector3 playerStart;
        protected Vector3 playerSafeZones = new Vector3(10.0f, 5.0f, 10.0f);

        protected Vector3 blockDimensions = new Vector3(165,60,165);

        public Map(Game _game, Vector3 coordinates, LightSource light, int totalPeeps)
        {
            game = _game;
            playAreaCeiling = (coordinates.Y * blockDimensions.Y) * 1.2f;
            layout = new int[(int)coordinates.Z, (int)coordinates.X];
            playerStart = Vector3.Zero;
            peeps = new Person[totalPeeps];
            GenerateMap((int)coordinates.Y, light);
        }

        public void Update(GameTime gameTime)
        {
            for (int c = 0; c < blocks.Length; c++)
            {
                blocks[c].Update(gameTime);
            }

            for (int c = 0; c < peeps.Length; c++)
            {
                peeps[c].Update(gameTime);
            }            
        }

        public void Draw(BaseCamera camera, ref RenderTarget2D shadowRenderTarget)
        {
            for (int c = 0; c < blocks.Length; c++)
            {
                blocks[c].Draw(camera, ref shadowRenderTarget);
            }

            for (int c = 0; c < peeps.Length; c++)
            {
                peeps[c].Draw(camera, ref shadowRenderTarget);
            }       
        }

        public CollisionType CheckBuildingCollision(Base3DObject bob)
        {
            //3 types of collision: no collision, crash and landing
            CollisionType ct = CollisionType.None;
            for (int c = 0; c < blocks.Length; c++)
            {
                if (blocks[c].Collided(bob))
                {
                    blocks[c].HitTest = true;
                    bob.HitTest = true;
                    ct = CollisionType.Building;
                    if (bob.Bounds.Min.Y > (blocks[c].Bounds.Max.Y - playerSafeZones.Y) && (bob.Bounds.Min.X > blocks[c].Bounds.Min.X + playerSafeZones.X && bob.Bounds.Max.X < blocks[c].Bounds.Max.X - playerSafeZones.X && bob.Bounds.Min.Z > blocks[c].Bounds.Min.Z + playerSafeZones.Z && bob.Bounds.Max.Z < blocks[c].Bounds.Max.Z - playerSafeZones.Z))
                    {
                        ct = CollisionType.Roof;
                    }
                    break;
                }
                else
                {
                    blocks[c].HitTest = false;
                    bob.HitTest = false;
                }
            }
            return ct;
        }

        public bool CheckPeepCollision(Base3DObject bob)
        {
            for (int c = 0; c < peeps.Length; c++)
            {
                if (peeps[c].Collided(bob))
                {
                    peeps[c].Active = false;
                    return true;
                }
            }
            return false;
        }

        protected Vector3 CalculatePosition(Vector3 coordinates)
        {
            return new Vector3(coordinates.X * blockDimensions.X, coordinates.Y * blockDimensions.Y, coordinates.Z * blockDimensions.Z);
        }

        protected void GenerateMap(int maxHeight, LightSource light)
        {
            int blockCounter = 0;
            Random random = new Random();
            List<int[]> positions = new List<int[]>();
            int peepsLeft = 0;
            
            for (int d = 0; d < layout.GetLength(0); d++)
            {
                for (int w = 0; w < layout.GetLength(1); w++)
                {
                    if (d == 0 && w == 0)
                    {
                        layout[d, w] = maxHeight;
                    }
                    else
                    {
                        layout[d, w] = random.Next((layout.GetLength(0) - d), maxHeight);
                    }
                    blockCounter += layout[d, w];
                    for (int h = 0; h < layout[d, w]; h++)
                    {
                        int[] coords = new int[3] { w, h, d };
                        positions.Add(coords);
                        //this is the top block of the current building
                        if (h == layout[d, w] - 1)
                        {
                            //if this is the first building make it the player start position
                            if (d == 0 && w == 0)
                            {
                                playerStart = new Vector3(w, ((h * blockDimensions.Y) + blockDimensions.Y)+playerSafeZones.Y, d);
                            }
                            //else use the building for a peep or a power up
                            else
                            {
                                if (peepsLeft < peeps.Length && (random.Next(0, 5) == 1 || d >= layout.GetLength(0)-2))
                                {
                                    Person peep = new Person(game, "Models/person", light);
                                    peep.LoadContent(true);
                                    peep.Position = new Vector3(w * blockDimensions.X, (h * blockDimensions.Y) + blockDimensions.Y, d * blockDimensions.Z);
                                    peep.Initialize();
                                    peeps[peepsLeft] = peep;
                                    peepsLeft++;
                                }
                            }
                        }
                    }
                }                
            }
            blocks = new Base3DObject[blockCounter];            
            Vector3 minPlayArea = Vector3.Zero;
            Vector3 maxPlayArea = Vector3.Zero;
            for (int c = 0; c < blocks.Length; c++)
            {
                int[] coords = (int[])positions[c];
                blocks[c] = new Base3DObject(game, "Models/cube", light);
                blocks[c].LoadContent(true);
                blocks[c].Position = CalculatePosition(new Vector3(coords[0], coords[1], coords[2]));
                blocks[c].Initialize();

                //top left                
                if (coords[0] == 0 && coords[1] == 0 && coords[2] == 0)
                {
                    minPlayArea = blocks[c].Position;                    
                }

                //bottom right                
                if (c == blocks.Length-1)
                {
                    maxPlayArea = blocks[c].Position;
                    maxPlayArea.Y = playAreaCeiling;                    
                }
            }
            playArea = new BoundingBox(minPlayArea, maxPlayArea);            
        }

        public BoundingBox GetPlayArea()
        {
            return playArea;
        }

        public Vector3 GetPlayerStartPosition()
        {
            return playerStart;
        }
    }
}
