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
    class BaseCamera
    {
        public Vector3 position;
        public Vector3 view;
        protected float nearPlane;
        protected float farPlane;
        protected float aspectRatio;

        public BaseCamera(float _aspectRatio, float _nearPlane, float _farPlane)
        {
            aspectRatio = _aspectRatio;
            nearPlane = _nearPlane;
            farPlane = _farPlane;
            position = Vector3.Zero;
            view = Vector3.Zero;
        }

        public Matrix GetViewMatrix()
        {
            return Matrix.CreateLookAt(position, view, Vector3.Up);
        }

        public Matrix GetProjectionMatrix()
        {
            return Matrix.CreatePerspectiveFieldOfView(MathHelper.ToRadians(45.0f), aspectRatio, nearPlane, farPlane);
        }
    }
}
