using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace CoT {
    public class Creature : GameObject {
        public Creature(Vector2 position) : base(position) {
        }
        public virtual void Update(GameTime gameTime) {
            Move();
        }
        public virtual void Move() {

        }
        public virtual void Attack() {

        }
        public virtual void GetHit() {

        }
        public virtual void Die() {

        }
    }
}
