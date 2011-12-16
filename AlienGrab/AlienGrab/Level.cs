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
    class Level
    {
        private GraphicsDevice device;
        private Rectangle gameArea;
        private Map map;
        private Player playerOne;
        private Matrix viewMatrix;
        private Matrix projectionMatrix;

        private Vector3 camerapos;
        private Vector3 cameraviewpos;

        public Level(Rectangle _gameArea, GraphicsDevice _device)
        {
            gameArea = _gameArea;
            device = _device;
            map = new Map(new Vector3(6, 8, 4), device);
            camerapos = new Vector3(30, 15, -8);
            cameraviewpos = new Vector3(-179,-127,-8) ;

            SetUpCamera();
            playerOne = new Player();
        }

        public void LoadContent(ContentManager content)
        {
            map.LoadContent(content);
            playerOne.LoadContent(content);
            playerOne.Reset(Vector2.Zero);
        }

        public void SetUpCamera()
        {
            viewMatrix = Matrix.CreateLookAt(camerapos, cameraviewpos, new Vector3(0, 1, 0));
            projectionMatrix = Matrix.CreatePerspectiveFieldOfView(MathHelper.PiOver4, device.Viewport.AspectRatio, 0.2f, 500.0f);
        }

        public void Update(GameTime gameTime)
        {
            map.Update(gameTime);
            playerOne.Update(gameTime);

            if (Keyboard.GetState().IsKeyDown(Keys.NumPad8))
            {
                camerapos.Y += 1;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.NumPad2))
            {
                camerapos.Y -= 1;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.NumPad4))
            {
                camerapos.X -= 1;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.NumPad6))
            {
                camerapos.X += 1;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.NumPad9))
            {
                camerapos.Z += 1;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.NumPad3))
            {
                camerapos.Z -= 1;
            }

            if (Keyboard.GetState().IsKeyDown(Keys.W))
            {
                cameraviewpos.Y += 1;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.S))
            {
                cameraviewpos.Y -= 1;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.A))
            {
                cameraviewpos.X -= 1;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.D))
            {
                cameraviewpos.X += 1;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.R))
            {
                cameraviewpos.Z += 1;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.F))
            {
                cameraviewpos.Z -= 1;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.P))
            {
                Console.WriteLine(camerapos + " " + cameraviewpos);
            }


            viewMatrix = Matrix.CreateLookAt(camerapos, cameraviewpos, new Vector3(0, 1, 0));
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            map.Draw(gameTime, spriteBatch, viewMatrix, projectionMatrix);
            playerOne.Draw(gameTime, spriteBatch);
        }
    }
}
