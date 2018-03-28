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
        
        
        
        


        public Player(Vector2 position) : base(position) {
            Position = position;

        }
        public override void Update() {

            
            if (Input.IsLeftClickPressed())
            {
                Position = Camera.ScreenToWorld(Input.CurrentMousePosition) ;

                Console.WriteLine(Input.CurrentMousePosition);
                Console.WriteLine(Position);
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(ResourceManager.Get<Texture2D>("Dude"), Position, Color.White);
        }
    }
}
