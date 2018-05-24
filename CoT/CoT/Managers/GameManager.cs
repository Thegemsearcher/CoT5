using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Myra.Graphics2D.UI;
using Penumbra;

namespace CoT
{
    public class GameManager
    {
        public static GameManager Instance { get; set; }

        public List<IManager> Managers { get; set; }

        public ParticleManager ParticleManager { get; set; }
        public SoundManager SoundManager { get; set; }
        public ItemManager ItemManager { get; set; }
        public CreatureManager CreatureManager { get; set; }
        public ProjectileManager ProjectileManager { get; set; }
        public ScreenManager ScreenManager { get; set; }

        public PenumbraComponent Penumbra { get; set; }

        private SpriteBatch SpriteBatch { get; set; }

        public Game Game { get; set; }

        public GameManager()
        {
            Console.WriteLine("GameManager - Contructor");

            Game = Game1.Game;
            Instance = this;
        }

        public void Initialize()
        {
            Console.WriteLine("GameManager - Initialize");

            Penumbra = new PenumbraComponent(Game1.Game)
            {
                AmbientColor = new Color(50, 50, 50, 255)
            };

            Game1.Game.Services.AddService(Penumbra);

            ParticleManager = new ParticleManager();
            SoundManager = new SoundManager();
            ItemManager = new ItemManager();
            CreatureManager = new CreatureManager();
            ProjectileManager = new ProjectileManager();
            ScreenManager = new ScreenManager();

            Managers = new List<IManager>
            {
                ParticleManager,
                SoundManager,
                ItemManager,
                CreatureManager,
                ProjectileManager,
                ScreenManager
            };

            Managers.ForEach(x => x.Initialize());
            Penumbra.Initialize();
        }

        public void LoadContent()
        {
            Console.WriteLine("GameManager - LoadContent");

            SpriteBatch = new SpriteBatch(Game.GraphicsDevice);
            ResourceManager.RegisterResource(TextureCreator.CreateCircleTexture(30), "circle");
            ResourceManager.RegisterResource(TextureCreator.CreateRectangleTexture(new Point(160, 80)), "rectangle");
            ResourceManager.RegisterResource(Game1.Game.Content.Load<Texture2D>("lightMask"), "lightMask");
            ResourceManager.RegisterResource(Game1.Game.Content.Load<SpriteFont>("font1"), "font1");
            Managers.ForEach(x => x.LoadContent());
        }
        
        public void UnloadContent()
        {
            Console.WriteLine("GameManager - UnloadContent");
        }

        public void ClearManagers()
        {
            Penumbra.Hulls.Clear();
            Penumbra.Lights.Clear();
            CreatureManager.Instance.Creatures.Clear();
            ParticleManager.Instance.Particles.Clear();
            ItemManager.Instance.Items.Clear();
            ProjectileManager.Instance.Projectiles.Clear();
        }

        public void Update(GameTime gameTime)
        {
            if (!ScreenManager.ContainsScreenType(typeof(PauseMenuScreen)))
            {
                CreatureManager.Update();
                ItemManager.Update();
                ProjectileManager.Update();
            }

            ParticleManager.Update();
            SoundManager.Update();
            ScreenManager.Update();

            Input.Update();
            Time.Update(gameTime);
            GameDebugger.Update();
        }

        public void Draw(GameTime gameTime)
        {
            Penumbra.BeginDraw();
            Penumbra.Transform = Camera.Transform;
            Game.GraphicsDevice.Clear(Color.Black);

            SpriteBatch.Begin(SpriteSortMode.FrontToBack, BlendState.AlphaBlend, SamplerState.PointWrap, null, null, null, Camera.Transform);
            DrawToWorld();
            SpriteBatch.End();

            Penumbra.Draw(gameTime);

            SpriteBatch.Begin(SpriteSortMode.FrontToBack, BlendState.AlphaBlend, null, null, null, null, Camera.Transform);
            DrawToWorldWithoutShader();
            SpriteBatch.End();

            SpriteBatch.Begin(SpriteSortMode.FrontToBack, null, SamplerState.PointWrap, null, null, null, null);
            DrawUserInterface();
            SpriteBatch.End();

            SpriteBatch.Begin(SpriteSortMode.FrontToBack, BlendState.Additive, null, null, null, null, Camera.Transform);
            DrawToWorldAdditiveBlend();
            SpriteBatch.End();
        }

        public void DrawToWorld()
        {
            Managers.ForEach(x => x.Draw(SpriteBatch));
        }

        public void DrawToWorldAdditiveBlend()
        {
            Managers.ForEach(x => x.DrawToWorldAdditiveBlend(SpriteBatch));
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
