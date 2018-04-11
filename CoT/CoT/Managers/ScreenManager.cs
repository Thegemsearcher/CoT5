using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CoT
{
    public class ScreenManager : IManager
    {
        public static ScreenManager Instance { get; set; }
        public List<GameScreen> Screens { get; set; } = new List<GameScreen>();

        public ScreenManager()
        {
            Console.WriteLine("ScreenManager - Constructor");

            Instance = this;
        }

        public bool ContainsScreenType(Type type)
        {
            return Screens.Any(x => x.GetType() == type);
        }

        public void AddScreen(GameScreen screen)
        {
            screen.Load();
            Screens.Add(screen);
        }

        public void RemoveScreen(GameScreen screen)
        {
            screen.Unload();
            Screens.Remove(screen);
        }

        public void ChangeScreen(GameScreen screen)
        {
            for (int i = Screens.Count - 1; i >= 0; i--)
            {
                RemoveScreen(Screens[i]);
            }
            AddScreen(screen);
        }

        public void Initialize()
        {
            Console.WriteLine("ScreenManager - Initialize");
        }

        public void LoadContent()
        {
            Console.WriteLine("ScreenManager - LoadContent");
            AddScreen(new MainMenuScreen(false));
        }

        public void UnloadContent()
        {
            Console.WriteLine("ScreenManager - UnloadContent");
            Screens.ForEach(x => x.Unload());
        }

        public void Update()
        {
            GameScreen firstScreen = Screens[Screens.Count - 1];
            for (int i = Screens.Count - 1; i >= 0; i--)
            {
                GameScreen currentScreen = Screens[i];

                if (currentScreen == firstScreen || currentScreen.IsPopup)
                {
                    Screens[i].Update();
                }
                else
                {
                    break;
                }
            }

            Screens.ForEach(GameDebugger.WriteLine);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            Screens.ForEach(x => x.Draw(spriteBatch));
        }

        public void DrawToWorldAdditiveBlend(SpriteBatch spriteBatch)
        {
        }

        public void DrawUserInterface(SpriteBatch spriteBatch)
        {
            Screens.ForEach(x => x.DrawUserInterface(spriteBatch));
        }

        public void DrawBlackRectangle(SpriteBatch spriteBatch, float alpha, float layerDepth)
        {
            spriteBatch.Draw(ResourceManager.Get<Texture2D>("rectangle"), GameManager.Instance.Game.GraphicsDevice.Viewport.Bounds, null, Color.Black * alpha, 0f, Vector2.Zero, SpriteEffects.None, layerDepth);
        }
    }
}
