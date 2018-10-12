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
        protected enum FacingDirection
        {
            North,
            NorthEast,
            East,
            SouthEast,
            South,
            SouthWest,
            West,
            NorthWest
        }

        public Grid Grid;
        public Map Map;
        public Player Player;
        
        protected float attackRange;
        protected bool isAttacking;
        protected bool invulnerable;
        protected bool dealtDamage;
        protected int attackTimer; // Tiden det tar att utföra en attack
        protected int invTimer;
        protected int invTimeTotal = 40; // Hur lång invulnerability ska vara efter tagit skada
        protected int deathTimer = 0;
        protected int deathTimeTotal = 1;

        public Vector2 GroundPosition { get; protected set; }
        public Stats Stats { get; protected set; }

        protected Vector2 offsetAttackPosition;
        protected FloatRectangle attackHitbox;

        protected Vector2 groundPositionOffset;

        protected Vector2 direction;
        protected float speed;

        protected FacingDirection facingDirection;

        public Creature(Spritesheet spritesheet, Vector2 position, Vector2 groundPositionOffset, Vector2 depthSortingOffset, Stats stats, Map map, Grid grid, Player player) : base(spritesheet, position, depthSortingOffset, true)
        {
            Stats = stats;
            Map = map;
            Grid = grid;
            Player = player;
            this.groundPositionOffset = groundPositionOffset;
            attackHitbox = new FloatRectangle(new Vector2(), new Vector2(attackRange, attackRange));
            GroundPosition = Position + Center + groundPositionOffset;
        }

        public override void OnRemove() { }

        protected Position[] Pathing(Vector2 destination)
        {
            Vector2 cartesianTileWorldPosEnemy = new Vector2(GroundPosition.X / Map.TileSize.Y,
                GroundPosition.Y / Map.TileSize.Y);
            Point isometricScreenTileCreature = (cartesianTileWorldPosEnemy.ToCartesian() + new Vector2(-0.5f, 0.5f)).ToPoint();
            //Gör om positionen för fienden till en position vi kan använda. 

            Vector2 cartesianTileWorldPosPlayer = new Vector2(destination.X / Map.TileSize.Y,
                destination.Y / Map.TileSize.Y);
            Point isometricScreenTileDestination = (cartesianTileWorldPosPlayer.ToCartesian() + new Vector2(-0.5f, 0.5f)).ToPoint();
            //Gör om spelarens position till en position vi kan använda.

            Position[] chosenPath = Grid.GetPath(new Position(isometricScreenTileCreature.X, isometricScreenTileCreature.Y),
                new Position(isometricScreenTileDestination.X, isometricScreenTileDestination.Y), MovementPatterns.LateralOnly);

            return chosenPath;
        }


        #region  Bresenham algoritm
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
            for (int x = (int)x0; x <= x1; x += (Map.TileSize.Y/10))
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
            if (!isAttacking)
            {
                dealtDamage = false;
                isAttacking = true;
                direction.Normalize();
                direction *= -1;
                attackHitbox.Position = Position + Center + (direction * attackRange) + offsetAttackPosition;
                attackHitbox.Size = new Vector2(attackRange, attackRange);
            }
        }
        public virtual void InvulnerabilityTimer()
        {
            invTimer++;
            if (invTimer > invTimeTotal)
            {
                invTimer = 0;
                invulnerable = false;
            }
        }
        public virtual void GetHit(Creature attacker)
        {
            if (!invulnerable)
            {
                if (attacker.Stats.Attack - Stats.Defense >= 0)
                {
                    Stats.Health -= (attacker.Stats.Attack - Stats.Defense);
                    //invulnerable = true;
                    if (this is Treent || this is Imp)
                    {
                        SoundManager.Instance.PlaySound("treentHurt1", 0.5f, 0.0f, 0.0f);
                    }
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
                if (this is Treent || this is Imp)
                {
                    SoundManager.Instance.PlaySound("treentDeath1", 0.5f, 0.0f, 0.0f);
                }
            }
        }

        public override void Update()
        {
            base.Update();
            GroundPosition = Position + Center + groundPositionOffset;

            float angle = (float)Math.Atan2(direction.Y, direction.X);
            float pi8 = (float)Math.PI / 8;
           
            if (angle > -pi8 && angle < pi8)
            {
                facingDirection = FacingDirection.East;
            }
            else if (angle > pi8 && angle < pi8 * 3)
            {
                facingDirection = FacingDirection.SouthEast;
            }
            else if (angle > pi8 * 3 && angle < pi8 * 5)
            {
                facingDirection = FacingDirection.South;
            }
            else if (angle > pi8 * 5 && angle < pi8 * 7)
            {
                facingDirection = FacingDirection.SouthWest;
            }
            else if (angle > pi8 * 7 || angle < -pi8 * 7)
            {
                facingDirection = FacingDirection.West;
            }
            else if (angle < -pi8 && angle > -pi8 * 3)
            {
                facingDirection = FacingDirection.NorthEast;
            }
            else if (angle < -pi8 * 3 && angle > -pi8 * 5)
            {
                facingDirection = FacingDirection.North;
            }
            else if (angle < -pi8 * 5 && angle > -pi8 * 7)
            {
                facingDirection = FacingDirection.NorthWest;
            }
            //beräknar ut riktning på creature


            if (GetType() == typeof(Player))
            {
                GameDebugger.WriteLine("player direction: " + facingDirection);
            }

            if (Stats.Health <= 0)
            {
                Die();
                return;
            }
            if (invulnerable)
            {
                InvulnerabilityTimer();
            }
        }
    }
}
