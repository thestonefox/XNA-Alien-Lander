using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace AlienGrab
{
    public enum CollisionType { None, Building, Roof };

    public enum GameState { Splash, Home, Options, Trial, Playing, LevelComplete, GameOver, GameComplete };

    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;

        private InputState input;
        private PlayerIndex[] controllingPlayer;

        private Level level;

        private ParticleLibrary particleLibrary;

        private GameState gameState;

        private int startPeeps;
        private int startFuel;
        private int levelCount;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);            
            Content.RootDirectory = "Content";
            InitParticles();

            startPeeps = 3;
            startFuel = 1000;
            levelCount = 1;
            gameState = GameState.Playing;
        }

        protected void InitParticles()
        {
            ParticleSystem explosionParticles = new ExplosionParticleSystem(this, Content);
            ParticleSystem explosionSmokeParticles = new ExplosionSmokeParticleSystem(this, Content);
            ParticleSystem energyParticles = new EnergyParticleSystem(this, Content);
            particleLibrary = new ParticleLibrary();
            explosionSmokeParticles.DrawOrder = 200;
            energyParticles.DrawOrder = 300;
            explosionParticles.DrawOrder = 400;

            Components.Add(explosionParticles);
            Components.Add(explosionSmokeParticles);
            Components.Add(energyParticles);

            particleLibrary.ExplosionParticles = explosionParticles;
            particleLibrary.ExplosionSmokeParticles = explosionSmokeParticles;
            particleLibrary.EnergyParticles = energyParticles;
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            input = new InputState();
            controllingPlayer = new PlayerIndex[2]{PlayerIndex.One, new PlayerIndex()};
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            this.GraphicsDevice.BlendState = BlendState.Opaque;
            this.GraphicsDevice.DepthStencilState = DepthStencilState.Default;
            this.GraphicsDevice.SamplerStates[0] = SamplerState.LinearWrap;
            // TODO: use this.Content to load your game content here
            CreateLevel();
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        protected void CreateLevel()
        {
            level = new Level(this, particleLibrary, startPeeps, startFuel, levelCount);
            level.LoadContent(Content);
            levelCount++;
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            // TODO: Add your update logic here
            input.Update();

            if (gameState == GameState.LevelComplete)
            {
                startPeeps++;
                if (startPeeps > 16)
                {
                    startPeeps = 3;
                    startFuel -= 100;
                }
                CreateLevel();
                gameState = GameState.Playing;
            }

            if (gameState == GameState.Playing)
            {
                level.Update(gameTime, input, controllingPlayer, ref gameState);
            }

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            // TODO: Add your drawing code here
            this.GraphicsDevice.Clear(Color.CornflowerBlue);            

            this.GraphicsDevice.BlendState = BlendState.Opaque;
            this.GraphicsDevice.DepthStencilState = DepthStencilState.Default;
            this.GraphicsDevice.RasterizerState = RasterizerState.CullCounterClockwise;
            this.GraphicsDevice.SamplerStates[1] = SamplerState.PointClamp;

            if (gameState == GameState.Playing)
            {
                level.Draw(gameTime, spriteBatch);
            }
            this.GraphicsDevice.RasterizerState = RasterizerState.CullNone;                        
            this.GraphicsDevice.SamplerStates[0] = SamplerState.LinearWrap;
            
            base.Draw(gameTime);
        }
    }
}
