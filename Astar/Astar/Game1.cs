using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace Astar
{

    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        List<Node> path = new List<Node>();
        Texture2D ball;
        Texture2D redBall;
        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {

            base.Initialize();
        }


        protected override void LoadContent()
        {
            IsMouseVisible = true;
            spriteBatch = new SpriteBatch(GraphicsDevice);
            Search.SearchInit(30, 30);
            //Siffrorna bestämmer storlek i X och Y led för nätet av noder //DS
            path = Search.Pathing();
            ball = Content.Load<Texture2D>("BlackBall");
            redBall = Content.Load<Texture2D>("RedBall");
        }

        protected override void UnloadContent()
        {

        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            spriteBatch.Begin();
            foreach (Node n in Search.MapOfNodes.Nodes)
            {
                spriteBatch.Draw(ball, new Rectangle((int)n.Position.X, (int)n.Position.Y, 13, 13), Color.White);
            }
            foreach (Node n in path)
            {
                spriteBatch.Draw(redBall, new Rectangle((int)n.Position.X, (int)n.Position.Y, 13, 13), Color.White);
            }
            spriteBatch.Draw(redBall, new Rectangle((int)Search.Start.Position.X, (int)Search.Start.Position.Y, 13, 13), Color.White);
            spriteBatch.Draw(redBall, new Rectangle((int)Search.End.Position.X, (int)Search.End.Position.Y, 13, 13), Color.White);
            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
