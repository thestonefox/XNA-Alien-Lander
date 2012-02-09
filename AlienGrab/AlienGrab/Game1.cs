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
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;

        private InputState input;
        private PlayerIndex[] controllingPlayer;
        private ApplicationState appState;

        private GameState gameState;

        private SplashScreen splashScreen;
        private HomeScreen homeScreen;
        private OptionsScreen optionsScreen;
        private GameOverScreen gameOverScreen;
        private QuitScreen quitScreen;
        private TrialScreen trialScreen;
        private OptionsHolder gameOptions = OptionsHolder.Instance;
        
        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferWidth = 1280;
            graphics.PreferredBackBufferHeight = 720;
            Content.RootDirectory = "Content";
            Components.Add(new GamerServicesComponent(this));

            appState = ApplicationState.Splash;
            gameState = new GameState();
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

            splashScreen = new SplashScreen(this.Content, "Screens/splash", "Fonts/OCR");
            homeScreen = new HomeScreen(this.Content, "Screens/home", "Fonts/OCR");
            optionsScreen = new OptionsScreen(this.Content, "Screens/options", "Fonts/OCR");
            gameOverScreen = new GameOverScreen(this.Content, "Screens/gameover", "Fonts/OCR");
            quitScreen = new QuitScreen(this.Content, "Screens/quit", "Fonts/OCR");
            trialScreen = new TrialScreen(this.Content, "Screens/trial", "Fonts/OCR");
            
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
            //this.GraphicsDevice.BlendState = BlendState.Opaque;
            //this.GraphicsDevice.DepthStencilState = DepthStencilState.Default;
            //this.GraphicsDevice.SamplerStates[0] = SamplerState.LinearWrap;
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }



        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // TODO: Add your update logic here
            input.Update();
            switch (appState)
            {
                case ApplicationState.Splash:       splashScreen.Update(ref appState, input, ref controllingPlayer);
                                                    break;
                case ApplicationState.InitaliseApp: break;
                case ApplicationState.Home:         homeScreen.Update(ref appState, input, controllingPlayer);                                                    
                                                    break;
                case ApplicationState.Options:      optionsScreen.Update(ref appState, input, controllingPlayer);
                                                    break;
                case ApplicationState.Quit:         
                    {
                     switch(quitScreen.Update(input, controllingPlayer))
                     {
                         case 0: appState = ApplicationState.InitaliseApp;
                                    break;
                         case 1:    this.Exit();
                                    break;
                     }
                     break;
                    }
                case ApplicationState.Trial:        trialScreen.Update(ref appState, input, controllingPlayer);
                                                    break;
                case ApplicationState.GameOver:     gameOverScreen.Update(gameState.GetFinalLevel(), gameState.GetFinalScore(), ref appState, input, controllingPlayer);
                                                    break;
                case ApplicationState.GameComplete: break;
                case ApplicationState.InitaliseGame: MediaPlayer.Stop(); 
                                                    break;
                //ApplicationState.Playing || ApplicationState.Paused || ApplicationState.LevelComplete
                default:                            gameState.Update(gameTime, ref appState, input, controllingPlayer);
                                                    break;
            }
            if (appState == ApplicationState.InitaliseGame)
            {
                gameState.Initialize(this);
                appState = ApplicationState.Playing;
            }

            if (appState == ApplicationState.InitaliseApp)
            {
                MediaPlayer.IsRepeating = true;
                MediaPlayer.Volume = 0.20f;
                MediaPlayer.Play(this.Content.Load<Song>("Audio\\Music\\Start"));
                homeScreen.Reset();
                trialScreen.Reset();
                gameOverScreen.Reset();
                appState = ApplicationState.Home;
            }

            gameOptions.IsTrial = Guide.IsTrialMode;
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            // TODO: Add your drawing code here
            this.GraphicsDevice.Clear(Color.Black);

            switch (appState)
            {
                case ApplicationState.Splash:       splashScreen.Draw(spriteBatch);
                                                    break;
                case ApplicationState.Home:         homeScreen.Draw(spriteBatch);
                                                    break;
                case ApplicationState.Quit:         homeScreen.Draw(spriteBatch);
                                                    quitScreen.Draw(spriteBatch);
                                                    break;
                case ApplicationState.Options:      optionsScreen.Draw(spriteBatch);
                                                    break;
                case ApplicationState.Trial:        trialScreen.Draw(spriteBatch);
                                                    break;
                case ApplicationState.GameOver:     gameOverScreen.Draw(spriteBatch);
                                                    break;
                case ApplicationState.GameComplete: break;
                case ApplicationState.InitaliseApp: break;
                case ApplicationState.InitaliseGame: break;
                //ApplicationState.Playing || ApplicationState.Paused || ApplicationState.LevelComplete
                default:                            gameState.Draw(gameTime, appState, spriteBatch);
                                                    break;
            }
            
            base.Draw(gameTime);
        }
    }
}
