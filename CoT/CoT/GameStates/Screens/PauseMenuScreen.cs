using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Button = Myra.Graphics2D.UI.Button;
using Keys = Microsoft.Xna.Framework.Input.Keys;

namespace CoT.GameStates.Screens
{
    public class PauseMenuScreen : GameScreen
    {
        public PauseMenuScreen(bool isPopup) : base(isPopup)
        {
        }

        public override void Load()
        {
            Button resumeButton = new Button
            {
                Text = "Resume",
                TextColor = Color.Red,
                PaddingLeft = 20,
                PaddingRight = 20,
                PaddingBottom = 10,
                PaddingTop = 10,
                GridPositionX = 0,
                GridPositionY = 1
            };
            resumeButton.Up += (s, a) =>
            {
                ScreenManager.RemoveScreen(this);
            };
            Grid.Widgets.Add(resumeButton);

            Button exitButton = new Button
            {
                Text = "Exit to menu",
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
                ScreenManager.ChangeScreen(new MainMenuScreen(false));
            };
            Grid.Widgets.Add(exitButton);

            base.Load();
        }

        public override void Update()
        {
            if (Input.IsKeyPressed(Keys.Escape))
            {
                ScreenManager.RemoveScreen(this);
            }
            base.Update();
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
        }

        public override void DrawUserInterface(SpriteBatch spriteBatch)
        {
            ScreenManager.DrawBlackRectangle(spriteBatch, 0.5f, 1f);
            base.DrawUserInterface(spriteBatch);
        }
    }
}
