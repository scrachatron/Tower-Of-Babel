using Android.Views;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Threading;
using Tower_Of_Babel;

namespace Tower_Of_Babel
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        public static GraphicsDeviceManager graphics;
        public static Random RNG;
        public static int TILESIZE = 64;
        public static bool PlayerTurn = true;
        public static int FloorNumber = 0;

        
        SpriteBatch spriteBatch;
        List<Level> m_map;
        Camera m_cam;
        Ui m_ui;
        TouchInputManager m_touch;
        private Thread m_manager;
        Player m_p;
        minimap m_minimap;

        public Game1()
        {
            RNG = new Random();
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            m_p = new Player();
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
            m_map = new List<Level>();
            m_map.Add(new Level());
            m_manager = new Thread(new ThreadStart(ThreadMap));
            m_manager.Start();
            m_cam = new Camera(GraphicsDevice.Viewport);
            m_minimap = new minimap(m_map[0].Map.GetLength(0));
            m_ui = new Ui(new Vector2(graphics.PreferredBackBufferWidth / 6, graphics.PreferredBackBufferHeight - graphics.PreferredBackBufferHeight / 5),(graphics.PreferredBackBufferWidth/20));
            m_p.Position = new Vector2(m_map[0].m_StartPos.X * TILESIZE, m_map[0].m_StartPos.Y * TILESIZE);
            m_p.VirtualPosition = m_map[0].m_StartPos;
            m_minimap.UpdateMap(m_map[0]);

            base.Initialize();
        }

        private void ThreadMap()
        {
            if (FloorNumber % 25 == 0)
                m_map.Add(new Town());
            else
                m_map.Add(new Level());
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

        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        { 
            m_touch.UpdateMe();
            if(new Rectangle(m_p.Position.ToPoint(),new Point (TILESIZE,TILESIZE)).Intersects(
                new Rectangle(m_map[0].m_WinPos.X * TILESIZE,m_map[0].m_WinPos.Y * TILESIZE,TILESIZE,TILESIZE)))
            {
            
                //m_manager.
                m_manager.Join();
                m_map.RemoveAt(0);
                m_manager = new Thread(new ThreadStart(ThreadMap));
                m_manager.Start();
                m_p.Position = new Vector2(m_map[0].m_StartPos.X * TILESIZE, m_map[0].m_StartPos.Y * TILESIZE);
                m_p.VirtualPosition = m_map[0].m_StartPos;
                m_minimap.UpdateMap(m_map[0]);
                //m_map.Add(new Level());
            }
            m_minimap.UpdateMe(gameTime, m_p,m_map[0]);

            m_ui.UpdateMe(m_touch);
            if (PlayerTurn)
            {
                m_p.TakeTurn(m_ui);
               
            }
            m_p.UpdateMe(gameTime,m_map[0], m_ui,m_touch);

            m_map[0].UpdateMe(gameTime, m_p, m_touch);

            m_cam.UpdateMe(m_p.Position, new Point(TILESIZE * m_map[0].Map.GetLength(0), TILESIZE * m_map[0].Map.GetLength(1)));
                //m_cam.UpdateMe(m_touch.m_Touches[0].Position, new Point(0, 0));


            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, null, RasterizerState.CullNone, null, m_cam.Transform);

            m_map[0].DrawMe(spriteBatch,m_minimap,m_p.VirtualPosition);
            spriteBatch.Draw(Pixelclass.Pixel, new Rectangle(m_p.Position.ToPoint(), new Point(TILESIZE, TILESIZE)), Color.CornflowerBlue);

            spriteBatch.End();

            spriteBatch.Begin();

            m_ui.DrawMe(spriteBatch);
            if (m_touch.m_Touches.Count > 2)
                m_minimap.DrawMe(spriteBatch, m_map[0],m_p.VirtualPosition);


            

            
#if DEBUG
            spriteBatch.DrawString(Pixelclass.Font, m_p.Position.ToString(), new Vector2(0, 0), Color.CornflowerBlue);

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
