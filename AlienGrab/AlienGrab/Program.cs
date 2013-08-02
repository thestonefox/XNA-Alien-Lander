using System;

namespace AlienGrab
{
#if WINDOWS || XBOX
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            using (Game1 game = new Game1())
            {
                game.Run();
            }
            /*try
            {
                using (Game1 game = new Game1())
                {
                    game.Run();
                }
            }
            catch (Exception e)
            {
                using (CrashDebugGame game = new CrashDebugGame(e))
                {
                    game.Run();
                }
            }*/
        }
    }
#endif
}

