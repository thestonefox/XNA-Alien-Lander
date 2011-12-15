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
    class Player
    {
        private Texture2D graphic;
        private Rectangle graphicSource;
        private Vector2 position;
        private Vector2 center;
        private Vector2 velocity;
        private float scale;

        public Player()
        {
        }

        public void Reset(Vector2 _position)
        {
            position = _position;
            center = new Vector2(graphic.Width / 2, graphic.Height / 2);
            velocity = Vector2.Zero;
            scale = 0.5f;
        }

        public void LoadContent(ContentManager content)
        {
            graphic = content.Load<Texture2D>("ship");
            graphicSource = new Rectangle(0, 0, graphic.Width, graphic.Height);
        }

        public void Update(GameTime gameTime)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Left))
            {
                position.X -= 1;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.Right))
            {
                position.X += 1;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.Space))
            {
                position.Y -= 1;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.Enter))
            {
                position.Y += 1;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.Up))
            {
                scale -= 0.003f;                
                if (scale < 0.5f)
                {
                    scale = 0.5f;
                }
                else
                {
                    position.Y -= 0.5f;
                    position.X += 0.5f;
                }
            }
            if (Keyboard.GetState().IsKeyDown(Keys.Down))
            {
                scale += 0.003f;
                if (scale > 1.0f)
                {
                    scale = 1.0f;
                }
                else
                {
                    position.Y += 0.5f;
                    position.X -= 0.5f;
                }
            }
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(graphic, position, graphicSource, Color.White, 0.0f, center, scale, SpriteEffects.None, 1.0f);
        }
    }
}
