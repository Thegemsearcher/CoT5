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

    public class Creature : GameObject
    {
        protected Grid grid;
        protected Rectangle destinationRectangle = new Rectangle(0,0,0,0);
        public FloatRectangle AttackHitBox { get; protected set; } = new FloatRectangle(new Vector2(0,0),new Vector2(0,0));
        public Vector2 CenterMass { get; protected set; }
        protected float attackSize;
        bool attacking = false;
        int timer = 0;
        public Creature(string texture, Vector2 position, Rectangle sourceRectangle) : base(texture, position, sourceRectangle)
        {
        }

        public override void OnRemove()
        {
            throw new NotImplementedException();
        }

        protected Position[] Pathing(Vector2 destination)
        {
            Vector2 cartesianTileWorldPosEnemy = new Vector2(Position.X / Game1.Game.map.TileSize.Y,
                Position.Y / Game1.Game.map.TileSize.Y);
            Point isometricScreenTileCreature = (cartesianTileWorldPosEnemy.ToScreen() + new Vector2(-0.5f, 0.5f)).ToPoint();
            //Gör om positionen för fienden till en position vi kan använda. 

            Vector2 cartesianTileWorldPosPlayer = new Vector2(destination.X / Game1.Game.map.TileSize.Y,
                destination.Y / Game1.Game.map.TileSize.Y);
            Point isometricScreenTileDestination = (cartesianTileWorldPosPlayer.ToScreen() + new Vector2(-0.5f, 0.5f)).ToPoint();
            //Gör om spelarens position till en position vi kan använda.

            Position[] chosenPath = grid.GetPath(new Position(isometricScreenTileCreature.X, isometricScreenTileCreature.Y),
                new Position(isometricScreenTileDestination.X, isometricScreenTileDestination.Y), MovementPatterns.LateralOnly);

            return chosenPath;
        }

        public virtual void Attack(Vector2 direction)
        {
            if (!attacking)
            {
                attacking = true;
                direction.Normalize();
                direction *= -1;
                AttackHitBox.Position = CenterMass + attackSize * direction;
                AttackHitBox.Size = new Vector2(attackSize, attackSize);
            } else
            {
                timer++;
                if (timer == 100)
                {
                    timer = 0;
                    attacking = false;
                }
            }
        }
        public virtual void GetHit()
        {

        }
        public virtual void Die()
        {

        }

        public override void Update()
        {
            base.Update();
            //Move();
        }

        public override void Draw()
        {
            base.Draw();
        }
    }
}
