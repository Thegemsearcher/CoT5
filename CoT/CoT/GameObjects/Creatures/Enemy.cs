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
    public class Enemy : Creature
    {
        protected Position[] path;
        protected Position nextTileInPath;
        protected float aggroRange;
        protected bool hasAggro = false;
        protected Vector2 nextPosition;

        public Enemy(Spritesheet spritesheet, Vector2 position, Vector2 groundPositionOffset, Vector2 depthSortingOffset, Stats stats, Map map, Grid grid, Player player) : base(spritesheet, position, groundPositionOffset, depthSortingOffset, stats, map, grid, player)
        {
            Scale = 0.1f;
            LayerDepth = 0.2f;
            path = new Position[0];

            //Det behövdes en offset för att attacken skulle bli lika stor åt alla håll.
            offsetAttackPosition = new Vector2(-spritesheet.SourceRectangle.Width * Scale / (float)4, -spritesheet.SourceRectangle.Height * Scale/ (float)4);
            //Ska flyttas.
        }

        public bool DetectPlayer()
        {
            if (!hasAggro && (Vector2.Distance(Player.Position + Player.Center, Position + Center) <= aggroRange) && VisionRange(Position + Center, Player.Position + Player.Center))
            {
                return hasAggro = true;
            } else if (!hasAggro)
                return false;
            else
                return true;
        }

        #region bresenham algoritm

        public bool VisionRange(Vector2 start,Vector2 finish)
        {
            Vector2 cartesianTileWorldPos = new Vector2(0, 0);
            List<Vector2> vision = BresenhamLine(start, finish);
            Tile t;
            foreach (Vector2 pos in vision)
            {
                cartesianTileWorldPos.X = pos.X / Map.TileSize.Y;
                cartesianTileWorldPos.Y = pos.Y / Map.TileSize.Y;
                Point isometricScreenTile = (cartesianTileWorldPos.ToCartesian() + new Vector2(-0.5f, 0.5f)).ToPoint();
                for (int i = 0; i < Map.TileMap.GetLength(0); i++)
                {
                    for (int j = 0; j < Map.TileMap.GetLength(1); j++)
                    {
                        t = Map.TileMap[i, j];
                        if (isometricScreenTile == new Point(i, j))
                        {
                            if (t.TileType == TileType.Collision)
                                return false;
                        }
                    }
                }
            }
            return true;
        }
        #endregion

        public override void Update()
        {
            base.Update();
            if (Stats.Health <= 0)
            {
                return;
            }
            //Fienden kommer ha en animation när den attackerar, den ska då stå stilla.
            if (!isAttacking && (DetectPlayer() || hasAggro))
            {
                Move();
            }
        }

        public virtual void CheckAttackDistance()
        {
            if (!isAttacking)
            {
                if (Vector2.Distance(Position + Center, Player.Position + Player.Center) <= attackRange)
                {
                    Attack((Position + Center) - (Player.Position + Player.Center));
                }
            } else
            {
                attackTimer++;
                if (attackTimer == 5)
                {
                    for (int i = 0; i< 10; i++)
                        ParticleManager.CreateStandard(Position + Center, (Player.Position + Player.Center) - (Position + Center), Color.Green, 200, 2, 0.3f);
                }
                if (attackTimer == 35)
                {
                    DamageToPlayer();
                }
                if (attackTimer == 100)
                {
                    attackTimer = 0;
                    isAttacking = false;
                }
            }
        }

        public virtual void DamageToPlayer()
        {
            if (Player.Hitbox.Intersects(attackHitbox) && !dealtDamage)
            {
                dealtDamage = true;
                Player.GetHit(this);

                for (int i = 0; i < 15; i++)
                {
                    ParticleManager.CreateStandard(Player.Position + Player.Center, Helper.RandomDirection(), Color.Red);
                    //ParticleManager.Instance.Particles.Add(new Particle("lightMask", Player.Position,
                    //    new Rectangle(0, 0, ResourceManager.Get<Texture2D>("lightMask").Width, ResourceManager.Get<Texture2D>("lightMask").Height),
                    //    Helper.RandomDirection(), 300f, 2f, Color.Red, 0f, 0.3f));
                }

                Camera.ScreenShake(0.15f, 20);
            }
        }

        public override void OnRemove()
        {
            for (int i = 0; i < 25; i++)
            {
                ParticleManager.CreateStandard(Position + Center, Helper.RandomDirection(), Color.Orange);
            }
            
            int rngN = Game1.Random.Next(0,6);
            if (rngN == 5)
                ItemManager.Instance.CreatePotion(Potion.PotionType.SpeedPotion, Position + Center, false);
            else if (rngN == 4)
                ItemManager.Instance.CreatePotion(Potion.PotionType.HealthLarge, Position + Center, false);
            else if (rngN == 3)
                ItemManager.Instance.CreatePotion(Potion.PotionType.HealthMedium, Position + Center, false);
            else if (rngN == 2)
                ItemManager.Instance.CreatePotion(Potion.PotionType.HealthSmall, Position + Center, false);
            else if (rngN == 1)
                ItemManager.Instance.CreatePotion(Potion.PotionType.FireBall, Position + Center, false);

            Camera.ScreenShake(0.15f, 20);
            base.OnRemove();
        }

        public virtual void Move()
        {
            nextPosition = new Vector2(nextTileInPath.X * Map.TileSize.Y, nextTileInPath.Y * Map.TileSize.Y).ToIsometric();
            nextPosition.X += Map.TileSize.X / 2;
            nextPosition.Y += Map.TileSize.Y / 2;
            direction.X = nextPosition.X - GroundPosition.X;
            direction.Y = nextPosition.Y - GroundPosition.Y;
            direction.Normalize();
            Position += direction * speed * Time.DeltaTime;
        }

        public override void Draw(SpriteBatch sb)
        {
            for (int i = 0; i < path.Length; i++) //Ritar ut pathen som fienden rör sig efter.
            {
                sb.Draw(ResourceManager.Get<Texture2D>("tile1"), new Vector2(path[i].X * Map.TileSize.Y,
                path[i].Y * Map.TileSize.Y).ToIsometric(), null, Color.Gray * 0.5f, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0.5f);
            }
            //sb.Draw(ResourceManager.Get<Texture2D>(Texture), destinationRectangle, SourceRectangle, Color * Transparency, Rotation, Vector2.Zero, SpriteEffects.None, 0.9f);

            //if (isAttacking)
            //{
            //    sb.Draw(ResourceManager.Get<Texture2D>("tile1"), new Rectangle((int)attackHitbox.Position.X, (int)attackHitbox.Position.Y, (int)attackHitbox.Size.X, (int)attackHitbox.Size.Y)
            //    , Spritesheet.SourceRectangle, Color.Red * 0.5f, Rotation, Vector2.Zero, SpriteEffects.None, 0f);
            //}
            base.Draw(sb);
        }
    }
}
