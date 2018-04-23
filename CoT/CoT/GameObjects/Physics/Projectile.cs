using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CoT
{
    public class Projectile : PhysicsObject
    {
        public Creature Owner { get; set; }
        public Projectile(string texture, Vector2 position, Rectangle sourceRectangle, Vector2 direction, float speed) : base(texture, position, sourceRectangle, direction, speed)
        {
        }

        public override void Update()
        {
            base.Update();

            for (int i = 0; i < 3; i++)
            {
                Color color = Color.Red;
                if (i == 0){
                    color = Color.Orange;
                }
                ParticleManager.Instance.Particles.Add(new Particle("lightMask", Position,
                   new Rectangle(0, 0, ResourceManager.Get<Texture2D>("lightMask").Width, ResourceManager.Get<Texture2D>("lightMask").Height),
                   Direction, 150f, 2f, color, 0f, 0.1f));
            }
           
        }

        public override void Draw(SpriteBatch sb)
        {
            sb.Draw(ResourceManager.Get<Texture2D>(Texture), Position, SourceRectangle, Color * Transparency, Rotation, Vector2.Zero, Scale, SpriteEffects.None, LayerDepth);
            //base.Draw(sb);
        }
    }
}
