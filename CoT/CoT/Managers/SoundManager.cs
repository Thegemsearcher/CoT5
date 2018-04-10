using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CoT
{
    public class SoundManager : IManager
    {
        public static SoundManager Instance { get; set; }

        public SoundManager()
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

        public void DrawToWorldAdditiveBlend(SpriteBatch spriteBatch)
        {
        }

        public void DrawUserInterface(SpriteBatch spriteBatch)
        {
        }
    }
}
