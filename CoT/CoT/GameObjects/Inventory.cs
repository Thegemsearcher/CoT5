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
    public class Inventory
    {
        Texture2D pixel = new Texture2D(Game1.Game.GraphicsDevice, 1, 1);
        Color[] colorData = { Color.DarkGray };

        Rectangle rectangle;

        public static bool active = false;
        
        public Inventory()
        {
            pixel.SetData(colorData);
            rectangle = new Rectangle(Game1.ScreenWidth - 420, 20, 400, Game1.ScreenHeight - 40);
        }

        public void Update()
        {
            if (Input.CurrentKeyboard.IsKeyDown(Keys.I) && Input.LastKeyboard.IsKeyUp(Keys.I))
            {
                if (active)
                {
                    active = false;
                }
                else active = true;
            }
        }

        public void Draw(SpriteBatch sb)
        {
            if (active)
            {
                sb.Draw(pixel, rectangle, Color.White);
                sb.DrawString(ResourceManager.Get<SpriteFont>("font1"), "PlcaeHolderText", new Vector2(rectangle.X + 130, rectangle.Y + 20), Color.White);
            }
        }
    }

    class InvTile
    {
        Rectangle rectangle;
        bool occupied;

        public bool TileIsFree()
        {
            return false;
        }
    }
}


