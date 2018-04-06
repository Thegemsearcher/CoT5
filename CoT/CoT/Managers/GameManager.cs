using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Myra.Graphics2D.UI;
using Penumbra;

namespace CoT
{
    public class GameManager : DrawableGameComponent
    {
        public static GameManager Instance { get; set; }

        public List<IManager> Managers { get; set; }
        public GameStateManager GameStateManager { get; set; }
        public ParticleManager ParticleManager { get; set; }
        public SoundManager SoundManager { get; set; }
        public ItemManager ItemManager { get; set; }
        public CreatureManager CreatureManager { get; set; }
        public ProjectileManager ProjectileManager { get; set; }
        public PenumbraComponent Penumbra { get; set; }

        private SpriteBatch SpriteBatch { get; set; }

        public GameManager(Game game) : base(game)
        {
            Instance = this;
        }

        public override void Initialize()
        {
            Penumbra = new PenumbraComponent(Game1.Game)
            {
                AmbientColor = new Color(100, 100, 100, 255)
            };
            Game1.Game.Services.AddService(Penumbra);

            GameStateManager = new GameStateManager();
            ParticleManager = new ParticleManager();
            SoundManager = new SoundManager();
            ItemManager = new ItemManager();
            CreatureManager = new CreatureManager();
            ProjectileManager = new ProjectileManager();

            Managers = new List<IManager>
            {
                GameStateManager, ParticleManager, SoundManager, ItemManager, CreatureManager, ProjectileManager
            };
            Managers.ForEach(x => x.Initialize());
            Penumbra.Initialize();
            base.Initialize();
        }

        protected override void LoadContent()
        {
            SpriteBatch = new SpriteBatch(GraphicsDevice);
            ResourceManager.LoadContent(Game1.Game.Content);
            Managers.ForEach(x => x.LoadContent());
            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            Managers.ForEach(x => x.Update());
            Camera.Update();
            Input.Update();
            Time.Update(gameTime);
            GameDebugger.Update();
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            Penumbra.BeginDraw();
            Penumbra.Transform = Camera.Transform;
            GraphicsDevice.Clear(Color.CornflowerBlue);

            SpriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointWrap, null, null, null, Camera.Transform);
            DrawToWorld();
            SpriteBatch.End();

            Penumbra.Draw(gameTime);

            SpriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, null, null, null, null, Camera.Transform);
            DrawToWorldWithoutShader();
            SpriteBatch.End();

            SpriteBatch.Begin(SpriteSortMode.Immediate, null, null, null, null, null, null);
            DrawUserInterface();
            SpriteBatch.End();

            base.Draw(gameTime);
        }

        public void DrawToWorld()
        {
            Managers.ForEach(x => x.Draw(SpriteBatch));
        }

        public void DrawToWorldWithoutShader()
        {
            GameDebugger.DrawToWorld(SpriteBatch);
        }

        public void DrawUserInterface()
        {
            Managers.ForEach(x => x.DrawUserInterface(SpriteBatch));
            GameDebugger.DrawToScreen(SpriteBatch);
        }
    }
}
