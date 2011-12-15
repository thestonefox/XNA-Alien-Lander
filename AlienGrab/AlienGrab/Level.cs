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
    class Level
    {
        private Rectangle gameArea;
        private Map map;
        private Player playerOne;

        public Level(Rectangle _gameArea)
        {
            gameArea = _gameArea;
            map = new Map(4, 6, 8);
            playerOne = new Player();
        }

        public void LoadContent(ContentManager content)
        {
            map.LoadContent(content);
            playerOne.LoadContent(content);
            playerOne.Reset(Vector2.Zero);
        }

        public void Update(GameTime gameTime)
        {
            map.Update(gameTime);
            playerOne.Update(gameTime);
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            map.Draw(gameTime, spriteBatch);
            playerOne.Draw(gameTime, spriteBatch);
        }
    }
}
