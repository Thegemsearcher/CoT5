using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.BitmapFonts;
using Myra;
using Myra.Graphics2D.UI;

namespace CoT
{
    public class MainMenuScreen : GameScreen
    {
        public MainMenuScreen() : base ()
        {
        }

        public override void Activate()
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

            playButton.Down += (s, a) =>
            {
                LoadingScreen.Load(ScreenManager, new GameplayScreen());
                //ScreenManager.AddScreen(new OptionsMenuScreen());
                //Grid.Widgets.Remove(button);
                //ExitScreen();
            };

            Grid.Widgets.Add(playButton);
            Game1.Game.host.Widgets.Add(Grid);
            base.Activate();
        }

        public override void Update()
        {
            base.Update();
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
        }
    }
}
