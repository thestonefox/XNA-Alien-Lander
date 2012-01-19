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
    class HomeScreen : GameScreen
    {
        public HomeScreen(ContentManager content, String assetName, String fontName)
            : base(content, assetName, fontName)
        {
            SetOptions(new String[] { "Play Game", "View Controls", "Quit To Dashboard" }, new Vector2(650, 310), 1);
        }

        public void Update(ref ApplicationState gameState, InputState input, PlayerIndex[] controllingPlayer)
        {
            base.Update(input, controllingPlayer);
            switch (selectedIndex)
            {
                case 0: gameState = ApplicationState.Playing;
                    break;
                case 1: gameState = ApplicationState.Options;
                    break;
                case 2: gameState = ApplicationState.Quit;
                    break;
            }
        }
    }
}
