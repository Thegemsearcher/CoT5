using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.BitmapFonts;
using Myra;
using Button = Myra.Graphics2D.UI.Button;

namespace CoT
{
    public class MainMenuScreen : GameScreen
    {
        public MainMenuScreen(bool isPopup) : base (isPopup)
        {
        }

        public override void Load()
        {
            Button playButton = new Button
            {
                Text = "Play",
                TextColor = Color.Red,
                PaddingLeft = 20,
                PaddingRight = 20,
                PaddingBottom = 10,
                PaddingTop = 10,
                GridPositionX = 0,
                GridPositionY = 0
            };
            playButton.Up += (s, a) =>
            {
                ScreenManager.ChangeScreen(new GameplayScreen(false));
            };
            Grid.Widgets.Add(playButton);

            Button optionButton = new Button
            {
                Text = "Options",
                TextColor = Color.Red,
                PaddingLeft = 20,
                PaddingRight = 20,
                PaddingBottom = 10,
                PaddingTop = 10,
                GridPositionX = 0,
                GridPositionY = 1
            };
            optionButton.Up += (s, a) =>
            {
            };
            Grid.Widgets.Add(optionButton);

            Button exitButton = new Button
            {
                Text = "Exit",
                TextColor = Color.Red,
                PaddingLeft = 20,
                PaddingRight = 20,
                PaddingBottom = 10,
                PaddingTop = 10,
                GridPositionX = 0,
                GridPositionY = 2
            };
            exitButton.Up += (s, a) =>
            {
                Application.Exit();
            };
            Grid.Widgets.Add(exitButton);

            ContentManager content = Game1.Game.Content;
            ResourceManager.RegisterResource<Texture2D>(content.Load<Texture2D>("crypt of traitors crop"), "cot");
            GameManager.Instance.Penumbra.Enabled = false;
            base.Load();
        }

        public override void Unload()
        {
            base.Unload();
        }

        public override void Update()
        {
            ParticleManager.CreateStandard(new Vector2(Game1.MonitorWidth / 4, Game1.MonitorHeight / 4.2f).ScreenToWorld(), new Vector2(Game1.Random.NextFloat(-1, 1), Game1.Random.NextFloat(-1, 1)), new Color(255, 100, 100, 50), 1000f, 1f);
            ParticleManager.CreateStandard(new Vector2(Game1.MonitorWidth / 1.5f, Game1.MonitorHeight / 4.2f).ScreenToWorld(), new Vector2(Game1.Random.NextFloat(-1, 1), Game1.Random.NextFloat(-1, 1)), new Color(255, 100, 100, 50), 1000f, 1f);

            ParticleManager.CreateStandard(new Vector2(Game1.MonitorWidth / 2.2f, Game1.MonitorHeight / 1.6f).ScreenToWorld(), new Vector2(Game1.Random.NextFloat(-1, 1), Game1.Random.NextFloat(-1, 1)), new Color(200, 200, 200, 50), 300, 2f);

            ParticleManager.CreateStandard(new Vector2(Game1.Random.Next(0, Game1.MonitorWidth), Game1.MonitorHeight / 1.3f).ScreenToWorld(), new Vector2(Game1.Random.NextFloat(-0.5f, 0.5f), Game1.Random.NextFloat(-1f, 0)), new Color(70, 50, 50), 500, 0.1f, 3f);
            base.Update();
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
        }

        public override void DrawUserInterface(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(ResourceManager.Get<Texture2D>("cot"), new Rectangle(0, 0, Game1.ScreenWidth, Game1.ScreenHeight), null, Color.White, 0f, Vector2.Zero, SpriteEffects.None, 1f);
            base.DrawUserInterface(spriteBatch);
        }
    }
}
