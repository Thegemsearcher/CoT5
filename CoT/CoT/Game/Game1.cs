using System;
using System.Diagnostics;
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


namespace CoT
{
    public class Game1 : Game
    {
        #region Constants

        public static readonly int ScreenWidth = 1920;
        public static readonly int ScreenHeight = 1080;
        public static readonly Vector2 TileOffset = new Vector2(-0.5f, 0.5f);
        public static readonly int MonitorWidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
        public static readonly int MonitorHeight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;

        #endregion

        #region Fields

        public static Game1 Game { get; set; }
        public GraphicsDeviceManager Graphics { get; set; }
        public Form WindowForm { get; set; }
        public GameManager GameManager { get; set; }
        public Desktop host = new Desktop();

        #endregion

        public Game1()
        {
            Game = this;
            Graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            IsMouseVisible = true;
            IsFixedTimeStep = false;
            Graphics.SynchronizeWithVerticalRetrace = true;
            Graphics.GraphicsProfile = GraphicsProfile.HiDef;
            Graphics.PreferredBackBufferWidth = ScreenWidth;
            Graphics.PreferredBackBufferHeight = ScreenHeight;
            Graphics.IsFullScreen = false;

            WindowForm = (Form)Control.FromHandle(Window.Handle);
            Window.AllowAltF4 = true;
            Window.AllowUserResizing = false;
            Window.IsBorderless = false;
            Window.Position = new Point(WindowBorderSize.X + 1, 0);
            Graphics.ApplyChanges();

            Console.WriteLine("Game1 - Constructor");
        }

        public Point WindowBorderSize => new Point(
                WindowForm.RectangleToScreen(WindowForm.ClientRectangle).Right - WindowForm.Right,
                WindowForm.RectangleToScreen(WindowForm.ClientRectangle).Top - WindowForm.Top);

        protected override void Initialize()
        {
            Console.WriteLine("Game1 - Initialize");

            MyraEnvironment.Game = this;

            GameManager = new GameManager();
            GameManager.Initialize();

            base.Initialize();
        }

        protected override void LoadContent()
        {
            Console.WriteLine("Game1 - LoadContent");

            GameManager.LoadContent();

            base.LoadContent();
            #region GUI
            //var grid = new Grid
            //{
            //    RowSpacing = 8,
            //    ColumnSpacing = 8
            //};

            //Myra.Graphics2D.UI.GridBased g;
            //g = new GridBased();
            //g.GridPositionX = 3;
            //g.GridPositionY = 3;
            //g.TotalColumnsPart = 10;
            //g.TotalRowsPart = 10;
            //g.Enabled = true;

            //grid.Widgets.Add(g);
            //grid.ColumnsProportions.Add(new Grid.Proportion(Grid.ProportionType.Auto));
            //grid.ColumnsProportions.Add(new Grid.Proportion(Grid.ProportionType.Auto));
            //grid.RowsProportions.Add(new Grid.Proportion(Grid.ProportionType.Auto));
            //grid.RowsProportions.Add(new Grid.Proportion(Grid.ProportionType.Auto));

            // TextBlock
            //var helloWorld = new TextBlock
            //{
            //    Text = "Hello, World!"
            //};
            //grid.Widgets.Add(helloWorld);

            //// ComboBox
            //var combo = new ComboBox
            //{
            //    GridPositionX = 1,
            //    GridPositionY = 0
            //};

            //combo.Items.Add(new ListItem("Red", Color.Red));
            //combo.Items.Add(new ListItem("Green", Color.Green));
            //combo.Items.Add(new ListItem("Blue", Color.Blue));
            //grid.Widgets.Add(combo);

            // Button
            //var button = new Button
            //{
            //    GridPositionX = 2,
            //    GridPositionY = 0,
            //    Text = "Show"
            //};
            //button.Down += (s, a) =>
            //{
            //    var messageBox = Dialog.CreateMessageBox("Message", "Some message!");
            //    messageBox.ShowModal(host);
            //};

            //grid.Widgets.Add(button);

            //// Spin button
            //var spinButton = new SpinButton
            //{
            //    GridPositionX = 3,
            //    GridPositionY = 0,
            //    WidthHint = 100,
            //    Nullable = true
            //};
            //grid.Widgets.Add(spinButton);

            // Add it to the desktop
            //host.Widgets.Add(grid);
            #endregion
        }
        protected override void UnloadContent()
        {
            Console.WriteLine("Game1 - UnloadContent");

            GameManager.UnloadContent();

            base.UnloadContent();
        }

        protected override void Update(GameTime gameTime)
        {
            GameManager.Update(gameTime);
            base.Update(gameTime);
        }
        protected override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);

            GameManager.Draw(gameTime);

            host.Bounds = new Rectangle(0, 0, GraphicsDevice.PresentationParameters.BackBufferWidth, GraphicsDevice.PresentationParameters.BackBufferHeight);
            host.Render();
        }
    }
}
