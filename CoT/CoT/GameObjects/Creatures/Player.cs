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
using CoT.Helpers;

namespace CoT
{
    public class Player : Creature
    {

        private float speed = 200f;
        private Vector2 direction, nextPosition, targetPos;
        private Position[] path;
        private Position nextTileInPath;
        private bool normalMoving, pathMoving;
        private FloatRectangle bottomHitBox;
        private int frame, animationOffset = 27, animationStarts = 0, amountOfFrames = 5;
        private float frameTimer = 100, frameInterval = 100;

        enum HeroClass
        {

        }

        private Penumbra.Light light;

        public Player(string texture, Vector2 position, Rectangle sourceRectangle, Vector2 depthSortingOffset, Grid grid, Map map, int hp, int attack, int defense) : base(texture, position, sourceRectangle, depthSortingOffset, map, hp, attack, defense)
        {
            //this.enemies = enemies;
            this.map = map;
            this.grid = grid;
            attackSize = 150;
            light = new PointLight();
            light.Scale = new Vector2(5000, 5000).ToCartesian();
            light.Intensity = 0.2f;
            light.ShadowType = ShadowType.Solid;
            GameManager.Instance.Penumbra.Lights.Add(light);
            Scale = 3;
            LayerDepth = 1f;

            CenterMass = new Vector2(PositionOfFeet.X, Position.Y - SourceRectangle.Height * Scale);
            destinationRectangle.Width = (int)(sourceRectangle.Width);
            destinationRectangle.Height = (int)(sourceRectangle.Height);
            bottomHitBox = new FloatRectangle(new Vector2(Position.X, Position.Y + (int)(SourceRectangle.Height * 0.90 * Scale)),
                new Vector2(SourceRectangle.Width * Scale, (SourceRectangle.Height * Scale) / 10));
            Offset = new Vector2((float)SourceRectangle.Width / 2, (float)SourceRectangle.Height / 2);
        }
        public override void Update()
        {
            Hitbox = new FloatRectangle(Position, new Vector2(SourceRectangle.Width * Scale, SourceRectangle.Height * Scale));
            base.Update();
            if (Health <= 0)
            {
                return;
            }
            light.Position = PositionOfFeet;
            Camera.Focus = PositionOfFeet;

            if (Input.IsRightClickPressed && !attacking) //Vid musklick får spelaren en ny måldestination och börjar röra sig,
                //spelaren kan inte röra sig under tiden det tar att utföra en attack
            {
                targetPos = Input.CurrentMousePosition.ScreenToWorld();
                direction = GetDirection(PositionOfFeet, targetPos);
                normalMoving = true;
                pathMoving = false;
            }

            if (normalMoving)
            {
                CheckForCollision();
            }

            if (Vector2.Distance(PositionOfFeet, targetPos) < map.TileSize.Y / 16) //Spelaren slutar röra sig inom 10 pixlar av sin destination
            {
                normalMoving = false;
                pathMoving = false;
            }

            if (normalMoving)
            {
                Move(direction);
            }

            if (pathMoving)
            {
                path = Pathing(targetPos);
                if (path.Length > 1)
                {
                    nextTileInPath = path[1];
                    PathMove();
                }
                else
                {
                    pathMoving = false;
                    normalMoving = true;
                    direction = GetDirection(PositionOfFeet, targetPos);
                }
               
            }
            Animation();
            AttackLockTimer();
            InputAttack();
            UpdateVariables();
        }

        public void InputAttack() //Vid högerklick attackerar spelaren
        {
            Vector2 attackDirection;

            if (Input.IsLeftClickPressed && !attacking)
            {
                attacking = true;
                normalMoving = false;
                pathMoving = false;
                attackDirection = GetDirection(CenterMass, Input.CurrentMousePosition.ScreenToWorld());
                DecideEnemiesInRange(attackDirection);

                for (int i = 0; i < 20; i++)
                {
                    ParticleManager.Instance.Particles.Add(new Particle("lightMask", Position,
                        new Rectangle(0, 0, ResourceManager.Get<Texture2D>("lightMask").Width, ResourceManager.Get<Texture2D>("lightMask").Height),
                        attackDirection + Helper.RandomDirection() / 3, 1000f, 5f, Color.Green, 0f, 0.2f));
                }

                Camera.ScreenShake(0.1f, 2);
            }
        }

        public void AttackLockTimer() //Låser spelaren i en attack under 30 frames
        {
            int attackDuration = 30;

            if (attacking)
            {
                attackTimer++;
                if (attackTimer >= attackDuration)
                {
                    attackTimer = 0;
                    attacking = false;
                }
            }
        }

        public void DecideEnemiesInRange(Vector2 direction) //Ser ifall fiendernas mittpunkt är inom 45-grader av den ursprungliga attackvinkeln och inom attackrange
        {
            foreach (Creature c in CreatureManager.Instance.Creatures)
            {
                if (c is Enemy e)
                {
                    if (Vector2.Distance(CenterMass, e.CenterMass) <= attackSize)
                    {
                        Vector2 directionToEnemy = GetDirection(CenterMass, e.CenterMass);
                        double angleBetweenEnemyAndAngleToAttack = Math.Acos(Vector2.Dot(direction, directionToEnemy));

                        Console.WriteLine(MathHelper.ToDegrees((float)angleBetweenEnemyAndAngleToAttack));
                        float attackCone = MathHelper.ToDegrees((float)(Math.PI / 5));//attackkonen är en kon med 45 graders vinkel

                        if (MathHelper.ToDegrees((float)angleBetweenEnemyAndAngleToAttack) < attackCone)
                        {
                            Console.WriteLine("Hit!");
                            e.GetHit(this);
                        }
                        else
                        {
                            Console.WriteLine("Miss!");
                        }
                    }
                }
            }
        }

        public void UpdateVariables() //Samlar uppdatering av variabler
        {
            float bottomHitBoxWidth = SourceRectangle.Width * Scale / 5;
            bottomHitBox = new FloatRectangle(new Vector2(Position.X + ((float)SourceRectangle.Width * Scale / 2) - ((float)bottomHitBoxWidth / 2), 
                Position.Y + (int)(SourceRectangle.Height * 0.90 * Scale)), new Vector2(bottomHitBoxWidth, (SourceRectangle.Height * Scale) / 10));
            CenterMass = new Vector2(PositionOfFeet.X, PositionOfFeet.Y - (SourceRectangle.Height / 2) * Scale);
            destinationRectangle.X = (int)Position.X;
            destinationRectangle.Y = (int)Position.Y;
            Position = new Vector2(PositionOfFeet.X - (SourceRectangle.Width * Scale) / 2,
                PositionOfFeet.Y - (SourceRectangle.Height * Scale));
        } 

        public void Move(Vector2 direction) //Förflyttar spelaren med en en riktningsvektor, hastighet och deltatid
        {
            PositionOfFeet += direction * speed * Time.DeltaTime;
        }

        public void CheckForCollision() //Kollision med väggtile-check
        {
            int stoppingDistance = map.TileSize.Y / 16;
            for (int x = 0; x < map.TileMap.GetLength(0); x++)
            {
                for (int y = 0; y < map.TileMap.GetLength(1); y++)
                {
                    Vector2 tilePos = map.GetTilePosition(new Vector2(x, y)).ToCartesian();
                    Vector2 estimatedHitboxPos = (PositionOfFeet + (direction * stoppingDistance)).ToCartesian();
                    //Vector2 hitboxPos = bottomHitBox.Position.ToCartesian();
                    FloatRectangle hitbox = new FloatRectangle(estimatedHitboxPos, bottomHitBox.Size);

                    if (hitbox.Intersects(new FloatRectangle(tilePos, new Vector2(80, 80))) && map.TileMap[x, y].TileType == TileType.Collision)
                    {
                        normalMoving = false;
                        pathMoving = true;
                    }
                }
            }
        } 

        public void PathMove() //Rörelse via Pathfinding
        {
            //if (pathMoving)
            {
                nextPosition = new Vector2(nextTileInPath.X * map.TileSize.Y, nextTileInPath.Y * map.TileSize.Y).ToIsometric();
                nextPosition.X += map.TileSize.X / 2;
                nextPosition.Y += map.TileSize.Y / 2;
                direction.X = nextPosition.X - PositionOfFeet.X;
                direction.Y = nextPosition.Y - PositionOfFeet.Y;
                direction.Normalize();
                PositionOfFeet += direction * speed * Time.DeltaTime;
                Position = new Vector2(PositionOfFeet.X - (SourceRectangle.Width * Scale) / 2, PositionOfFeet.Y - (SourceRectangle.Height * Scale));
            }     
        }

        public Vector2 GetDirection(Vector2 currentPos, Vector2 targetPos) //Ger en normaliserad riktning mellan två positioner
        {
            Vector2 targetDirection = targetPos - currentPos;

            targetDirection.Normalize();

            return targetDirection;
        }

        public void Animation()
        {
            SourceRectangle = AnimationHelper.Animation(SourceRectangle, ref frameTimer , frameInterval, ref frame, animationStarts, amountOfFrames,animationOffset);
        }

        public override void Draw(SpriteBatch sb)
        {
            //Debug
            //FullHitbox
            sb.Draw(ResourceManager.Get<Texture2D>("rectangle"), destinationRectangle/*new Rectangle((int)Hitbox.Position.X, (int)Hitbox.Position.Y, (int)Hitbox.Size.X, (int)Hitbox.Size.Y)*/, Color.Red * 0.1f);
            
            //BottomHitox 
            sb.Draw(ResourceManager.Get<Texture2D>("rectangle"), new Rectangle((int)bottomHitBox.Position.X, (int)bottomHitBox.Position.Y, (int)bottomHitBox.Size.X, (int)bottomHitBox.Size.Y), Color.Red * 0.5f);

            //CenterMass 
            sb.Draw(ResourceManager.Get<Texture2D>("rectangle"), new Rectangle((int)CenterMass.X, (int)CenterMass.Y, (int)bottomHitBox.Size.X, (int)bottomHitBox.Size.Y), Color.Black * 0.9f);

            //foreach (Creature c in CreatureManager.Instance.Creatures) invulnerability
            //{
            //    //Enemy CenterMass 
            //    sb.Draw(ResourceManager.Get<Texture2D>("rectangle"), new Rectangle((int)e.CenterMass.X, (int)e.CenterMass.Y, (int)bottomHitBox.Size.X, (int)bottomHitBox.Size.Y), Color.Black * 0.9f);
            //}



            //for (int i = 0; i < GameStateManager.Instance.Map.TileMap.GetLength(0); i++)
            //{
            //    for (int j = 0; j < GameStateManager.Instance.Map.TileMap.GetLength(1); j++)
            //    {
            //        Tile t = GameStateManager.Instance.Map.TileMap[i, j];

            //        Vector2 pos = GameStateManager.Instance.Map.GetTilePosition(new Vector2(i, j)).ToCartesian();

            //        if (t.TileType == TileType.Wall)
            //            sb.Draw(ResourceManager.Get<Texture2D>("rectangle"), new Rectangle((int)pos.X, (int)pos.Y, map.TileSize.Y, map.TileSize.Y), Color.Purple * 0.3f);
            //    }
            //}
            //Vector2 hitboxPos = bottomHitBox.Position.ToCartesian();
            //FloatRectangle hitbox = new FloatRectangle(hitboxPos, bottomHitBox.Size);
            //sb.Draw(ResourceManager.Get<Texture2D>("rectangle"), new Rectangle((int)hitbox.Position.X, (int)hitbox.Position.Y, (int)bottomHitBox.Size.X, (int)bottomHitBox.Size.Y), Color.Red * 0.5f);


            base.Draw(sb);

        }
    }
}
