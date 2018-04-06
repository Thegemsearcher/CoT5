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

        public ParticleManager()
        {
            Instance = this;
        }

        public void Initialize()
        {
        }

        public void LoadContent()
        {
        }

        public void Update()
        {
        }

        public void Draw(SpriteBatch sb)
        {
        }

        public void DrawUserInterface(SpriteBatch spriteBatch)
        {
        }
    }
}
