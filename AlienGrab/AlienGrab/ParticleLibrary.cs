using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace AlienGrab
{
    public class ParticleLibrary
    {
        public ParticleSystem ExplosionParticles;
        public ParticleSystem ExplosionSmokeParticles;
        public ParticleSystem EnergyParticles;
        protected Random random;

        public ParticleLibrary()
        {
            random = new Random();
        }

        public void Draw(BaseCamera camera)
        {
            ExplosionParticles.SetCamera(camera.GetViewMatrix(), camera.GetProjectionMatrix());
            ExplosionSmokeParticles.SetCamera(camera.GetViewMatrix(), camera.GetProjectionMatrix());
            EnergyParticles.SetCamera(camera.GetViewMatrix(), camera.GetProjectionMatrix());
        }

        public Vector3 GenerateCircle()
        {
            const float radius = 10;
            const float height = 0;

            double angle = random.NextDouble() * Math.PI * 2;

            float x = (float)Math.Cos(angle);
            float y = (float)Math.Sin(angle);

            return new Vector3(x * radius, 0, y * radius + height);
        }
    }
}
