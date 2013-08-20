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
        protected Scene scene;

        protected Map map;
        protected Player playerOne;
        protected ParticleLibrary particleEffects;
        protected int peepsLeft; 
        protected CollisionType playerCollisionCheck;
        protected Base3DObject skybox;
        protected int levelNumber;
        protected Hud gameHud;
        protected Countdown countdown;
        protected SoundPlayer soundPlayer;
        protected int initTimer;
        protected int timerIndex;

        private OptionsHolder gameOptions = OptionsHolder.Instance;

        public Level(Game game, ParticleLibrary _particleEffects, ref SoundPlayer _soundPlayer, Scene _scene, Player _playerOne, int peeps, int _levelNumber)
        {
            levelNumber = _levelNumber;
            particleEffects = _particleEffects;
            peepsLeft = peeps;
            scene = _scene;
            scene.Camera.ResetCamera();
            soundPlayer = _soundPlayer;
            map = new Map(game, new Vector3(8, 7, 4), scene.Light, peepsLeft); 
            playerOne = _playerOne;
			playerOne.SetStartPosition(map.GetPlayerStartPosition());
            playerOne.SetPlayArea(map.GetPlayArea());
            playerOne.AttachParticleLibrary(particleEffects);
            playerOne.AttachSoundPlayer(ref _soundPlayer);
			playerOne.Reset();
            playerCollisionCheck = CollisionType.None;
            skybox = new Base3DObject(game, "Models/skybox", scene.Light);
            gameHud = new Hud(game.Content, game.GraphicsDevice.Viewport.TitleSafeArea);
            countdown = new Countdown(game.Content, game.GraphicsDevice.Viewport.TitleSafeArea);
            initTimer = 140;
            timerIndex = 0;
        }

        public void LoadContent(ContentManager content)
        {
            skybox.LoadContent(false);
            skybox.Scale = new Vector3(10.0f, 5.0f, 10.0f);
            skybox.Rotation = new Vector3(0.0f, MathHelper.ToRadians(180.0f), 0.0f);
            skybox.Position = new Vector3(700.0f, 0.0f, -1000.0f);
            playerOne.LoadContent(true);
        }

        public void Update(GameTime gameTime, InputState input, PlayerIndex[] controllingPlayer, ref ApplicationState appState, ref Player playerOne)
        {
            skybox.Update(gameTime);
            initTimer--;
            if (initTimer > 0)
            {
                if(initTimer % 35 == 34) {
                    timerIndex++;
                }
                countdown.Update(gameTime, timerIndex);
            }
            else
            {
                scene.Camera.Move(input, controllingPlayer);
                map.Update(gameTime);
                playerOne.Move(input, controllingPlayer);
                playerOne.Update(gameTime);

                playerCollisionCheck = map.CheckBuildingCollision(playerOne);
                //has player hit the building, if so they deaded
                if (playerCollisionCheck == CollisionType.Building || (playerOne.Fuel <= 0 && playerOne.deathCounter == 0))
                {
                    playerOne.Die();
                    soundPlayer.StopAllSounds();
                    soundPlayer.PlaySound("Explosion");
                }
                //has player landed too hard, if so they deaded, if there is a peep they deaded too
                if (playerCollisionCheck == CollisionType.Roof && playerOne.SafeDescent() == false)
                {
                    if (map.CheckPeepCollision(playerOne))
                    {
                        peepsLeft--;
                    }
                    playerOne.Die();
                    soundPlayer.StopAllSounds();
                    soundPlayer.PlaySound("Explosion");
                }
                //has the player landed safely, if there is a peep he is abducted
                if (playerCollisionCheck == CollisionType.Roof && playerOne.SafeDescent() == true)
                {
                    if (map.CheckPeepCollision(playerOne))
                    {
                        peepsLeft--;
                        playerOne.Score += gameOptions.PeepValue;
                        soundPlayer.PlaySound("Scream");
                    }

                    if (map.CheckPowerupCollision(playerOne))
                    {
                        playerOne.Fuel += gameOptions.PowerupFuel;
                        soundPlayer.PlaySound("Reward");
                    }
                }

                if (playerOne.Lives <= 0)
                {
                    soundPlayer.StopAllSounds();
                    appState = ApplicationState.GameOver;
                }
                if (peepsLeft <= 0 && playerOne.Lives > 0)
                {
                    soundPlayer.StopAllSounds();
                    soundPlayer.PlaySound("ScoreUp");
                    appState = ApplicationState.LevelComplete;
                }

                gameHud.Update(gameTime, playerOne.Lives, playerOne.Score, playerOne.Fuel, peepsLeft);
                if (playerOne.deathCounter == 0)
                {
                    scene.Camera.Position.X = playerOne.Position.X - 550;
                }

                if (Keyboard.GetState().IsKeyDown(Keys.P))
                {
                    Console.WriteLine(scene.Camera.Position + " - " + scene.Camera.View + "." + " - " + playerOne.Position);
                }
            }
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            particleEffects.Draw(scene.Camera);
            if (initTimer <= 0)
            {
                playerOne.DrawShadow(scene.Camera, ref scene.ShadowRenderTarget);
            }
            map.Draw(scene.Camera, ref scene.ShadowRenderTarget);
            if (initTimer <= 0)
            {
                playerOne.Draw(scene.Camera, ref scene.ShadowRenderTarget);
            }
            skybox.Draw(scene.Camera);

            spriteBatch.Begin();
            gameHud.Draw(spriteBatch);            
            if (initTimer > 0)
            {
                countdown.Draw(spriteBatch);
            }
            spriteBatch.End();

        }
    }
}
