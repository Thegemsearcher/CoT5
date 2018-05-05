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

        public Projectile(Spritesheet spritesheet, Vector2 position, Vector2 direction, float speed) : base(spritesheet, position, direction, speed)
        {
            this.Direction = direction;
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

                ParticleManager.CreateStandard(Position, Direction, color, 0.1f);
                //ParticleManager.Instance.Particles.Add(new Particle("lightMask", Position,
                //   new Rectangle(0, 0, ResourceManager.Get<Texture2D>("lightMask").Width, ResourceManager.Get<Texture2D>("lightMask").Height),
                //   Direction, 150f, 2f, color, 0f, 0.1f));
            }
           
        }
    }
}
