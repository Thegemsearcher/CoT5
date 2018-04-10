using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CoT
{
    public class PhysicsObject : GameObject
    {
        public Vector2 Direction { get; set; }
        public float Speed { get; set; }

        public PhysicsObject(string texture, Vector2 position, Rectangle sourceRectangle, Vector2 direction, float speed) : base(texture, position, sourceRectangle)
        {
            Direction = direction;
            Speed = speed;
        }

        public override void OnRemove()
        {
        }

        public override void Update()
        {
            Position += Direction * Speed * Time.DeltaTime;

            base.Update();
        }

        public override void Draw(SpriteBatch sb)
        {
            base.Draw(sb);
        }
    }
}
