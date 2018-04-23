using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using RoyT.AStar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoT.GameObjects.Creatures
{
    class Treent : Enemy
    {
        public Treent(string texture, Vector2 position, Rectangle sourceRectangle, Vector2 depthSortingOffset, Player player, Grid grid, Map map, int hp, int attack, int defense) : base(texture, position, sourceRectangle, depthSortingOffset, player, grid, map, hp, attack, defense)
        {
            attackSize = 100;
            aggroRange = 1000;
            speed = 100f;
        }
        public override void Update()
        {
            base.Update();

            if (DetectPlayer() || hasAggro)
            {
                path = Pathing(player.PositionOfFeet);
            }
            if (path.Length > 1)
            {
                nextTileInPath = path[1];
            }
            CheckAttackDistance();
        }

        public override void Draw(SpriteBatch sb)
        {
            base.Draw(sb);
        }
    }
}
