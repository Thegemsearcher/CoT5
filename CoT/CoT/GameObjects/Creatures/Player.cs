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
        private FloatRectangle bottomHitBox;
        //private int frame, animationOffset = 27, animationStarts = 0, amountOfFrames = 5;
        //private float frameTimer = 100, frameInterval = 100;

        enum MovementState 
        {
            Idle,
            DirectMoving,
            Pathfinding,
            Collide,
        }

        private MovementState currentMovementState;

        private Penumbra.Light light;

        //public Player(string texture, Vector2 position, Rectangle sourceRectangle, Vector2 depthSortingOffset, Grid grid, Map map, int hp, int attack, int defense) : base(texture, position, sourceRectangle, depthSortingOffset, map, hp, attack, defense)
        //{
        //    this.map = map;
        //    this.grid = grid;
        //    attackSize = 150;
        //    light = new PointLight();
        //    light.Scale = new Vector2(5000, 5000).ToCartesian();
        //    light.Intensity = 0.2f;
        //    light.ShadowType = ShadowType.Solid;
        //    GameManager.Instance.Penumbra.Lights.Add(light);
        //    Scale = 3;
        //    LayerDepth = 1f;
        //    //
        //    defense = 10000;
        //    //
        //    CenterMass = new Vector2(PositionOfFeet.X, Position.Y - SourceRectangle.Height * Scale);
        //    destinationRectangle.Width = (int)(sourceRectangle.Width);
        //    destinationRectangle.Height = (int)(sourceRectangle.Height);
        //    bottomHitBox = new FloatRectangle(new Vector2(Position.X, Position.Y + (int)(SourceRectangle.Height * 0.90 * Scale)),
        //        new Vector2(SourceRectangle.Width * Scale, (SourceRectangle.Height * Scale) / 10));
        //    Offset = new Vector2((float)SourceRectangle.Width / 2, (float)SourceRectangle.Height / 2);


        //    spriteSheet = new Spritesheet(texture, new Point(5, 1), SourceRectangle, 100);

        //}

        public Player(Spritesheet spritesheet, Vector2 position, Vector2 groundPositionOffset, Vector2 depthSortingOffset, Stats stats, Map map, Grid grid, Player player) : base(spritesheet, position, groundPositionOffset, depthSortingOffset, stats, map, grid, player)
        {
            attackRange = 150;

            light = new PointLight();
            light.Scale = new Vector2(5000, 5000).ToCartesian();
            light.Intensity = 0.2f;
            light.ShadowType = ShadowType.Solid;
            GameManager.Instance.Penumbra.Lights.Add(light);
            Scale = 3;
            LayerDepth = 1f;
            bottomHitBox = new FloatRectangle(new Vector2(Position.X, Position.Y + (int)(spritesheet.SourceRectangle.Height * 0.90 * Scale)),
                new Vector2(spritesheet.SourceRectangle.Width * Scale, (spritesheet.SourceRectangle.Height * Scale) / 10));

            spritesheet.SetFrameCount(new Point(5, 1));
        }

        public override void Update()
        {
            base.Update();
            if (Stats.Health <= 0)
            {
                return;
            }

            if (Input.IsRightClickPressed && !isAttacking) //Vid musklick får spelaren en ny måldestination och börjar röra sig,
                                                         //spelaren kan inte röra sig under tiden det tar att utföra en attack
            {
                targetPos = Input.CurrentMousePosition.ScreenToWorld();
                direction = GetDirection(GroundPosition, targetPos);
                path = Pathing(targetPos);
                currentMovementState = MovementState.DirectMoving;
            }

            switch (currentMovementState)
            {
                case MovementState.Idle:
                    Animation();
                    break;
                case MovementState.DirectMoving:
                    Move(direction);
                    if (CheckForCollision())
                    {
                        currentMovementState = MovementState.Collide;
                    }
                    break;
                case MovementState.Pathfinding:
                    PathMoving();
                    if (path.Length <= 1)
                    {
                        direction = GetDirection(GroundPosition, targetPos);
                        currentMovementState = MovementState.DirectMoving;
                    }
                    break;
                case MovementState.Collide:
                    Move((direction * -1));
                    if (CheckForCollision())
                    {
                        break;
                    }
                    if (path.Length < 1)
                    {
                        currentMovementState = MovementState.Idle;
                    }
                    else
                    {
                        currentMovementState = MovementState.Pathfinding;
                    }
                    break;
                default:
                    break;
            }
            StopMoving();
            AttackLockTimer();
            InputAttack();
            UpdateVariables();
        }

        private void PathMoving()
        {
            path = Pathing(targetPos);
            if (path.Length > 1)
            {
                nextTileInPath = path[1];
                PathMove();
            }
        }

        private void StopMoving()
        {
            if (Vector2.Distance(GroundPosition, targetPos) < Map.TileSize.Y / 16) //Spelaren slutar röra sig inom 10 pixlar av sin destination
            {
                currentMovementState = MovementState.Idle;
            }
        }

        public void InputAttack() //Vid vänsterklick attackerar spelaren
        {
            Vector2 attackDirection;

            if (Input.IsLeftClickPressed && !isAttacking)
            {
                isAttacking = true;
                attackDirection = GetDirection(Position + Center, Input.CurrentMousePosition.ScreenToWorld());
                DecideEnemiesInRange(attackDirection);

                for (int i = 0; i < 20; i++)
                {
                    ParticleManager.CreateStandard(Position + Center/2, attackDirection + Helper.RandomDirection(), Color.BlueViolet);
                    //ParticleManager.Instance.Particles.Add(new Particle("lightMask", Center,
                    //    new Rectangle(0, 0, ResourceManager.Get<Texture2D>("lightMask").Width, ResourceManager.Get<Texture2D>("lightMask").Height),
                    //    attackDirection + Helper.RandomDirection() / 3, 1000f, 5f, Color.Green, 0f, 0.2f));
                }

                Camera.ScreenShake(0.1f, 2);
            }
        }

        public void AttackLockTimer() //Låser spelaren i en attack under 30 frames
        {
            int attackDuration = 20;

            if (isAttacking)
            {
                attackTimer++;
                if (attackTimer >= attackDuration)
                {
                    attackTimer = 0;
                    isAttacking = false;
                }
            }
        }

        public void DecideEnemiesInRange(Vector2 direction) //Ser ifall fiendernas mittpunkt är inom 45-grader av den ursprungliga attackvinkeln och inom attackrange
        {
            foreach (Creature c in CreatureManager.Instance.Creatures)
            {
                if (c is Enemy e)
                {
                    if (Vector2.Distance(Position + Center, e.Position + e.Center) <= attackRange)
                    {
                        Vector2 directionToEnemy = GetDirection(Position + Center, e.Position + e.Center);
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
            light.Position = GroundPosition;
            Camera.Focus = GroundPosition;
            float bottomHitBoxWidth = Spritesheet.SourceRectangle.Width * Scale / 5;
            bottomHitBox = new FloatRectangle(new Vector2(Position.X + ((float)Spritesheet.SourceRectangle.Width * Scale / 2) - ((float)bottomHitBoxWidth / 2), 
                Position.Y + (int)(Spritesheet.SourceRectangle.Height * 0.90 * Scale) - 60), new Vector2(bottomHitBoxWidth, (Spritesheet.SourceRectangle.Height * Scale) / 25));
            
            //Position = new Vector2(this.PositionOfFeet.X - (spriteSheet.SourceRectangle.Width * Scale) / 2,
            //    this.PositionOfFeet.Y - (spriteSheet.SourceRectangle.Height * Scale));
        } 

        public void Move(Vector2 direction) //Förflyttar spelaren med en en riktningsvektor, hastighet och deltatid
        {
            Position += direction * speed * Time.DeltaTime;

            if (direction.X > 0)
            {
                Spritesheet.SetFrameCount(new Point(5, 3));
                if (Spritesheet.StartFrame != 10)
                {
                    Spritesheet.Interval = 100;
                    Spritesheet.StartFrame = 10;
                    Spritesheet.CurrentFrame = 10;
                }
            }
            else if (direction.X < 0)
            {
                Spritesheet.SetFrameCount(new Point(5, 2));
                if (Spritesheet.StartFrame != 5)
                {
                    Spritesheet.Interval = 100;
                    Spritesheet.StartFrame = 5;
                    Spritesheet.CurrentFrame = 5;
                }
            }
        }

        public bool CheckForCollision() //Kollision med väggtile-check
        {
            int stoppingDistance = Map.TileSize.Y / 16;
            Vector2 estimatedHitboxPos = (GroundPosition + (direction * stoppingDistance)).ToCartesian();
            FloatRectangle hitbox = new FloatRectangle(estimatedHitboxPos - new Vector2(bottomHitBox.Size.X / 2, 0), bottomHitBox.Size);
            for (int x = 0; x < Map.TileMap.GetLength(0); x++)
            {
                for (int y = 0; y < Map.TileMap.GetLength(1); y++)
                {
                    Vector2 tilePos = Map.GetTilePosition(new Vector2(x, y)).ToCartesian();

                    if (hitbox.Intersects(new FloatRectangle(tilePos, new Vector2(80, 80))) && Map.TileMap[x, y].TileType == TileType.Collision)
                    {
                        return true;
                    }
                }
            }
            return false;
        } 

        public void PathMove() //Rörelse via Pathfinding
        {
            nextPosition = new Vector2(nextTileInPath.X * Map.TileSize.Y, nextTileInPath.Y * Map.TileSize.Y).ToIsometric();
            nextPosition.X += Map.TileSize.X / 2;
            nextPosition.Y += Map.TileSize.Y / 2;
            direction.X = nextPosition.X - GroundPosition.X;
            direction.Y = nextPosition.Y - GroundPosition.Y;
            direction.Normalize();
            Position += direction * speed * Time.DeltaTime;
        }

        public Vector2 GetDirection(Vector2 currentPos, Vector2 targetPos) //Ger en normaliserad riktning mellan två positioner
        {
            Vector2 targetDirection = targetPos - currentPos;

            targetDirection.Normalize();

            return targetDirection;
        }

        public void Animation()
        {
            //if (!attacking && Vector2.Distance(PositionOfFeet, targetPos) < 20)
            {
                //spriteSheet.SetCurrentFrame(5);
                //spriteSheet.SetFrameCount(new Point(5, 2));
                //SourceRectangle = spriteSheet.SourceRectangle;
                Spritesheet.Interval = 100;
                Spritesheet.SetFrameCount(new Point(5, 1));

                if (Spritesheet.StartFrame != 0)
                {
                    Spritesheet.StartFrame = 0;
                    Spritesheet.CurrentFrame = 0;
                }

                //SourceRectangle = AnimationHelper.Animation(SourceRectangle, ref frameTimer, frameInterval, ref frame, animationStarts, amountOfFrames, animationOffset);
            }
        }

        public override void Draw(SpriteBatch sb)
        {
            //Debug
            //FullHitbox
            //sb.Draw(ResourceManager.Get<Texture2D>("rectangle"), Hitbox/*new Rectangle((int)Hitbox.Position.X, (int)Hitbox.Position.Y, (int)Hitbox.Size.X, (int)Hitbox.Size.Y)*/, Color.Red * 0.5f);
            
            //////BottomHitox 
            sb.Draw(ResourceManager.Get<Texture2D>("rectangle"), new Rectangle((int)GroundPosition.X - (int)bottomHitBox.Size.X, (int)GroundPosition.Y, (int)bottomHitBox.Size.X, (int)bottomHitBox.Size.Y), Color.Red * 0.5f);

            //FloatRectangle f = new FloatRectangle(PositionOfFeet - new Vector2(5, 5), new Vector2(10, 10));

            //////CenterMass 
            //sb.Draw(ResourceManager.Get<Texture2D>("rectangle"), new Rectangle((int)CenterMass.X, (int)CenterMass.Y, (int)bottomHitBox.Size.X, (int)bottomHitBox.Size.Y), Color.Black * 0.9f);

            //foreach (Creature c in CreatureManager.Instance.Creatures) invulnerability
            //{
            //    //Enemy CenterMass 
            //    sb.Draw(ResourceManager.Get<Texture2D>("rectangle"), new Rectangle((int)e.CenterMass.X, (int)e.CenterMass.Y, (int)bottomHitBox.Size.X, (int)bottomHitBox.Size.Y), Color.Black * 0.9f);
            //}

            //sb.Draw(ResourceManager.Get<Texture2D>(Texture), Position + Offset, spriteSheet.SourceRectangle, Color * Transparency, Rotation, Vector2.Zero, Scale, SpriteEffects.None, LayerDepth);

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
