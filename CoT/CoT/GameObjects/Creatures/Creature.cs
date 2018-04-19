using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CoT.GameObjects;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using RoyT.AStar;

namespace CoT
{

    public class Creature : WorldObject
    {
        protected Grid grid;
        protected Rectangle destinationRectangle = new Rectangle(0, 0, 0, 0);
        public FloatRectangle AttackHitBox { get; protected set; } = new FloatRectangle(new Vector2(0, 0), new Vector2(0, 0));
        public Vector2 CenterMass { get; protected set; }
        int invTimeTotal = 40, deathTimer = 0, deathTimeTotal = 100;
        protected float attackSize;
        protected bool attacking = false, dealtDamage = false, invulnerability = false;
        protected int attackTimer = 0/*Tiden det tar att utföra en attack.*/, invTimer = 0;/*Invulnerability efter att karaktären blivit attackerad.*/
        public Vector2 PositionOfFeet { get; protected set; }
        public int Health { get; protected set; }
        public int AttackStat { get; protected set; }
        public int Defense { get; protected set; }
        public Map map;
        protected Vector2 offsetAttackPosition;

        public Creature(string texture, Vector2 position, Rectangle sourceRectangle, Vector2 depthSortingOffset, Map map, int hp, int attack, int defense) : base(texture, position, sourceRectangle, depthSortingOffset)
        {
            AttackStat = attack;
            Defense = defense;
            Health = hp;
            this.map = map;
            PositionOfFeet = new Vector2(position.X /*+ (ResourceManager.Get<Texture2D>(Texture).Width * Scale)/2*/, position.Y /*+ (ResourceManager.Get<Texture2D>(Texture).Height * Scale)*/);
        }

        public override void OnRemove()
        {
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
        #region oanvänd Bresenham algoritm
        // Swap the values of A and B
        private void Swap<T>(ref T a, ref T b)
        {
            T c = a;
            a = b;
            b = c;
        }
        // Returns the list of points from p0 to p1 
        public List<Vector2> BresenhamLine(Vector2 p0, Vector2 p1)
        {
            return BresenhamRayCast(p0.X, p0.Y, p1.X, p1.Y);
        }

        // Returns the list of points from (x0, y0) to (x1, y1), stora delar(typ, allt) av denna metod kommer från olika internetkällor
        private List<Vector2> BresenhamRayCast(float x0, float y0, float x1, float y1)
        {
            List<Vector2> result = new List<Vector2>();

            bool steep = Math.Abs(y1 - y0) > Math.Abs(x1 - x0);
            // Ser till så att linjen går från punkten med det lägsta Y-värdet till punkten med det större Y-värdet
            if (steep)
            {
                Swap(ref x0, ref y0);
                Swap(ref x1, ref y1);
            }
            if (x0 > x1)
            {
                Swap(ref x0, ref x1);
                Swap(ref y0, ref y1);
            }

            int deltaX = (int)(x1 - x0);
            int deltaY = Math.Abs((int)(y1 - y0));
            int error = 0;
            int ystep;
            int y = (int)y0;
            if (y0 < y1) ystep = 1; else ystep = -1;
            for (int x = (int)x0; x <= x1; x += (map.TileSize.Y/2))
            {
                if (steep) result.Add(new Vector2(y, x));
                else result.Add(new Vector2(x, y));
                error += deltaY;
                if (2 * error >= deltaX)
                {
                    y += ystep;
                    error -= deltaX;
                }
            }

            return result;
        }
        #endregion

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
        public virtual void InvulnerabilityTimer()
        {
            invTimer++;
            if (invTimer > invTimeTotal)
            {
                invTimer = 0;
                invulnerability = false;
            }
        }
        public virtual void GetHit(Creature attacker)
        {
            if (!invulnerability)
            {
                if (attacker.AttackStat - Defense >= 0)
                {
                    Health -= (attacker.AttackStat - Defense);
                    invulnerability = true;
                }
            } 
        }
        public virtual void Die()
        {
            deathTimer++;
            if (deathTimer > deathTimeTotal)
            {
                deathTimer = 0;
                Remove = true;
            }
        }

        public override void Update()
        {
            base.Update();
            if (Health <= 0)
            {
                Die();
                return;
            }
            if (invulnerability)
            {
                InvulnerabilityTimer();
            }
        }

        public override void Draw(SpriteBatch sb)
        {
            base.Draw(sb);
        }
    }
}
