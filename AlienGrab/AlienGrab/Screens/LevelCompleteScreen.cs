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
    class LevelCompleteScreen : GameScreen
    {
        protected int fuelLeft;
        protected int livesLeft;
        protected int score;
        protected int drawState;

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
            drawState = 170;
        }

        public bool Update(int _fuelLeft, int _livesLeft, int _score, InputState input, PlayerIndex[] controllingPlayer)
        {
            fuelLeft = _fuelLeft;
            livesLeft = _livesLeft;
            score = _score;
            if (drawState <= 0 && (input.IsNewButtonPress(Buttons.A, controllingPlayer[0], out controllingPlayer[1]) ||
                    input.IsNewKeyPress(Keys.Space, controllingPlayer[0], out controllingPlayer[1])))
            {
                Reset();
                return true;
            }
            drawState--;
            if (drawState > 0 && (input.IsNewButtonPress(Buttons.A, controllingPlayer[0], out controllingPlayer[1]) ||
                    input.IsNewKeyPress(Keys.Space, controllingPlayer[0], out controllingPlayer[1])))
            {
                drawState = 0;
            }
            base.Update(input, controllingPlayer);
            return false;
        }

        protected override void DrawOptions(SpriteBatch spriteBatch)
        {
            if (drawState < 150)
            {
                TextWriter.WriteText(spriteBatch, font, fuelLeft + " X 2", new Vector2(660, 292), Color.White, 0);
            }
            if (drawState < 100)
            {
                TextWriter.WriteText(spriteBatch, font, "X " + livesLeft, new Vector2(660, 352), Color.White, 0);
            }
            if (drawState < 50)
            {
                drawState = 0;
                TextWriter.WriteText(spriteBatch, font, score.ToString().PadLeft(12, '0'), new Vector2(660, 412), Color.White, 0);
            }
        }
    }
}
