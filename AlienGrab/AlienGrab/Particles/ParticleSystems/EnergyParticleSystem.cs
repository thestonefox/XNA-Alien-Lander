#region File Description
//-----------------------------------------------------------------------------
// FireParticleSystem.cs
//
// Microsoft XNA Community Game Platform
// Copyright (C) Microsoft Corporation. All rights reserved.
//-----------------------------------------------------------------------------
#endregion

#region Using Statements
using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
#endregion

namespace AlienGrab
{
    /// <summary>
    /// Custom particle system for creating a flame effect.
    /// </summary>
    class EnergyParticleSystem : ParticleSystem
    {
        public EnergyParticleSystem(Game game, ContentManager content)
            : base(game, content)
        { }


        protected override void InitializeSettings(ParticleSettings settings)
        {
            settings.TextureName = "Textures/energy";

            settings.MaxParticles = 2400;

            settings.Duration = TimeSpan.FromSeconds(1);

            settings.DurationRandomness = 1;

            settings.MinHorizontalVelocity = 0;
            settings.MaxHorizontalVelocity = 15;

            settings.MinVerticalVelocity = -10;
            settings.MaxVerticalVelocity = 10;

            // Set gravity upside down, so the flames will 'fall' upward.
            settings.Gravity = new Vector3(0, -10, 0);

            settings.MinColor = new Color(255, 255, 255, 60);
            settings.MaxColor = new Color(255, 255, 255, 80);

            settings.MinStartSize = 15;
            settings.MaxStartSize = 25;

            settings.MinEndSize = 25;
            settings.MaxEndSize = 35;

            // Use additive blending.
            settings.BlendState = BlendState.Additive;
        }
    }
}
