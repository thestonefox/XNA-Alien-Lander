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
        protected ParticleLibrary particleEffects;
        protected int peepsLeft; 
        protected CollisionType playerCollisionCheck;
        protected Base3DObject skybox;
        protected int levelNumber;
        protected Hud gameHud;

        public Level(Game game, ParticleLibrary _particleEffects, int peeps, int fuel, int _levelNumber, int score, int lives)
        {
            levelNumber = _levelNumber;
            particleEffects = _particleEffects;
            camera = new GameCamera(50.0f, 
                                    new Vector3[] { new Vector3(-200.0f, -1050.0f, -520.0f), new Vector3(1350.0f, -150.0f, -430.0f) }, 
                                    new Vector3[] { new Vector3(-320.0f, 430.0f, 570.0f), new Vector3(1230.0f, 1230.0f, 1180.0f) }, 
                                    game.GraphicsDevice.Viewport.AspectRatio, 10.0f, 10000.0f, 
                                    new Vector3(-320.0f, 680.0f, 1180.0f),
                                    new Vector3(1350.0f, -400.0f, -520.0f));

            light = new LightSource(new Vector3(-0.3f, 500.0f, 0.5f), camera);

            shadowRenderTarget = new RenderTarget2D(game.GraphicsDevice, 2048, 2048, false, SurfaceFormat.Single, DepthFormat.Depth24);

            peepsLeft = peeps;
            map = new Map(game, new Vector3(8, 8, 4), light, peepsLeft);            
            playerOne = new Player(game, "Models/ship", light, map.GetPlayerStartPosition(), fuel, score, lives);
            playerOne.SetPlayArea(map.GetPlayArea());
            playerOne.AttachParticleLibrary(particleEffects);
            playerCollisionCheck = CollisionType.None;
            skybox = new Base3DObject(game, "Models/skybox", light);
            gameHud = new Hud(game.Content, game.GraphicsDevice.Viewport.TitleSafeArea);
        }

        public void LoadContent(ContentManager content)
        {
            skybox.LoadContent(false);
            skybox.Scale = new Vector3(10.0f, 5.0f, 10.0f);
            skybox.Rotation = new Vector3(0.0f, MathHelper.ToRadians(180.0f), 0.0f);
            skybox.Position = new Vector3(700.0f, 0.0f, -1000.0f);
            playerOne.LoadContent(true);
        }

        public void Update(GameTime gameTime, InputState input, PlayerIndex[] controllingPlayer, ref ApplicationState gameState, ref int score, ref int lives, ref int fuel)
        {
            camera.Move(input, controllingPlayer);
            skybox.Update(gameTime);
            map.Update(gameTime);
            playerOne.Move(input, controllingPlayer);
            playerOne.Update(gameTime);           

            playerCollisionCheck = map.CheckBuildingCollision(playerOne);
            //has player hit the building, if so they deaded
            if (playerCollisionCheck == CollisionType.Building)
            {
                playerOne.Die();
            }
            //has player landed too hard, if so they deaded, if there is a peep they deaded too
            if (playerCollisionCheck == CollisionType.Roof && playerOne.SafeDescent() == false)
            {
                if (map.CheckPeepCollision(playerOne))
                {
                    peepsLeft--;
                }
                playerOne.Die();
            }
            //has the player landed safely, if there is a peep he is abducted
            if (playerCollisionCheck == CollisionType.Roof && playerOne.SafeDescent() == true)
            {
                if (map.CheckPeepCollision(playerOne))
                {
                    peepsLeft--;
                    playerOne.Score += 100;
                }
            }

            score = playerOne.Score;
            lives = playerOne.Lives;
            fuel = playerOne.Fuel;


            if (playerOne.Lives <= 0)
            {
                gameState = ApplicationState.GameOver;
            }
            if (peepsLeft <= 0)
            {
                gameState = ApplicationState.LevelComplete;
            }

            gameHud.Update(gameTime, playerOne.Lives, playerOne.Score, playerOne.Fuel, peepsLeft);




            if (Keyboard.GetState().IsKeyDown(Keys.P))
            {
                Console.WriteLine(camera.Position + " - " + camera.View + "." + " - " + playerOne.Position);
            }            
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {            
            particleEffects.Draw(camera);            
            playerOne.DrawShadow(camera, ref shadowRenderTarget);
            map.Draw(camera, ref shadowRenderTarget);
            playerOne.Draw(camera, ref shadowRenderTarget);
            skybox.Draw(camera);

            spriteBatch.Begin();
            gameHud.Draw(spriteBatch);
            spriteBatch.End();
        }
    }
}
