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
    public class LightSource
    {
        public Vector3 LightDirection;
        protected BaseCamera camera;

        private Matrix lightRotation;
        private Vector3[] frustumCorners;
        private BoundingBox lightBox;
        private Vector3 boxSize;
        private Vector3 halfBoxSize;
        private Vector3 lightPosition;
        private Matrix lightView;
        private Matrix lightProjection;

        public LightSource(Vector3 _lightDirection, BaseCamera _camera)
        {
            LightDirection = _lightDirection;
            camera = _camera;
        }

        public Matrix CreateLightViewProjectionMatrix()
        {
            // Matrix with that will rotate in points the direction of the light
            lightRotation = Matrix.CreateLookAt(Vector3.Zero, -LightDirection, Vector3.Up);

            // Get the corners of the frustum
            frustumCorners = camera.cameraFrustum.GetCorners();

            // Transform the positions of the corners into the direction of the light
            for (int i = 0; i < frustumCorners.Length; i++)
            {
                frustumCorners[i] = Vector3.Transform(frustumCorners[i], lightRotation);
            }

            // Find the smallest box around the points
            lightBox = BoundingBox.CreateFromPoints(frustumCorners);

            boxSize = lightBox.Max - lightBox.Min;
            halfBoxSize = boxSize * 0.5f;

            // The position of the light should be in the center of the back
            // pannel of the box. 
            lightPosition = lightBox.Min + halfBoxSize;
            lightPosition.Z = lightBox.Min.Z;

            // We need the position back in world coordinates so we transform 
            // the light position by the inverse of the lights rotation
            lightPosition = Vector3.Transform(lightPosition,
                                              Matrix.Invert(lightRotation));

            // Create the view matrix for the light
            lightView = Matrix.CreateLookAt(lightPosition, lightPosition - LightDirection, Vector3.Up);

            // Create the projection matrix for the light
            // The projection is orthographic since we are using a directional light
            lightProjection = Matrix.CreateOrthographic(boxSize.X, boxSize.Y, -boxSize.Z, boxSize.Z);

            return lightView * lightProjection;
        }

    }
}
