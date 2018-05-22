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

        public enum PotionType { HealthSmall, HealthMedium, HealthLarge, SpeedPotion, FireBall }
        public PotionType currentPotionType;

        private Rectangle rectCurrentSprite, rectFirstSprite;
        private int currentSpriteID = 0, spriteUpdateDelay = 50, spriteUpdateDelayCounter = 0;

        public Potion(Spritesheet spritesheet, Vector2 position, Rectangle sourceRectangle, bool putInBag, PotionType potionType) : base(spritesheet, position, sourceRectangle, putInBag)
        {
            spritesheet.SetFrameCount(new Point(1, 1));
            spritesheet.Interval = 100;

            texItem = ResourceManager.Get<Texture2D>("potionSheet");
            verticalTileSlotSize = 1;
            currentPotionType = potionType;
            rectItemDrop = new Rectangle((int)position.X, (int)position.Y, 48, 48);
            
            switch (currentPotionType)
            {
                case PotionType.HealthSmall:
                    sourceRectSprite = new Rectangle(24, 120, 24, 24);
                    break;
                case PotionType.HealthMedium:
                    sourceRectSprite = new Rectangle(240, 120, 24, 24);
                    break;
                case PotionType.HealthLarge:
                    sourceRectSprite = new Rectangle(240, 264, 24, 24);
                    break;
                case PotionType.SpeedPotion:
                    sourceRectSprite = new Rectangle(24, 168, 24, 24);
                    break;
                case PotionType.FireBall:
                    sourceRectSprite = new Rectangle(24, 48, 24, 24);
                    break;
                default:
                    break;
            }

            rectCurrentSprite = sourceRectSprite;
            rectFirstSprite = sourceRectangle;
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
                rectCurrentSprite.X = sourceRectSprite.X + (currentSpriteID * 24);
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
                    consumptionAllowed = RestoredHealth(20); //Metoden försöker addera spelarens health, retunerar bool-värdet 'false' om spelarens health är redan full.
                    break;
                case PotionType.HealthMedium:
                    consumptionAllowed = RestoredHealth(40);
                    break;
                case PotionType.HealthLarge:
                    consumptionAllowed = RestoredHealth(80);
                    break;
                case PotionType.SpeedPotion:
                    GameplayScreen.Instance.Player.SpeedBoostTimer += 10;
                    break;
                case PotionType.FireBall:
                    if (!GameplayScreen.Instance.Player.CanFireBall)
                        GameplayScreen.Instance.Player.CanFireBall = true;
                    else
                        consumptionAllowed = false;
                    break;
                default:
                    break;
            }
            if (consumptionAllowed)
            {
                IsActive = false;
            }
        }

        protected bool RestoredHealth(int nrOfHealth)
        {
            int health = GameplayScreen.Instance.Player.Stats.Health;
            int maxHealth = GameplayScreen.Instance.Player.Stats.MaxHealth;

            bool isConsumed = true;

            if (health >= maxHealth)
            {
                isConsumed = false;
                Console.WriteLine("health is already full or overhealed");
            }
            else
            {
                Console.WriteLine("old: " + health);
                health += nrOfHealth;
                if (health > maxHealth)
                    health = maxHealth;
                Console.WriteLine("new: " + health);
            }

            GameplayScreen.Instance.Player.Stats.Health = health;

            return isConsumed;
        }

        public override void Draw(SpriteBatch sb)
        {
            base.Draw(sb);

            if (!isInBag)
                sb.Draw(texItem, rectItemDrop, rectCurrentSprite, Color.White, 0f, Vector2.Zero, SpriteEffects.None, 1f);
        }
    }
}