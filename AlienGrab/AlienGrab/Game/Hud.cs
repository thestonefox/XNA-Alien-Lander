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
    class Hud
    {
        protected Rectangle safeArea;
        protected int lives;
        protected int score;
        protected int fuel;
        protected int peeps;
        protected SpriteFont text;
        protected Sprite lifeIcon;
        protected Sprite peepIcon;
        protected Sprite bar;

        private Color color;
        private OptionsHolder gameOptions = OptionsHolder.Instance;

        public Hud(ContentManager content, Rectangle _safeArea)
        {
            safeArea = _safeArea;
            safeArea.Inflate(-40, -40);
            lives = 0;
            score = 0;
            fuel = 0;
            peeps = 0;
            text = content.Load<SpriteFont>("Fonts/OCR");
            lifeIcon = new Sprite(content.Load<Texture2D>("Sprites/lifeIcon"));
            peepIcon = new Sprite(content.Load<Texture2D>("Sprites/peepIcon"));
            bar = new Sprite(content.Load<Texture2D>("Sprites/pixel"));            
        }

        public void Update(GameTime gameTime, int _lives, int _score, int _fuel, int _peeps)
        {
            lives = _lives;
            score = _score;
            fuel = _fuel;
            peeps = _peeps;
        }

        public void Draw(SpriteBatch sb)
        {
            color = new Color(0.5f, 0.5f, 0.5f, 0.1f);
            TextWriter.WriteText(sb, text, "SCORE: " + score.ToString().PadLeft(gameOptions.ScorePadding, '0'), new Vector2(safeArea.Left, safeArea.Top), color, 0);

            //TextWriter.WriteText(sb, text, "" + gameOptions.IsTrial, new Vector2(safeArea.Left, safeArea.Top+40), color, 0);

            lifeIcon.Position = new Vector2(safeArea.Right-60, safeArea.Top+18);
            lifeIcon.Draw(sb);

            TextWriter.WriteText(sb, text, lives.ToString().PadLeft(2, '0'), new Vector2(safeArea.Right - 40, safeArea.Top), color, 0);

            Vector2 fontHeight = new Vector2(0, (text.MeasureString("A").Y * 1.0f));
            peepIcon.Position = new Vector2(safeArea.Right-60, safeArea.Top+28) + fontHeight;
            peepIcon.Draw(sb);
            TextWriter.WriteText(sb, text, peeps.ToString().PadLeft(2, '0'), new Vector2(safeArea.Right-40, safeArea.Top+12) + fontHeight, color, 0);

            DrawBar(sb, new Vector2(safeArea.Left+20, safeArea.Bottom-40), Color.Green, 32, safeArea.Width-40, 1000, fuel);

            TextWriter.WriteText(sb, text, "FUEL", new Vector2(safeArea.Width/2, safeArea.Bottom-44), Color.White, 0);
        }

        protected void DrawBar(SpriteBatch spriteBatch, Vector2 pos, Color col, int height, int maxWidth, int initialValue, int currentValue)
        {
            bar.Height = height;
            bar.Position = pos;
            bar.Alpha = 1.0f;

            bar.Width = maxWidth;
            bar.Colour = Color.Red;
            bar.Draw(spriteBatch);

            bar.Width = (float)((float)maxWidth / (float)initialValue) * (float)currentValue;
            bar.Colour = col;
            bar.Draw(spriteBatch);
        }

    }
}
