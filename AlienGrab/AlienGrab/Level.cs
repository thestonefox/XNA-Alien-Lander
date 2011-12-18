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

        protected void ControlCamera(InputState input, PlayerIndex[] controllingPlayer)
        {
            int spin = 50;
            if (input.IsNewButtonHeld(Buttons.LeftThumbstickLeft, controllingPlayer[0], out controllingPlayer[1]) ||
                input.IsNewKeyHeld(Keys.NumPad4, controllingPlayer[0], out controllingPlayer[1]))
            {
                cameraPosition.X -= spin;
                cameraView.X += spin;
            }
            if (input.IsNewButtonHeld(Buttons.LeftThumbstickRight, controllingPlayer[0], out controllingPlayer[1]) ||
                input.IsNewKeyHeld(Keys.NumPad6, controllingPlayer[0], out controllingPlayer[1]))
            {
                cameraPosition.X += spin;
                cameraView.X -= spin;
            }
            if (input.IsNewButtonHeld(Buttons.LeftThumbstickUp, controllingPlayer[0], out controllingPlayer[1]) ||
                input.IsNewKeyHeld(Keys.NumPad8, controllingPlayer[0], out controllingPlayer[1]))
            {
                cameraPosition.Y += spin;
                cameraView.Y -= spin;
            }
            if (input.IsNewButtonHeld(Buttons.LeftThumbstickDown, controllingPlayer[0], out controllingPlayer[1]) ||
                input.IsNewKeyHeld(Keys.NumPad2, controllingPlayer[0], out controllingPlayer[1]))
            {
                cameraPosition.Y -= spin;
                cameraView.Y += spin;
            }
            if (input.IsNewButtonHeld(Buttons.LeftTrigger, controllingPlayer[0], out controllingPlayer[1]) ||
                input.IsNewKeyHeld(Keys.NumPad7, controllingPlayer[0], out controllingPlayer[1]))
            {
                cameraPosition.Z += spin;
                cameraView.Z -= spin;
            }
            if (input.IsNewButtonHeld(Buttons.RightTrigger, controllingPlayer[0], out controllingPlayer[1]) ||
                input.IsNewKeyHeld(Keys.NumPad9, controllingPlayer[0], out controllingPlayer[1]))
            {
                cameraPosition.Z -= spin;
                cameraView.Z += spin;
            }
        }

        public void Update(GameTime gameTime, InputState input, PlayerIndex[] controllingPlayer)
        {
            ControlCamera(input, controllingPlayer);
            map.Update(gameTime, cameraPosition, cameraView);
            playerOne.Update(gameTime);            
            
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
