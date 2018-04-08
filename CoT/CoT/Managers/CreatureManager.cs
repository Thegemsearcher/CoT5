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

        public Player Player { get; set; }

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
            Player = new Player("player1", new Vector2(0, 0).ToIsometric(), new Rectangle(0, 0, ResourceManager.Get<Texture2D>("player1").Width, ResourceManager.Get<Texture2D>("player1").Height), GameStateManager.Instance.Map, GameStateManager.Instance.Map.Grid);

            Creatures.Add(Player);
            Creatures.Add(new Enemy("treent", new Vector2(400, 100).ToIsometric(), new Rectangle(0, 0, 1300, 1500), Player, GameStateManager.Instance.Map.Grid));
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
