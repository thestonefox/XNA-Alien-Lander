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

        public Player(Game game, String assetName, LightSource light)
            : base(game, assetName, light)
        {
            thrust = 0.025f;
            gravity = 0.035f;
        }

        public void Move(InputState input, PlayerIndex[] controllingPlayer)
        {
            Rotation.Y += MathHelper.ToRadians(5.0f);
            if (
                (input.IsNewButtonHeld(Buttons.LeftThumbstickLeft, controllingPlayer[0], out controllingPlayer[1]) ||
                input.IsNewKeyHeld(Keys.Left, controllingPlayer[0], out controllingPlayer[1]))
                && (hasPlayArea == false || (hasPlayArea == true && Bounds.Max.X > playArea.Min.X))
                )
            {
                velocity.X = -1;
                acceleration += accelerationBit;
            }
            else if (
                (input.IsNewButtonHeld(Buttons.LeftThumbstickRight, controllingPlayer[0], out controllingPlayer[1]) ||
                input.IsNewKeyHeld(Keys.Right, controllingPlayer[0], out controllingPlayer[1]))
                && (hasPlayArea == false || (hasPlayArea == true && Bounds.Min.X < playArea.Max.X))
                ) 
            {
                velocity.X = 1;
                acceleration += accelerationBit;
            }
            else
            {
                velocity.X = 0;
            }

            if (
                (input.IsNewButtonHeld(Buttons.LeftThumbstickUp, controllingPlayer[0], out controllingPlayer[1]) ||
                input.IsNewKeyHeld(Keys.Up, controllingPlayer[0], out controllingPlayer[1]))
                && (hasPlayArea == false || (hasPlayArea == true && Bounds.Max.Z > playArea.Min.Z))
                )
            {
                velocity.Z = -1;
                acceleration += accelerationBit;
            }
            else if (
                (input.IsNewButtonHeld(Buttons.LeftThumbstickDown, controllingPlayer[0], out controllingPlayer[1]) ||
                input.IsNewKeyHeld(Keys.Down, controllingPlayer[0], out controllingPlayer[1]))
                && (hasPlayArea == false || (hasPlayArea == true && Bounds.Min.Z < playArea.Max.Z))
                ) 
            {
                velocity.Z = 1;
                acceleration += accelerationBit;
            }
            else
            {
                velocity.Z = 0;
            }

            if (
                (input.IsNewButtonHeld(Buttons.RightTrigger, controllingPlayer[0], out controllingPlayer[1]) ||
                input.IsNewButtonHeld(Buttons.A, controllingPlayer[0], out controllingPlayer[1]) ||
                input.IsNewKeyHeld(Keys.Space, controllingPlayer[0], out controllingPlayer[1]))
                && (hasPlayArea == false || (hasPlayArea == true && Bounds.Min.Y < playArea.Max.Y))
                )
            {
                velocity.Y += thrust;
                acceleration += accelerationBit;
            }
            else if ((hasPlayArea == false || (hasPlayArea == true && Bounds.Max.Y > playArea.Min.Y)))
            {
                velocity.Y -= gravity;
                acceleration += accelerationBit;
            }
            else
            {
                velocity.Y = 0;
            }
            velocity.Y = MathHelper.Clamp(velocity.Y, -1.0f, 1.0f);
        }
    }
}
