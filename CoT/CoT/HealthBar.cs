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
        private Vector2 position;
        private Color hpState;
        private int maxHP, currentHP;
        private float percent = 1;
        Rectangle drawBox, backgroundBox;

        public HealthBar(int maxHP, Vector2 position)
        {
            this.maxHP = maxHP;
            currentHP = maxHP;
            hpState = Color.Green;
            drawBox = new Rectangle(50 , 50,200 * (int)percent, 25);
            backgroundBox = new Rectangle(49, 49, 202, 27);
        }
        public void UpdateHP(int hp, int maxHealth)
        {
            maxHP = maxHealth;
            currentHP = hp;
            percent = (float)hp / (float)maxHP;
            drawBox.Width = (int)(200 * percent);

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
            sb.Draw(texture, backgroundBox, Color.Gray);
            sb.Draw(texture, drawBox, hpState);
        }
    }
}
