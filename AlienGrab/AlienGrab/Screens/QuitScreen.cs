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
    class QuitScreen : BaseScreen
    {
        public QuitScreen(ContentManager content, String assetName, String fontName)
            : base(content, assetName, fontName)
        {
            menuColour = Color.Orange;
            selectedColour = Color.OrangeRed;
            options.Add("No, Go Back");
            options.Add("Yes, I'm Sure");
            SetOptions(new Vector2(650, 320), 1);
        }

        public new int Update(InputState input, PlayerIndex[] controllingPlayer)
        {
            base.Update(input, controllingPlayer);
            if (selectedIndex == 0 || (PressBack(input, controllingPlayer) || input.IsNewButtonPress(ButtonMappings.Pad_BBtn, controllingPlayer[0], out controllingPlayer[1]) ||
                    input.IsNewKeyPress(ButtonMappings.Keyboard_BBtn, controllingPlayer[0], out controllingPlayer[1])))
            {
                return 0;
            }
            else if (selectedIndex == 1)
            {
                return 1;
            }
            return -1;            
        }
    }
}
