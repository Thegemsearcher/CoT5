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
    public static class TextDebug
    {
        private static List<string> Texts { get; set; } = new List<string>();
        private static bool showDebug = false;

        public static void WriteLine(object text)
        {
            Texts.Add(text.ToString());
        }

        public static void Draw()
        {
            if (Input.IsKeyPressed(Keys.F8)) showDebug = !showDebug;

            if (showDebug)
            {
                Texts.Select((value, index) => new {value, index}).ForEach(x =>
                {
                    Game1.Game.SpriteBatch.DrawString(ResourceManager.Get<SpriteFont>("font1"), x.value,
                        new Vector2(300, x.index * ResourceManager.Get<SpriteFont>("font1").MeasureString("|").Y),
                        Color.Yellow);
                });
                Texts.Clear();
            }
        }
    }
}