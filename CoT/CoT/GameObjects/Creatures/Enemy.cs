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
        private Player player;
        //Grid grid;
        private Position[] path;
        private Position nextTileInPath;
        private float speed = 100f, aggroRange;
        private bool hasAggro = false;
        private Vector2 nextPosition, direction = new Vector2(0, 0);
        public Enemy(string texture, Vector2 position, Rectangle sourceRectangle, Vector2 depthSortingOffset, Player player, Grid grid, Map map, int hp, int attack, int defense) : base(texture, position, sourceRectangle, depthSortingOffset, map, hp, attack, defense)
        {
            this.player = player;
            this.grid = grid;
            attackSize = 100;
            this.Scale = 0.1f;
            LayerDepth = 0.7f;
            path = new Position[0];
            destinationRectangle.Width = (int)(ResourceManager.Get<Texture2D>(Texture).Width * Scale);
            destinationRectangle.Height = (int)(ResourceManager.Get<Texture2D>(Texture).Height * Scale);
            Hitbox.Size *= Scale;
            CenterMass = new Vector2(Position.X, Position.Y - destinationRectangle.Height / 2);
            //Det behövdes en offset för att attacken skulle bli lika stor åt alla håll.
            offsetAttackPosition = new Vector2(-destinationRectangle.Width / 4, -destinationRectangle.Height / 4);
            //Ska flyttas.
            aggroRange = 1000;
            Position = new Vector2(PositionOfFeet.X - (ResourceManager.Get<Texture2D>(Texture).Width * Scale) / 2, PositionOfFeet.Y - (ResourceManager.Get<Texture2D>(Texture).Height * Scale));
        }

        public bool DetectPlayer()
        {
            if (!hasAggro && (Vector2.Distance(player.Position, Position) <= aggroRange) && VisionRange())
            {
               
                return hasAggro = true;
            }
            else
                return false;
        }

        #region oanvänd bresenham algoritm
        public bool VisionRange()
        {
            Vector2 cartesianTileWorldPos = new Vector2(0, 0);
            List<Vector2> vision = BresenhamLine(Position, player.Position);
            Tile t;
            foreach (Vector2 pos in vision)
            {
                cartesianTileWorldPos.X = pos.X / map.TileSize.Y;
                cartesianTileWorldPos.Y = pos.Y / map.TileSize.Y;
                Point isometricScreenTile = (cartesianTileWorldPos.ToCartesian() + new Vector2(-0.5f, 0.5f)).ToPoint();
                for (int i = 0; i < map.TileMap.GetLength(0); i++)
                {
                    for (int j = 0; j < map.TileMap.GetLength(1); j++)
                    {
                        t = map.TileMap[i, j];
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
            Hitbox = new FloatRectangle(Position, new Vector2(SourceRectangle.Width * Scale, SourceRectangle.Height * Scale));
            base.Update();
            if (Health <= 0)
            {
                return;
            }
            if (DetectPlayer() || hasAggro)
            {
                path = Pathing(player.PositionOfFeet);
            }
            if (path.Length > 1)
            {
                nextTileInPath = path[1];
            }
            //Fienden kommer ha en animation när den attackerar, den ska då stå stilla.
            if (!attacking && (DetectPlayer() || hasAggro))
            {
                Move();
            }
            //Fienden går från sina fötter istället för 0,0 på bilden.
            destinationRectangle.X = (int)Position.X;
            destinationRectangle.Y = (int)Position.Y;
            CenterMass = new Vector2(PositionOfFeet.X, PositionOfFeet.Y - destinationRectangle.Height / 2);
            CheckAttackDistance();
        }
        public void CheckAttackDistance()
        {
            if (!attacking)
            {
                if (Vector2.Distance(CenterMass, player.CenterMass) <= attackSize)
                {
                    Attack(CenterMass - player.CenterMass);
                }
            } else
            {
                DamageToPlayer();
                attackTimer++;
                if (attackTimer == 100)
                {
                    attackTimer = 0;
                    attacking = false;
                }
            }
        }
        public virtual void DamageToPlayer()
        {
            if (player.Hitbox.Intersects(AttackHitBox) && !dealtDamage)
            {
                dealtDamage = true;
                player.GetHit(this);

                for (int i = 0; i < 15; i++)
                {
                    ParticleManager.Instance.Particles.Add(new Particle("lightMask", player.Position,
                        new Rectangle(0, 0, ResourceManager.Get<Texture2D>("lightMask").Width, ResourceManager.Get<Texture2D>("lightMask").Height),
                        Helper.RandomDirection(), 300f, 2f, Color.Red, 0f, 0.3f));
                }

                Camera.ScreenShake(0.1f, 20);
            }
        }

        public override void OnRemove()
        {
            for (int i = 0; i < 25; i++)
            {
                ParticleManager.Instance.Particles.Add(new Particle("lightMask", Position,
                    new Rectangle(0, 0, ResourceManager.Get<Texture2D>("lightMask").Width, ResourceManager.Get<Texture2D>("lightMask").Height),
                    Helper.RandomDirection(), 300f, 2f, Color.Orange, 0f, 0.3f));
            }

            base.OnRemove();
        }

        public void Move()
        {
            nextPosition = new Vector2(nextTileInPath.X * map.TileSize.Y, nextTileInPath.Y * map.TileSize.Y).ToIsometric();
            nextPosition.X += map.TileSize.X / 2;
            nextPosition.Y += map.TileSize.Y / 2;
            direction.X = nextPosition.X - PositionOfFeet.X;
            direction.Y = nextPosition.Y - PositionOfFeet.Y;
            direction.Normalize();
            PositionOfFeet += direction * speed * Time.DeltaTime;
            Position = new Vector2(PositionOfFeet.X - (ResourceManager.Get<Texture2D>(Texture).Width * Scale) / 2, PositionOfFeet.Y - (ResourceManager.Get<Texture2D>(Texture).Height * Scale));
        }

        public override void Draw(SpriteBatch sb)
        {
            for (int i = 0; i < path.Length; i++) //Ritar ut pathen som fienden rör sig efter.
            {
                sb.Draw(ResourceManager.Get<Texture2D>("tile1"), new Vector2(path[i].X * map.TileSize.Y,
                path[i].Y * map.TileSize.Y).ToIsometric(), null, Color.Gray * 0.5f, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0.5f);
            }
            //sb.Draw(ResourceManager.Get<Texture2D>(Texture), destinationRectangle, SourceRectangle, Color * Transparency, Rotation, Vector2.Zero, SpriteEffects.None, 0.9f);

            if (attacking)
            {
                sb.Draw(ResourceManager.Get<Texture2D>("tile1"), new Rectangle((int)AttackHitBox.Position.X, (int)AttackHitBox.Position.Y, (int)AttackHitBox.Size.X, (int)AttackHitBox.Size.Y)
                , SourceRectangle, Color.Red * 0.5f, Rotation, Vector2.Zero, SpriteEffects.None, 0f);
            }
            base.Draw(sb);
        }
    }
}
