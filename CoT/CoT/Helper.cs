using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CoT
{
    public static class Helper
    {
        public static Texture2D CreateCircleTexture(int radius)
        {
            int diameter = radius * 2;
            Texture2D texture = new Texture2D(Game1.Game.GraphicsDevice, diameter, diameter);
            Color[] colorData = new Color[diameter * diameter];
            float radiusSq = radius * radius;

            for (int x = 0; x < diameter; x++)
            {
                for (int y = 0; y < diameter; y++)
                {
                    int index = x * diameter + y;
                    Vector2 pos = new Vector2(x - radius, y - radius);
                    if (pos.LengthSquared() <= radiusSq)
                    {
                        colorData[index] = Color.White;
                    }
                    else
                    {
                        colorData[index] = Color.Transparent;
                    }
                }
            }
            texture.SetData(colorData);
            return texture;
        }

        public static Texture2D CreateRectangleTexture(Point size)
        {
            Texture2D texture = new Texture2D(Game1.Game.GraphicsDevice, size.X, size.Y);
            texture.SetData(Enumerable.Range(0, size.X * size.Y).Select(x => Color.White).ToArray());
            return texture;
        }
    }
}
