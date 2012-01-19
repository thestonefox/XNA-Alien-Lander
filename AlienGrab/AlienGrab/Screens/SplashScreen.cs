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
    class SplashScreen : GameScreen
    {
        public SplashScreen(ContentManager content, String assetName, String fontName)
            : base(content, assetName, fontName)
        {
        }

        public void Update(ref ApplicationState gameState, ref PlayerIndex[] controllingPlayer)
        {
            for (PlayerIndex index = PlayerIndex.One; index <= PlayerIndex.Four; index++)
            {
                if (GamePad.GetState(index).Buttons.Start == ButtonState.Pressed || GamePad.GetState(index).Buttons.A == ButtonState.Pressed)
                {
                    controllingPlayer[0] = index;
                    gameState = ApplicationState.Home;
                }

                if (Keyboard.GetState().IsKeyDown(Keys.Space))
                {
                    controllingPlayer[0] = PlayerIndex.One;
                    gameState = ApplicationState.Home;
                }
            }
        }
    }
}
