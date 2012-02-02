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
    class OptionsScreen : BaseScreen
    {
        public OptionsScreen(ContentManager content, String assetName, String fontName)
            : base(content, assetName, fontName)
        {
        }

        public void Update(ref ApplicationState appState, InputState input, PlayerIndex[] controllingPlayer)
        {
            base.Update(input, controllingPlayer);

            if ((input.IsNewButtonPress(ButtonMappings.Pad_ABtn, controllingPlayer[0], out controllingPlayer[1]) ||
                    input.IsNewKeyPress(ButtonMappings.Keyboard_ABtn, controllingPlayer[0], out controllingPlayer[1])) ||
                (input.IsNewButtonPress(ButtonMappings.Pad_BBtn, controllingPlayer[0], out controllingPlayer[1]) ||
                    input.IsNewKeyPress(ButtonMappings.Keyboard_BBtn, controllingPlayer[0], out controllingPlayer[1])))
            {
                appState = ApplicationState.Home;
            }
        }
    }
}
