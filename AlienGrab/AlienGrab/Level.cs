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
        private Map map;
        private Player playerOne;

        private Vector3 cameraPosition;
        private Vector3 cameraView;

        public Level(GraphicsDevice _device)
        {
            map = new Map(new Vector3(8, 8, 5));
            cameraPosition = new Vector3(-20.0f, 1230.0f, 1180.0f);
            cameraView = new Vector3(1050.0f, -1050.0f, -880.0f);
            playerOne = new Player();
        }

        public void LoadContent(ContentManager content)
        {
            map.LoadContent(content);
            playerOne.LoadContent(content);
            playerOne.Reset(Vector2.Zero);
        }

        public void Update(GameTime gameTime)
        {
            int spin = 50;
            map.Update(gameTime, cameraPosition, cameraView);
            playerOne.Update(gameTime);

            if (Keyboard.GetState().IsKeyDown(Keys.NumPad8))
            {
                cameraPosition.Y += spin;
                cameraView.Y -= spin;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.NumPad2))
            {
                cameraPosition.Y -= spin;
                cameraView.Y += spin;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.NumPad4))
            {
                cameraPosition.X -= spin;
                cameraView.X += spin;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.NumPad6))
            {
                cameraPosition.X += spin;
                cameraView.X -= spin;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.NumPad9))
            {
                cameraPosition.Z += spin;
                cameraView.Z -= spin;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.NumPad3))
            {
                cameraPosition.Z -= spin;
                cameraView.Z += spin;
            }

            if (Keyboard.GetState().IsKeyDown(Keys.W))
            {
                cameraView.Y += spin;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.S))
            {
                cameraView.Y -= spin;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.A))
            {
                cameraView.X -= spin;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.D))
            {
                cameraView.X += spin;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.R))
            {
                cameraView.Z += spin;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.F))
            {
                cameraView.Z -= spin;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.P))
            {
                Console.WriteLine(cameraPosition + " - " + cameraView + ".");
            }
        }

        public void Draw(GraphicsDevice device, GameTime gameTime, SpriteBatch spriteBatch)
        {
            map.Draw(device, gameTime, spriteBatch);
            spriteBatch.Begin();
            playerOne.Draw(gameTime, spriteBatch);
            spriteBatch.End();            
        }
    }
}
