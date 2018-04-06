using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;
using Myra;
using Myra.Graphics2D.UI;
using Penumbra;


namespace CoT
{
    public class Game1 : Game
    {
        public static Game1 Game { get; set; }
        public GraphicsDeviceManager Graphics { get; set; }
        public GameManager GameManager { get; set; }

        public static readonly int ScreenWidth = 1280;
        public static readonly int ScreenHeight = 720;
        public static readonly Vector2 TileOffset = new Vector2(-0.5f, 0.5f);

        public Desktop host;

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

            var form = (System.Windows.Forms.Form)System.Windows.Forms.Control.FromHandle(this.Window.Handle);
            form.Location = new System.Drawing.Point(0, 0);
            Graphics.ApplyChanges();

            GameManager = new GameManager(this);
            Components.Add(GameManager);
        }

        protected override void Initialize()
        {
            Console.WriteLine("Init - Game1");
            base.Initialize();
        }

        protected override void LoadContent()
        {
            Console.WriteLine("LoadContent - Game1");
            #region GUI
            MyraEnvironment.Game = this;

            var grid = new Grid
            {
                RowSpacing = 8,
                ColumnSpacing = 8
            };

            grid.ColumnsProportions.Add(new Grid.Proportion(Grid.ProportionType.Auto));
            grid.ColumnsProportions.Add(new Grid.Proportion(Grid.ProportionType.Auto));
            grid.RowsProportions.Add(new Grid.Proportion(Grid.ProportionType.Auto));
            grid.RowsProportions.Add(new Grid.Proportion(Grid.ProportionType.Auto));

            // TextBlock
            var helloWorld = new TextBlock
            {
                Text = "Hello, World!"
            };
            grid.Widgets.Add(helloWorld);

            // ComboBox
            var combo = new ComboBox
            {
                GridPositionX = 1,
                GridPositionY = 0
            };

            combo.Items.Add(new ListItem("Red", Color.Red));
            combo.Items.Add(new ListItem("Green", Color.Green));
            combo.Items.Add(new ListItem("Blue", Color.Blue));
            grid.Widgets.Add(combo);

            // Button
            var button = new Button
            {
                GridPositionX = 0,
                GridPositionY = 1,
                Text = "Show"
            };

            button.Down += (s, a) =>
            {
                var messageBox = Dialog.CreateMessageBox("Message", "Some message!");
                messageBox.ShowModal(host);
            };

            grid.Widgets.Add(button);

            // Spin button
            var spinButton = new SpinButton
            {
                GridPositionX = 1,
                GridPositionY = 1,
                WidthHint = 100,
                Nullable = true
            };
            grid.Widgets.Add(spinButton);

            // Add it to the desktop
            host = new Desktop();
            host.Widgets.Add(grid);
            #endregion
        }
        protected override void UnloadContent()
        {
            Console.WriteLine("UnloadContent - Game1");
            base.UnloadContent();
        }

        protected override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }
        protected override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
            host.Bounds = new Rectangle(0, 0, GraphicsDevice.PresentationParameters.BackBufferWidth, GraphicsDevice.PresentationParameters.BackBufferHeight);
            host.Render();
        }
    }
}
