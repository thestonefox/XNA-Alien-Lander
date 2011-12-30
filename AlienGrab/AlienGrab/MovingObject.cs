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
        protected float acceleration;
        protected float accelerationBit;

        public MovingObject(Game game, String assetName)
            : base(game, assetName)
        {
            velocity = Vector3.Zero;
            acceleration = 0.0f;
            accelerationBit = 0.05f;
        }

        public override void Update(GameTime gameTime)
        {            
            acceleration = MathHelper.Clamp(acceleration, 0.0f, 3.0f);
            if (!hitTest)
            {
                if (velocity != Vector3.Zero && acceleration != 0)
                {
                    activeVelocity = velocity * acceleration;                    
                }
                Position += velocity * acceleration;
            }
            else
            {
                Position -= activeVelocity;
            }
            base.Update(gameTime);
        }
    }

}
