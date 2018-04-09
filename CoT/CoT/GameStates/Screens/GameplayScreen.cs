using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace CoT
{
    public class GameplayScreen : GameScreen
    {
        public static GameplayScreen Instance { get; set; }

        public Inventory Inventory { get; set; }
        public Map Map { get; set; }
        public Player Player { get; set; }

        public GameplayScreen()
        {
            FadeOutTransitionOn = true;
            Instance = this;
            TransitionOnTime = TimeSpan.FromSeconds(0.5f);
        }

        public override void Load()
        {
            ContentManager content = Game1.Game.Content;
            ResourceManager.RegisterResource(content.Load<Texture2D>("isometricTile1"), "tile1"); // 160x80 textur
            ResourceManager.RegisterResource(content.Load<Texture2D>("isometricTile2"), "tile2"); // 160x80 textur
            ResourceManager.RegisterResource<Texture2D>(content.Load<Texture2D>("player1"), "player1");
            ResourceManager.RegisterResource<Texture2D>(content.Load<Texture2D>("treent"), "treent");

            Inventory = new Inventory();
            Map = new Map(new Point(160, 80));
            Map["tile1"] = new Tile(TileType.Ground, new Spritesheet("tile1", new Point(0, 0), new Rectangle(0, 0, 160, 80)));
            Map["tile2"] = new Tile(TileType.Wall, new Spritesheet("tile2", new Point(0, 0), new Rectangle(0, 0, 160, 80)));
            Map["tile3"] = new Tile(TileType.Water, new Spritesheet("tile1", new Point(0, 0), new Rectangle(0, 0, 160, 80)));
            //Map.Load("Map1.dat");

            Map.Create(new Point(10, 20));
            Map.MapData[1, 0] = "tile3";
            Map.MapData[3, 3] = "tile2";
            Map.MapData[3, 4] = "tile2";
            Map.MapData[3, 5] = "tile2";
            Map.MapData[3, 6] = "tile2";
            Map.MapData[4, 6] = "tile2";
            Map.MapData[4, 6] = "tile2";
            Map.MapData[6, 6] = "tile2";
            Map.MapData[7, 6] = "tile2";
            Map.MapData[7, 6] = "tile2";
            Map.MapData[7, 7] = "tile2";
            Map.MapData[7, 8] = "tile2";
            Map.MapData[7, 9] = "tile2";
            Map.Save("Map1.dat").Load("Map1.dat");
            Player = new Player("player1", new Vector2(0, 0).ToIsometric(), new Rectangle(0, 0, ResourceManager.Get<Texture2D>("player1").Width, ResourceManager.Get<Texture2D>("player1").Height), Map.Grid, Map);
            CreatureManager.Instance.Creatures.Add(Player);

            Enemy enemy = new Enemy("treent", new Vector2(Game1.Random.Next(0, 500), Game1.Random.Next(0, 500)).ToIsometric(), new Rectangle(0, 0, 1300, 1500), Player, Map.Grid, Map);
            CreatureManager.Instance.Creatures.Add(enemy);
            base.Load();
        }

        public override void Update()
        {
            Inventory.Update();
            Camera.Update();
            Map.Update();
            base.Update();
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            Map.Draw(spriteBatch);
            base.Draw(spriteBatch);
        }

        public override void DrawUserInterface(SpriteBatch spriteBatch)
        {
            Inventory.Draw(spriteBatch);
            base.DrawUserInterface(spriteBatch);
        }
    }
}