using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Myra;
using Myra.Graphics2D.UI;

namespace CoT
{
    public class Game1 : Game
    {
        public static Game1 Game { get; set; }
        public GraphicsDeviceManager Graphics { get; set; }
        public SpriteBatch SpriteBatch { get; set; }

        Player player = new Player("Dude", new Vector2(0, 0).ToWorld(), new Rectangle(0, 0, 383, 862));
        public Map map;

        private Desktop host;

        public Game1()
        {
            Game = this;
            Graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            IsMouseVisible = true;
            IsFixedTimeStep = false;
            Graphics.SynchronizeWithVerticalRetrace = true;
            Graphics.GraphicsProfile = GraphicsProfile.HiDef;
            Graphics.PreferredBackBufferWidth = 1280;
            Graphics.PreferredBackBufferHeight = 720;
            Graphics.IsFullScreen = false;

            var form = (System.Windows.Forms.Form)System.Windows.Forms.Control.FromHandle(this.Window.Handle);
            form.Location = new System.Drawing.Point(0, 0);
            Graphics.ApplyChanges();
        }

        protected override void Initialize() { base.Initialize(); }

        protected override void LoadContent()
        {
            SpriteBatch = new SpriteBatch(GraphicsDevice);
            ResourceManager.LoadContent(Content);

            #region Map
            map = new Map(new Point(160, 80));
            map["tile1"] = new Tile(TileType.Ground, new Spritesheet("tile1", new Point(0, 0), new Rectangle(0, 0, 160, 80)));
            map["tile2"] = new Tile(TileType.Wall, new Spritesheet("tile2", new Point(0, 0), new Rectangle(0, 0, 160, 80)));
            map["tile3"] = new Tile(TileType.Water, new Spritesheet("tile1", new Point(0, 0), new Rectangle(0, 0, 160, 80)));
            //map.Load("Map1.dat");

            map.Create(new Point(10, 20));
            map.MapData[3, 3] = "tile2";
            map.MapData[3, 4] = "tile2";
            map.MapData[3, 5] = "tile2";
            map.MapData[3, 6] = "tile2";
            map.MapData[4, 6] = "tile2";
            map.MapData[4, 6] = "tile2";
            map.MapData[6, 6] = "tile2";
            map.MapData[7, 6] = "tile2";
            map.MapData[7, 6] = "tile2";
            map.MapData[7, 7] = "tile2";
            map.MapData[7, 8] = "tile2";
            map.MapData[7, 9] = "tile2";
            map.Save("Map1.dat").Load("Map1.dat");

            #endregion

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
        protected override void UnloadContent() { }

        protected override void Update(GameTime gameTime)
        {
            map.Update();
            player.Update();
            Camera.Update();
            Camera.Focus = player.Position;
            Input.Update();
            Time.Update(gameTime);
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            SpriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp, null, null, null, Camera.Transform);
            map.Draw();
            player.Draw();
            SpriteBatch.End();

            SpriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp, null, null, null, null);
            host.Bounds = new Rectangle(0, 0, GraphicsDevice.PresentationParameters.BackBufferWidth, GraphicsDevice.PresentationParameters.BackBufferHeight);
            host.Render();
            SpriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
