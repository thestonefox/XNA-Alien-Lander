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
    class GameCamera : BaseCamera
    {
        public float Speed;
        protected Vector3[] viewLimits;
        protected Vector3[] positionLimits;

        public GameCamera(float _speed, Vector3[] _viewLimits, Vector3[] _positionLimits, float _aspectRatio, float _nearPlane, float _farPlane, Vector3 _startPosition, Vector3 _startView)
            : base(_aspectRatio, _nearPlane, _farPlane, _startPosition, _startView)
        {
            Speed = _speed;
            viewLimits = _viewLimits;
            positionLimits = _positionLimits;
        }

        public void Move(InputState input, PlayerIndex[] controllingPlayer)
        {
            if (input.IsNewButtonHeld(Buttons.RightThumbstickLeft, controllingPlayer[0], out controllingPlayer[1]) ||
                input.IsNewKeyHeld(Keys.NumPad4, controllingPlayer[0], out controllingPlayer[1]))
            {
                if (Position.Z > 130)
                {
                    Position.X -= Speed;
                    View.X += Speed;
                }
                else
                {
                    Position.X += Speed;
                    View.X -= Speed;
                }
            }
            if (input.IsNewButtonHeld(Buttons.RightThumbstickRight, controllingPlayer[0], out controllingPlayer[1]) ||
                input.IsNewKeyHeld(Keys.NumPad6, controllingPlayer[0], out controllingPlayer[1]))
            {
                if (Position.Z <= 130)
                {
                    Position.X -= Speed;
                    View.X += Speed;
                }
                else
                {
                    Position.X += Speed;
                    View.X -= Speed;
                }
            }
            if (input.IsNewButtonHeld(Buttons.RightThumbstickUp, controllingPlayer[0], out controllingPlayer[1]) ||
                input.IsNewKeyHeld(Keys.NumPad8, controllingPlayer[0], out controllingPlayer[1]))
            {
                Position.Y += Speed;
                View.Y -= Speed;
            }
            if (input.IsNewButtonHeld(Buttons.RightThumbstickDown, controllingPlayer[0], out controllingPlayer[1]) ||
                input.IsNewKeyHeld(Keys.NumPad2, controllingPlayer[0], out controllingPlayer[1]))
            {
                Position.Y -= Speed;
                View.Y += Speed;
            }
            if (input.IsNewButtonHeld(Buttons.LeftShoulder, controllingPlayer[0], out controllingPlayer[1]) ||
                input.IsNewKeyHeld(Keys.NumPad7, controllingPlayer[0], out controllingPlayer[1]))
            {
                Position.Z += Speed;
                View.Z -= Speed;
            }
            if (input.IsNewButtonHeld(Buttons.RightShoulder, controllingPlayer[0], out controllingPlayer[1]) ||
                input.IsNewKeyHeld(Keys.NumPad9, controllingPlayer[0], out controllingPlayer[1]))
            {
                Position.Z -= Speed;
                View.Z += Speed;
            }
            if (input.IsNewButtonHeld(Buttons.RightStick, controllingPlayer[0], out controllingPlayer[1]) ||
                input.IsNewKeyHeld(Keys.NumPad0, controllingPlayer[0], out controllingPlayer[1]))
            {
                ResetCamera();
            }

            Position.X = MathHelper.Clamp(Position.X, positionLimits[0].X, positionLimits[1].X);
            View.X = MathHelper.Clamp(View.X, viewLimits[0].X, viewLimits[1].X);
            Position.Y = MathHelper.Clamp(Position.Y, positionLimits[0].Y, positionLimits[1].Y);
            View.Y = MathHelper.Clamp(View.Y, viewLimits[0].Y, viewLimits[1].Y);
            Position.Z = MathHelper.Clamp(Position.Z, positionLimits[0].Z, positionLimits[1].Z);
            View.Z = MathHelper.Clamp(View.Z, viewLimits[0].Z, viewLimits[1].Z);

            cameraFrustum.Matrix = GetViewMatrix() * GetProjectionMatrix();
        }
    }
}
