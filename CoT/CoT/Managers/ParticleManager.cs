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
