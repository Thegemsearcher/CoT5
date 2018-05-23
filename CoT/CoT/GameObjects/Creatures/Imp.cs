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
        protected Position lastPos;
        protected List<Projectile> attackProj = new List<Projectile>();
        protected bool attackCD = false;
        protected int lastPosTimer;

        //public Imp(string texture, Vector2 position, Rectangle sourceRectangle, Vector2 depthSortingOffset, Player player, Grid grid, Map map, int hp, int attack, int defense) : 
        //{
        //    attackSize = 500;
        //    aggroRange = 1200;
        //    speed = 160f;
        //    attacking = false;
        //    Color = Color.Red;
        //}
        public Imp(Spritesheet spritesheet, Vector2 position, Vector2 groundPositionOffset, Vector2 depthSortingOffset, Stats stats, Map map, Grid grid, Player player) : base(spritesheet, position, groundPositionOffset, depthSortingOffset, stats, map, grid, player)
        {
            attackRange = 500;
            aggroRange = 1200;
            speed = 160f;
            isAttacking = false;
            Color = Color.Red;
            
        }

        public override void Update()
        {
            base.Update();
            if (Stats.Health <= 0)
            {
                return;
            }
            UpdateProjectiles();

            if (DetectPlayer() || hasAggro)
            {
                path = Pathing(Player.GroundPosition);
            }
            //Impen försöker röra sig från spelaren men håller sig innanför sin egen attack range
            if (((Vector2.Distance(Player.GroundPosition, GroundPosition) < attackRange - (attackRange / 20))))
            {
                Vector2 nextPos;
                nextPos.X = GroundPosition.X - (Player.GroundPosition.X - GroundPosition.X);
                nextPos.Y = GroundPosition.Y - (Player.GroundPosition.Y - GroundPosition.Y);
                path = Pathing(/*(GroundPosition) +*/ nextPos );
            }
            if (path.Length > 1)
            {
                //if ((hasAggro && (Vector2.Distance(player.CenterMass, CenterMass) > attackSize || !VisionRange(CenterMass, player.CenterMass))) ||
                //            (!attacking && (Vector2.Distance(player.CenterMass, CenterMass) < attackSize/* - (attackSize / 20)*/)))
                //{
                //    nextTileInPath = path[1];
                //}
                nextTileInPath = path[1];
            }
            CheckAttackDistance();
        }
       
        public void UpdateProjectiles()
        {
            List<Projectile> toRemove = new List<Projectile>();
            foreach (Projectile proj in attackProj)
            {
                if (proj.Owner != null)
                {
                    if (Player.Hitbox.Intersects(new FloatRectangle(proj.Position, new Vector2(proj.Spritesheet.SourceRectangle.Width, proj.Spritesheet.SourceRectangle.Height))))
                    {
                        DamageToPlayer();
                        toRemove.Add(proj);
                    }
                    Vector2 cartesianTileWorldPos = new Vector2(0, 0);
                    cartesianTileWorldPos.X = proj.Position.X / Map.TileSize.Y;
                    cartesianTileWorldPos.Y = proj.Position.Y / Map.TileSize.Y;
                    Tile t;
                    Point isometricScreenTile = (cartesianTileWorldPos.ToCartesian() + new Vector2(-0.5f, 0.5f)).ToPoint();
                    for (int i = 0; i < Map.TileMap.GetLength(0); i++)
                    {
                        for (int j = 0; j < Map.TileMap.GetLength(1); j++)
                        {
                            t = Map.TileMap[i, j];
                            if (isometricScreenTile == new Point(i, j))
                            {
                                if (t.TileType == TileType.Collision)
                                {
                                    toRemove.Add(proj);
                                }
                            }
                        }
                    }
                    proj.Update();
                }
            }
            foreach (Projectile removeProj in toRemove)
            {
                attackProj.Remove(removeProj);
            }
            toRemove.Clear();
        }
        
        public override void DamageToPlayer()
        {
            Player.GetHit(this);

            for (int i = 0; i < 15; i++)
            {
                ParticleManager.CreateStandard(Player.Position + Player.Center, Helper.RandomDirection(), Color.Red);
                //ParticleManager.Instance.Particles.Add(new Particle("lightMask", Player.Position,
                //    new Rectangle(0, 0, ResourceManager.Get<Texture2D>("lightMask").Width, ResourceManager.Get<Texture2D>("lightMask").Height),
                //    Helper.RandomDirection(), 300f, 2f, Color.Red, 0f, 0.3f));
            }

            Camera.ScreenShake(0.2f, 20);
        }

        public override void Draw(SpriteBatch sb)
        {
            foreach (Projectile proj in attackProj)
            {
                proj.Draw(sb);
            }
            base.Draw(sb);
        }

        public override void CheckAttackDistance()
        {
            if (!attackCD)
            {
                if (Vector2.Distance(Position + Center, Player.Position + Player.Center) <= attackRange)
                {
                    Attack((Position + Center) - (Player.Position + Player.Center));
                    //if (VisionRange(Position + Center, Player.Position + Player.Center))
                    //{
                        
                    //}
                }
            }
            else
            {
                //Denna fiende gör skillnad på att ha en cd på sin attack och att ha en animation där den attackerar.
                attackTimer++;
                if (attackTimer == 100)
                {
                    attackTimer = 0;
                    attackCD = false;
                }
                else if (attackCD && attackTimer == 50)
                {
                    isAttacking = false;
                }
            }
        }
        public override void Attack(Vector2 direction)
        {
            if (!attackCD)
            {
                dealtDamage = false;
                isAttacking = true;
                attackCD = true;
                direction.Normalize();
                direction *= -1;
                Projectile proj = new Projectile(new Spritesheet("tile2", new Point(1, 1), new Rectangle(0, 0, 20, 20)), Position + Center, direction, 500f);
                proj.Color = Color.Red;
                proj.Owner = this;
                proj.LayerDepth = 1F;
                attackProj.Add(proj);
            }
        }
    }
}
