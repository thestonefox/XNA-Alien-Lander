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
        protected LightSource light;
        protected RenderTarget2D shadowRenderTarget;

        protected Map map;
        protected Player playerOne;

        public Level(Game game)
        {
            camera = new GameCamera(50.0f, 
                                    new Vector3[] { new Vector3(-200.0f, -1150.0f, -880.0f), new Vector3(1350.0f, -150.0f, -430.0f) }, 
                                    new Vector3[] { new Vector3(-320.0f, 430.0f, 730.0f), new Vector3(1230.0f, 1430.0f, 1180.0f) }, 
                                    game.GraphicsDevice.Viewport.AspectRatio, 10.0f, 10000.0f);
            camera.Position = new Vector3(-20.0f, 1230.0f, 1180.0f);
            camera.View = new Vector3(1050.0f, -1050.0f, -880.0f);

            light = new LightSource(new Vector3(-0.3f, 500.0f, 0.5f), camera);

            shadowRenderTarget = new RenderTarget2D(game.GraphicsDevice, 2048, 2048, false, SurfaceFormat.Single, DepthFormat.Depth24);

            map = new Map(game, new Vector3(8, 8, 4), light);

            playerOne = new Player(game, "Models/ship", light);
            playerOne.Position = new Vector3(0.0f, 500.0f, 0.0f);
            playerOne.SetPlayArea(map.GetPlayArea());


            DebugShapeRenderer.Initialize(game.GraphicsDevice);
        }

        public void LoadContent(ContentManager content)
        {
            map.LoadContent();
            playerOne.LoadContent();            
        }

        public void Update(GameTime gameTime, InputState input, PlayerIndex[] controllingPlayer)
        {
            camera.Move(input, controllingPlayer);
            //camera.View = playerOne.Position;
            map.Update(gameTime);
            playerOne.Move(input, controllingPlayer);            
            map.CheckCollision(playerOne);
            playerOne.Update(gameTime);

            if (Keyboard.GetState().IsKeyDown(Keys.P))
            {
                Console.WriteLine(camera.Position + " - " + camera.View + "." + " - " + playerOne.Position);
            }
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            DebugShapeRenderer.AddBoundingBox(playerOne.Bounds, Color.Yellow);
            playerOne.DrawShadow(camera, ref shadowRenderTarget);
            map.Draw(camera, ref shadowRenderTarget);
            playerOne.Draw(camera, ref shadowRenderTarget);

            DebugShapeRenderer.Draw(gameTime, camera.GetViewMatrix(), camera.GetProjectionMatrix());
        }
    }
}
