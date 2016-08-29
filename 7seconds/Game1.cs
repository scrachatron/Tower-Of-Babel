using Android.Views;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Threading;

namespace _7seconds
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        public static GraphicsDeviceManager graphics;
        public static Random RNG;
        public static int TILESIZE = 16;

        SpriteBatch spriteBatch;
        Level m_map;
        Camera m_cam;
        Ui m_ui;
        TouchInputManager m_touch;

        public Game1()
        {
            RNG = new Random();
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            graphics.IsFullScreen = true;
           // graphics.PreferredBackBufferWidth = 1920;
           // graphics.PreferredBackBufferHeight = 1080;
            graphics.SupportedOrientations = DisplayOrientation.LandscapeLeft | DisplayOrientation.LandscapeRight;
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            m_touch = new TouchInputManager();
            m_map = new Level();
            m_cam = new Camera(GraphicsDevice.Viewport);
            m_ui = new Ui(new Vector2(graphics.PreferredBackBufferWidth / 6, graphics.PreferredBackBufferHeight - graphics.PreferredBackBufferHeight / 5),(graphics.PreferredBackBufferWidth/20));

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            Pixelclass.Content = Content;
            Pixelclass.Font = Content.Load<SpriteFont>("File");
            Pixelclass.Pixel = Content.Load<Texture2D>("Pixel");
            Pixelclass.Tfont = Content.Load<SpriteFont>("File");
                                          
            // TODO: use this.Content to load your game content here
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
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
            m_touch.UpdateMe();
            if(m_touch.WasTouchedBack())
            {
                m_map = new Level();
            }
            m_ui.UpdateMe(m_touch);

            m_cam.UpdateMe(new Vector2(0, 0), new Point(0, 0));

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.AliceBlue);

            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, null, RasterizerState.CullNone, null, m_cam.Transform);

            m_map.DrawMe(spriteBatch);

            
            spriteBatch.End();

            spriteBatch.Begin();

            m_ui.DrawMe(spriteBatch);
#if DEBUG
            for (int i = 0; i < m_touch.m_Touches.Count; i++)
            {
                spriteBatch.Draw(Pixelclass.Pixel, new Rectangle(m_touch.m_Touches[i].Position.ToPoint(), Pixelclass.Tfont.MeasureString((m_touch.m_Touches[i].Position.ToPoint() + "")).ToPoint()),Color.Blue);
                spriteBatch.DrawString(Pixelclass.Font, m_touch.m_Touches[i].Position.ToPoint() + "", m_touch.m_Touches[i].Position, Color.RosyBrown);
            }
#endif

            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
