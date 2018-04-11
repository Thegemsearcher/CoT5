using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace CoT
{
    public class WorldCreator
    {
        public Map map;
        public int converter;

        public WorldCreator(Map map)
        {
            this.map = map;
        }

        public void Generate()
        {
            map["tile1"] = new Tile(TileType.Ground, new Spritesheet("tile1", new Point(0, 0), new Rectangle(0, 0, 160, 80)));
            map["tile2"] = new Tile(TileType.Wall, new Spritesheet("tile2", new Point(0, 0), new Rectangle(0, 0, 160, 80)));
            map["tile3"] = new Tile(TileType.Water, new Spritesheet("tile1", new Point(0, 0), new Rectangle(0, 0, 160, 80)));

            converter = Game1.Random.Next(10, 100);

            map.Create(new Point(converter, converter));

            int min;
            int max;
            int wallConverter;
            int wallsX;
            int wallsY;
            if (converter < 30)
            {
                min = 45;
                max = 85;
                wallConverter = Game1.Random.Next(min, max);
                for (int i = 0; i < wallConverter; i++)
                {
                    wallsX = Game1.Random.Next(0, converter);
                    wallsY = Game1.Random.Next(0, converter);
                    map.MapData[wallsX, wallsY] = "tile2";
                }
            }
            else if (converter < 50 && converter >= 30)
            {
                min = 45;
                max = 85;
                wallConverter = Game1.Random.Next(min, max);
                for (int i = 0; i < wallConverter; i++)
                {
                    wallsX = Game1.Random.Next(0, converter);
                    wallsY = Game1.Random.Next(0, converter);
                    map.MapData[wallsX, wallsY] = "tile2";
                }
            }
            else if (converter < 70 && converter >= 50)
            {
                min = 45;
                max = 80;
                wallConverter = Game1.Random.Next(min, max);
                for (int i = 0; i < wallConverter; i++)
                {
                    wallsX = Game1.Random.Next(0, converter);
                    wallsY = Game1.Random.Next(0, converter);
                    map.MapData[wallsX, wallsY] = "tile2";
                }
            }
            else if (converter < 100 && converter >= 70)
            {
                min = 51;
                max = 85;
                wallConverter = Game1.Random.Next(min, max);
                for (int i = 0; i < wallConverter; i++)
                {
                    wallsX = Game1.Random.Next(0, converter);
                    wallsY = Game1.Random.Next(0, converter);
                    map.MapData[wallsX, wallsY] = "tile2";
                }
            }

        }
    }
}
