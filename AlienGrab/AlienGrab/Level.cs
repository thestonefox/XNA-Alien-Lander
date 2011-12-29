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
        protected BaseCamera camera;
        protected Map map;
        protected Player playerOne;

        public Level(Game game)
        {
            map = new Map(game, new Vector3(8, 8, 5));
            camera = new BaseCamera(game.GraphicsDevice.Viewport.AspectRatio, 10.0f, 10000.0f);
            camera.Position = new Vector3(-20.0f, 1230.0f, 1180.0f);
            camera.View = new Vector3(1050.0f, -1050.0f, -880.0f);
            playerOne = new Player(game, "ship");            
        }

        public void LoadContent(ContentManager content)
        {
            map.LoadContent(content);
            playerOne.LoadContent(content);
            playerOne.Position = new Vector3(0.0f, 500.0f, 0.0f);
        }

        protected void ControlCamera(InputState input, PlayerIndex[] controllingPlayer)
        {
            int spin = 50;
            if (input.IsNewButtonHeld(Buttons.RightThumbstickLeft, controllingPlayer[0], out controllingPlayer[1]) ||
                input.IsNewKeyHeld(Keys.NumPad4, controllingPlayer[0], out controllingPlayer[1]))
            {
                if (camera.Position.Z > 130)
                {
                    camera.Position.X -= spin;
                    camera.View.X += spin;
                }
                else
                {
                    camera.Position.X += spin;
                    camera.View.X -= spin;
                }
            }
            if (input.IsNewButtonHeld(Buttons.RightThumbstickRight, controllingPlayer[0], out controllingPlayer[1]) ||
                input.IsNewKeyHeld(Keys.NumPad6, controllingPlayer[0], out controllingPlayer[1]))
            {
                if (camera.Position.Z <= 130)
                {
                    camera.Position.X -= spin;
                    camera.View.X += spin;
                }
                else
                {
                    camera.Position.X += spin;
                    camera.View.X -= spin;
                }
            }
            if (input.IsNewButtonHeld(Buttons.RightThumbstickUp, controllingPlayer[0], out controllingPlayer[1]) ||
                input.IsNewKeyHeld(Keys.NumPad8, controllingPlayer[0], out controllingPlayer[1]))
            {
                camera.Position.Y += spin;
                camera.View.Y -= spin;
            }
            if (input.IsNewButtonHeld(Buttons.RightThumbstickDown, controllingPlayer[0], out controllingPlayer[1]) ||
                input.IsNewKeyHeld(Keys.NumPad2, controllingPlayer[0], out controllingPlayer[1]))
            {
                camera.Position.Y -= spin;
                camera.View.Y += spin;
            }
            if (input.IsNewButtonHeld(Buttons.LeftTrigger, controllingPlayer[0], out controllingPlayer[1]) ||
                input.IsNewKeyHeld(Keys.NumPad7, controllingPlayer[0], out controllingPlayer[1]))
            {
                camera.Position.Z += spin;
                camera.View.Z -= spin;
            }
            if (input.IsNewButtonHeld(Buttons.RightTrigger, controllingPlayer[0], out controllingPlayer[1]) ||
                input.IsNewKeyHeld(Keys.NumPad9, controllingPlayer[0], out controllingPlayer[1]))
            {
                camera.Position.Z -= spin;
                camera.View.Z += spin;
            }

            camera.Position.X = MathHelper.Clamp(camera.Position.X, -320.0f, 1230.0f);
            camera.View.X = MathHelper.Clamp(camera.View.X, -200.0f, 1350.0f);
            camera.Position.Y = MathHelper.Clamp(camera.Position.Y, 630.0f, 1130.0f);
            camera.View.Y = MathHelper.Clamp(camera.View.Y, -950.0f, -450.0f);
            camera.Position.Z = MathHelper.Clamp(camera.Position.Z, -570.0f, 1180.0f);
            camera.View.Z = MathHelper.Clamp(camera.View.Z, -880.0f, 870.0f);
        }

        public void Update(GameTime gameTime, InputState input, PlayerIndex[] controllingPlayer)
        {
            ControlCamera(input, controllingPlayer);
            map.Update(gameTime);            
            playerOne.Update(gameTime);
            if (map.CheckCollision(playerOne))
            {
                playerOne.Position = playerOne.lastPosition;
            }
            else
            {
                playerOne.lastPosition = playerOne.Position;
            }
            
            if (Keyboard.GetState().IsKeyDown(Keys.P))
            {
                Console.WriteLine(camera.Position + " - " + camera.View + "." + " - " + playerOne.Position);
            }
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            map.Draw(gameTime, camera);
            playerOne.Draw(gameTime, camera);
            spriteBatch.Begin();
            spriteBatch.End();
        }
    }
}
