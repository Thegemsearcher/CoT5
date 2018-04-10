using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CoT
{
    public class CreatureManager : IManager
    {
        public static CreatureManager Instance { get; set; }
        public List<Creature> Creatures { get; set; }

        public CreatureManager()
        {
            Instance = this;
        }

        public void Initialize()
        {
            Creatures = new List<Creature>();
        }

        public void LoadContent()
        {
        }

        public void Update()
        {
            Creatures.ForEach(x => x.Update());

            for (int i = Creatures.Count - 1; i >= 0; i--)
            {
                if (Creatures[i].Remove)
                {
                    Creatures[i].OnRemove();
                    Creatures.RemoveAt(i);
                }
            }
        }

        public void Draw(SpriteBatch sb)
        {
            Creatures.ForEach(x => x.Draw(sb));
        }

        public void DrawToWorldAdditiveBlend(SpriteBatch spriteBatch)
        {
        }

        public void DrawUserInterface(SpriteBatch spriteBatch)
        {
        }
    }
}
