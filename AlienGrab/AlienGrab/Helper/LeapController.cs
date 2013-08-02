using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LeapLibrary;

namespace AlienGrab.Helper
{
    public sealed class LeapController
    {
        private static volatile LeapController instance;
        private static object syncRoot = new Object();

        private LeapController() { }

        private LeapComponet leap;
        private float deadzone = 15;

        public void Init(LeapComponet _leap) {
            leap = _leap;
        }

        public LeapComponet Get()
        {
            return leap;
        }

        public float XAxis()
        {
            if (leap.FirstHand != null)
            {
                return leap.handSpace.X - leap.handBaseline.X;
            }
            return 0.0f;
        }

        public float YAxis()
        {
            if (leap.FirstHand != null)
            {
                return leap.handSpace.Y - leap.handBaseline.Y;
            }
            return 0.0f;
        }

        public float ZAxis()
        {
            if (leap.FirstHand != null)
            {
                return leap.handSpace.Z - leap.handBaseline.Z;
            }
            return 0.0f;
        }

        public bool Right()
        {
            if (XAxis() < (leap.handBaseline.X - deadzone))
            {
                return true;
            }
            return false;
        }

        public bool Left()
        {
            if (XAxis() > (leap.handBaseline.X + deadzone))
            {
                return true;
            }
            return false;
        }

        public bool Up()
        {
            if (YAxis() > (leap.handBaseline.Y + deadzone))
            {
                return true;
            }
            return false;
        }

        public bool Down()
        {
            if (YAxis() < (leap.handBaseline.Y - deadzone))
            {
                return true;
            }
            return false;
        }

        public bool Forward()
        {
            if (ZAxis() > (leap.handBaseline.Z + deadzone))
            {
                return true;
            }
            return false;
        }

        public bool Backward()
        {
            if (ZAxis() < (leap.handBaseline.Z - deadzone))
            {
                return true;
            }
            return false;
        }


        public static LeapController Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (syncRoot)
                    {
                        if (instance == null)
                            instance = new LeapController();
                    }
                }

                return instance;
            }
        }
    }
}
