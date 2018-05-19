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
        private enum HPState
        {
            green,
            yellow,
            red
        }
        private HPState hpState;
        private int maxHP, currentHP;
        private float percent = 1;
        Rectangle drawBox;

        public HealthBar(int maxHP, Vector2 position)
        {
            this.maxHP = maxHP;
            currentHP = maxHP;
            hpState = HPState.green;
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
                hpState = HPState.red;
            }
            else if (percent < 0.5)
            {
                hpState = HPState.yellow;
            }
        }
        public void draw(SpriteBatch sb)
        {
           
        }
    }
}
