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
    class InventoryTile : GameObject
    {
        Texture2D pixel;
        Rectangle rectangle;

        public InventoryTile(Vector2 position, int tileSize, Texture2D pixel, string texture, Rectangle sourceRectangle) : base(texture, position, sourceRectangle)
        {
            this.pixel = pixel;
            rectangle = new Rectangle((int)position.X, (int)position.Y, tileSize, tileSize);
        }

        public void Update()
        {
        }

        public void Draw(SpriteBatch sb)
        {
            sb.Draw(pixel, rectangle, null, Color.White, 0f, Vector2.Zero, SpriteEffects.None, 1);
        }

        public override void OnRemove()
        {
            throw new NotImplementedException();
        }
    }
}
