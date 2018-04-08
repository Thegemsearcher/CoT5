using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CoT
{
    public class LoadingScreen : GameScreen
    {
        public GameScreen[] ScreensToLoad { get; set; }
        public bool PerformLoad { get; set; }

        public LoadingScreen(GameScreen[] screensToLoad)
        {
            ScreensToLoad = screensToLoad;
            FadeInTransitionOn = true;
            TransitionOnTime = TimeSpan.FromSeconds(0.1f);
        }

        public static void Load(ScreenManager screenManager, params GameScreen[] screensToLoad)
        {
            for (var i = 0; i < screenManager.Screens.Count; i++)
            {
                screenManager.Screens[i].ExitScreen();
            }

            screenManager.AddScreen(new LoadingScreen(screensToLoad));
        }

        public override void Update()
        {
            if (PerformLoad)
            {
                ScreenManager.RemoveScreen(this);
                foreach (GameScreen screen in ScreensToLoad)
                {
                    if (screen != null)
                    {
                        ScreenManager.AddScreen(screen);
                    }
                }
            }
            base.Update();
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (ScreenState == ScreenState.Active && ScreenManager.Screens.Count.Equals(1))
            {
                PerformLoad = true;
            }

            base.Draw(spriteBatch);
        }

        public override void DrawUserInterface(SpriteBatch spriteBatch)
        {
            if (ScreenState == ScreenState.Active)
            {
                ScreenManager.DrawBlackRectangle(spriteBatch, 1);
            }

            base.DrawUserInterface(spriteBatch);

            spriteBatch.DrawString(ResourceManager.Get<SpriteFont>("font1"), "Loading ...", new Vector2(300, 300), Color.White);
        }
    }
}
