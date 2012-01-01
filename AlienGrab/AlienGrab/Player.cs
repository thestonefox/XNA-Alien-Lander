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
    class Player:MovingObject
    {
        protected float thrust;
        protected float gravity;

        public Player(Game game, String assetName)
            : base(game, assetName)
        {
            thrust = 0.025f;
            gravity = 0.035f;
        }

        public void Move()
        {
            Rotation.Y += MathHelper.ToRadians(5.0f);
            if (Keyboard.GetState().IsKeyDown(Keys.Left) 
                && (hasPlayArea==false || (hasPlayArea==true && bounds.Max.X>playArea.Min.X) ))
            {
                velocity.X = -1;
                acceleration += accelerationBit;
            }
            else if (Keyboard.GetState().IsKeyDown(Keys.Right) 
                && (hasPlayArea == false || (hasPlayArea == true && bounds.Min.X < playArea.Max.X)))
            {
                velocity.X = 1;
                acceleration += accelerationBit;
            }
            else
            {
                velocity.X = 0;
            }

            if (Keyboard.GetState().IsKeyDown(Keys.Up)
                && (hasPlayArea == false || (hasPlayArea == true && bounds.Max.Z > playArea.Min.Z)))
            {
                velocity.Z = -1;
                acceleration += accelerationBit;
            }
            else if (Keyboard.GetState().IsKeyDown(Keys.Down)
                && (hasPlayArea == false || (hasPlayArea == true && bounds.Min.Z < playArea.Max.Z)))
            {
                velocity.Z = 1;
                acceleration += accelerationBit;
            }
            else
            {
                velocity.Z = 0;
            }

            if (Keyboard.GetState().IsKeyDown(Keys.Space)
                && (hasPlayArea == false || (hasPlayArea == true && bounds.Min.Y < playArea.Max.Y)))
            {
                velocity.Y += thrust;
                acceleration += accelerationBit;
            }
            else if (hasPlayArea == false || (hasPlayArea == true && bounds.Max.Y > playArea.Min.Y))
            {
                velocity.Y -= gravity;
                acceleration += accelerationBit;
            }
            velocity.Y = MathHelper.Clamp(velocity.Y, -1.0f, 1.0f);
        }
    }
}
