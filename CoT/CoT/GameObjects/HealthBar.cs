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
        private Texture2D texture = ResourceManager.Get<Texture2D>("healthbarframe"/*"rectangle"*/);
        private Texture2D texture2 = ResourceManager.Get<Texture2D>("healthbarticks");
        private Vector2 size;
        public Vector2 Position { get; set; }
        private int maxHP, currentHP;
        private float hpPercent = 1, overhpPercent = 0;
        private float Scale { get; set; }
        private Rectangle rectOverhealTicks, rectHealthTicks;

        public HealthBar(int maxHP, float scale, Vector2 position)
        {
            Position = position;
            Scale = scale;
            this.maxHP = maxHP;
            currentHP = maxHP;
            
            rectHealthTicks = new Rectangle((int)position.X + (int)(39 * Scale) - 1, (int)position.Y + (int)(39 * Scale), (int)(450 * Scale), (int)(39 * Scale) + 1);
            rectOverhealTicks = new Rectangle((int)position.X + (int)(39 * Scale) - 1, (int)position.Y + (int)(39 * Scale), (int)(450 * Scale), (int)(39 * Scale) + 1);

        }
        public void UpdateHP(int hp, int maxHealth)
        {
            maxHP = maxHealth;
            currentHP = hp;
            hpPercent = (float)hp / (float)maxHP;
            if (currentHP > maxHP)
            {
                hpPercent = 1f;
                overhpPercent = (float)(currentHP - maxHP) / (float)maxHP;
            }
            else if (currentHP <= maxHP)
                overhpPercent = 0f;

            rectHealthTicks.Width = (int)(texture2.Width * hpPercent * Scale);
            rectOverhealTicks.Width = (int)(texture2.Width * overhpPercent * Scale);
        }
        public void Draw(SpriteBatch sb)
        {
            sb.Draw(texture, new Vector2(Position.X, Position.Y), new Rectangle(0, 0, texture.Width, texture.Height), Color.White, 0f, Vector2.Zero, Scale, SpriteEffects.None, 0.1f);
            sb.Draw(texture2, rectHealthTicks, new Rectangle(0, 0, (int)(texture2.Width * hpPercent * Scale), texture2.Height), Color.DarkRed, 0f, Vector2.Zero, SpriteEffects.None, 0.2f);
            sb.Draw(texture2, rectOverhealTicks, new Rectangle(0, 0, (int)(texture2.Width * overhpPercent * Scale), texture2.Height), Color.Orange, 0f, Vector2.Zero, SpriteEffects.None, 0.3f);

            if (new Rectangle((int)Position.X, (int)Position.Y, texture.Width, texture.Height).Contains(Input.CurrentMousePosition))
            {
                sb.DrawString
                (
                    ResourceManager.Get<SpriteFont>("font1"),
                    "Health: " + currentHP + "/" + maxHP + " (Max overhealth: 200)\n''Your lifeforce. If it reaches 0, you die.''",
                    new Vector2(Input.CurrentMousePosition.X + 15, Input.CurrentMousePosition.Y + 15),
                    Color.White,
                    0f,
                    Vector2.Zero,
                    1f,
                    SpriteEffects.None,
                    0.4f
                );
            }
        }
    }
}
