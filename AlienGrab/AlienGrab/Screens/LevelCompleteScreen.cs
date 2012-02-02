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
            Reset();
        }

        public override void Reset()
        {
            fuelLeft = 0;
            livesLeft = 0;
            score = 0;
            drawState = 120;
        }

        public bool Update(int _fuelLeft, int _livesLeft, int _score, InputState input, PlayerIndex[] controllingPlayer)
        {
            fuelLeft = _fuelLeft;
            livesLeft = _livesLeft;
            score = _score;
            if (drawState <= 0 && (input.IsNewButtonPress(ButtonMappings.Pad_ABtn, controllingPlayer[0], out controllingPlayer[1]) ||
                    input.IsNewKeyPress(ButtonMappings.Keyboard_ABtn, controllingPlayer[0], out controllingPlayer[1])))
            {
                Reset();
                return true;
            }
            drawState--;
            if (drawState > 0 && (input.IsNewButtonPress(ButtonMappings.Pad_ABtn, controllingPlayer[0], out controllingPlayer[1]) ||
                    input.IsNewKeyPress(ButtonMappings.Keyboard_ABtn, controllingPlayer[0], out controllingPlayer[1])))
            {
                drawState = 0;
            }
            base.Update(input, controllingPlayer);
            return false;
        }

        protected override void DrawOptions(SpriteBatch spriteBatch)
        {
            if (drawState < 100)
            {
                TextWriter.WriteText(spriteBatch, font, fuelLeft + " X "+gameOptions.FuelMultiplier, new Vector2(660, 292), Color.White, 0);
            }
            if (drawState < 50)
            {
                TextWriter.WriteText(spriteBatch, font, "X " + livesLeft, new Vector2(660, 352), Color.White, 0);
            }
            if (drawState <= 0)
            {
                TextWriter.WriteText(spriteBatch, font, score.ToString().PadLeft(gameOptions.ScorePadding, '0'), new Vector2(660, 412), Color.White, 0);
            }
        }
    }
}
