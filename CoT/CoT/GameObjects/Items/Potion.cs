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

        public Potion(string texture, Vector2 position, Rectangle sourceRectangle, bool putInBag, PotionType potionType) : base(texture, position, sourceRectangle, putInBag)
        {
            Texture = texture;
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
        }

        public override void Draw(SpriteBatch sb) {
            base.Draw(sb);

            if (!isInBag)
                sb.Draw(ResourceManager.Get<Texture2D>(Texture), rectItemDrop, rectCurrentSprite, Color.White, 0f, Vector2.Zero, SpriteEffects.None, 1f);
        }
    }
}