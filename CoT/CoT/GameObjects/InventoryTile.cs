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
    public class InventoryTile : GameObject
    {
        private Texture2D pixel;
        public Rectangle rectangle;

        public bool occupied = false;

        public InventoryTile(Vector2 position, int tileSize, Texture2D pixel, string texture, Rectangle sourceRectangle) : base(texture, position, sourceRectangle)
        {
            this.pixel = pixel;
            rectangle = new Rectangle((int)position.X, (int)position.Y, tileSize, tileSize);
        }

        public override void Update()
        {
            bool itemIsOccupying = false;
            foreach (Item item in ItemManager.Instance.Items)
            {
                if (rectangle.Intersects(item.rectItemInv))
                {
                    itemIsOccupying = true;
                }
            }
            if (!itemIsOccupying)
                occupied = false;
            else occupied = true;
        }

        public override void Draw(SpriteBatch sb)
        {
            sb.Draw(pixel, rectangle, null, Color.White * 0.5f, 0f, Vector2.Zero, SpriteEffects.None, 0.4f);
        }

        public override void OnRemove()
        {
            throw new NotImplementedException();
        }
    }
}
