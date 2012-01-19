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
    class TextWriter
    {
        private static Vector2 FontOrigin;

        public static void WriteText(SpriteBatch sb, SpriteFont font, String text, Vector2 position, Color color, int align)
        {
            FontOrigin = Vector2.Zero;
            if (align == 1)
                FontOrigin = font.MeasureString(text) / 2;
            if (align == 2)
                FontOrigin = font.MeasureString(text);
            sb.DrawString(font, text, position, color, 0, FontOrigin, 1.0f, SpriteEffects.None, 0.5f);
        }
    }
}
