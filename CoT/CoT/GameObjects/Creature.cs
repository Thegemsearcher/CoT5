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
        protected bool attacking = false, dealtDamage = false;
        protected int attackTimer = 0;
        public Vector2 PositionOfFeet { get; protected set; }

        public Map map;
        protected Vector2 offsetAttackPosition;

        public Creature(string texture, Vector2 position, Rectangle sourceRectangle, Map map) : base(texture, position, sourceRectangle)
        {
            PositionOfFeet = new Vector2(position.X /*+ (ResourceManager.Get<Texture2D>(Texture).Width * Scale)/2*/, position.Y /*+ (ResourceManager.Get<Texture2D>(Texture).Height * Scale)*/);
        }

        public override void OnRemove()
        {
            throw new NotImplementedException();
        }

        protected Position[] Pathing(Vector2 destination)
        {
            Vector2 cartesianTileWorldPosEnemy = new Vector2(PositionOfFeet.X / map.TileSize.Y,
                PositionOfFeet.Y / map.TileSize.Y);
            Point isometricScreenTileCreature = (cartesianTileWorldPosEnemy.ToCartesian() + new Vector2(-0.5f, 0.5f)).ToPoint();
            //Gör om positionen för fienden till en position vi kan använda. 

            Vector2 cartesianTileWorldPosPlayer = new Vector2(destination.X / map.TileSize.Y,
                destination.Y / map.TileSize.Y);
            Point isometricScreenTileDestination = (cartesianTileWorldPosPlayer.ToCartesian() + new Vector2(-0.5f, 0.5f)).ToPoint();
            //Gör om spelarens position till en position vi kan använda.

            Position[] chosenPath = grid.GetPath(new Position(isometricScreenTileCreature.X, isometricScreenTileCreature.Y),
                new Position(isometricScreenTileDestination.X, isometricScreenTileDestination.Y), MovementPatterns.LateralOnly);

            return chosenPath;
        }

        public virtual void Attack(Vector2 direction)
        {
            if (!attacking)
            {
                dealtDamage = false;
                attacking = true;
                direction.Normalize();
                direction *= -1;
                AttackHitBox.Position = CenterMass + (direction * attackSize) + offsetAttackPosition;
                AttackHitBox.Size = new Vector2(attackSize, attackSize);
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

        public override void Draw(SpriteBatch sb)
        {
            base.Draw(sb);
        }
    }
}
