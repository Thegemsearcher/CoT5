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
        float speed = 100f,scale = 0.1f;
        Vector2 nextPosition, direction = new Vector2(0, 0);
        public Enemy(string texture, Vector2 position, Rectangle sourceRectangle, Player player, Grid grid) : base(texture, position, sourceRectangle)
        {
            this.player = player;
            this.grid = grid;

            attackSize = 100;

            destinationRectangle.Width = (int)(ResourceManager.Get<Texture2D>(Texture).Width * scale);
            destinationRectangle.Height = (int)(ResourceManager.Get<Texture2D>(Texture).Height * scale);
            CenterMass = new Vector2(Position.X, Position.Y - destinationRectangle.Height/2);
        }

        public void DetectPlayer()
        {

        }

        public override void Update()
        {
            base.Update();
            path = Pathing(player.Position);

            if (path.Length > 1)
            {
                nextTileInPath = path[1];
            }
            Move();
            //Fienden går från sina fötter istället för 0,0 på bilden.
            destinationRectangle.X = (int)Position.X - destinationRectangle.Width / 2;
            destinationRectangle.Y = (int)Position.Y - destinationRectangle.Height;
            CenterMass = new Vector2(Position.X, Position.Y - destinationRectangle.Height / 2);
            CheckAttackDistance();
        }
        public void CheckAttackDistance()
        {
            if (Vector2.Distance(CenterMass, player.CenterMass) <= attackSize)
            {
                Attack(CenterMass - player.CenterMass);
            }
        }
        public void Move()
        {
            nextPosition = new Vector2(nextTileInPath.X * GameStateManager.Instance.Map.TileSize.Y, nextTileInPath.Y * GameStateManager.Instance.Map.TileSize.Y).ToIsometric();
            nextPosition.X += GameStateManager.Instance.Map.TileSize.X / 2;
            nextPosition.Y += GameStateManager.Instance.Map.TileSize.Y / 2;
            direction.X = nextPosition.X - Position.X;
            direction.Y = nextPosition.Y - Position.Y;
            direction.Normalize();
            Position += direction * speed * Time.DeltaTime;
        }

        public override void Draw(SpriteBatch sb)
        {
            for (int i = 0; i < path.Length; i++) //Ritar ut pathen som fienden rör sig efter.
            {
               sb.Draw(ResourceManager.Get<Texture2D>("tile1"), new Vector2(path[i].X * GameStateManager.Instance.Map.TileSize.Y,
               path[i].Y * GameStateManager.Instance.Map.TileSize.Y).ToIsometric(), Color.Gray * 0.5f);
            }
            sb.Draw(ResourceManager.Get<Texture2D>(Texture), destinationRectangle, SourceRectangle, Color * Transparency, Rotation, Vector2.Zero, SpriteEffects.None, 0f);

            //sb.Draw(ResourceManager.Get<Texture2D>(Texture), new Rectangle(/*(int)player.CenterMass.X,(int)player.CenterMass.Y,10,10)*/(int)AttackHitBox.Position.X, (int)AttackHitBox.Position.Y, (int)AttackHitBox.Size.X, (int)AttackHitBox.Size.Y)
            //    , SourceRectangle, Color.Red, Rotation, Vector2.Zero, SpriteEffects.None, 0f);
        }
    }
}
