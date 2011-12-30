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
            gravity = -1.0f;
        }

        public void Move()
        {
            bool playerInput = false;

            if (Keyboard.GetState().IsKeyDown(Keys.Left))
            {
                velocity.X = -1;
                acceleration += accelerationBit;
                playerInput = true;
            }
            else if (Keyboard.GetState().IsKeyDown(Keys.Right))
            {
                velocity.X = 1;
                acceleration += accelerationBit;
                playerInput = true;
            }
            else
            {
                velocity.X = 0;
            }

            if (Keyboard.GetState().IsKeyDown(Keys.Up))
            {
                velocity.Z = -1;
                acceleration += accelerationBit;
                playerInput = true;
            }
            else if (Keyboard.GetState().IsKeyDown(Keys.Down))
            {
                velocity.Z = 1;
                acceleration += accelerationBit;
                playerInput = true;
            }
            else
            {
                velocity.Z = 0;
            }

            if (Keyboard.GetState().IsKeyDown(Keys.Space))
            {
                velocity.Y += thrust;
                if (velocity.Y > 1.0f) velocity.Y = 1.0f;
                acceleration += accelerationBit;
                playerInput = true;
            }
            else
            {
                velocity.Y -= thrust;
                if (velocity.Y < gravity) velocity.Y = gravity;
                acceleration += accelerationBit;
            }

            if (playerInput == false)
            {
                //acceleration -= accelerationBit;
                //velocity = Vector3.Zero;
            }
        }
    }
}
