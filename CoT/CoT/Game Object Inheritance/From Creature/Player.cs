using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace CoT {
    class Player : Creature {
        enum heroClass {

        }
        public Texture2D pC;
        MouseState mS;
        Vector2 mousePosition;
        Vector2 position;


        public Player(Vector2 position) : base(position) {
            this.position = position;
        }
        public override void Update(GameTime gameTime) {


            if (mS.LeftButton == ButtonState.Pressed && mS.LeftButton != ButtonState.Released && mousePosition != position)
            {
                position = mousePosition;
            }
        }
    }
}
