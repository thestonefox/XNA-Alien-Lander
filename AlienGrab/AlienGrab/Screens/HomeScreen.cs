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
    class HomeScreen : BaseScreen
    {
        private OptionsHolder gameOptions = OptionsHolder.Instance;
        private bool addBuyOption = false;

        public HomeScreen(ContentManager content, String assetName, String fontName)
            : base(content, assetName, fontName)
        {
            SetTransition(Color.Black, 0.06f);
            menuColour = Color.Purple;
            selectedColour = Color.Magenta;
            options.Add("Play Game");
            options.Add("How To Play");
            options.Add("Quit Game");
            SetOptions(new Vector2(650, 290), 1);
        }

        public void Update(ref ApplicationState appState, InputState input, PlayerIndex[] controllingPlayer)
        {
            base.Update(input, controllingPlayer);
            /*
            if (gameOptions.IsTrial == true && addBuyOption == false)
            {
                options.Add("Unlock Full Game");
                addBuyOption = true;
            }
            
            if (gameOptions.IsTrial == false && addBuyOption == true)
            {
                options.Remove("Unlock Full Game");
            }*/


            switch (selectedIndex)
            {
                case 0: SetTransitionOut(ApplicationState.InitaliseGame);
                    break;
                case 1: SetTransitionOut(ApplicationState.Options);
                    break;
                case 2: SetTransitionOut(ApplicationState.Quit);
                    break;
                //case 3: if (controllingPlayer[0].CanBuyGame()) { Guide.ShowMarketplace(controllingPlayer[0]); }
                    //break;
            }
            appState = ReturnState(ApplicationState.Home);
        }
    }
}
