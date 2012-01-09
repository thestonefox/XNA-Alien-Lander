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
        protected Vector3 startPosition;
        protected Vector3 startView;
        public BoundingFrustum cameraFrustum;
        protected float nearPlane;
        protected float farPlane;
        protected float aspectRatio;

        public BaseCamera(float _aspectRatio, float _nearPlane, float _farPlane, Vector3 _startPosition, Vector3 _startView)
        {
            aspectRatio = _aspectRatio;
            nearPlane = _nearPlane;
            farPlane = _farPlane;
            startPosition = _startPosition;
            startView = _startView;
            cameraFrustum = new BoundingFrustum(Matrix.Identity);
            ResetCamera();
        }

        public void ResetCamera()
        {
            Position = startPosition;
            View = startView;
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
