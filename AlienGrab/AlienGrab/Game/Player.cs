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
        protected int startFuel;

        public int Lives;        
        public int Fuel;
        public int Score; 

        private OptionsHolder gameOptions = OptionsHolder.Instance;
        public Player(Game game, String assetName, LightSource light, Vector3 _startPosition, int _startFuel, int _score, int _lives)
            : base(game, assetName, light)
        {
            thrust = gameOptions.Thrust;
            gravity = gameOptions.Gravity;
            particleLibrary = null;
            deathTimer = 50;
            Lives = _lives;
            Score = _score;
            startPosition = _startPosition;
            startFuel = _startFuel;
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
            Position = new Vector3(-50000,-50000,-50000);                
        }

        public bool SafeDescent()
        {
            if (velocity.Y < gameOptions.SafeVelocity)
            {
                return false;
            }
            return true;
        }

        public override void Update(GameTime gameTime)
        {
            if (Fuel > startFuel)
            {
                Fuel = startFuel;
            }
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
            Fuel = startFuel;
            Position = startPosition;
            Active = true;
        }

        public void Move(InputState input, PlayerIndex[] controllingPlayer)
        {
            if (Active == true)
            {
                Rotation.Y += MathHelper.ToRadians(5.0f);
                if (
                    (input.IsNewButtonHeld(ButtonMappings.Pad_LeftStickLeft, controllingPlayer[0], out controllingPlayer[1]) ||
                    input.IsNewKeyHeld(ButtonMappings.Keyboard_LeftStickLeft, controllingPlayer[0], out controllingPlayer[1]))
                    && (hasPlayArea == false || (hasPlayArea == true && Bounds.Max.X > playArea.Min.X))
                    )
                {
                    velocity.X = -1;
                    acceleration.X += accelerationBit;
                }
                else if (
                    (input.IsNewButtonHeld(ButtonMappings.Pad_LeftStickRight, controllingPlayer[0], out controllingPlayer[1]) ||
                    input.IsNewKeyHeld(ButtonMappings.Keyboard_LeftStickRight, controllingPlayer[0], out controllingPlayer[1]))
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
                    (input.IsNewButtonHeld(ButtonMappings.Pad_LeftStickUp, controllingPlayer[0], out controllingPlayer[1]) ||
                    input.IsNewKeyHeld(ButtonMappings.Keyboard_LeftStickUp, controllingPlayer[0], out controllingPlayer[1]))
                    && (hasPlayArea == false || (hasPlayArea == true && Bounds.Max.Z > playArea.Min.Z))
                    )
                {
                    velocity.Z = -1;
                    acceleration.Z += accelerationBit;
                }
                else if (
                    (input.IsNewButtonHeld(ButtonMappings.Pad_LeftStickDown, controllingPlayer[0], out controllingPlayer[1]) ||
                    input.IsNewKeyHeld(ButtonMappings.Keyboard_LeftStickDown, controllingPlayer[0], out controllingPlayer[1]))
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
                    ((input.IsNewButtonHeld(ButtonMappings.Pad_RightTrigger, controllingPlayer[0], out controllingPlayer[1]) ||
                    input.IsNewButtonHeld(ButtonMappings.Pad_ABtn, controllingPlayer[0], out controllingPlayer[1]) ||
                    input.IsNewKeyHeld(ButtonMappings.Keyboard_ABtn, controllingPlayer[0], out controllingPlayer[1]))
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
