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
    class TrialScreen : BaseScreen
    {
        private OptionsHolder gameOptions = OptionsHolder.Instance;

        public TrialScreen(ContentManager content, String assetName, String fontName)
            : base(content, assetName, fontName)
        {
            SetTransition(Color.Black, 0.02f);
        }

        public void Update(ref ApplicationState appState, InputState input, PlayerIndex[] controllingPlayer)
        {
            base.Update(input, controllingPlayer);

            if (gameOptions.IsTrial == false)
                {
                    appState = ApplicationState.LevelComplete;
                }

            if ((input.IsNewButtonPress(ButtonMappings.Pad_XBtn, controllingPlayer[0], out controllingPlayer[1]) ||
                    input.IsNewKeyPress(ButtonMappings.Keyboard_XBtn, controllingPlayer[0], out controllingPlayer[1])))
            {
                if (gameOptions.IsTrial == true)
                {
                    if (controllingPlayer[0].CanBuyGame())
                    {
                        Guide.ShowMarketplace(controllingPlayer[0]);
                    }
                }
            }

            if ((input.IsNewButtonPress(ButtonMappings.Pad_BBtn, controllingPlayer[0], out controllingPlayer[1]) ||
                    input.IsNewKeyPress(ButtonMappings.Keyboard_BBtn, controllingPlayer[0], out controllingPlayer[1])))
            {
                appState = ApplicationState.Quit;
            }
        }
    }
}
