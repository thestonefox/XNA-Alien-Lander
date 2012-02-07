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
    class GameState
    {
        private Level level;
		private Player playerOne;
        private Scene scene;
        private ParticleLibrary particleLibrary;

        private int levelCount;
        private int startPeeps;
        private bool updateScore;
        private Game game;

        private PauseScreen pauseScreen;
        private LevelCompleteScreen levelCompleteScreen;
        private OptionsHolder gameOptions = OptionsHolder.Instance;
        private SoundPlayer soundPlayer;

        public void Initialize(Game _game)
        {
            game = _game;
            scene = new Scene(game);
            pauseScreen = new PauseScreen(game.Content, "Screens/pause", "Fonts/OCR");
            levelCompleteScreen = new LevelCompleteScreen(game.Content, "Screens/levelcomplete", "Fonts/OCR");
            InitParticles();
            startPeeps = gameOptions.StartPeeps;
            levelCount = 0;
            soundPlayer = new SoundPlayer(game.Content);
            soundPlayer.AddSound("Reward", "Audio\\Effects\\reward", false);
            soundPlayer.AddSound("Explosion", "Audio\\Effects\\explosion", false);
            soundPlayer.AddSound("Thrust", "Audio\\Effects\\thrust", true);
            soundPlayer.AddSound("Scream", "Audio\\Effects\\scream", false);
            soundPlayer.AddSound("ScoreUp", "Audio\\Effects\\scoreup", false);
            MediaPlayer.IsRepeating = true;
            MediaPlayer.Volume = gameOptions.MusicVolumeAtPlay;
            MediaPlayer.Play(game.Content.Load<Song>("Audio\\Music\\Pulse")); 

			playerOne = new Player(game, "Models/ship", scene.Light);
			playerOne.Fuel=gameOptions.StartFuel;
			playerOne.StartFuel=gameOptions.StartFuel;
			playerOne.Score = 0;
			playerOne.Lives = gameOptions.StartLives;
            CreateLevel();
        }

        protected void InitParticles()
        {
            ParticleSystem explosionParticles = new ExplosionParticleSystem(game, game.Content);
            ParticleSystem explosionSmokeParticles = new ExplosionSmokeParticleSystem(game, game.Content);
            ParticleSystem energyParticles = new EnergyParticleSystem(game, game.Content);
            particleLibrary = new ParticleLibrary();
            explosionSmokeParticles.DrawOrder = 200;
            energyParticles.DrawOrder = 300;
            explosionParticles.DrawOrder = 400;

            game.Components.Add(explosionParticles);
            game.Components.Add(explosionSmokeParticles);
            game.Components.Add(energyParticles);

            particleLibrary.ExplosionParticles = explosionParticles;
            particleLibrary.ExplosionSmokeParticles = explosionSmokeParticles;
            particleLibrary.EnergyParticles = energyParticles;
        }

        protected void CreateLevel()
        {
            soundPlayer.StopAllSounds();
            level = new Level(game, particleLibrary, ref soundPlayer, scene, playerOne, startPeeps, levelCount);
            level.LoadContent(game.Content);
            levelCount++;
        }

        public int GetFinalScore()
        {
            return playerOne.Score;
        }

        public int GetFinalLevel()
        {
            return levelCount;
        }

        public void Update(GameTime gameTime, ref ApplicationState appState, InputState input, PlayerIndex[] controllingPlayer)
        {
            switch (appState)
            {
                case ApplicationState.LevelComplete:
                    {
                        soundPlayer.StopAllSounds();
                        MediaPlayer.Volume = gameOptions.MusicVolumeAtTransition;
                        if (gameOptions.IsTrial == true && levelCount >= 3)
                        {
                            appState = ApplicationState.Trial;
                            break;
                        }
                        if (updateScore == false)
                        {
                            playerOne.Score += (playerOne.Fuel * gameOptions.FuelMultiplier) * playerOne.Lives;
                            startPeeps += gameOptions.IncPeeps;
                            if (startPeeps > gameOptions.MaxPeeps)
                            {
                                startPeeps = gameOptions.StartPeeps;
                                playerOne.StartFuel -= gameOptions.DecreaseFuel;
                            }
                            updateScore = true;
                        }                        
                        
                        if (levelCompleteScreen.Update(playerOne.Fuel, playerOne.Lives, playerOne.Score, input, controllingPlayer))
                        {
                            updateScore = false;
                            CreateLevel();
                            appState = ApplicationState.Playing;
                        }
                        break;
                    }
                case ApplicationState.Playing:
                    {
                        MediaPlayer.Volume = gameOptions.MusicVolumeAtPlay;
                        level.Update(gameTime, input, controllingPlayer, ref appState, ref playerOne);                
                        break;
                    }
                case ApplicationState.Paused:
                    {
                        MediaPlayer.Volume = gameOptions.MusicVolumeAtPause;
                        soundPlayer.StopAllSounds();
                        pauseScreen.Update(ref appState, input, controllingPlayer);
                        break;
                    }
            }

            //check for pause button or game pad being disconnected
            if ((appState != ApplicationState.LevelComplete || appState != ApplicationState.Trial) && ((input.IsNewButtonPress(ButtonMappings.Pad_Start, controllingPlayer[0], out controllingPlayer[1]) ||
                input.IsNewKeyPress(ButtonMappings.Keyboard_Start, controllingPlayer[0], out controllingPlayer[1])) ||
                input.GamePadConnected(controllingPlayer[0], out controllingPlayer[1]) == GamePadStateValues.Disconnected)
				)
            {
                if (appState == ApplicationState.Playing)
                {
                    pauseScreen.Reset();
                    appState = ApplicationState.Paused;
                }
                else
                {
                    appState = ApplicationState.Playing;
                }
            }

            if (appState == ApplicationState.GameComplete || appState == ApplicationState.GameOver)
            {
                MediaPlayer.Stop();
            }
        }

        public void Draw(GameTime gameTime, ApplicationState appState, SpriteBatch spriteBatch)
        {
            game.GraphicsDevice.BlendState = BlendState.Opaque;
            game.GraphicsDevice.DepthStencilState = DepthStencilState.Default;
            game.GraphicsDevice.RasterizerState = RasterizerState.CullCounterClockwise;
            game.GraphicsDevice.SamplerStates[1] = SamplerState.PointClamp;
            level.Draw(gameTime, spriteBatch);
            game.GraphicsDevice.RasterizerState = RasterizerState.CullNone;
            game.GraphicsDevice.SamplerStates[0] = SamplerState.LinearWrap;

            //draw overlays
            switch (appState)
            {
                case ApplicationState.Paused: pauseScreen.Draw(spriteBatch);
                    break;
                case ApplicationState.LevelComplete: levelCompleteScreen.Draw(spriteBatch);
                    break;
            }
        }
    }
}
