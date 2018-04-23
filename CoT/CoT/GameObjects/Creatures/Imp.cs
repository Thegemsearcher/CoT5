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
            foreach (Projectile proj in attackProj)
            {
                if (proj.Owner != null)
                {
                    if (player.Hitbox.Intersects(new FloatRectangle(proj.Position,new Vector2(proj.SourceRectangle.Width, proj.SourceRectangle.Height))))
                    {
                        DamageToPlayer();
                        attackProj.Remove(proj);
                        break;
                    }
                    proj.Update();
                }
               
            }

            if (DetectPlayer() || hasAggro)
            {
                path = Pathing(player.PositionOfFeet);
            }
            if (path.Length > 1)
            {
                if (hasAggro && Vector2.Distance(player.Position, Position) > attackSize)
                {
                    nextTileInPath = path[1];
                }
            }
            CheckAttackDistance();
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
                    if (VisionRange())
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
                Projectile proj = new Projectile("tile2", CenterMass, new Rectangle(0, 0, 20, 20), direction, 1000f);
                proj.Color = Color.Red;
                proj.Owner = this;
                proj.LayerDepth = 1F;
                attackProj.Add(proj);
            }
        }
    }
}
