using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Penumbra;
using Console = System.Console;
using RoyT.AStar;

namespace CoT
{
    public class Player : Creature
    {

        float speed = 200f;
        Vector2 direction;
        Vector2 targetPos;
        bool moving;

        enum HeroClass
        {

        }

        private Penumbra.Light light;

        //Grid grid;
        //Position[] path;
        //Position toTheNextTile;

        public Player(string texture, Vector2 position, Rectangle sourceRectangle) : base(texture, position, sourceRectangle)
        {
            light = new PointLight();
            light.Scale = new Vector2(5000, 5000).ToScreen();
            light.Intensity = 0.5f;
            Game1.Game.Penumbra.Lights.Add(light);
            Scale = 3;
        }

        public override void Update()
        {

            base.Update();
            light.Position = Position;
            if (Input.IsLeftClickPressed)
            {
                //Position = Camera.ScreenToWorld(Input.CurrentMousePosition);
                targetPos = Camera.ScreenToWorld(Input.CurrentMousePosition);
                direction = GetDirection(Position, targetPos);
                moving = true;

            }

            if (Vector2.Distance(Position, targetPos) < 10)
            {
                moving = false;
            }

            if (moving)
            {
                Move(direction);
            }
        }

        public void Move(Vector2 direction)
        {
            Position += direction * speed * Time.DeltaTime;
        }

        //public void CheckForCollision()
        //{
        //    int stoppingDistance = 10;
        //    Vector2 newDestination = Position + direction * speed * Time.DeltaTime * stoppingDistance;

        //    //if (GetTileAtPosition(newDestination).IsWall)
        //    if (Map.TileMap[x, y].TileType == TileType.Wall)
        //    {
        //        moving = false;
        //    }
        //}

        //public Tile GetTileAtPosition(Vector2 position)
        //{
        //    return tiles[(int)position.X / tileSize, (int)position.Y / tileSize];
        //}

        public Vector2 GetDirection(Vector2 currentPos, Vector2 targetPos)
        {
            Vector2 travellDirection = targetPos - currentPos;

            travellDirection.Normalize();

            return travellDirection;
        }

        public override void Draw()
        {
            base.Draw();
        }
    }
}
