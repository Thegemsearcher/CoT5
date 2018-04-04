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
        Map map;

        enum HeroClass
        {

        }

        private Penumbra.Light light;

        //Grid grid;
        //Position[] path;
        //Position toTheNextTile;

        public Player(string texture, Vector2 position, Rectangle sourceRectangle, Map map) : base(texture, position, sourceRectangle)
        {
            this.map = map;
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
            if (Input.IsLeftClickPressed) //Vid musklick får spelaren en ny måldestination och börjar röra sig
            {
                //Position = Camera.ScreenToWorld(Input.CurrentMousePosition);
                targetPos = Camera.ScreenToWorld(Input.CurrentMousePosition);
                direction = GetDirection(Position, targetPos);
                moving = true;

            }

            if (Vector2.Distance(Position, targetPos) < 10) //Spelaren slutar röra sig inom 10 pixlar av sin destination
            {
                moving = false;
            }

            if (moving)
            {
                Move(direction);
            }

            CheckForCollision();
        }

        public void Move(Vector2 direction) //Förflyttar spelaren med en en riktningsvektor, hastighet och deltatid
        {
            Position += direction * speed * Time.DeltaTime;
        }

        public void CheckForCollision() //Detta fungerar inte
        {
            //int stoppingDistance = 10;
            //Vector2 newDestination = Position + direction * speed * Time.DeltaTime * stoppingDistance;

            //int x = (int)newDestination.X / map.TileSize.X;
            //int y = (int)newDestination.Y / map.TileSize.Y;

            //if (map.TileMap[x, y].TileType == TileType.Wall)
            //{
            //    moving = false;
            //}
        }

        public Vector2 GetDirection(Vector2 currentPos, Vector2 targetPos) //Ger en normaliserad riktning mellan två positioner
        {
            Vector2 travelDirection = targetPos - currentPos;

            travelDirection.Normalize();

            return travelDirection;
        }

        public override void Draw()
        {
            base.Draw();
        }
    }
}
