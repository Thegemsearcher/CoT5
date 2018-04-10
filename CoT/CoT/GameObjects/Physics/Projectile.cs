using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CoT
{
    public class Projectile : PhysicsObject
    {
        public Projectile(string texture, Vector2 position, Rectangle sourceRectangle, Vector2 direction, float speed) : base(texture, position, sourceRectangle, direction, speed)
        {
        }

        public override void Update()
        {
            base.Update();
        }

        public override void Draw(SpriteBatch sb)
        {
            base.Draw(sb);
        }
    }
}
