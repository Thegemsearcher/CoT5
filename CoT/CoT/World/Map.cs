using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Penumbra;
using RoyT.AStar;

namespace CoT
{
    public class Map
    {
        public Dictionary<string, Tile> Tiles { get; set; }
        public Tile[,] TileMap { get; set; }
        public string[,] MapData { get; set; }
        public Point TileSize { get; set; }
        public RoyT.AStar.Grid Grid { get; private  set; }
        
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
                    SetCell(x, y);
                }
            }

            for (int x = 0; x < TileMap.GetLength(0); x++)
            {
                for (int y = 0; y < TileMap.GetLength(1); y++)
                {
                    if (TileMap[x, y].TileType == TileType.Wall)
                    {
                        Hull hull = new Hull(new Vector2[]
                        {
                            new Vector2(0, 40),
                            new Vector2(80, 0),
                            new Vector2(160, 40),
                            new Vector2(80, 80),
                        });
                        
                        hull.Position = new Vector2(x * TileSize.Y, y * TileSize.Y).ToIsometric();
                        GameManager.Instance.Penumbra.Hulls.Add(hull);
                    }
                }
            }
            return this;
        }

        public void SetCell(int x, int y)
        {
            if (TileMap[x, y].TileType == TileType.Wall)
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
        }

        public void Draw(SpriteBatch sb)
        {
            Vector2 cartesianTileWorldPos =
                new Vector2(
                    Input.CurrentMousePosition.ScreenToWorld().X / TileSize.Y,
                    Input.CurrentMousePosition.ScreenToWorld().Y / TileSize.Y);

            Point isometricScreenTile = (cartesianTileWorldPos.ToCartesian() + new Vector2(-0.5f, 0.5f)).ToPoint();

            for (int i = 0; i < TileMap.GetLength(0); i++)
            {
                for (int j = 0; j < TileMap.GetLength(1); j++)
                {
                    Tile t = TileMap[i, j];

                    if (isometricScreenTile == new Point(i, j))
                    {
                        sb.Draw(ResourceManager.Get<Texture2D>(t.Spritesheet.Texture), new Vector2(i * TileSize.Y, j * TileSize.Y).ToIsometric(), Color.Red);
                    }
                    else
                    {
                        sb.Draw(ResourceManager.Get<Texture2D>(t.Spritesheet.Texture), new Vector2(i * TileSize.Y, j * TileSize.Y).ToIsometric(), Color.White);
                    }
                    if (TileMap[i, j].TileType == TileType.Water)
                    {
                        sb.Draw(ResourceManager.Get<Texture2D>(t.Spritesheet.Texture), new Vector2(i * TileSize.Y, j * TileSize.Y).ToIsometric(), Color.Blue);
                    }
                }
            }

            #region Test pathfinding
            Position[] testPath = Grid.GetPath(new Position(0, 0), new Position(isometricScreenTile.X, isometricScreenTile.Y), MovementPatterns.LateralOnly);

            for (int i = 0; i < testPath.Length; i++)
            {
                sb.Draw(ResourceManager.Get<Texture2D>("tile1"), new Vector2(testPath[i].X * TileSize.Y, testPath[i].Y * TileSize.Y).ToIsometric(), Color.Green * 0.5f);
            }
            #endregion
        }
    }
}