using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CoT.GameObjects;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Penumbra;
using RoyT.AStar;

namespace CoT
{
    /// <summary>
    /// Ska lägga till kommentarer
    /// </summary>
    public class Map
    {
        public Dictionary<string, Tile> Tiles { get; set; }
        public Tile[,] TileMap { get; set; }
        public string[,] MapData { get; set; }
        public Point TileSize { get; set; }
        public RoyT.AStar.Grid Grid { get; private  set; }
 
        public List<GameObject> WorldObjects { get; set; }
 
       
        public Map(Point tileSize)
        {
            TileSize = tileSize;
 
            Tiles = new Dictionary<string, Tile>();
            TileMap = new Tile[0, 0];
            MapData = new string[0, 0];
 
            WorldObjects = new List<GameObject>();
        }
 
        public Tile this[string index]
        {
            get { if (Tiles.ContainsKey(index)) return Tiles[index].Clone(); else return null; }
            set { if (!Tiles.ContainsKey(index)) Tiles.Add(index, value); }
        }
 
        public Vector2 GetTileIndex(Vector2 position)
        {
            return (position / TileSize.Y).ToCartesian() + Game1.TileOffset;
        }
 
        public Vector2 GetTilePosition(Vector2 index)
        {
            return ((index - Game1.TileOffset) * TileSize.Y).ToIsometric();
        }
 
        public Map Create(Point size)
        {
            MapData = new string[size.X, size.Y];
 
            for (int x = 0; x < MapData.GetLength(0); x++)
            {
                for (int y = 0; y < MapData.GetLength(1); y++)
                {
                    MapData[x, y] = "tile1";
                }
            }
            return this;
        }
 
        public Map Create(string[,] mapData)
        {
            MapData = mapData;
            return this;
        }
 
        public Map Save(string fileName, bool saveTileMap)
        {
            if (saveTileMap)
            {
                for (int x = 0; x < TileMap.GetLength(0); x++)
                {
                    for (int y = 0; y < TileMap.GetLength(1); y++)
                    {
                        MapData[x, y] = TileMap[x, y].Tag;
                    }
                }
            }
            Helper.Serialize(fileName, MapData);
            return this;
        }
 
        public Map Load(string fileName)
        {
            MapData = (string[,])Helper.Deserialize(fileName);
            TileMap = new Tile[MapData.GetLength(0), MapData.GetLength(1)];
            Grid = new Grid(MapData.GetLength(0), MapData.GetLength(1), 1f);
           
            for (int x = 0; x < TileMap.GetLength(0); x++)
            {
                for (int y = 0; y < TileMap.GetLength(1); y++)
                {
                    TileMap[x, y] = this[MapData[x, y]];
                    SetCell(x, y);
 
                    //if (TileMap[x, y].TileType == TileType.Collision)
                    //{
                    //    Hull hull = new Hull(new Vector2[]
                    //    {
                    //        new Vector2(0, 40),
                    //        new Vector2(80, 0),
                    //        new Vector2(160, 40),
                    //        new Vector2(80, 80),
                    //    });
 
                    //    GameManager.Instance.Penumbra.DrawOrder = 5;
 
                    //    hull.Position = new Vector2(x * TileSize.Y, y * TileSize.Y).ToIsometric();
                    //    GameManager.Instance.Penumbra.Hulls.Add(hull);
                    //}
 
 
 
                    if (TileMap[x, y].TileType == TileType.Collision)
                    {
                        //int rnd = Game1.Random.Next(0, 3);
                        //if (rnd.Equals(0))
                        //{
                        WorldObject obj = new WorldObject(new Spritesheet("wall", new Point(1, 1), new Rectangle(0, 0, 160, 320)), GetTilePosition(new Vector2(x, y)) + new Vector2(-(float)TileSize.X / 2, -320 + TileSize.Y), new Vector2(80, 320 - TileSize.Y / 2), false);
                        WorldObjects.Add(obj);
                        //}
                        //else if (rnd.Equals(1))
                        //{
                        //    WorldObject obj = new WorldObject("tree", GetTilePosition(new Vector2(x, y)), new Rectangle(0, 0, 262, 316), new Vector2(131, 270));
                        //    //obj.Offset = new Vector2(140, 155);
                        //    obj.Offset = new Vector2( - 262 / 2, -316 + 2 * (TileSize.Y/3));
                        //    WorldObjects.Add(obj);
                        //}
                        //else
                        //{
                        //    WorldObject obj = new WorldObject("stone", GetTilePosition(new Vector2(x, y)), new Rectangle(0, 0, 80, 64), new Vector2(80 / 2, 40));
                        //    obj.Offset = new Vector2(-40, -84 + TileSize.Y);
                        //    WorldObjects.Add(obj);
                        //}
                    }
                }
            }
            return this;
        }
 
        public void SetCell(int x, int y)
        {
            if (TileMap[x, y].TileType == TileType.Collision)
            {
                Grid.BlockCell(new Position(x, y));
            }
            else if (TileMap[x, y].TileType == TileType.Water)
            {
                Grid.SetCellCost(new Position(x, y), 5);
            }
        }
        public void Update()
        {
            WorldObjects.ForEach(x => x.Update());
        }
 
        public void Draw(SpriteBatch sb)
        {
            Vector2 cartesianTileWorldPos =
                new Vector2(
                    Input.CurrentMousePosition.ScreenToWorld().X / TileSize.Y,
                    Input.CurrentMousePosition.ScreenToWorld().Y / TileSize.Y);
 
            Point isometricScreenTile = (cartesianTileWorldPos.ToCartesian() + new Vector2(-0.5f, 0.5f)).ToPoint();
 
            Rectangle visibleArea = Camera.VisibleArea;
 
            for (int i = 0; i < TileMap.GetLength(0); i++)
            {
                for (int j = 0; j < TileMap.GetLength(1); j++)
                {
                    Tile t = TileMap[i, j];
                    Vector2 tempPos = new Vector2(i * TileSize.Y, j * TileSize.Y).ToIsometric();
 
                    if (visibleArea.Contains(tempPos.ToPoint()))
                    {
                        if (t.TileType == TileType.Collision || t.TileType == TileType.Ground /*|| t.TileType == TileType.Water*/)
                        {
                            sb.Draw(ResourceManager.Get<Texture2D>(t.Spritesheet.Texture), tempPos, null, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
                        }
                        else if (t.TileType == TileType.Teleport)
                        {
                            ParticleManager.CreateStandard(tempPos + new Vector2(TileSize.X/2,TileSize.Y/4), new Vector2(Game1.Random.NextFloat(-0.5f, 0.5f), -1),  new Color(Game1.Random.Next(30, 60), Game1.Random.Next(30, 60), Game1.Random.Next(30, 100), Game1.Random.Next(150, 255)), Game1.Random.Next(30, 300), Game1.Random.NextFloat(0.1f, 3f), Game1.Random.NextFloat(0.2f, 0.8f));
                        }
                    }
 
                    //if (isometricScreenTile == new Point(i, j))
                    //{
                    //    sb.Draw(ResourceManager.Get<Texture2D>(t.Spritesheet.Texture), new Vector2(i * TileSize.Y, j * TileSize.Y).ToIsometric(), Color.Red);
                    //} else
                    //{
                    //    sb.Draw(ResourceManager.Get<Texture2D>(t.Spritesheet.Texture), new Vector2(i * TileSize.Y, j * TileSize.Y).ToIsometric(), null, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
                    //}
                    //if (TileMap[i, j].TileType == TileType.Water)
                    //{
                    //    sb.Draw(ResourceManager.Get<Texture2D>(t.Spritesheet.Texture), new Vector2(i * TileSize.Y, j * TileSize.Y).ToIsometric(), Color.Blue);
                    //}
                }
            }
 
 
            WorldObjects.ForEach(x => x.Draw(sb));
 
            if (GameDebugger.Debug)
            {
                for (int i = 0; i < WorldObjects.Count; i++)
                {
                    GameObject obj = WorldObjects[i];
                    //sb.Draw(ResourceManager.Get<Texture2D>(obj.Texture), new Rectangle((int)obj.Hitbox.Position.X, (int)obj.Hitbox.Position.Y, (int)obj.Hitbox.Size.X, (int)obj.Hitbox.Size.Y), null, Color.White, 0f, Vector2.Zero, SpriteEffects.None, 0.3f);
                }
            }
        }
    }
}