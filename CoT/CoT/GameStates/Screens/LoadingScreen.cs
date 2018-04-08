using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;

namespace CoT
{
    public class LoadingScreen : GameScreen
    {
        public GameScreen[] ScreensToLoad { get; set; }

        public LoadingScreen(GameScreen[] screensToLoad)
        {
            ScreensToLoad = screensToLoad;
        }

        public static void Load(ScreenManager screenManager, params GameScreen[] screensToLoad)
        {
            foreach (GameScreen screen in screenManager.Screens)
                screen.ExitScreen();
            
            screenManager.AddScreen(new LoadingScreen(screensToLoad));
        }

        public override void Update()
        {
            ScreenManager.RemoveScreen(this);

            foreach (GameScreen screen in ScreensToLoad)
            {
                if (screen != null)
                {
                    ScreenManager.AddScreen(screen);
                }
            }

            base.Update();
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
        }
    }
}
