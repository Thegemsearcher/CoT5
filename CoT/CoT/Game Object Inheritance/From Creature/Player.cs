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

        float speed = 0.01f;
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
            Scale = 3;
        }

        public override void Update()
        {
            base.Update();
            light.Position = Position;
            if (Input.IsLeftClickPressed)
            {
                Position = Camera.ScreenToWorld(Input.CurrentMousePosition);
            }

            Move(GetDirection(Position, Input.CurrentMousePosition));
        }

        public void Move(Vector2 direction)
        {
            Position += direction * speed * Time.DeltaTime;
        }



        public Vector2 GetDirection(Vector2 currentPos, Vector2 targetPos)
        {
            Vector2 travellDirection = targetPos - currentPos;

            travellDirection.Normalize();

            return travellDirection;
        }

        public override void Draw()
        {
            base.Draw();
        }
    }
}
