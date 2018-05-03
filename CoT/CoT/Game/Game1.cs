using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Windows.Forms;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;
using Myra;
using Myra.Graphics2D.UI;
using Penumbra;
using Button = Myra.Graphics2D.UI.Button;
using ComboBox = Myra.Graphics2D.UI.ComboBox;
using Console = System.Console;
using Point = Microsoft.Xna.Framework.Point;
using Rectangle = Microsoft.Xna.Framework.Rectangle;


namespace CoT
{
    public enum Direction
    {
        North, East, South, West
    }

    public class Game1 : Game
    {
        #region Constants

        public static readonly Vector2 TileOffset = new Vector2(-0.5f, 0.5f);
        public static readonly int MonitorWidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
        public static readonly int MonitorHeight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;
        public static readonly int ScreenWidth = MonitorWidth;
        public static readonly int ScreenHeight = MonitorHeight;

        #endregion

        #region Fields

        public static Game1 Game { get; set; }
        public GraphicsDeviceManager Graphics { get; set; }
        public Form WindowForm { get; set; }
        public GameManager GameManager { get; set; }
        public Desktop host = new Desktop();
        public bool IsLoaded { get; set; }

        public static Random Random { get; set; } = new Random();
        #endregion

        public Game1()
        {
            Console.WriteLine("Game1 - Constructor");

            Game = this;
            Graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            IsMouseVisible = true;
            IsFixedTimeStep = false;
            Graphics.SynchronizeWithVerticalRetrace = true;
            Graphics.GraphicsProfile = GraphicsProfile.HiDef;
            Graphics.PreferredBackBufferWidth = MonitorWidth;
            Graphics.PreferredBackBufferHeight = MonitorHeight;
            Graphics.IsFullScreen = false;
            WindowForm = (Form)Control.FromHandle(Window.Handle);
            Window.AllowAltF4 = true;
            Window.AllowUserResizing = false;
            Window.IsBorderless = true;
            Window.Position = new Point(0, 0);
            Graphics.ApplyChanges();

            WindowForm.Load += (sender, args) =>
            {
                IsLoaded = true;
                GameManager = new GameManager();
                GameManager.Initialize();
                GameManager.LoadContent();
            };
        }

        protected override void Initialize()
        {
            Console.WriteLine("Game1 - Initialize");

            MyraEnvironment.Game = this;

            base.Initialize();
        }

        protected override void LoadContent()
        {
            Console.WriteLine("Game1 - LoadContent");
            base.LoadContent();
        }
        protected override void UnloadContent()
        {
            Console.WriteLine("Game1 - UnloadContent");
            GameManager.UnloadContent();
            base.UnloadContent();
        }

        protected override void Update(GameTime gameTime)
        {
            if (IsLoaded)
            {
                GameManager.Update(gameTime);
                base.Update(gameTime);
            }
        }
        protected override void Draw(GameTime gameTime)
        {
            if (IsLoaded)
            {
                base.Draw(gameTime);
                GameManager.Draw(gameTime);
                host.Bounds = new Rectangle(0, 0, GraphicsDevice.PresentationParameters.BackBufferWidth, GraphicsDevice.PresentationParameters.BackBufferHeight);
                host.Render();
            }
        }
    }
}
