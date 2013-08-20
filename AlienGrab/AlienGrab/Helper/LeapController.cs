using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LeapLibrary;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace AlienGrab.Helper
{
    public sealed class LeapController
    {
        private static volatile LeapController instance;
        private static object syncRoot = new Object();

        private LeapController() { }

        private LeapComponet leap;
        private float deadzone = 15;
        private float swipeAllowance = 0.3f;
        private Vector4 swipeTimer = new Vector4(0,0,0,0);
        private float swipeTimerReset = 3;

        public void Init(LeapComponet _leap) {
            leap = _leap;
        }

        public LeapComponet Get()
        {
            return leap;
        }

        public bool HandActive()
        {
            return leap.handActive;
        }

        public void SetDebug(bool debugMode)
        {
            leap.DrawDebug = debugMode;
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

        private void ResetSwipeTimers() {
            swipeTimer.X = swipeTimerReset;
            swipeTimer.Y = swipeTimerReset;
            swipeTimer.Z = swipeTimerReset;
            swipeTimer.W = swipeTimerReset;
        }

        public bool SwipeDown()
        {
            if(leap.swipe != null) {
                if (swipeTimer.W <= 0 && leap.swipe.Direction.y < (swipeAllowance * -1) && leap.swipe.State.ToString() == "STATESTART")
                {
                    ResetSwipeTimers();
                    return true;
                }
                swipeTimer.W--;
            }
            if (swipeTimer.W < 0)
            {
                swipeTimer.W = 0;
            }
            return false;
        }

        public bool SwipeUp()
        {
            if (leap.swipe != null)
            {
                if (swipeTimer.Y <= 0 && leap.swipe.Direction.y > swipeAllowance && leap.swipe.State.ToString() == "STATESTART")
                {
                    ResetSwipeTimers();
                    return true;
                }
                swipeTimer.Y--;
            }
            if (swipeTimer.Y < 0)
            {
                swipeTimer.Y = 0;
            }
            return false;
        }

        public bool SwipeLeft()
        {
            if (leap.swipe != null)
            {
                if (swipeTimer.Z <= 0 && leap.swipe.Direction.x < (swipeAllowance * -1) && leap.swipe.State.ToString() == "STATESTART")
                {
                    ResetSwipeTimers();
                    return true;
                }
                swipeTimer.Z--;
            }
            if (swipeTimer.Z < 0)
            {
                swipeTimer.Z = 0;
            }
            return false;
        }

        public bool SwipeRight()
        {
            if (leap.swipe != null)
            {
                if (swipeTimer.X <= 0 && leap.swipe.Direction.x > swipeAllowance && leap.swipe.State.ToString() == "STATESTART")
                {
                    ResetSwipeTimers();
                    return true;
                }
                swipeTimer.X--;
            }
            if (swipeTimer.X < 0)
            {
                swipeTimer.X = 0;
            }
            return false;
        }

        public bool SwipeLeftOrRight()
        {
            if (SwipeLeft() || SwipeRight())
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
