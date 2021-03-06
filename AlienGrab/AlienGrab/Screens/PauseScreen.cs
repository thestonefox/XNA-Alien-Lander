﻿using System;
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
    class PauseScreen : BaseScreen
    {
        protected QuitScreen quitScreen;
        protected bool confirm;

        public PauseScreen(ContentManager content, String assetName, String fontName)
            : base(content, assetName, fontName)
        {
            menuColour = Color.Purple;
            selectedColour = Color.Magenta;
            options.Add("Resume Game");
            options.Add("Quit To Menu");
            SetOptions(new Vector2(650, 310), 1);
            quitScreen = new QuitScreen(content, "Screens/quit", fontName);
            Reset();
        }

        public override void Reset()
        {
            confirm = false;
            base.Reset();
        }

        public void Update(ref ApplicationState appState, InputState input, PlayerIndex[] controllingPlayer)
        {
            base.Update(input, controllingPlayer);

            if (confirm == true)
            {
                switch(quitScreen.Update(input, controllingPlayer))
                {
                    case 1: appState = ApplicationState.InitaliseApp;
                            break;

                    case 0: confirm = false;
                            menuIndex = 1;
                            return;
                }
            }

            if (selectedIndex == 0 || (input.IsNewButtonPress(ButtonMappings.Pad_BBtn, controllingPlayer[0], out controllingPlayer[1]) ||
                    input.IsNewKeyPress(ButtonMappings.Keyboard_BBtn, controllingPlayer[0], out controllingPlayer[1])))
            {
                menuIndex = 0;
                appState = ApplicationState.Playing;
            }
            else if(selectedIndex == 1)
            {
                confirm = true;
                quitScreen.Reset();
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
            if (confirm == true)
            {
                quitScreen.Draw(spriteBatch);
            }            
        }
    }
}
