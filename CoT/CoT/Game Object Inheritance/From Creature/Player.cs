using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Penumbra;
using Console = System.Console;

namespace CoT
{
    public class Player : Creature
    {
        enum HeroClass
        {

        }

        private Penumbra.Light light;

        public Player(string texture, Vector2 position, Rectangle sourceRectangle) : base(texture, position, sourceRectangle)
        {
            light = new PointLight();
            light.Scale = new Vector2(5000, 5000).ToScreen();
            light.Intensity = 0.5f;
            Game1.Game.Penumbra.Lights.Add(light);
        }

        public override void Update()
        {
            base.Update();
            light.Position = Position;
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
