using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
//using CoT.GameObjects.Creatures;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Audio;

namespace CoT
{
    public class GameplayScreen : GameScreen
    {
        public static GameplayScreen Instance { get; set; }

        public Inventory Inventory { get; set; }
        public Map Map { get; set; }
        public Player Player { get; set; }
        bool assetsHaveBeenLoaded = false;
        int level = 1;

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
            #region Resources

            ContentManager content = Game1.Game.Content;

            #region Textures
            ResourceManager.RegisterResource(content.Load<Texture2D>("isometricTile1"), "tile1"); // 160x80 textur
            ResourceManager.RegisterResource(content.Load<Texture2D>("isometricTile2"), "tile2"); // 160x80 textur
            ResourceManager.RegisterResource(content.Load<Texture2D>("player1"), "player1");
            ResourceManager.RegisterResource(content.Load<Texture2D>("treent"), "treent");
            ResourceManager.RegisterResource(content.Load<Texture2D>("tree"), "tree");
            ResourceManager.RegisterResource(content.Load<Texture2D>("stone"), "stone");
            ResourceManager.RegisterResource(content.Load<Texture2D>("stationary animation sheet"), "stationaryPCSheet");
            ResourceManager.RegisterResource(content.Load<Texture2D>("playerAnimation2"), "playerAnimation");
            ResourceManager.RegisterResource(content.Load<Texture2D>("wall"), "wall");
            ResourceManager.RegisterResource(content.Load<Texture2D>("potions"), "potionSheet");
            ResourceManager.RegisterResource(content.Load<Texture2D>("pixelarthealthbar_empty"), "healthbarframe");
            ResourceManager.RegisterResource(content.Load<Texture2D>("pixelarthealthbar_status"), "healthbarticks");
            #endregion

            #region Backgroundtracks
            ResourceManager.RegisterResource(content.Load<Song>("DiabloDungeon"), "DiabloDungeon");
            #endregion

            #region Soundeffects
            ResourceManager.RegisterResource(content.Load<SoundEffect>("fball"), "fireballMiss");
            ResourceManager.RegisterResource(content.Load<SoundEffect>("fbolt1"), "fireballHit1");
            ResourceManager.RegisterResource(content.Load<SoundEffect>("fbolt2"), "fireballHit2");
            ResourceManager.RegisterResource(content.Load<SoundEffect>("firimp1"), "fireballCast1");
            ResourceManager.RegisterResource(content.Load<SoundEffect>("firimp2"), "fireballCast2");
            ResourceManager.RegisterResource(content.Load<SoundEffect>("acids1"), "acidCast1");
            ResourceManager.RegisterResource(content.Load<SoundEffect>("acids2"), "acidCast2");
            ResourceManager.RegisterResource(content.Load<SoundEffect>("bspirit"), "acidHit1");
            ResourceManager.RegisterResource(content.Load<SoundEffect>("shielfkd"), "treentHurt1");
            ResourceManager.RegisterResource(content.Load<SoundEffect>("barrel"), "treentDeath1");
            ResourceManager.RegisterResource(content.Load<SoundEffect>("swing"), "swing1");
            ResourceManager.RegisterResource(content.Load<SoundEffect>("swing2"), "swing2");
            ResourceManager.RegisterResource(content.Load<SoundEffect>("invgrab"), "invGrab");
            ResourceManager.RegisterResource(content.Load<SoundEffect>("invpot"), "invPotion");
            ResourceManager.RegisterResource(content.Load<SoundEffect>("flippot"), "dropPotion");
            ResourceManager.RegisterResource(content.Load<SoundEffect>("caldron"), "drinkPotion");
            ResourceManager.RegisterResource(content.Load<SoundEffect>("sarca"), "invOpen");
            ResourceManager.RegisterResource(content.Load<SoundEffect>("sarcb"), "invClose");
            #endregion

            #endregion

            //Inventory = new Inventory(new Spritesheet("", new Point(1, 1), new Rectangle(1, 1, 1, 1), Vector2.Zero));
            Inventory = new Inventory(new Spritesheet("potionSheet", new Point(0, 0), new Rectangle(0, 0, 1, 1)), Vector2.Zero);

            //Map = new Map(new Point(160, 80)).Load("Map1.dat");
            //worldCreator = new WorldCreator(Map);
            //worldCreator.Generate();
            //Map.Save("Map1.dat").Load("Map1.dat");

            //Item item = new Item("test", new Vector2(100, 100), new Rectangle(0, 0, 50, 200));
            //item.Color = Color.Red;

            //WriteObject("Test.dat", item);
            //Item item = ReadObject<Item>("Test.dat");
            //Console.WriteLine($"texture: {item.Texture}, color: {item.Color}");

            //GameObject obj = new Item("test", new Vector2(100, 100), new Rectangle(0, 0, 50, 200));


            Map = new Map(new Point(160, 80));
            Map["tile1"] = new Tile(TileType.Ground, new Spritesheet("tile1", new Point(0, 0), new Rectangle(0, 0, 160, 80)));
            Map["tile2"] = new Tile(TileType.Water, new Spritesheet("tile2", new Point(0, 0), new Rectangle(0, 0, 160, 80)));
            Map["tile3"] = new Tile(TileType.Collision, new Spritesheet("tile1", new Point(0, 0), new Rectangle(0, 0, 160, 80)));
            Map["tile4"] = new Tile(TileType.Teleport, new Spritesheet("rectangle", new Point(0, 0), new Rectangle(0, 0, 160, 80)));
            SoundManager.Instance.PlaySong("DiabloDungeon");
            //Map.Create(new Point(100, 100)).Save("Map1.dat", false).Load("Map1.dat");


            MapGenerator generation = new MapGenerator();

            Map.Create(generation.MapData).Save("Map1.dat", false).Load("Map1.dat");

            Console.WriteLine(generation.PlayerStartPosition);
            Player = new Player(new Spritesheet("playerAnimation", new Point(5, 1), new Rectangle(0, 0, 100, 100)), generation.PlayerStartPosition.ToIsometric() * Map.TileSize.Y, new Vector2(0, 60), new Vector2(150, 300), new Stats(100, 5, 25), Map, Map.Grid, Player);
            CreatureManager.Instance.Creatures.Add(Player);
          


            //for (int i = 0; i < 1; i++)
            //{
            //    Vector2 randomTileIndex = new Vector2(Game1.Random.Next(0, Map.TileMap.GetLength(0)), Game1.Random.Next(0, Map.TileMap.GetLength(1)));
            //    Vector2 randomTilePos = Map.GetTilePosition(randomTileIndex);

            //    if (Map.TileMap[(int)randomTileIndex.X, (int)randomTileIndex.Y].TileType != TileType.Collision)
            //    {
            //        Treent enemy = new Treent(new Spritesheet("treent", new Point(1, 1), new Rectangle(0, 0, 1300, 1500)), randomTilePos, new Vector2(0, 0), new Vector2(650, 1500), new Stats(5, 25, 5), Map, Map.Grid, Player);
            //        CreatureManager.Instance.Creatures.Add(enemy);
            //    }
            //}
            //Imp enemyImp = new Imp("treent", new Vector2(/*Player.Position.X + 420, Player.Position.Y + 420*/), new Rectangle(0, 0, 1300, 1500), new Vector2(650, 1500), Player, Map.Grid, Map, 5 /*HP*/, 25 /*Attack*/, 5 /*Defense*/);
            //CreatureManager.Instance.Creatures.Add(enemyImp);

            for (int i = 1; i < generation.Rooms.Length; i++)
            {
                int r = Game1.Random.Next(1, 3);
                Room room = generation.Rooms[i];

                if (r != 1)
                {
                    Treent treent = new Treent(new Spritesheet("treent", new Point(1, 1), new Rectangle(0, 0, 1300, 1500)), new Vector2(room.Position.X + 1, room.Position.Y).ToIsometric() * Map.TileSize.Y, new Vector2(0, 750 * 0.1f), new Vector2(70, 140), new Stats(15 * level, 8 * level, 15 * level), Map, Map.Grid, Player);
                    CreatureManager.Instance.Creatures.Add(treent);
                }
                else
                {
                    Imp enemyImp = new Imp(new Spritesheet("treent", new Point(1, 1), new Rectangle(0, 0, 1300, 1500)), new Vector2(room.Position.X + 1, room.Position.Y).ToIsometric() * Map.TileSize.Y, new Vector2(0, 750 * 0.1f), new Vector2(70, 140), new Stats(10 * level, 3 * level, 10 * level), Map, Map.Grid, Player);
                    CreatureManager.Instance.Creatures.Add(enemyImp);
                }
           

            }
            Map.TileMap[generation.Rooms[generation.Rooms.Length - 1].Position.X, generation.Rooms[generation.Rooms.Length - 1].Position.Y] = Map.Tiles["tile4"].Clone();
            Camera.ScaleInput = 1f;
            Camera.Scale = 100f;
            Camera.ScaleSpeed = 10f;
            Camera.Position = new Vector2(0, 1000);
            base.Load();

        }

        public void ChangeToNextLevel()
        {
            CreatureManager.Instance.Creatures.Clear();

            Map = new Map(new Point(160, 80));
            Map["tile1"] = new Tile(TileType.Ground, new Spritesheet("tile1", new Point(0, 0), new Rectangle(0, 0, 160, 80)));
            Map["tile2"] = new Tile(TileType.Water, new Spritesheet("tile2", new Point(0, 0), new Rectangle(0, 0, 160, 80)));
            Map["tile3"] = new Tile(TileType.Collision, new Spritesheet("tile1", new Point(0, 0), new Rectangle(0, 0, 160, 80)));
            Map["tile4"] = new Tile(TileType.Teleport, new Spritesheet("rectangle", new Point(0, 0), new Rectangle(0, 0, 160, 80)));
            MapGenerator generation = new MapGenerator();

            Map.Create(generation.MapData).Save("Map1.dat", false).Load("Map1.dat");

            //Player = new Player(new Spritesheet("playerAnimation", new Point(5, 1), new Rectangle(0, 0, 100, 100)), generation.PlayerStartPosition.ToIsometric() * Map.TileSize.Y, new Vector2(0, 60), new Vector2(0, 0), new Stats(100, 100, 100), Map, Map.Grid, Player);
            Player.Position = generation.PlayerStartPosition.ToIsometric() * Map.TileSize.Y;
            CreatureManager.Instance.Creatures.Add(Player);
            level += 1;

            for (int i = 1; i < generation.Rooms.Length; i++)
            {
                int r = Game1.Random.Next(1, 3);
                Room room = generation.Rooms[i];

                if (r == 1)
                {
                    Treent treent = new Treent(new Spritesheet("treent", new Point(1, 1), new Rectangle(0, 0, 1300, 1500)), new Vector2(room.Position.X + 1, room.Position.Y).ToIsometric() * Map.TileSize.Y, new Vector2(0, 750 * 0.1f), new Vector2(70, 140), new Stats(15 * level, 8 * level, 15 * level), Map, Map.Grid, Player);
                    CreatureManager.Instance.Creatures.Add(treent);
                }
                else
                {
                    Imp enemyImp = new Imp(new Spritesheet("treent", new Point(1, 1), new Rectangle(0, 0, 1300, 1500)), new Vector2(room.Position.X + 1, room.Position.Y).ToIsometric() * Map.TileSize.Y, new Vector2(0, 750 * 0.1f), new Vector2(70, 140), new Stats(10 * level, 3 * level, 10 * level), Map, Map.Grid, Player);
                    CreatureManager.Instance.Creatures.Add(enemyImp);
                }
            }
            Map.TileMap[generation.Rooms[generation.Rooms.Length - 1].Position.X, generation.Rooms[generation.Rooms.Length - 1].Position.Y] = Map.Tiles["tile4"].Clone();
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
                ScreenManager.AddScreen(new PauseMenuScreen(false, false));
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
            Player.DrawHPBar(spriteBatch);
            base.DrawUserInterface(spriteBatch);
        }
    }
}