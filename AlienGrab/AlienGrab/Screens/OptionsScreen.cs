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
    class OptionsScreen : GameScreen
    {
        public OptionsScreen(ContentManager content, String assetName, String fontName)
            : base(content, assetName, fontName)
        {
        }

        public void Update(ref ApplicationState gameState, InputState input, PlayerIndex[] controllingPlayer)
        {
            base.Update(input, controllingPlayer);

            if ((input.IsNewButtonPress(Buttons.A, controllingPlayer[0], out controllingPlayer[1]) ||
                    input.IsNewKeyPress(Keys.Space, controllingPlayer[0], out controllingPlayer[1])) ||
                (input.IsNewButtonPress(Buttons.B, controllingPlayer[0], out controllingPlayer[1]) ||
                    input.IsNewKeyPress(Keys.Escape, controllingPlayer[0], out controllingPlayer[1])))
            {
                gameState = ApplicationState.Home;
            }
        }
    }
}
