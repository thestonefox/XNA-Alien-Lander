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
        private ParticleLibrary particleLibrary;

        private int levelCount;
        private int startPeeps;
        private int startFuel;        
        private int score;
        private int lives;
        private int fuel;
        private bool updateScore;
        private Game game;

        private PauseScreen pauseScreen;
        private LevelCompleteScreen levelCompleteScreen;

        public void Initialize(Game _game)
        {
            game = _game;
            pauseScreen = new PauseScreen(game.Content, "Screens/pause", "Fonts/OCR");
            levelCompleteScreen = new LevelCompleteScreen(game.Content, "Screens/levelcomplete", "Fonts/OCR");
            InitParticles();
            startPeeps = 1;
            startFuel = 1000;
            levelCount = 0;
            score = 0;
            lives = 3;
            fuel = startFuel;            
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
            level = new Level(game, particleLibrary, startPeeps, startFuel, levelCount, score, lives);
            level.LoadContent(game.Content);
            levelCount++;
        }

        public int GetFinalScore()
        {
            return score;
        }

        public int GetFinalLevel()
        {
            return levelCount;
        }

        public void Update(GameTime gameTime, ref ApplicationState gameState, InputState input, PlayerIndex[] controllingPlayer)
        {
            switch (gameState)
            {
                case ApplicationState.LevelComplete:
                    {
                        if (updateScore == false)
                        {
                            score += (fuel * 2) * lives;
                            startPeeps++;
                            if (startPeeps > 16)
                            {
                                startPeeps = 3;
                                startFuel -= 100;
                            }
                            updateScore = true;
                        }                        
                        
                        if (levelCompleteScreen.Update(fuel, lives, score, input, controllingPlayer))
                        {
                            updateScore = false;
                            CreateLevel();
                            gameState = ApplicationState.Playing;
                        }
                        break;
                    }
                case ApplicationState.Playing:
                    {
                        level.Update(gameTime, input, controllingPlayer, ref gameState, ref score, ref lives, ref fuel);                
                        break;
                    }
                case ApplicationState.Paused:
                    {
                        pauseScreen.Update(ref gameState, input, controllingPlayer);
                        break;
                    }
            }

            //check for pause button
            if (gameState != ApplicationState.LevelComplete && (input.IsNewButtonPress(Buttons.Start, controllingPlayer[0], out controllingPlayer[1]) ||
                input.IsNewKeyPress(Keys.Delete, controllingPlayer[0], out controllingPlayer[1])))
            {
                if (gameState == ApplicationState.Playing)
                {
                    pauseScreen.Reset();
                    gameState = ApplicationState.Paused;
                }
                else
                {
                    gameState = ApplicationState.Playing;
                }
            }
        }

        public void Draw(GameTime gameTime, ApplicationState gameState, SpriteBatch spriteBatch)
        {
            game.GraphicsDevice.BlendState = BlendState.Opaque;
            game.GraphicsDevice.DepthStencilState = DepthStencilState.Default;
            game.GraphicsDevice.RasterizerState = RasterizerState.CullCounterClockwise;
            game.GraphicsDevice.SamplerStates[1] = SamplerState.PointClamp;
            level.Draw(gameTime, spriteBatch);
            game.GraphicsDevice.RasterizerState = RasterizerState.CullNone;
            game.GraphicsDevice.SamplerStates[0] = SamplerState.LinearWrap;

            //draw overlays
            switch (gameState)
            {
                case ApplicationState.Paused: pauseScreen.Draw(spriteBatch);
                    break;
                case ApplicationState.LevelComplete: levelCompleteScreen.Draw(spriteBatch);
                    break;
            }
        }
    }
}
