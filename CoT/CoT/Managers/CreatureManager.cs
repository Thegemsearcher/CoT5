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
