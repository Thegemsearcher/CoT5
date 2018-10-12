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
        private int currentSpriteID = 0, spriteUpdateDelay = 50, spriteUpdateDelayCounter = 0;

        public Potion(Spritesheet spritesheet, Vector2 position, Rectangle sourceRectangle, bool putInBag, PotionType potionType) : base(spritesheet, position, sourceRectangle, putInBag)
        {
            texItem = ResourceManager.Get<Texture2D>("potionSheet");
            verticalTileSlotSize = 1;
            currentPotionType = potionType;
            rectItemDrop = new Rectangle((int)position.X, (int)position.Y, 48, 48);
            
            switch (currentPotionType)
            {
                case PotionType.HealthSmall:
                    itemName = "Potion of Minor Health";
                    itemDescription = "Heals the player 20 points of health.";
                    sourceRectSprite = new Rectangle(24, 120, 24, 24);
                    break;
                case PotionType.HealthMedium:
                    itemName = "Potion of Moderate Health";
                    itemDescription = "Heals the player 40 points of health.";
                    sourceRectSprite = new Rectangle(240, 120, 24, 24);
                    break;
                case PotionType.HealthLarge:
                    itemName = "Potion of Potent Health";
                    itemDescription = "Heals the player 80 points of health.";
                    sourceRectSprite = new Rectangle(240, 264, 24, 24);
                    break;
                case PotionType.SpeedPotion:
                    itemName = "Potion of Swiftness";
                    itemDescription = "Grants the player 2x walking\nspeed for 10 seconds.";
                    sourceRectSprite = new Rectangle(24, 168, 24, 24);
                    break;
                case PotionType.FireBall:
                    itemName = "Elixir of Fireball";
                    itemDescription = "Grants the player a single\nfireball charge to fire.";
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
                    GameplayScreen.Instance.Player.SpeedBoostTimer = 10;
                    break;
                case PotionType.FireBall:
                    if (GameplayScreen.Instance.Player.CanFireBall > 0)
                        GameplayScreen.Instance.Player.CanFireBall += 2;
                    break;
                default:
                    break;
            }
            if (consumptionAllowed)
            {
                IsActive = false;
                SoundManager.Instance.PlaySound("drinkPotion", 0.8f, 0.0f, 0.0f);
            }
        }

        protected bool RestoredHealth(int nrOfHealth)
        {
            int health = GameplayScreen.Instance.Player.Stats.Health;
            int maxHealth = GameplayScreen.Instance.Player.Stats.MaxHealth;

            bool isConsumed = true;

            if (health >= maxHealth * 2)
            {
                isConsumed = false;
                Console.WriteLine("health is already fully overhealed");
            }
            else
            {
                Console.WriteLine("old: " + health);
                health += nrOfHealth;
                if (health > maxHealth * 2)
                    health = maxHealth * 2;
                Console.WriteLine("new: " + health);
            }

            GameplayScreen.Instance.Player.Stats.Health = health;

            return isConsumed;
        }

        public override void Draw(SpriteBatch sb)
        {
            base.Draw(sb);
        }
    }
}