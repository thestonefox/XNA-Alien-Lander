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
    class BaseScreen
    {
        protected Sprite background;
        protected List<String> options;
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
        private SoundPlayer soundPlayer;

        private Sprite overlay;
        private float overlayAlpha;
        private float originalFadeSpeed;
        protected float fadeSpeed;

        public BaseScreen(ContentManager content, String assetName, String fontName)
        {
            background = new Sprite(content.Load<Texture2D>(assetName));
            background.Position = new Vector2(background.Width/2, background.Height/2);
            font = content.Load<SpriteFont>(fontName);
            overlay = new Sprite(content.Load<Texture2D>("Sprites/pixel"));
            overlay.Width = background.Width;
            overlay.Height= background.Height;
            overlay.Position = Vector2.Zero;
            overlay.Colour = Color.Black;
            overlayAlpha = 1.0f;
            fadeSpeed = 1.0f;
            originalFadeSpeed = fadeSpeed;
            soundPlayer = new SoundPlayer(content);
            soundPlayer.AddSound("Select", "Audio\\Effects\\select", false);
            options = new List<String>();  
            menuAlignment = 0;
            menuColour = Color.White;
            selectedColour = Color.Blue;
            loaded = 0;
            Reset();
        }

        protected void SetTransition(Color color, float speed)
        {
            overlay.Colour = color;
            fadeSpeed = speed;
            originalFadeSpeed = fadeSpeed;
        }

        protected void SetOptions(Vector2 position, int _menuAlignment)
        {
            optionsPosition = position;
            menuAlignment = _menuAlignment;
        }

        public virtual void Reset()
        {
            menuIndex = 0;
            selectedIndex = -1;
            fadeSpeed = originalFadeSpeed;
            overlay.Alpha = 1.0f;
            overlayAlpha = 1.0f;
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

            menuIndex = (int)MathHelper.Clamp(menuIndex, 0, options.Count-1);

            if ((input.IsNewButtonPress(ButtonMappings.Pad_ABtn, controllingPlayer[0], out controllingPlayer[1]) ||
                    input.IsNewKeyPress(ButtonMappings.Keyboard_ABtn, controllingPlayer[0], out controllingPlayer[1])))
            {
                selectedIndex = menuIndex;
                soundPlayer.PlaySound("Select");
            }
            else
            {
                selectedIndex = -1;
            }
            if (overlayAlpha > 0.0f)
            {
                overlayAlpha -= fadeSpeed;                
                overlay.Alpha = overlayAlpha;
            }
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend);
            if (loaded > 1)
            {
                background.Draw(spriteBatch);
                DrawOptions(spriteBatch);
            }
            else
            {
                TextWriter.WriteText(spriteBatch, font, "LOADING, PLEASE WAIT", background.Position, menuColour, 1);
            }            
            overlay.Draw(spriteBatch);
            spriteBatch.End();
        }

        protected virtual void DrawOptions(SpriteBatch spriteBatch)
        {
            for (int i = 0; i < options.Count; i++)
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
