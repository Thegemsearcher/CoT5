using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CoT
{
    public class ItemManager : IManager
    {
        public static ItemManager Instance { get; set; }

        public List<Item> Items { get; set; }

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
        }

        public void Draw(SpriteBatch sb)
        {
            Items.ForEach(x => x.Draw(sb));
        }

        public void DrawToWorldAdditiveBlend(SpriteBatch spriteBatch)
        {
        }

        public void DrawUserInterface(SpriteBatch spriteBatch)
        {
        }
    }
}