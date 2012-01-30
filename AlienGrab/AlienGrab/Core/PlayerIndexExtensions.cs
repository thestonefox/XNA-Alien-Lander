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
    public static class PlayerIndexExtensions
    {
        public static bool CanBuyGame(this PlayerIndex player)
        {
            SignedInGamer gamer = Gamer.SignedInGamers[player];

            if (gamer == null)
                return false;

            if (!gamer.IsSignedInToLive)
                return false;

            return gamer.Privileges.AllowPurchaseContent;
        }
    }
}
