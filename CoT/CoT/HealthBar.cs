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
        private Texture2D texture;
        private Vector2 position;
        private Color hpState;
        private int maxHP, currentHP;
        private float percent = 1;
        Rectangle drawBox;

        public HealthBar(int maxHP, Vector2 position)
        {
            this.maxHP = maxHP;
            currentHP = maxHP;
            hpState = Color.Green;
            drawBox = new Rectangle((int)position.X, (int)position.Y,100 * (int)percent, 10);
        }
        public void UpdateHP(int hp, int maxHealth)
        {
            maxHP = maxHealth;
            currentHP = hp;
            percent = (float)hp / (float)maxHP;
            drawBox.Width = 100 * (int)percent;

            if (percent < 0.2)
            {
                hpState = Color.Red;
            }
            else if (percent < 0.5)
            {
                hpState = Color.Yellow;
            } else if (percent > 0.5)
            {
                hpState = Color.Green;
            }
        }
        public void draw(SpriteBatch sb)
        {
            sb.Draw(texture, drawBox, hpState);
        }
    }
}
