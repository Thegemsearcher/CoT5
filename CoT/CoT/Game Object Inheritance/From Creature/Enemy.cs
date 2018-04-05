﻿using System;
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
        float scale = 0.1f;
        public Enemy(string texture, Vector2 position, Rectangle sourceRectangle, Player player, Grid grid) : base(texture, position, sourceRectangle)
        {
            this.player = player;
            this.grid = grid;
            destinationRectangle.Width = (int)(ResourceManager.Get<Texture2D>(Texture).Width * scale);
            destinationRectangle.Height = (int)(ResourceManager.Get<Texture2D>(Texture).Height * scale);
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
            nextPosition = new Vector2(nextTileInPath.X * Game1.Game.map.TileSize.Y, nextTileInPath.Y * Game1.Game.map.TileSize.Y).ToWorld();
            destinationRectangle.X = (int)Position.X;
            destinationRectangle.Y = (int)Position.Y;
            Move();
        }

        public void Move()
        {
            direction.X = nextPosition.X - Position.X;
            direction.Y = nextPosition.Y - Position.Y;
            direction.Normalize();

            Position += direction * speed * Time.DeltaTime;
        }
        

        public override void Draw()
        {
            for (int i = 0; i < path.Length; i++)
            {
                Game1.Game.SpriteBatch.Draw(ResourceManager.Get<Texture2D>("tile1"), new Vector2(path[i].X * Game1.Game.map.TileSize.Y,
                    path[i].Y * Game1.Game.map.TileSize.Y).ToWorld(), Color.Gray * 0.5f);
            }
            Game1.Game.SpriteBatch.Draw(ResourceManager.Get<Texture2D>(Texture), destinationRectangle, SourceRectangle, Color * Transparency, Rotation, Vector2.Zero, SpriteEffects.None, 0f);
            //base.Draw();
        }
    }
}
