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
                if (Input.CurrentKeyboard.IsKeyDown(Keys.A) && Input.LastKeyboard.IsKeyUp(Keys.A))
                {
                    Items.Add(new Potion(new Spritesheet("potionSheet", new Point(0, 0), new Rectangle(0, 0, 1, 1)),
                        Input.CurrentMousePosition.ScreenToWorld(), new Rectangle(1, 1, 1, 1),
                        false, Potion.PotionType.HealthSmall));
                }
                if (Input.CurrentKeyboard.IsKeyDown(Keys.Z) && Input.LastKeyboard.IsKeyUp(Keys.Z))
                {
                    Items.Add(new Potion(new Spritesheet("potionSheet", new Point(0, 0), new Rectangle(0, 0, 1, 1)),
                        Input.CurrentMousePosition.ScreenToWorld(), new Rectangle(1, 1, 1, 1),
                        false, Potion.PotionType.FireBall));
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