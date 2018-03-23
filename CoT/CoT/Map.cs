using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CoT
{
    class Map
    {
        public Map()
        {
        }

        public void Update()
        {
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            for (int i = 0; i < 100; i++)
            {
                for (int j = 0; j < 100; j++)
                {
                    if (new Vector2(
                            Convert.ToInt32(Vector2.Divide(Camera.ScreenToWorld(Input.CurrentMousePosition), new Vector2(80, 80)).ToScreen().X - 1),
                            Convert.ToInt32(Vector2.Divide(Camera.ScreenToWorld(Input.CurrentMousePosition), new Vector2(80, 80)).ToScreen().Y - 0)) == new Vector2(i, j))
                    {
                        spriteBatch.Draw(ResourceManager.Get<Texture2D>("tile1"), new Vector2(i * 80, j * 80).ToWorld(), Color.Red);
                    }
                    else
                    {
                        spriteBatch.Draw(ResourceManager.Get<Texture2D>("tile1"), new Vector2(i * 80, j * 80).ToWorld(), Color.White);
                    }
                }
            }
        }
    }
}