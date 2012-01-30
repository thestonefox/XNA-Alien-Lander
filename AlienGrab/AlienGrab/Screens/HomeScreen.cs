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
        private OptionsHolder gameOptions = OptionsHolder.Instance;
        private bool addBuyOption = false;

        public HomeScreen(ContentManager content, String assetName, String fontName)
            : base(content, assetName, fontName)
        {
            options.Add("Play Game");
            options.Add("View Controls");
            options.Add("Quit To Dashboard");
            SetOptions(new Vector2(650, 310), 1);
        }

        public void Update(ref ApplicationState gameState, InputState input, PlayerIndex[] controllingPlayer)
        {
            base.Update(input, controllingPlayer);
            if (gameOptions.IsTrial == true && addBuyOption == false)
            {
                options.Add("Unlock Full Game");
                addBuyOption = true;
            }
            if (gameOptions.IsTrial == false && addBuyOption == true)
            {
                options.Remove("Unlock Full Game");
            }


            switch (selectedIndex)
            {
                case 0: gameState = ApplicationState.InitaliseGame;
                    break;
                case 1: gameState = ApplicationState.Options;
                    break;
                case 2: gameState = ApplicationState.Quit;
                    break;
                case 3: if (controllingPlayer[0].CanBuyGame()) { Guide.ShowMarketplace(controllingPlayer[0]); }
                    break;
            }
        }
    }
}
