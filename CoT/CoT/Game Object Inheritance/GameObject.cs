using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoT {
    public abstract class GameObject {
        protected bool isActive;
        protected bool remove;
        public Vector2 Position {
            get;
            set;
        }
        public GameObject(Vector2 position) {
            Position = position;
        }
        
    }
}
