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
    class MovingObject : Base3DObject
    {
        protected Vector3 velocity;
        protected Vector3 activeVelocity;
        protected Vector3 acceleration;
        protected float accelerationBit;
        protected BoundingBox playArea;
        protected bool hasPlayArea;

        public MovingObject(Game game, String assetName, LightSource _light)
            : base(game, assetName, _light)
        {
            velocity = Vector3.Zero;
            acceleration = Vector3.Zero;
            accelerationBit = 0.25f;
            playArea = new BoundingBox();
            hasPlayArea = false;
        }

        public void SetPlayArea(BoundingBox _playArea)
        {
            playArea = _playArea;
            hasPlayArea = true;
        }

        public override void Update(GameTime gameTime)
        {
            if (Active == true)
            {
                acceleration.X = MathHelper.Clamp(acceleration.X, 0.0f, 3.0f);
                acceleration.Y = MathHelper.Clamp(acceleration.Y, 0.0f, 3.0f);
                acceleration.Z = MathHelper.Clamp(acceleration.Z, 0.0f, 3.0f);
                if (!HitTest)
                {
                    if (velocity != Vector3.Zero && acceleration != Vector3.Zero)
                    {
                        activeVelocity = velocity * acceleration;
                    }
                    Position += velocity * acceleration;
                }
                else
                {
                    Position -= activeVelocity;
                    velocity = Vector3.Zero;
                }

                base.Update(gameTime);
            }
        }
    }

}
