using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoT
{
    class HealthBar
    {
        private Texture2D texture = ResourceManager.Get<Texture2D>("rectangle");
        private Vector2 size;
        public Vector2 Position { get; set; }
        private Color hpState;
        private int maxHP, currentHP;
        private float percent = 1;
        private Rectangle drawBox, backgroundBox;

        public HealthBar(int maxHP, Vector2 size, Vector2 position)
        {
            Position = position;
            this.maxHP = maxHP;
            this.size = size;
            currentHP = maxHP;
            hpState = Color.Green;
            drawBox = new Rectangle((int) position.X ,(int)position.Y,(int)size.X * (int)percent, (int)size.Y);
            backgroundBox = new Rectangle((int)position.X - 1, (int)position.Y - 1, (int)size.X * (int)percent + 2, (int)size.Y + 2);
        }
        public void UpdateHP(int hp, int maxHealth)
        {
            maxHP = maxHealth;
            currentHP = hp;
            percent = (float)hp / (float)maxHP;
            drawBox.Width = (int)(size.X * percent);

            if (percent < 0.3)
            {
                hpState = Color.Red;
            }
            else if (percent < 0.6)
            {
                hpState = Color.Yellow;
            } else if (percent > 0.5)
            {
                hpState = Color.Green;
            }
        }
        public void Draw(SpriteBatch sb)
        {
            sb.Draw(texture, backgroundBox, new Rectangle(0, 0, texture.Width, texture.Height), Color.Gray, 0f, Vector2.Zero, SpriteEffects.None, 0.1f);
            sb.Draw(texture, drawBox, new Rectangle(0, 0, texture.Width, texture.Height), hpState, 0f, Vector2.Zero, SpriteEffects.None, 0.2f);
        }
    }
}
