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
        protected GameCamera camera;
        protected Map map;
        protected Player playerOne;

        public Level(Game game)
        {
            map = new Map(game, new Vector3(8, 8, 5));

            /*
             Position.X = MathHelper.Clamp(Position.X, -320.0f, 1230.0f);
            View.X = MathHelper.Clamp(View.X, -200.0f, 1350.0f);
            Position.Y = MathHelper.Clamp(Position.Y, 630.0f, 1130.0f);
            View.Y = MathHelper.Clamp(View.Y, -950.0f, -450.0f);
            Position.Z = MathHelper.Clamp(Position.Z, -570.0f, 1180.0f);
            View.Z = MathHelper.Clamp(View.Z, -880.0f, 870.0f);*/


            camera = new GameCamera(50.0f, 
                                    new Vector3[] { new Vector3(-200.0f, -950.0f, -880.0f), new Vector3(1350.0f, -450.0f, 870.0f) }, 
                                    new Vector3[] { new Vector3(-320.0f, 630.0f, -570.0f), new Vector3(1230.0f, 1130.0f, 1180.0f) }, 
                                    game.GraphicsDevice.Viewport.AspectRatio, 10.0f, 10000.0f);
            camera.Position = new Vector3(-20.0f, 1230.0f, 1180.0f);
            camera.View = new Vector3(1050.0f, -1050.0f, -880.0f);
            playerOne = new Player(game, "ship");
            playerOne.Position = new Vector3(0.0f, 500.0f, 0.0f);
        }

        public void LoadContent(ContentManager content)
        {
            map.LoadContent(content);
            playerOne.LoadContent(content);            
        }

        public void Update(GameTime gameTime, InputState input, PlayerIndex[] controllingPlayer)
        {
            camera.Move(input, controllingPlayer);
            //camera.View = playerOne.Position;
            map.Update(gameTime);
            playerOne.Move();
            playerOne.Update(gameTime);
            map.CheckCollision(playerOne);            

            if (Keyboard.GetState().IsKeyDown(Keys.P))
            {
                Console.WriteLine(camera.Position + " - " + camera.View + "." + " - " + playerOne.Position);
            }
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            map.Draw(gameTime, camera);
            playerOne.Draw(gameTime, camera);
            spriteBatch.Begin();
            spriteBatch.End();
        }
    }
}
