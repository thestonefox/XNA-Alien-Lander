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
        protected ParticleLibrary particleLibrary;
        protected int deathTimer;
        protected int deathCounter;
        protected Vector3 startPosition;
        protected Vector3 diePosition;

        public int Lives;
        public int Fuel;
        public int Score; 
        public Player(Game game, String assetName, LightSource light, Vector3 _startPosition)
            : base(game, assetName, light)
        {
            thrust = 0.035f;
            gravity = 0.035f;
            particleLibrary = null;
            deathTimer = 50;
            Lives = 3;
            Score = 0;
            startPosition = _startPosition;
            ResetPlayer();
        }

        public void AttachParticleLibrary(ParticleLibrary _particleLibrary)
        {
            particleLibrary = _particleLibrary;
        }

        protected void GenerateThrustParticles()
        {
            // Create a number of fire particles, randomly positioned around a circle.
            for (int i = 0; i < 20; i++)
            {
                particleLibrary.EnergyParticles.AddParticle(Position + particleLibrary.GenerateCircle(), Vector3.Zero);
            }
        }

        public void Die()
        {
            deathCounter = deathTimer;
            diePosition = Position;
            Lives--;
            Position = new Vector3(-5000,-5000,-5000);                
        }

        public bool SafeDescent()
        {
            if (velocity.Y < -0.8f)
            {
                return false;
            }
            return true;
        }

        public override void Update(GameTime gameTime)
        {
            if (deathCounter == deathTimer - 10)
            {
                Active = false;
            }
            if (deathCounter > 1)
            {
                particleLibrary.ExplosionParticles.AddParticle(diePosition, Vector3.Zero);
                particleLibrary.ExplosionSmokeParticles.AddParticle(diePosition, Vector3.Zero);
                deathCounter--;
            }

            if (deathCounter == 1)
            {
                ResetPlayer();
            }
            base.Update(gameTime);
        }

        protected void ResetPlayer()
        {            
            deathCounter = 0;
            Fuel = 1000;
            Position = startPosition;
            Active = true;
        }

        public void Move(InputState input, PlayerIndex[] controllingPlayer)
        {
            if (Active == true)
            {
                Rotation.Y += MathHelper.ToRadians(5.0f);
                if (
                    (input.IsNewButtonHeld(Buttons.LeftThumbstickLeft, controllingPlayer[0], out controllingPlayer[1]) ||
                    input.IsNewKeyHeld(Keys.Left, controllingPlayer[0], out controllingPlayer[1]))
                    && (hasPlayArea == false || (hasPlayArea == true && Bounds.Max.X > playArea.Min.X))
                    )
                {
                    velocity.X = -1;
                    acceleration.X += accelerationBit;
                }
                else if (
                    (input.IsNewButtonHeld(Buttons.LeftThumbstickRight, controllingPlayer[0], out controllingPlayer[1]) ||
                    input.IsNewKeyHeld(Keys.Right, controllingPlayer[0], out controllingPlayer[1]))
                    && (hasPlayArea == false || (hasPlayArea == true && Bounds.Min.X < playArea.Max.X))
                    )
                {
                    velocity.X = 1;
                    acceleration.X += accelerationBit;
                }
                else
                {
                    acceleration.X -= accelerationBit;
                }

                if (
                    (input.IsNewButtonHeld(Buttons.LeftThumbstickUp, controllingPlayer[0], out controllingPlayer[1]) ||
                    input.IsNewKeyHeld(Keys.Up, controllingPlayer[0], out controllingPlayer[1]))
                    && (hasPlayArea == false || (hasPlayArea == true && Bounds.Max.Z > playArea.Min.Z))
                    )
                {
                    velocity.Z = -1;
                    acceleration.Z += accelerationBit;
                }
                else if (
                    (input.IsNewButtonHeld(Buttons.LeftThumbstickDown, controllingPlayer[0], out controllingPlayer[1]) ||
                    input.IsNewKeyHeld(Keys.Down, controllingPlayer[0], out controllingPlayer[1]))
                    && (hasPlayArea == false || (hasPlayArea == true && Bounds.Min.Z < playArea.Max.Z))
                    )
                {
                    velocity.Z = 1;
                    acceleration.Z += accelerationBit;
                }
                else
                {
                    acceleration.Z -= accelerationBit;
                }

                if (Fuel>0 && 
                    ((input.IsNewButtonHeld(Buttons.RightTrigger, controllingPlayer[0], out controllingPlayer[1]) ||
                    input.IsNewButtonHeld(Buttons.A, controllingPlayer[0], out controllingPlayer[1]) ||
                    input.IsNewKeyHeld(Keys.Space, controllingPlayer[0], out controllingPlayer[1]))
                    && (hasPlayArea == false || (hasPlayArea == true && Bounds.Min.Y < playArea.Max.Y)))
                    )
                {
                    GenerateThrustParticles();
                    velocity.Y += thrust;
                    Fuel--;
                    acceleration.Y += accelerationBit;
                }
                else if ((hasPlayArea == false || (hasPlayArea == true && Bounds.Max.Y > playArea.Min.Y)))
                {
                    velocity.Y -= gravity;                    
                    acceleration.Y += accelerationBit;                    
                }
                else
                {
                    velocity.Y = 0;
                }                
                velocity.Y = MathHelper.Clamp(velocity.Y, -1.0f, 1.0f);
            }
        }
    }
}
