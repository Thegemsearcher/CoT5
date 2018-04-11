﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Xna.Framework;
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

            base.Load();
        }

        public override void Unload()
        {
            base.Unload();
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
