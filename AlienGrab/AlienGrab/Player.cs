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
    class Player:Base3DObject
    {
        protected Vector3 velocity;
        protected float acceleration;
        protected float accelerationBit;

        public Vector3 lastPosition;

        public Player(Game game, String assetName) :base (game, assetName)
        {
            velocity = Vector3.Zero;
            acceleration = 0.0f;
            accelerationBit = 0.2f;
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            acceleration = MathHelper.Clamp(acceleration, 0.0f, 3.0f);

            bool playerInput = false;
            if (Keyboard.GetState().IsKeyDown(Keys.Left))
            {
                velocity.X = -1;
                acceleration += accelerationBit;
                playerInput = true;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.Right))
            {
                velocity.X = 1;
                acceleration += accelerationBit;
                playerInput = true;
            }

            if (Keyboard.GetState().IsKeyDown(Keys.Up))
            {
                velocity.Z = -1;
                acceleration += accelerationBit;
                playerInput = true;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.Down))
            {
                velocity.Z = 1;
                acceleration += accelerationBit;
                playerInput = true;
            }

            if (Keyboard.GetState().IsKeyDown(Keys.Space))
            {
                velocity.Y = 1;
                acceleration += accelerationBit;
                playerInput = true;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.Enter))
            {
                velocity.Y = -1;
                acceleration += accelerationBit;
                playerInput = true;
            }

            Position += velocity * acceleration;

            if (playerInput == false)
            {
                acceleration -= accelerationBit;
                velocity = Vector3.Zero;
            }
        }

    }
}
