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
        public Treent(Spritesheet spritesheet, Vector2 position, Vector2 groundPositionOffset, Vector2 depthSortingOffset, Stats stats, Map map, Grid grid, Player player) : base(spritesheet, position, groundPositionOffset, depthSortingOffset, stats, map, grid, player)
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
                path = Pathing(Player.GroundPosition);
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
