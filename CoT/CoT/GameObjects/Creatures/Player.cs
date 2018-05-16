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
        private Vector2 nextPosition, targetPos;
        private Position[] path;
        private Position nextTileInPath;
        private FloatRectangle bottomHitBox;
        //private int frame, animationOffset = 27, animationStarts = 0, amountOfFrames = 5;
        //private float frameTimer = 100, frameInterval = 100;
        private bool castingFireBall = false;
        private Projectile fireBall = null;
        enum PlayerState 
        {
            Idle,
            Attacking,
            DirectMoving,
            Pathfinding,
            Collide,
        }

        private PlayerState currentPlayerState;
        private Penumbra.Light light;

        public Player(Spritesheet spritesheet, Vector2 position, Vector2 groundPositionOffset, Vector2 depthSortingOffset, Stats stats, Map map, Grid grid, Player player) : base(spritesheet, position, groundPositionOffset, depthSortingOffset, stats, map, grid, player)
        {
            attackRange = 150;

            light = new PointLight();
            light.Scale = new Vector2(1000, 1500).ToCartesian();
            light.Intensity = 1f;
            light.ShadowType = ShadowType.Solid;
            GameManager.Instance.Penumbra.Lights.Add(light);
            Scale = 3;
            speed = 200f;
            LayerDepth = 0.2f;
            bottomHitBox = new FloatRectangle(new Vector2(Position.X, Position.Y + (int)(spritesheet.SourceRectangle.Height * 0.90 * Scale)),
                new Vector2(spritesheet.SourceRectangle.Width * Scale, (spritesheet.SourceRectangle.Height * Scale) / 10));

            spritesheet.SetFrameCount(new Point(5, 1));
            spritesheet.Interval = 100;
          
        }

        public override void Update()
        {
            if (Map.TileMap[(int)Map.GetTileIndex(GroundPosition).X, (int)Map.GetTileIndex(GroundPosition).Y].TileType == TileType.Teleport)
            {
                GameplayScreen.Instance.ChangeToNextLevel();
                Map = GameplayScreen.Instance.Map;
                Grid = Map.Grid;
                currentPlayerState = PlayerState.Idle;
            }
            base.Update();
            if (Stats.Health <= 0)
            {
                return;
            }

            if (Input.IsRightClickPressed && currentPlayerState != PlayerState.Attacking) //Vid musklick får spelaren en ny måldestination och börjar röra sig,
                                                                                          //spelaren kan inte röra sig under tiden det tar att utföra en attack. Musklickspositionen måste vara
                                                                                          //på en giltig groundtile innanför kartan
            {
                targetPos = Input.CurrentMousePosition.ScreenToWorld();
                Vector2 TargetTileIndex = Map.GetTileIndex(targetPos);

                if ((int)TargetTileIndex.X >= 0 && (int)TargetTileIndex.X <= Map.TileMap.GetLength(0) &&
                    (int)TargetTileIndex.Y >= 0 && (int)TargetTileIndex.Y <= Map.TileMap.GetLength(1))
                {
                    if (Map.TileMap[(int)TargetTileIndex.X, (int)TargetTileIndex.Y].TileType == TileType.Ground || Map.TileMap[(int)TargetTileIndex.X, (int)TargetTileIndex.Y].TileType == TileType.Teleport)
                    {
                        direction = GetDirection(GroundPosition, targetPos);
                        path = Pathing(targetPos);
                        currentPlayerState = PlayerState.DirectMoving;
                    }
                }
            }

            if (Input.IsKeyPressed(Keys.F))
            {
                if (castingFireBall)
                    castingFireBall = false;
                else
                    castingFireBall = true;
            }
            Animation();

            switch (currentPlayerState)
            {
                case PlayerState.Idle:
                    break;
                case PlayerState.DirectMoving:
                    Move(direction);
                    if (CheckForCollision())
                    {
                        currentPlayerState = PlayerState.Collide;
                    }
                    break;
                case PlayerState.Attacking:
                    AttackLockTimer();
                    break;
                case PlayerState.Pathfinding:
                    PathMoving();
                    if (path.Length <= 1)
                    {
                        direction = GetDirection(GroundPosition, targetPos);
                        currentPlayerState = PlayerState.DirectMoving;
                    }
                    break;
                case PlayerState.Collide:
                    //Move((direction * -1));
                    //if (CheckForCollision())
                    //{
                    //    break;
                    //}
                    if (path.Length < 1)
                    {
                        currentPlayerState = PlayerState.Idle;
                    }
                    else
                    {
                        currentPlayerState = PlayerState.Pathfinding;
                    }
                    break;
                default:
                    break;
            }
            UpdateFireBall();
            StopMoving();
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
                currentPlayerState = PlayerState.Idle;
            }
        }

        public void InputAttack() //Vid vänsterklick attackerar spelaren
        {
            Vector2 attackDirection;

            if (Input.IsLeftClickPressed && currentPlayerState != PlayerState.Attacking)
            {
                if (castingFireBall == true)
                {
                    FireBall();
                    castingFireBall = false;
                    return;
                }
                else
                {

                    attackDirection = GetDirection(Position + Center, Input.CurrentMousePosition.ScreenToWorld());
                    DecideEnemiesInRange(attackDirection);

                }
                currentPlayerState = PlayerState.Attacking;

                for (int i = 0; i < 20; i++)
                {
                    ParticleManager.CreateStandard(Position + Center + attackDirection * 80, attackDirection + Helper.RandomDirection(), Color.BlueViolet, 1000, 3f, 0.5f);
                }

                Camera.ScreenShake(0.1f, 10);
            }
        }

        public void AttackLockTimer() //Låser spelaren i en attack under 30 frames
        {
            int attackDuration = 20;
            attackTimer++;

            if (attackTimer >= attackDuration)
            {
                attackTimer = 0;
                currentPlayerState = PlayerState.Idle;
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
                            e.GetHit(this);
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
        } 

        public void Move(Vector2 direction) //Förflyttar spelaren med en en riktningsvektor, hastighet och deltatid
        {
            Position += direction * speed * Time.DeltaTime;
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

        public void FireBall()
        {
            if (fireBall != null)
            {
                fireBall = null;
            }
           else
            {
                Vector2 temp = Input.CurrentMousePosition.ScreenToWorld();
                Vector2 fireBallDirection = temp - (Position + Center);
                fireBallDirection.Normalize();
                fireBall = new Projectile(new Spritesheet("tile2", new Point(1, 1), new Rectangle(0, 0, 20, 20)), Position + Center + fireBallDirection * 50, fireBallDirection, 500f);
            }
        }

        public void UpdateFireBall()
        {
            if (fireBall != null)
            {
                fireBall.Update();
                foreach (Creature c in CreatureManager.Instance.Creatures)
                {
                    if (c is Enemy e)
                    {
                        if (c.Hitbox.Intersects(fireBall.Hitbox))
                        {
                            e.GetHit(this);
                            fireBall = null;
                            return;
                        }
                    }
                }
                //Fireballen ska inte gå igenom väggar
                Vector2 cartesianTileWorldPos = new Vector2(0, 0);
                cartesianTileWorldPos.X = fireBall.Position.X / Map.TileSize.Y;
                cartesianTileWorldPos.Y = fireBall.Position.Y / Map.TileSize.Y;
                Tile t;
                Point isometricScreenTile = (cartesianTileWorldPos.ToCartesian() + new Vector2(-0.5f, 0.5f)).ToPoint();
                for (int i = 0; i < Map.TileMap.GetLength(0); i++)
                {
                    for (int j = 0; j < Map.TileMap.GetLength(1); j++)
                    {
                        t = Map.TileMap[i, j];
                        if (isometricScreenTile == new Point(i, j))
                        {
                            if (t.TileType == TileType.Collision)
                            {
                                fireBall = null;
                                return;
                            }
                        }
                    }
                }
                if (Vector2.Distance(fireBall.Position, Position) > 1200)
                    fireBall = null;
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
            if (currentPlayerState != PlayerState.Idle)
            {
                switch (facingDirection)
                {
                    case FacingDirection.North:
                        Spritesheet.SetFrameCount(new Point(5, 5));
                        Spritesheet.SetCurrentFrame(20);
                        break;
                    case FacingDirection.NorthEast:
                        Spritesheet.SetFrameCount(new Point(5, 9));
                        Spritesheet.SetCurrentFrame(40);
                        break;
                    case FacingDirection.East:
                        Spritesheet.SetFrameCount(new Point(5, 3));
                        Spritesheet.SetCurrentFrame(10);
                        break;
                    case FacingDirection.SouthEast:
                        Spritesheet.SetFrameCount(new Point(5, 6));
                        Spritesheet.SetCurrentFrame(25);
                        break;
                    case FacingDirection.South:
                        Spritesheet.SetFrameCount(new Point(5, 4));
                        Spritesheet.SetCurrentFrame(15);
                        break;
                    case FacingDirection.SouthWest:
                        Spritesheet.SetFrameCount(new Point(5, 7));
                        Spritesheet.SetCurrentFrame(30);
                        break;
                    case FacingDirection.West:
                        Spritesheet.SetFrameCount(new Point(5, 2));
                        Spritesheet.SetCurrentFrame(5);
                        break;
                    case FacingDirection.NorthWest:
                        Spritesheet.SetFrameCount(new Point(5, 8));
                        Spritesheet.SetCurrentFrame(35);
                        break;
                }
            }
            else
            {
                Spritesheet.SetFrameCount(new Point(5, 1));
                Spritesheet.SetCurrentFrame(0);
            }
        }

        public override void Draw(SpriteBatch sb)
        {

            sb.Draw(ResourceManager.Get<Texture2D>("tile1"), Input.CurrentMousePosition.ToIsometric(), new Rectangle(0, 0, 100, 100)
                , Color.Red, Rotation, Vector2.Zero, Scale, SpriteEffects.None, 1f);

            //Debug
            //FullHitbox
            //sb.Draw(ResourceManager.Get<Texture2D>("rectangle"), Hitbox/*new Rectangle((int)Hitbox.Position.X, (int)Hitbox.Position.Y, (int)Hitbox.Size.X, (int)Hitbox.Size.Y)*/, Color.Red * 0.5f);

            //////BottomHitox 
            //sb.Draw(ResourceManager.Get<Texture2D>("rectangle"), new Rectangle((int)GroundPosition.X - (int)bottomHitBox.Size.X, (int)GroundPosition.Y, (int)bottomHitBox.Size.X, (int)bottomHitBox.Size.Y), Color.Red * 0.5f);

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
