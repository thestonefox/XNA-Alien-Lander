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
    class GameScreen
    {
        protected Sprite background;
        protected String[] options;
        protected Vector2 optionsPosition;
        protected SpriteFont font;
        protected int menuIndex;
        protected int selectedIndex;
        protected int menuAlignment;

        protected int loaded;

        public Color menuColour;
        public Color selectedColour;

        private Vector2 heightOffset;
        private Color drawColour;

        public GameScreen(ContentManager content, String assetName, String fontName)
        {
            background = new Sprite(content.Load<Texture2D>(assetName));
            background.Position = new Vector2(background.Width/2, background.Height/2);
            font = content.Load<SpriteFont>(fontName);
            options = new String[0];  
            menuAlignment = 0;
            menuColour = Color.White;
            selectedColour = Color.Yellow;
            loaded = 0;
            Reset();
        }

        public void SetOptions(String[] _options, Vector2 position, int _menuAlignment)
        {
            options = _options;
            optionsPosition = position;
            menuAlignment = _menuAlignment;
        }

        public virtual void Reset()
        {
            menuIndex = 0;
            selectedIndex = -1;
        }

        public void Update(InputState input, PlayerIndex[] controllingPlayer)
        {
            if (loaded < 2)
            {
                loaded++;
            }
            if((input.IsNewButtonPress(ButtonMappings.Pad_LeftStickUp, controllingPlayer[0], out controllingPlayer[1]) ||
                    input.IsNewKeyPress(ButtonMappings.Keyboard_LeftStickUp, controllingPlayer[0], out controllingPlayer[1])))
            {
                menuIndex--;
            }
            if ((input.IsNewButtonPress(ButtonMappings.Pad_LeftStickDown, controllingPlayer[0], out controllingPlayer[1]) ||
                    input.IsNewKeyPress(ButtonMappings.Keyboard_LeftStickDown, controllingPlayer[0], out controllingPlayer[1])))
            {
                menuIndex++;
            }

            menuIndex = (int)MathHelper.Clamp(menuIndex, 0, options.Length-1);

            if ((input.IsNewButtonPress(ButtonMappings.Pad_ABtn, controllingPlayer[0], out controllingPlayer[1]) ||
                    input.IsNewKeyPress(ButtonMappings.Keyboard_ABtn, controllingPlayer[0], out controllingPlayer[1])))
            {
                selectedIndex = menuIndex;
            }
            else
            {
                selectedIndex = -1;
            }
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            if (loaded > 1)
            {
                background.Draw(spriteBatch);
                DrawOptions(spriteBatch);
            }
            else
            {
                TextWriter.WriteText(spriteBatch, font, "LOADING, PLEASE WAIT", background.Position, menuColour, 1);
            }
            spriteBatch.End();
        }

        protected virtual void DrawOptions(SpriteBatch spriteBatch)
        {            
            for (int i = 0; i < options.Length; i++)
            {
                drawColour = menuColour;
                if (menuIndex == i)
                {

                    drawColour = selectedColour;
                }
                heightOffset = new Vector2(0, (font.MeasureString("A").Y * i));
                TextWriter.WriteText(spriteBatch, font, options[i], optionsPosition + heightOffset, drawColour, menuAlignment);
            }
        }
    }
}
