using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CoT
{
    public class Potion : Item
    {

        public enum PotionType { HealthSmall, ExplosiveMedium }
        public PotionType currentPotionType;

        private Rectangle rectCurrentSprite;
        private int currentSpriteID = 0, spriteUpdateDelay = 50, spriteUpdateDelayCounter = 0;

        public Potion(Spritesheet texture, Vector2 position, Rectangle sourceRectangle, bool putInBag, PotionType potionType) : base(texture, position, sourceRectangle, putInBag)
        {
            texItem = ResourceManager.Get<Texture2D>("potions");
            verticalSize = 1;
            currentPotionType = potionType;
            rectItemDrop = new Rectangle((int)position.X, (int)position.Y, 28, 44);

            switch (currentPotionType)
            {
                case PotionType.HealthSmall:
                    sourceRectSprite = new Rectangle(29, 121, 14, 22);
                    break;
                case PotionType.ExplosiveMedium:
                    break;
                default:
                    break;
            }

            rectCurrentSprite = sourceRectSprite;
        }

        public override void Update()
        {
            base.Update();

            if (!isInBag)
                UpdateSpriteAnimation();
        }

        public void UpdateSpriteAnimation()
        {
            spriteUpdateDelayCounter += Time.GameTime.ElapsedGameTime.Milliseconds;

            if (spriteUpdateDelayCounter >= spriteUpdateDelay)
            {
                switch (currentPotionType)
                {
                    case PotionType.HealthSmall:
                        rectCurrentSprite.X = 29 + (currentSpriteID * 24); //Flyttar 24 pixlar(X) för nästa sprite
                        break;
                    case PotionType.ExplosiveMedium:
                        break;
                    default:
                        break;
                }
                currentSpriteID += 1;
                if (currentSpriteID >= 7)
                    currentSpriteID = 0;

                spriteUpdateDelayCounter = 0;
            }
        }

        public override void Pickup()
        {
            base.Pickup();
        }

        public override void Use()
        {
            base.Use();
            bool consumptionAllowed = true;
            switch (currentPotionType)
            {
                case PotionType.HealthSmall:
                    int health = GameplayScreen.Instance.Player.Stats.Health;
                    if (health >= 200)
                    {
                        consumptionAllowed = false;
                        Console.WriteLine("health is already full or overhealed");
                    } else
                    {
                        Console.WriteLine("old: " + health);
                        health += 50;
                        if (health > 200)
                            health = 200;
                        Console.WriteLine("new: " + health);
                    }

                    GameplayScreen.Instance.Player.Stats.Health = health;
                    break;
                case PotionType.ExplosiveMedium:
                    break;
                default:
                    break;
            }
            if (consumptionAllowed)
            {
                IsActive = false;
            }

        }

        public override void Draw(SpriteBatch sb)
        {
            base.Draw(sb);

            if (!isInBag)
                sb.Draw(texItem, rectItemDrop, rectCurrentSprite, Color.White, 0f, Vector2.Zero, SpriteEffects.None, 1f);
        }
    }
}