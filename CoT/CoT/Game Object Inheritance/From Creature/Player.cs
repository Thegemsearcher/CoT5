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
    public class Player : Creature
    {
        enum HeroClass
        {

        }

        public Player(string texture, Vector2 position, Rectangle sourceRectangle) : base(texture, position, sourceRectangle)
        {
        }

        public override void Update()
        {
            base.Update();

            if (Input.IsLeftClickPressed)
            {
                Position = Camera.ScreenToWorld(Input.CurrentMousePosition);
            }
        }

        public override void Draw()
        {
            base.Draw();
        }
    }
}
