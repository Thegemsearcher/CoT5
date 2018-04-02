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
        Grid grid;
        Position[] path;
        Position nextTileInPath;
        public Enemy(string texture, Vector2 position, Rectangle sourceRectangle, Player player, Grid grid) : base(texture, position, sourceRectangle)
        {
            this.player = player;
            this.grid = grid;
        }

        public void DetectPlayer()
        {

        }

        public override void Update()
        {
            base.Update();
            path = Pathing();
            nextTileInPath = path[0];
        }

        public void Move()
        {
            Vector2 nextPosition = new Vector2(nextTileInPath.X * Game1.Game.map.TileSize.Y, nextTileInPath.Y * Game1.Game.map.TileSize.Y).ToWorld();
        }

        public Position[] Pathing()
        {
            Vector2 cartesianTileWorldPosEnemy = new Vector2(Camera.ScreenToWorld(Position).X / Game1.Game.map.TileSize.Y,
                Camera.ScreenToWorld(Position).Y / Game1.Game.map.TileSize.Y);
            Point isometricScreenTileEnemy = (cartesianTileWorldPosEnemy.ToScreen() + new Vector2(-0.5f, 0.5f)).ToPoint();
            //Gör om positionen för fienden till en position vi kan använda. 

            Vector2 cartesianTileWorldPosPlayer = new Vector2(Camera.ScreenToWorld(Position).X / Game1.Game.map.TileSize.Y,
                Camera.ScreenToWorld(Position).Y / Game1.Game.map.TileSize.Y);
            Point isometricScreenTilePlayer = (cartesianTileWorldPosPlayer.ToScreen() + new Vector2(-0.5f, 0.5f)).ToPoint();
            //Gör om spelarens position till en position vi kan använda.
            
            Position[] enemyPath = grid.GetPath(new Position(isometricScreenTileEnemy.X, isometricScreenTileEnemy.Y), 
                new Position(isometricScreenTilePlayer.X, isometricScreenTilePlayer.Y), MovementPatterns.LateralOnly);

            return enemyPath;
        }
        public override void Draw()
        {

            for (int i = 0; i < path.Length; i++)
            {
                Game1.Game.SpriteBatch.Draw(ResourceManager.Get<Texture2D>("tile1"), new Vector2(path[i].X * Game1.Game.map.TileSize.Y,
                    path[i].Y * Game1.Game.map.TileSize.Y).ToWorld(), Color.Green * 0.5f);
            }
            Game1.Game.SpriteBatch.Draw(ResourceManager.Get<Texture2D>(Texture), Position, SourceRectangle, Color * Transparency, Rotation, Vector2.Zero, 0.1f, SpriteEffects.None, 0f);
            //base.Draw();
        }
    }
}
