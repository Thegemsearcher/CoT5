using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CoT
{
    public class GameStateManager : IManager
    {
        public static GameStateManager Instance { get; set; }

        public Map Map { get; set; }

        public void Initialize()
        {
            Instance = this;
            Map = new Map(new Point(160, 80));
        }

        public void LoadContent()
        {
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
        }

        public void Update()
        {
            Map.Update();
        }

        public void Draw(SpriteBatch sb)
        {
            Map.Draw(sb);
        }
    }
}
