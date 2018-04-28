using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using CoT.GameObjects.Creatures;
using CoT.GameStates.Screens;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace CoT
{
    public class GameplayScreen : GameScreen
    {
        public static GameplayScreen Instance { get; set; }

        public Inventory Inventory { get; set; }
        public Map Map { get; set; }
        public Player Player { get; set; }

        public WorldCreator worldCreator;

        public GameplayScreen(bool isPopup) : base(isPopup)
        {
            Instance = this;
        }

        static void WriteObject(string fileName, object obj)
        {
            FileStream writer = new FileStream(fileName, FileMode.Create);
            DataContractSerializer ser = new DataContractSerializer(obj.GetType());
            ser.WriteObject(writer, obj);
            writer.Close();
        }

        public static T ReadObject<T>(string fileName)
        {
            Console.WriteLine("Deserializing an instance of the object.");
            FileStream fs = new FileStream(fileName,
                FileMode.Open);
            XmlDictionaryReader reader =
                XmlDictionaryReader.CreateTextReader(fs, new XmlDictionaryReaderQuotas());
            DataContractSerializer ser = new DataContractSerializer(typeof(T));

            // Deserialize the data and read it from the instance.
            object deserializedObj = ser.ReadObject(reader, true);
            reader.Close();
            fs.Close();
            return (T)deserializedObj;
        }

        public override void Load()
        {
            ContentManager content = Game1.Game.Content;
            ResourceManager.RegisterResource(content.Load<Texture2D>("isometricTile1"), "tile1"); // 160x80 textur
            ResourceManager.RegisterResource(content.Load<Texture2D>("isometricTile2"), "tile2"); // 160x80 textur
            ResourceManager.RegisterResource<Texture2D>(content.Load<Texture2D>("player1"), "player1");
            ResourceManager.RegisterResource<Texture2D>(content.Load<Texture2D>("treent"), "treent");
            ResourceManager.RegisterResource<Texture2D>(content.Load<Texture2D>("tree"), "tree");
            ResourceManager.RegisterResource<Texture2D>(content.Load<Texture2D>("stone"), "stone");
            ResourceManager.RegisterResource<Texture2D>(content.Load<Texture2D>("stationary animation sheet"), "stationaryPCSheet");

            ResourceManager.RegisterResource<Texture2D>(content.Load<Texture2D>("wall"), "wall");
            Inventory = new Inventory(null, Vector2.Zero, new Rectangle(1,1,1,1));

            Map = new Map(new Point(160, 80));
            worldCreator = new WorldCreator(Map);
            worldCreator.Generate();
            Map.Save("Map1.dat").Load("Map1.dat");

            //Item item = new Item("test", new Vector2(100, 100), new Rectangle(0, 0, 50, 200));
            //item.Color = Color.Red;

            //WriteObject("Test.dat", item);
            //Item item = ReadObject<Item>("Test.dat");
            //Console.WriteLine($"texture: {item.Texture}, color: {item.Color}");

            //GameObject obj = new Item("test", new Vector2(100, 100), new Rectangle(0, 0, 50, 200));

            //Helper.Serialize("Test.dat", obj);

            //Map = new Map(new Point(160, 80));
            //Map["tile1"] = new Tile(TileType.Ground, new Spritesheet("tile1", new Point(0, 0), new Rectangle(0, 0, 160, 80)));
            //Map["tile2"] = new Tile(TileType.Wall, new Spritesheet("tile2", new Point(0, 0), new Rectangle(0, 0, 160, 80)));
            //Map["tile3"] = new Tile(TileType.Water, new Spritesheet("tile1", new Point(0, 0), new Rectangle(0, 0, 160, 80)));
            ////Map.Load("Map1.dat");

            //Map.Create(new Point(10, 20));
            //Map.MapData[1, 0] = "tile3";
            //Map.MapData[3, 3] = "tile2";
            //Map.MapData[3, 4] = "tile2";
            //Map.MapData[3, 5] = "tile2";
            //Map.MapData[3, 6] = "tile2";
            //Map.MapData[4, 6] = "tile2";
            //Map.MapData[4, 6] = "tile2";
            //Map.MapData[6, 6] = "tile2";
            //Map.MapData[7, 6] = "tile2";
            //Map.MapData[7, 6] = "tile2";
            //Map.MapData[7, 7] = "tile2";
            //Map.MapData[7, 8] = "tile2";
            //Map.MapData[7, 9] = "tile2";
            //Map.Save("Map1.dat").Load("Map1.dat");
            Player = new Player("stationaryPCSheet", new Vector2(10, 10).ToIsometric(), new Rectangle(0, 0, 26,53), 
                new Vector2(30, 130), Map.Grid, Map, 200/*HP*/, 25/*Attack*/, 5/*Defense*/);
            CreatureManager.Instance.Creatures.Add(Player);

            
            for (int i = 0; i < 1; i++)
            {
                Vector2 randomTileIndex = new Vector2(Game1.Random.Next(0, Map.TileMap.GetLength(0)), Game1.Random.Next(0, Map.TileMap.GetLength(1)));
                Vector2 randomTilePos = Map.GetTilePosition(randomTileIndex);

                if (Map.TileMap[(int)randomTileIndex.X, (int)randomTileIndex.Y].TileType != TileType.Collision)
                {
                    Treent enemy = new Treent("treent", randomTilePos, new Rectangle(0, 0, 1300, 1500), new Vector2(650, 1500),  Player, Map.Grid, Map, 5 /*HP*/, 25 /*Attack*/, 5 /*Defense*/);
                    CreatureManager.Instance.Creatures.Add(enemy);
                }
            }
            Imp enemyImp = new Imp("treent", new Vector2(420, 420), new Rectangle(0, 0, 1300, 1500), new Vector2(650, 1500), Player, Map.Grid, Map, 5 /*HP*/, 25 /*Attack*/, 5 /*Defense*/);
            CreatureManager.Instance.Creatures.Add(enemyImp);

            Camera.ScaleInput = 1f;
            Camera.Scale = 100f;
            Camera.ScaleSpeed = 10f;
            Camera.Position = new Vector2(0, 1000);
            base.Load();
        }

        public override void Unload()
        {
            GameManager.Instance.ClearManagers();
            base.Unload();
        }

        public override void Update()
        {
            Inventory.Update();
            Camera.Update();
            Map.Update();
            if (Input.IsKeyPressed(Keys.Escape))
            {
                ScreenManager.AddScreen(new PauseMenuScreen(false));
            }

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