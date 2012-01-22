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
    class GameOverScreen : GameScreen
    {
        protected int finalLevel;
        protected int finalScore;
        protected int drawState;
        private OptionsHolder gameOptions = OptionsHolder.Instance;

        public GameOverScreen(ContentManager content, String assetName, String fontName)
            : base(content, assetName, fontName)
        {
            Reset();
        }

        public override void Reset()
        {
            drawState = 70;
            base.Reset();
        }

        public void Update(int _finalLevel, int _finalScore, ref ApplicationState gameState, InputState input, PlayerIndex[] controllingPlayer)
        {
            finalLevel = _finalLevel;
            finalScore = _finalScore;
            if (drawState <= 0 && (input.IsNewButtonPress(ButtonMappings.Pad_ABtn, controllingPlayer[0], out controllingPlayer[1]) ||
                    input.IsNewKeyPress(ButtonMappings.Keyboard_ABtn, controllingPlayer[0], out controllingPlayer[1])))
            {                
                gameState = ApplicationState.Home;
            }
            drawState--;
            if (drawState > 0 && (input.IsNewButtonPress(ButtonMappings.Pad_ABtn, controllingPlayer[0], out controllingPlayer[1]) ||
                    input.IsNewKeyPress(ButtonMappings.Keyboard_ABtn, controllingPlayer[0], out controllingPlayer[1])))
            {
                drawState = 0;
            }
            base.Update(input, controllingPlayer);
            if (gameState == ApplicationState.Home)
            {
                Reset();
            }
        }

        protected override void DrawOptions(SpriteBatch spriteBatch)
        {
            if (drawState < 50)
            {
                TextWriter.WriteText(spriteBatch, font, finalLevel.ToString(), new Vector2(660, 324), Color.White, 0);
            }
            if (drawState <= 0)
            {
                TextWriter.WriteText(spriteBatch, font, finalScore.ToString().PadLeft(gameOptions.ScorePadding, '0'), new Vector2(660, 389), Color.White, 0);
            }
        }
    }
}
