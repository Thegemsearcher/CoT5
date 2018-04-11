using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace CoT
{
    public static class GameDebugger
    {
        private static List<string> Texts { get; set; } = new List<string>();
        public static bool Debug = false;

        public static void WriteLine(object text)
        {
            Texts.Add(text.ToString());
        }

        public static void Update()
        {
            if (Input.IsKeyPressed(Keys.F8)) Debug = !Debug;
        }

        private static void DrawTexts(SpriteBatch sb)
        {
            Texts.Select((value, index) => new { value, index }).ForEach(x =>
            {
                sb.DrawString(ResourceManager.Get<SpriteFont>("font1"), x.value,
                    new Vector2(300 + 1, x.index * ResourceManager.Get<SpriteFont>("font1").MeasureString("|").Y + 1),
                    Color.Black, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0.9f);

                sb.DrawString(ResourceManager.Get<SpriteFont>("font1"), x.value,
                    new Vector2(300, x.index * ResourceManager.Get<SpriteFont>("font1").MeasureString("|").Y),
                    Color.Yellow, 0f, Vector2.Zero, 1f, SpriteEffects.None, 1f);
            });
            Texts.Clear();
        }

        public static void DrawToScreen(SpriteBatch sb)
        {
            if (!Debug) return;
            DrawTexts(sb);

            if (ScreenManager.Instance.ContainsScreenType(typeof(GameplayScreen)))
            {
                WriteLine("Mouse position to Tile Index: " + GameplayScreen.Instance.Map.GetTileIndex(Input.CurrentMousePosition.ScreenToWorld()));
                WriteLine("Tile Index to Mouse position: " + GameplayScreen.Instance.Map.GetTilePosition(GameplayScreen.Instance.Map.GetTileIndex(Input.CurrentMousePosition.ScreenToWorld())));
                WriteLine("Mouse Position: " + Input.CurrentMousePosition);
                WriteLine("Player Position: " + GameplayScreen.Instance.Player.Position);

                Point posWorldToScreen = GameplayScreen.Instance.Player.Position.WorldToScreen().ToPoint();
                Rectangle tempRec2 = new Rectangle(100, 100, posWorldToScreen.X - 100, posWorldToScreen.Y - 100);
                sb.Draw(ResourceManager.Get<Texture2D>("rectangle"), tempRec2, Color.LightGreen * 0.4f);
                WriteLine("Lightgreen Rectangle: " + tempRec2);
            }
        }
        public static void DrawToWorld(SpriteBatch sb)
        {
            if (!Debug) return;
            if (ScreenManager.Instance.ContainsScreenType(typeof(GameplayScreen)))
            {
                Point pos = Input.CurrentMousePosition.ToPoint();
                Point posScreenToWorld = Input.CurrentMousePosition.ScreenToWorld().ToPoint();
                Point posWorldToScreen = GameplayScreen.Instance.Player.Position.WorldToScreen().ToPoint();


                sb.Draw(ResourceManager.Get<Texture2D>("rectangle"), new Rectangle(0, 0, Game1.ScreenWidth, Game1.ScreenHeight), Color.Red * 0.2f);

                sb.Draw(ResourceManager.Get<Texture2D>("rectangle"), new Rectangle(posScreenToWorld.X - 5, posScreenToWorld.Y - 5, 10, 10), Color.LightGreen * 0.3f);


                Vector2 tempPos = GameplayScreen.Instance.Map.GetTilePosition(GameplayScreen.Instance.Map.GetTileIndex(Input.CurrentMousePosition.ScreenToWorld()));
                sb.Draw(ResourceManager.Get<Texture2D>("rectangle"),
                    new Rectangle((int)tempPos.X, (int)tempPos.Y, 10, 10), Color.Black * 0.2f);

                sb.Draw(ResourceManager.Get<Texture2D>("rectangle"), new Rectangle(pos.X, pos.Y, 10, 10), Color.White * 1f);

                Rectangle tempRec = new Rectangle(
                    new Vector2(100, 100).ScreenToWorld().ToPoint().X,
                    new Vector2(100, 100).ScreenToWorld().ToPoint().Y,
                    (int)((posWorldToScreen.X - 100) / Camera.Transform.Scale.X),
                    (int)((posWorldToScreen.Y - 100) / Camera.Transform.Scale.Y));

                sb.Draw(ResourceManager.Get<Texture2D>("rectangle"), tempRec, Color.Black * 0.3f);
                WriteLine("Black Rectangle: " + tempRec);
            }
        }
    }
}