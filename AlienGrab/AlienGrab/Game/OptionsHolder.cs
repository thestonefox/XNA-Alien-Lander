using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AlienGrab
{
    public class OptionsHolder
    {
        private static OptionsHolder instance;
	    public int StartLives = 3;
	    public int StartFuel = 1000;
	    public int DecreaseFuel = 100;
        public int PowerupFuel = 100;
	    public int FuelMultiplier = 2;
	    public int StartPeeps = 1;
        public int StartPowerUps = 2;
        public int TrialLevel = 3;
	    public int PeepValue = 10;
	    public int IncPeeps = 1;
	    public int MaxPeeps = 16;
	    public float Thrust = 0.035f;
	    public float Gravity = 0.035f;
	    public float SafeVelocity = -0.8f;
	    public int ScorePadding = 10;
        public bool IsTrial = false;
        public int LifeAtScore = 100000;

        public float MusicVolumeAtPlay = 0.18f;
        public float MusicVolumeAtTransition = 0.18f;
        public float MusicVolumeAtPause = 0.025f;

        private OptionsHolder()
        {
        }

        public static OptionsHolder Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new OptionsHolder();
                }
                return instance;
            }
        }
    }
}
