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
    class LevelCompleteScreen : BaseScreen
    {
        protected int fuelLeft;
        protected int livesLeft;
        protected int score;
        protected int drawState;
        private OptionsHolder gameOptions = OptionsHolder.Instance;

        public LevelCompleteScreen(ContentManager content, String assetName, String fontName)
            : base(content, assetName, fontName)
        {
            SetTransition(Color.Black, 0.03f);
            menuColour = Color.Orange;
            selectedColour = Color.OrangeRed;
            Reset();
        }

        public override void Reset()
        {
            fuelLeft = 0;
            livesLeft = 0;
            score = 0;
            drawState = 0;
            base.Reset();
        }

        public bool Update(int _fuelLeft, int _livesLeft, int _score, InputState input, PlayerIndex[] controllingPlayer)
        {
            base.Update(input, controllingPlayer);
            fuelLeft = _fuelLeft;
            livesLeft = _livesLeft;
            score = _score;
            drawState = 0;
            if (drawState <= 0 && (PressBack(input, controllingPlayer)))
            {
                Reset();
                return true;
            }            
            return false;
        }

        protected override void DrawOptions(SpriteBatch spriteBatch)
        {
            if (drawState < 100)
            {
                TextWriter.WriteText(spriteBatch, font, fuelLeft + " X "+gameOptions.FuelMultiplier, new Vector2(660, 287), menuColour, 0);
            }
            if (drawState < 50)
            {
                TextWriter.WriteText(spriteBatch, font, "X " + livesLeft, new Vector2(660, 349), menuColour, 0);
            }
            if (drawState <= 0)
            {
                TextWriter.WriteText(spriteBatch, font, score.ToString().PadLeft(gameOptions.ScorePadding, '0'), new Vector2(660, 410), menuColour, 0);
            }
        }
    }
}
