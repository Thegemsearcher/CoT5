using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace CoT {
    public class PhysicsObject : GameObject {
        public PhysicsObject(Vector2 position) : base(position) {
        }
        public virtual void Update(GameTime gameTime) {

        }
    }
}
