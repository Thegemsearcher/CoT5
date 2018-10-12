using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
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
            ResourceManager.RegisterResource<Song>(content.Load<Song>("DiabloIntro"), "DiabloIntro");
            SoundManager.Instance.PlaySong("DiabloIntro");
            GameManager.Instance.Penumbra.Enabled = false;
            base.Load();
        }

        public override void Unload()
        {
            base.Unload();
        }

        private float particleTimer = 0;

        public override void Update()
        {
            particleTimer += Time.DeltaTime * 20;
            if (particleTimer > 1)
            {
                particleTimer = 0;
                ParticleManager.CreateStandard(new Vector2(Game1.ScreenWidth / 3.4f, Game1.ScreenHeight / 3f).ScreenToWorld(), new Vector2(Game1.Random.NextFloat(-1, 1), Game1.Random.NextFloat(-1, 1)), new Color(255, 100, 100, 70), 300f, 1f, 0.5f);
                ParticleManager.CreateStandard(new Vector2(Game1.ScreenWidth / 1.4f, Game1.ScreenHeight / 3f).ScreenToWorld(), new Vector2(Game1.Random.NextFloat(-1, 1), Game1.Random.NextFloat(-1, 1)), new Color(255, 100, 100, 70), 300f, 1f, 0.5f);

                if (Game1.Random.Next(0, 5) == 0)
                ParticleManager.CreateStandard(new Vector2(Game1.ScreenWidth / 2f, Game1.ScreenHeight / 1.4f).ScreenToWorld(), new Vector2(Game1.Random.NextFloat(-1, 1), Game1.Random.NextFloat(-1, 1)), new Color(100, 100, 255, 255), 100, 5f, 4);

                ParticleManager.CreateStandard(new Vector2(Game1.Random.Next(0, Game1.ScreenWidth), Game1.ScreenHeight / 0.9f).ScreenToWorld(), new Vector2(Game1.Random.NextFloat(-0.5f, 0.5f), Game1.Random.NextFloat(-1f, 0)), new Color(140, 90, 60), 300, 0.5f, 4f);
            }
            base.Update();
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
        }

        public override void DrawUserInterface(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(ResourceManager.Get<Texture2D>("cot"), new Rectangle(0, 0, Game1.ScreenWidth, Game1.ScreenHeight), null, Color.Gray, 0f, Vector2.Zero, SpriteEffects.None, 0.9f);
            spriteBatch.DrawString(ResourceManager.Get<SpriteFont>("titlefont"), "Crypt of Traitors", new Vector2(Game1.ScreenWidth / 5f, Game1.ScreenHeight / 15f), Color.DarkRed * 0.8f, 0f, Vector2.Zero, 1.1f, SpriteEffects.None, 1f);
            base.DrawUserInterface(spriteBatch);
        }
    }
}
