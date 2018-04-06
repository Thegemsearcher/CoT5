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
    class Inventory
    {
        Texture2D colorPixel = new Texture2D(Game1.Game.GraphicsDevice, 1, 1);
        Color[] colorData = { Color.White, };

        Rectangle rectangle;
        Vector2 screenPos;

        int rectXOffset = 400,
            rectYOffset = 0;

        public static bool active = false;
        

        public Inventory()
        {
            colorPixel.SetData<Color>(colorData);
        }

        public void Update()
        {
            screenPos = new Vector2(Game1.Game.Window.ClientBounds.Left, Game1.Game.Window.ClientBounds.Top);
            rectangle = new Rectangle((int)screenPos.X + rectXOffset, (int)screenPos.Y, 200, 200);

            if (Input.CurrentKeyboard.IsKeyDown(Keys.I) && Input.LastKeyboard.IsKeyUp(Keys.I))
            {
                if (active)
                {
                    active = false;
                }
                else active = true;
            }
        }

        public void Draw()
        {
            if (active)
            {
                Game1.Game.SpriteBatch.Draw(colorPixel, new Rectangle(0, 0, 300, 300), Color.Purple);
                Game1.Game.SpriteBatch.DrawString(ResourceManager.Get<SpriteFont>("font1"), "text", new Vector2(rectangle.X + 20, rectangle.Y + 20), Color.White);
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
