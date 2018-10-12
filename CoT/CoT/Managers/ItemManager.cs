using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace CoT
{
    public class ItemManager : IManager
    {
        public static ItemManager Instance { get; set; }

        public List<Item> Items { get; set; }

        Item.ItemType itemType = Item.ItemType.EquipmentItem;

        //public static Texture2D PotionSpriteSheet = ResourceManager.Get<Texture2D>("potions");

        public ItemManager()
        {
            Instance = this;
        }

        public void Initialize()
        {
            Items = new List<Item>();
        }

        public void LoadContent()
        {
        }

        public void Update()
        {
            Items.ForEach(x => x.Update());

            if (GameDebugger.Debug)
            {
                if (Input.IsKeyPressed(Keys.T))
                    itemType = Item.ItemType.PotionItem;
                else if (Input.IsKeyPressed(Keys.U))
                    itemType = Item.ItemType.EquipmentItem;

                if (itemType == Item.ItemType.PotionItem)
                {
                    if (Input.IsKeyPressed(Keys.Z))
                        CreatePotion(Potion.PotionType.FireBall, Input.CurrentMousePosition.ScreenToWorld(), false);
                    if (Input.IsKeyPressed(Keys.A))
                        CreatePotion(Potion.PotionType.HealthSmall, Input.CurrentMousePosition.ScreenToWorld(), false);
                    if (Input.IsKeyPressed(Keys.X))
                        CreatePotion(Potion.PotionType.HealthMedium, Input.CurrentMousePosition.ScreenToWorld(), false);
                    if (Input.IsKeyPressed(Keys.C))
                        CreatePotion(Potion.PotionType.HealthLarge, Input.CurrentMousePosition.ScreenToWorld(), false);
                    if (Input.IsKeyPressed(Keys.V))
                        CreatePotion(Potion.PotionType.SpeedPotion, Input.CurrentMousePosition.ScreenToWorld(), false);
                }
                else if(itemType == Item.ItemType.EquipmentItem)
                {
                    if (Input.IsKeyPressed(Keys.Z))
                        CreateEquipment(Equipment.GearType.Sword, Input.CurrentMousePosition.ScreenToWorld(), false);
                    if (Input.IsKeyPressed(Keys.A))
                        CreateEquipment(Equipment.GearType.Boots, Input.CurrentMousePosition.ScreenToWorld(), false);
                    if (Input.IsKeyPressed(Keys.X))
                        CreateEquipment(Equipment.GearType.Bodyarmor, Input.CurrentMousePosition.ScreenToWorld(), false);
                    if (Input.IsKeyPressed(Keys.C))
                        CreateEquipment(Equipment.GearType.Helmet, Input.CurrentMousePosition.ScreenToWorld(), false);
                    if (Input.IsKeyPressed(Keys.V))
                        CreateEquipment(Equipment.GearType.Ring, Input.CurrentMousePosition.ScreenToWorld(), false);
                }
            }
            for (int i = 0; i < Items.Count; i++)
            {
                if (!Items[i].IsActive)
                {
                    Items.RemoveAt(i);
                    break;
                }
            }
        }

   
        public void CreatePotion(Potion.PotionType potionType, Vector2 position, bool putInBag)
        {
                 
            Items.Add(new Potion(new Spritesheet("potionSheet", new Point(0, 0), new Rectangle(0, 0, 1, 1)), position, new Rectangle(1, 1, 1, 1), putInBag, potionType));
        }

        public void CreateEquipment(Equipment.GearType gearType, Vector2 position, bool putInBag)
        {
            Items.Add(new Equipment(new Spritesheet("potionSheet", new Point(0, 0), new Rectangle(0, 0, 1, 1)), position, new Rectangle(1, 1, 1, 1), putInBag, gearType));
        }

        public void Draw(SpriteBatch sb)
        {
            //Items.ForEach(x => x.Draw(sb));
            foreach (Item item in Items)
            {
                if (!item.isInBag)
                    item.Draw(sb);
            }
        }

        public void DrawToWorldAdditiveBlend(SpriteBatch spriteBatch)
        {
        }

        public void DrawUserInterface(SpriteBatch spriteBatch)
        {
        }
    }
}