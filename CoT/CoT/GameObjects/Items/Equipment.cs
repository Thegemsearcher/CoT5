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
    public class Equipment : Item
    {

        public enum GearType { Sword, Shield, Helmet, Ring, Bodyarmor, Boots, Gloves}
        public GearType currentGearType;

        public int buffAttack = 0, buffDefense = 0, buffSpeed = 0;

        public Equipment(Spritesheet spritesheet, Vector2 position, Rectangle sourceRectangle, bool putInBag, GearType gearType) : base(spritesheet, position, sourceRectangle, putInBag)
        {
            currentGearType = gearType;
            verticalTileSlotSize = 1;
            texItem = ResourceManager.Get<Texture2D>("rectangle");
            rectItemDrop = new Rectangle((int)position.X, (int)position.Y, 48, 48);

            switch (currentGearType)
            {
                case GearType.Sword:
                    itemName = "Wooden Sword";
                    itemDescription = "placeholder";
                    //sourceRectSprite = new Rectangle(24, 120, 24, 24);
                    break;
                case GearType.Shield:
                    itemName = "Wooden Shield";
                    itemDescription = "placeholder";
                    //sourceRectSprite = new Rectangle(240, 120, 24, 24);
                    break;
                case GearType.Helmet:
                    itemName = "Wooden Helmet";
                    itemDescription = "placeholder";
                    //sourceRectSprite = new Rectangle(240, 264, 24, 24);
                    break;
                case GearType.Ring:
                    itemName = "Wooden Leggings";
                    itemDescription = "placeholder";
                    //sourceRectSprite = new Rectangle(24, 168, 24, 24);
                    break;
                case GearType.Boots:
                    itemName = "Wooden Boots";
                    itemDescription = "placeholder";
                    //sourceRectSprite = new Rectangle(24, 48, 24, 24);
                    break;
                case GearType.Bodyarmor:
                    itemName = "Wooden Bracers";
                    itemDescription = "placeholder";
                    //sourceRectSprite = new Rectangle(24, 48, 24, 24);
                    break;
                case GearType.Gloves:
                    itemName = "Wool Bracers";
                    itemDescription = "placeholder";
                    //sourceRectSprite = new Rectangle(24, 48, 24, 24);
                    break;
                default:
                    break;
            }
        }

        public override void Update()
        {
            base.Update();
        }

        public override void Use()
        {
            base.Use();
        }

        public override void Drop()
        {
            base.Drop();
        }

        public override void Pickup()
        {
            base.Pickup();
        }

        public override void Draw(SpriteBatch sb)
        {
            base.Draw(sb);
        }
    }
}
