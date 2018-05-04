using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CoT
{
    public class ParticleManager : IManager
    {
        public static ParticleManager Instance { get; set; }

        public List<Particle> Particles { get; set; }

        public static void CreateStandard(Vector2 position, Vector2 direction, Color color)
        {
            Instance.Particles.Add(new Particle(new Spritesheet("lightMask", new Point(1, 1), 
                new Rectangle(0, 0, ResourceManager.Get<Texture2D>("lightMask").Width, ResourceManager.Get<Texture2D>("lightMask").Height)), 
                position, direction, 300f, 2f, color, 0f, 0.3f));
        }

        public ParticleManager()
        {
            Instance = this;
        }

        public void Initialize()
        {
            Particles = new List<Particle>();
        }

        public void LoadContent()
        {
        }

        public void Update()
        {
            Particles.ForEach(x => x.Update());

            for (int i = Particles.Count - 1; i >= 0; i--)
            {
                if (Particles[i].Remove)
                {
                    Particles[i].OnRemove();
                    Particles.RemoveAt(i);
                }
            }
        }

        public void Draw(SpriteBatch sb)
        {
        }

        public void DrawToWorldAdditiveBlend(SpriteBatch spriteBatch)
        {
            Particles.ForEach(x => x.Draw(spriteBatch));
        }

        public void DrawUserInterface(SpriteBatch spriteBatch)
        {
        }
    }
}
