using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using RoyT.AStar;

namespace CoT
{
    public class Map
    {
        public Dictionary<string, Tile> Tiles { get; set; }
        public Tile[,] TileMap { get; set; }
        public string[,] MapData { get; set; }
        private Point TileSize { get; set; }
        private RoyT.AStar.Grid Grid { get; set; }

        public Map(Point tileSize)
        {
            TileSize = tileSize;

            Tiles = new Dictionary<string, Tile>();
            TileMap = new Tile[0, 0];
            MapData = new string[0, 0];
        }

        public Tile this[string index]
        {
            get { if (Tiles.ContainsKey(index)) return Tiles[index].Clone(); else return null; }
            set { if (!Tiles.ContainsKey(index)) Tiles.Add(index, value); }
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

        public Map Save(string fileName)
        {
            for (int x = 0; x < TileMap.GetLength(0); x++)
            {
                for (int y = 0; y < TileMap.GetLength(1); y++)
                {
                    MapData[x, y] = TileMap[x, y].Tag;
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

                    if (TileMap[x, y].TileType == TileType.Wall)
                    {
                        Grid.BlockCell(new Position(x, y));
                    }
                }
            }
            return this;
        }

        public void Update()
        {
        }

        public void Draw()
        {
            Vector2 cartesianTileWorldPos =
                new Vector2(Camera.ScreenToWorld(Input.CurrentMousePosition).X / TileSize.Y,
                    Camera.ScreenToWorld(Input.CurrentMousePosition).Y / TileSize.Y);

            Point isometricScreenTile = (cartesianTileWorldPos.ToScreen() + new Vector2(-0.5f, 0.5f)).ToPoint();

            for (int i = 0; i < TileMap.GetLength(0); i++)
            {
                for (int j = 0; j < TileMap.GetLength(1); j++)
                {
                    Tile t = TileMap[i, j];

                    if (isometricScreenTile == new Point(i, j))
                    {
                        Game1.Game.SpriteBatch.Draw(ResourceManager.Get<Texture2D>(t.Spritesheet.Texture), new Vector2(i * TileSize.Y, j * TileSize.Y).ToWorld(), Color.Red);
                    }
                    else
                    {
                        Game1.Game.SpriteBatch.Draw(ResourceManager.Get<Texture2D>(t.Spritesheet.Texture), new Vector2(i * TileSize.Y, j * TileSize.Y).ToWorld(), Color.White);
                    }
                }
            }

            #region Test pathfinding
            Position[] testPath = Grid.GetPath(new Position(0, 0), new Position(isometricScreenTile.X, isometricScreenTile.Y), MovementPatterns.LateralOnly);

            for (int i = 0; i < testPath.Length; i++)
            {
                Game1.Game.SpriteBatch.Draw(ResourceManager.Get<Texture2D>("tile1"), new Vector2(testPath[i].X * TileSize.Y, testPath[i].Y * TileSize.Y).ToWorld(), Color.Green * 0.5f);
            }
            #endregion
        }
    }
}