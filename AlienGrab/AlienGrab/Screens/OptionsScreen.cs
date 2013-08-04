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
            SetTransition(Color.Black, 0.06f);
        }

        public void Update(ref ApplicationState appState, InputState input, PlayerIndex[] controllingPlayer)
        {
            base.Update(input, controllingPlayer);

            if (PressBack(input, controllingPlayer))
            {
                SetTransitionOut(ApplicationState.Home);
            }
            appState = ReturnState(ApplicationState.Options);
        }
    }
}
