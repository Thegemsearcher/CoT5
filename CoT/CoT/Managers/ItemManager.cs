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
                if (Input.CurrentKeyboard.IsKeyDown(Keys.Z) && Input.LastKeyboard.IsKeyUp(Keys.Z))
                    CreatePotion(Potion.PotionType.FireBall, Input.CurrentMousePosition.ScreenToWorld(), false);
                if (Input.CurrentKeyboard.IsKeyDown(Keys.A) && Input.LastKeyboard.IsKeyUp(Keys.A))
                    CreatePotion(Potion.PotionType.HealthSmall, Input.CurrentMousePosition.ScreenToWorld(), false);
                if (Input.CurrentKeyboard.IsKeyDown(Keys.X) && Input.LastKeyboard.IsKeyUp(Keys.X))
                    CreatePotion(Potion.PotionType.HealthMedium, Input.CurrentMousePosition.ScreenToWorld(), false);
                if (Input.CurrentKeyboard.IsKeyDown(Keys.C) && Input.LastKeyboard.IsKeyUp(Keys.C))
                    CreatePotion(Potion.PotionType.HealthLarge, Input.CurrentMousePosition.ScreenToWorld(), false);
                if (Input.CurrentKeyboard.IsKeyDown(Keys.V) && Input.LastKeyboard.IsKeyUp(Keys.V))
                    CreatePotion(Potion.PotionType.SpeedPotion, Input.CurrentMousePosition.ScreenToWorld(), false);
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
            Items.Add(new Potion(new Spritesheet("potionSheet", new Point(0, 0), new Rectangle(0, 0, 1, 1)),
                        position, new Rectangle(1, 1, 1, 1),
                        putInBag, potionType));
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