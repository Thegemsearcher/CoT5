using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CoT
{
    public class ProjectileManager : IManager
    {
        public static ProjectileManager Instance { get; set; }

        public List<Projectile> Projectiles { get; set; }

        public ProjectileManager()
        {
            Instance = this;
        }

        public void Initialize()
        {
            Projectiles = new List<Projectile>();
        }

        public void LoadContent()
        {
        }

        public void Update()
        {
            Projectiles.ForEach(x => x.Update());
        }

        public void Draw(SpriteBatch sb)
        {
            Projectiles.ForEach(x => x.Draw(sb));
        }

        public void DrawToWorldAdditiveBlend(SpriteBatch spriteBatch)
        {
        }

        public void DrawUserInterface(SpriteBatch spriteBatch)
        {
        }
    }
}
