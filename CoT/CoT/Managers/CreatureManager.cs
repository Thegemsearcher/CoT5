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
            List<Creature> removeCreatures = new List<Creature>();
            foreach (Creature c in Creatures)
            {
                if (c.Remove)
                {
                    if (c is Enemy e)
                    {
                        removeCreatures.Add(e);
                    }
                    if (c is Player p)
                    {

                    }
                }
            }
            foreach (Creature c in removeCreatures)
            {
                Creatures.Remove(c);
            }
        }
        public void ToRemove(object obj)
        {

        }
        public void Draw(SpriteBatch sb)
        {
            Creatures.ForEach(x => x.Draw(sb));
        }

        public void DrawUserInterface(SpriteBatch spriteBatch)
        {
        }
    }
}
