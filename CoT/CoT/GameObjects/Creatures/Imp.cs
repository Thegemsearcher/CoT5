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
        protected List<Projectile> attackProj = new List<Projectile>();
        public Imp(string texture, Vector2 position, Rectangle sourceRectangle, Vector2 depthSortingOffset, Player player, Grid grid, Map map, int hp, int attack, int defense) : base(texture, position, sourceRectangle, depthSortingOffset, player, grid, map, hp, attack, defense)
        {
            attackSize = 500;
            aggroRange = 1200;
            speed = 160f;
            attacking = true;
            Color = Color.Red;
        }
        public override void Update()
        {
            base.Update();
            if (Health <= 0)
            {
                return;
            }
            UpdateProjectiles();

            if (DetectPlayer() || hasAggro)
            {
                path = Pathing(player.PositionOfFeet);
            }
            if (path.Length > 1)
            {
                if (hasAggro && (Vector2.Distance(player.Position, Position) > attackSize || !VisionRange(CenterMass, player.CenterMass)))
                {
                    nextTileInPath = path[1];
                }
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
                    if (player.Hitbox.Intersects(new FloatRectangle(proj.Position, new Vector2(proj.SourceRectangle.Width, proj.SourceRectangle.Height))))
                    {
                        DamageToPlayer();
                        toRemove.Add(proj);
                    }

                    Vector2 cartesianTileWorldPos = new Vector2(0,0);
                    cartesianTileWorldPos.X = proj.Position.X / map.TileSize.Y;
                    cartesianTileWorldPos.Y = proj.Position.Y / map.TileSize.Y;
                    Tile t;
                    Point isometricScreenTile = (cartesianTileWorldPos.ToCartesian() + new Vector2(-0.5f, 0.5f)).ToPoint();
                    for (int i = 0; i < map.TileMap.GetLength(0); i++)
                    {
                        for (int j = 0; j < map.TileMap.GetLength(1); j++)
                        {
                            t = map.TileMap[i, j];
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
            player.GetHit(this);

            for (int i = 0; i < 15; i++)
            {
                ParticleManager.Instance.Particles.Add(new Particle("lightMask", player.Position,
                    new Rectangle(0, 0, ResourceManager.Get<Texture2D>("lightMask").Width, ResourceManager.Get<Texture2D>("lightMask").Height),
                    Helper.RandomDirection(), 300f, 2f, Color.Red, 0f, 0.3f));
            }

            Camera.ScreenShake(0.1f, 20);
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
            if (!attacking)
            {
                if (Vector2.Distance(CenterMass, player.CenterMass) <= attackSize)
                {
                    if (VisionRange(CenterMass, player.CenterMass))
                    {
                        Attack(CenterMass - player.CenterMass);
                    }
                }
            } else
            {
                attackTimer++;
                if (attackTimer == 100)
                {
                    attackTimer = 0;
                    attacking = false;
                }
            }
        }
        public override void Attack(Vector2 direction)
        {
            if (!attacking)
            {
                dealtDamage = false;
                attacking = true;
                
                direction.Normalize();
                direction *= -1;
                Projectile proj = new Projectile("tile2", CenterMass, new Rectangle(0, 0, 20, 20), direction, 600f);
                proj.Color = Color.Red;
                proj.Owner = this;
                proj.LayerDepth = 1F;
                attackProj.Add(proj);
            }
        }
    }
}
