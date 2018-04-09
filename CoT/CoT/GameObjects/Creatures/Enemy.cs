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
    enum BehaveState
    {

    }

    public class Enemy : Creature
    {
        Player player;
        //Grid grid;
        Position[] path;
        Position nextTileInPath;
        float speed = 100f;
        Vector2 nextPosition, direction = new Vector2(0, 0);
        public Enemy(string texture, Vector2 position, Rectangle sourceRectangle, Player player, Grid grid, Map map) : base(texture, position, sourceRectangle, map)
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
            offsetAttackPosition = new Vector2(-destinationRectangle.Width/4, -destinationRectangle.Height / 4);
        }

        public void DetectPlayer()
        {

        }

        public override void Update()
        {
            base.Update();
            path = Pathing(player.PositionOfFeet);

            if (path.Length > 1)
            {
                nextTileInPath = path[1];
            }
            //Fienden kommer ha en animation när den attackerar, den ska då stå stilla.
            if (!attacking)
            {
                Move();
            }

            //Move();
            
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
                //Player tar damage.
            }
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
            sb.Draw(ResourceManager.Get<Texture2D>(Texture), destinationRectangle, SourceRectangle, Color * Transparency, Rotation, Vector2.Zero, SpriteEffects.None, 0.9f);

            //sb.Draw(ResourceManager.Get<Texture2D>("rectangle"), new Rectangle((int)Hitbox.Position.X - (int)((SourceRectangle.Width * Scale) / 2),
            //  (int)Hitbox.Position.Y - (int)(SourceRectangle.Height * Scale), (int)Hitbox.Size.X, (int)Hitbox.Size.Y), Color.Red * 0.1f);
            
            //sb.Draw(ResourceManager.Get<Texture2D>(Texture), new Rectangle((int)CenterMass.X, (int)CenterMass.Y, 10, 10)/*(int)AttackHitBox.Position.X, (int)AttackHitBox.Position.Y, (int)AttackHitBox.Size.X, (int)AttackHitBox.Size.Y)*/
            //, SourceRectangle, Color.Red, Rotation, Vector2.Zero, SpriteEffects.None, 0f);

            if (attacking)
            {
                sb.Draw(ResourceManager.Get<Texture2D>("tile1"), new Rectangle((int)AttackHitBox.Position.X, (int)AttackHitBox.Position.Y, (int)AttackHitBox.Size.X, (int)AttackHitBox.Size.Y)
                , SourceRectangle, Color.Red * 0.5f, Rotation, Vector2.Zero, SpriteEffects.None, 0f);
            }
        }
    }
}
