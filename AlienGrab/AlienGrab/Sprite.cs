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
    class Sprite
    {        
        public float Depth;
        public Vector2 Position;        
        public float Rotation;
        public float Scale;
        public Color Colour;
        public float Alpha;
        public float Width;
        public float Height;

        protected Vector2 center;        
        protected Texture2D texture;
        protected bool alive;

        private Rectangle source;

        public Sprite(Texture2D _texture)
        {
            Initialise(_texture, _texture.Width, _texture.Height);
        }

        private void Initialise(Texture2D _texture, float _width, float _height)
        {
            texture = _texture;
            alive = true;
            Width = _width;
            Height = _height;
            Position = Vector2.Zero;
            Colour = Color.White;
            Alpha = 1.0f;
            Rotation = 0.0f;
            Scale = 1.0f;
            Depth = 0.0f;
            center = new Vector2(Width / 2, Height / 2);
            Rotation = MathHelper.ToRadians(0);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (alive)
            {
                source = new Rectangle(0, 0, (int)Width, (int)Height);
                spriteBatch.Draw(texture, Position, source, new Color(Colour.R, Colour.G, Colour.B, Alpha), Rotation, center, Scale, SpriteEffects.None, Depth);
            }
        }
    }
}
