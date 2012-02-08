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
    class GameOverScreen : BaseScreen
    {
        protected int finalLevel;
        protected int finalScore;
        protected int drawState;
        protected bool firstCall;
        private OptionsHolder gameOptions = OptionsHolder.Instance;
        private Song mySong;

        public GameOverScreen(ContentManager content, String assetName, String fontName)
            : base(content, assetName, fontName)
        {
            mySong = content.Load<Song>("Audio\\Music\\Gameover");
            Reset();
        }

        public override void Reset()
        {
            menuColour = Color.Crimson;
            selectedColour = Color.Red;

            drawState = 70;
            firstCall = true;
            base.Reset();
        }

        public void Update(int _finalLevel, int _finalScore, ref ApplicationState appState, InputState input, PlayerIndex[] controllingPlayer)
        {
            if (firstCall == true)
            {
                MediaPlayer.IsRepeating = true;
                MediaPlayer.Volume = gameOptions.MusicVolumeAtPlay;
                MediaPlayer.Play(mySong);
                firstCall = false;
            }
            finalLevel = _finalLevel;
            finalScore = _finalScore;
            if (drawState <= 0 && (input.IsNewButtonPress(ButtonMappings.Pad_ABtn, controllingPlayer[0], out controllingPlayer[1]) ||
                    input.IsNewKeyPress(ButtonMappings.Keyboard_ABtn, controllingPlayer[0], out controllingPlayer[1])))
            {
                appState = ApplicationState.InitaliseApp;
            }
            drawState--;
            if (drawState > 0 && (input.IsNewButtonPress(ButtonMappings.Pad_ABtn, controllingPlayer[0], out controllingPlayer[1]) ||
                    input.IsNewKeyPress(ButtonMappings.Keyboard_ABtn, controllingPlayer[0], out controllingPlayer[1])))
            {
                drawState = 0;
            }
            base.Update(input, controllingPlayer);
            if (appState == ApplicationState.InitaliseApp)
            {
                MediaPlayer.Stop();
                Reset();
            }
        }

        protected override void DrawOptions(SpriteBatch spriteBatch)
        {
            if (drawState < 50)
            {
                TextWriter.WriteText(spriteBatch, font, finalLevel.ToString(), new Vector2(660, 289), menuColour, 0);
            }
            if (drawState <= 0)
            {
                TextWriter.WriteText(spriteBatch, font, finalScore.ToString().PadLeft(gameOptions.ScorePadding, '0'), new Vector2(660, 377), menuColour, 0);
            }
        }
    }
}
