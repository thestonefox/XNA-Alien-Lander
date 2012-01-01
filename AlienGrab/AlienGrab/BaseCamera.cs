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
    public class BaseCamera
    {
        public Vector3 Position;
        public Vector3 View;
        public BoundingFrustum cameraFrustum;
        protected float nearPlane;
        protected float farPlane;
        protected float aspectRatio;

        public BaseCamera(float _aspectRatio, float _nearPlane, float _farPlane)
        {
            aspectRatio = _aspectRatio;
            nearPlane = _nearPlane;
            farPlane = _farPlane;
            Position = Vector3.Zero;
            View = Vector3.Zero;
            cameraFrustum = new BoundingFrustum(Matrix.Identity);
        }

        public Matrix GetViewMatrix()
        {
            return Matrix.CreateLookAt(Position, View, Vector3.Up);
        }

        public Matrix GetProjectionMatrix()
        {
            return Matrix.CreatePerspectiveFieldOfView(MathHelper.ToRadians(45.0f), aspectRatio, nearPlane, farPlane);
        }
    }
}
