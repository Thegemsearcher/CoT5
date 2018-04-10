using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Console = System.Console;

namespace CoT
{
    public class Particle : PhysicsObject
    {
        public float FadeSpeed { get; set; }

        public Particle(string texture, Vector2 position, Rectangle sourceRectangle, Vector2 direction, float speed, float fadeSpeed, Color color, float rotation = 0, float scale = 1) : 
            base(texture, position, sourceRectangle, direction, speed)
        {
            FadeSpeed = fadeSpeed;
            Color = color;
            Rotation = rotation;
            Scale = scale;

            LayerDepth = 1f;
        }

        public override void Update()
        {
            Transparency -= Time.DeltaTime * FadeSpeed;
            if (Transparency <= 0)
            {
                Remove = true;
            }

            base.Update();
        }

        public override void Draw(SpriteBatch sb)
        {
            base.Draw(sb);
        }
    }
}
