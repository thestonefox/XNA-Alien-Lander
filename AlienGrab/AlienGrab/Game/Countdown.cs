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
    class Countdown
    {
        protected Rectangle safeArea;
        protected SpriteFont text;
        protected Sprite bar;
        private String[] counter;
        private int counterIndex;
        private OptionsHolder gameOptions = OptionsHolder.Instance;

        public Countdown(ContentManager content, Rectangle _safeArea)
        {
            safeArea = _safeArea;
            counter = new String[] { "Ready!", "3!", "2!", "1!", "Go"};
            text = content.Load<SpriteFont>("Fonts/OCRLarge");
            bar = new Sprite(content.Load<Texture2D>("Sprites/pixel"));
        }

        public void Update(GameTime gameTime, int _index)
        {
            counterIndex = _index;
        }

        public void Draw(SpriteBatch sb)
        {            
            DrawBar(sb, new Vector2(safeArea.Left, safeArea.Top), Color.Green, safeArea.Height, safeArea.Width);
            TextWriter.WriteText(sb, text, counter[counterIndex], new Vector2(safeArea.Left + ((safeArea.Width / 2)-112), safeArea.Top + ((safeArea.Height / 2)-128)), new Color(1.0f, 1.0f, 0.0f, 1.0f), 0);
        }

        protected void DrawBar(SpriteBatch spriteBatch, Vector2 pos, Color col, int height, int maxWidth)
        {
            bar.Height = height;
            bar.Position = pos;
            bar.Alpha = 0.8f;
            bar.Width = maxWidth;
            bar.Colour = col;
            bar.Draw(spriteBatch);
        }

    }
}
