using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using RoyT.AStar;

namespace CoT
{
    public class Imp : Enemy
    {

        public override void Update()
        {
            base.Update();
        }

        public override void Draw(SpriteBatch sb)
        {
            base.Draw(sb);
        }

        public Imp(string texture, Vector2 position, Rectangle sourceRectangle, Vector2 depthSortingOffset, Player player, Grid grid, Map map, int hp, int attack, int defense) : base(texture, position, sourceRectangle, depthSortingOffset, player, grid, map, hp, attack, defense)
        {
        }

    }
}
