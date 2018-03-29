using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CoT
{
    public class Creature : GameObject
    {
        public Creature(string texture, Vector2 position, Rectangle sourceRectangle) : base(texture, position, sourceRectangle)
        {
        }

        public override void OnRemove()
        {
            throw new NotImplementedException();
        }

        public virtual void Move()
        {

        }
        public virtual void Attack()
        {

        }
        public virtual void GetHit()
        {

        }
        public virtual void Die()
        {

        }

        public override void Update()
        {
            base.Update();
            Move();
        }

        public override void Draw()
        {
            base.Draw();
        }
    }
}
