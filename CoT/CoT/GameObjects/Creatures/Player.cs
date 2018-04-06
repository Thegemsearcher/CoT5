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
using System.Threading;

namespace CoT
{
    public class Player : Creature
    {

        float speed = 200f;
        Vector2 direction;
        Vector2 targetPos;
        bool moving;
        Map map;
        FloatRectangle bottomHitBox;
        public Vector2 CenterMass { get; private set; }

        enum HeroClass
        {

        }

        private Penumbra.Light light;

        public Player(string texture, Vector2 position, Rectangle sourceRectangle, Map map) : base(texture, position, sourceRectangle)
        {
            this.map = map;
            light = new PointLight();
            light.Scale = new Vector2(5000, 5000).ToCartesian();
            light.Intensity = 0.2f;
            light.ShadowType = ShadowType.Occluded;
            GameManager.Instance.Penumbra.Lights.Add(light);
            Scale = 3;

            CenterMass = new Vector2(Position.X, Position.Y - SourceRectangle.Height * Scale);
            destinationRectangle.Width = (int)(ResourceManager.Get<Texture2D>(Texture).Width * Scale);
            destinationRectangle.Height = (int)(ResourceManager.Get<Texture2D>(Texture).Height * Scale);

            bottomHitBox = new FloatRectangle(new Vector2(Position.X, Position.Y + (int)(SourceRectangle.Height * 0.90 * Scale)),
                new Vector2(SourceRectangle.Width * Scale, (SourceRectangle.Height * Scale) / 10));


            Offset = new Vector2((float)SourceRectangle.Width / 2, (float)SourceRectangle.Height / 2);
        }

        public override void Update()
        {

            base.Update();
            light.Position = Position;
            Camera.Focus = Position;

            bottomHitBox = new FloatRectangle(new Vector2(Position.X, Position.Y + (int)(SourceRectangle.Height * 0.90 * Scale)),
                new Vector2(SourceRectangle.Width * Scale, (SourceRectangle.Height * Scale) / 10));

            if (Input.IsLeftClickPressed) //Vid musklick får spelaren en ny måldestination och börjar röra sig
            {
                //Position = Camera.ScreenToWorld(Input.CurrentMousePosition);
                targetPos = Input.CurrentMousePosition.ScreenToWorld();
                direction = GetDirection(PositionOfFeet, targetPos);
                moving = true;

            }

            CheckForCollision();

            if (Vector2.Distance(PositionOfFeet, targetPos) < 5) //Spelaren slutar röra sig inom 10 pixlar av sin destination
            {
                moving = false;
            }

            if (moving)
            {
                Move(direction);
            }
            UpdateVariables();


        }

        public void UpdateVariables()
        {
            CenterMass = new Vector2(PositionOfFeet.X, PositionOfFeet.Y - (SourceRectangle.Height / 2) * Scale);
            destinationRectangle.X = (int)Position.X;
            destinationRectangle.Y = (int)Position.Y;

            Position = new Vector2(PositionOfFeet.X - (ResourceManager.Get<Texture2D>(Texture).Width * Scale) / 2,
                PositionOfFeet.Y - (ResourceManager.Get<Texture2D>(Texture).Height * Scale));
        }

        public void Move(Vector2 direction) //Förflyttar spelaren med en en riktningsvektor, hastighet och deltatid
        {
            PositionOfFeet += direction * speed * Time.DeltaTime;
        }

        //public void CheckForCollision()//Detta fungerar inte
        //{
        //    //int stoppingDistance = 10;
        //    //Vector2 newDestination = Position + direction * speed * Time.DeltaTime * stoppingDistance;

        //    //Vector2 cartesianTileWorldPos = new Vector2(Camera.ScreenToWorld(newDestination).X / map.TileSize.Y,
        //    //Camera.ScreenToWorld(newDestination).Y / map.TileSize.Y);

        //    //Point isometricScreenTile = (cartesianTileWorldPos.ToScreen() + new Vector2(-0.5f, 0.5f)).ToPoint();

        //    //int x = (int)newDestination.X / 160;
        //    //int y = (int)newDestination.Y / 80;

        //    //if (map.TileMap[isometricScreenTile.X, isometricScreenTile.Y].TileType == TileType.Wall)
        //    //{
        //    //    moving = false;
        //    //}
        //}

        public void CheckForCollision()
        {
            for (int x = 0; x < map.TileMap.GetLength(0); x++)
            {
                for (int y = 0; y < map.TileMap.GetLength(1); y++)
                {
                    if (bottomHitBox.Intersects(new FloatRectangle(new Vector2(x * map.TileSize.Y, y * map.TileSize.Y).ToCartesian(),
                        map.TileSize.ToVector2())) && map.TileMap[x, y].TileType == TileType.Wall)
                    {
                        Position += -direction * speed * Time.DeltaTime;
                        moving = false;
                    }
                }
            }
        }

        public Vector2 GetDirection(Vector2 currentPos, Vector2 targetPos) //Ger en normaliserad riktning mellan två positioner
        {
            Vector2 travelDirection = targetPos - currentPos;

            travelDirection.Normalize();

            return travelDirection;
        }

        public override void Draw(SpriteBatch sb)
        {
            //Game1.Game.SpriteBatch.Draw(ResourceManager.Get<Texture2D>(Texture), Position, SourceRectangle, Color * Transparency, Rotation, Vector2.Zero, Scale, SpriteEffects.None, 0f);
            //Game1.Game.SpriteBatch.Draw(ResourceManager.Get<Texture2D>(Texture), new Rectangle((int)Hitbox.Position.X - (int)((SourceRectangle.Width * Scale) / 2),
            //   (int)Hitbox.Position.Y - (int)(SourceRectangle.Height * Scale), (int)Hitbox.Size.X, (int)Hitbox.Size.Y), Color.White);

            sb.Draw(ResourceManager.Get<Texture2D>(Texture), destinationRectangle,
                SourceRectangle, Color * Transparency, Rotation, Vector2.Zero, SpriteEffects.None, 0f);

            //Debug
            //FullHitbox
            //sb.Draw(ResourceManager.Get<Texture2D>("rectangle"), new Rectangle((int)Hitbox.Position.X - (int)((SourceRectangle.Width * Scale) / 2),
               //(int)Hitbox.Position.Y - (int)(SourceRectangle.Height * Scale), (int)Hitbox.Size.X, (int)Hitbox.Size.Y), Color.Red * 0.1f);
            sb.Draw(ResourceManager.Get<Texture2D>("rectangle"), new Rectangle((int)Hitbox.Position.X, (int)Hitbox.Position.Y, (int)Hitbox.Size.X, (int)Hitbox.Size.Y), Color.Red * 0.1f);

            //BottomHitox 
            sb.Draw(ResourceManager.Get<Texture2D>("rectangle"), new Rectangle((int)bottomHitBox.Position.X, (int)bottomHitBox.Position.Y, (int)bottomHitBox.Size.X, (int)bottomHitBox.Size.Y), Color.Red * 0.5f);

            //sb.Draw(ResourceManager.Get<Texture2D>("rectangle"), new Rectangle((int)bottomHitBox.Position.X - (int)((SourceRectangle.Width * Scale) / 2),
            //(int)bottomHitBox.Position.Y - (int)(SourceRectangle.Height * Scale), (int)bottomHitBox.Size.X, (int)bottomHitBox.Size.Y), Color.Red * 0.5f);
            base.Draw(sb);

        }
    }
}
