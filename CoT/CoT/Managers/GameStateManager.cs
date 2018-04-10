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
        Random rngRutNet;
        Random rngWalls;
        int converter;


        public void Initialize()
        {
            Instance = this;
            Map = new Map(new Point(160, 80));
        }

        public void LoadContent()
        {
            Tiles();
            //Map.Load("Map1.dat");
            MapCreation();
            //Map.MapData[1, 0] = "tile3";
            Wallcreation();
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
            Map.Save("Map1.dat").Load("Map1.dat");
        }

        public void Tiles()
        {
            Map["tile1"] = new Tile(TileType.Ground, new Spritesheet("tile1", new Point(0, 0), new Rectangle(0, 0, 160, 80)));
            Map["tile2"] = new Tile(TileType.Wall, new Spritesheet("tile2", new Point(0, 0), new Rectangle(0, 0, 160, 80)));
            Map["tile3"] = new Tile(TileType.Water, new Spritesheet("tile1", new Point(0, 0), new Rectangle(0, 0, 160, 80)));
        }

        public void MapCreation()
        {
            rngRutNet = new Random();
            int min = 10;
            int max = 100;
            converter = rngRutNet.Next(min, max);
            Map.Create(new Point(converter, converter));
        }


        public void Wallcreation()
        {
            
            rngWalls = new Random();
            Random positionForWall = new Random();
            int min;
            int max;
            int wallConverter;
            int wallsX;
            int wallsY;
            if (converter < 30)
            {
                min = 45;
                max = 85;
                wallConverter = rngWalls.Next(min, max);
                for (int i = 0; i < wallConverter; i++)
                {
                    wallsX = positionForWall.Next(0, converter);
                    wallsY = positionForWall.Next(0, converter);
                    Map.MapData[wallsX, wallsY] = "tile2";
                }
            }
            else if (converter < 50 && converter >= 30)
            {
                min = 45;
                max = 85;
                wallConverter = rngWalls.Next(min, max);
                for (int i = 0; i < wallConverter; i++)
                {
                    wallsX = positionForWall.Next(0, converter);
                    wallsY = positionForWall.Next(0, converter);
                    Map.MapData[wallsX, wallsY] = "tile2";
                }
            }
            else if (converter < 70 && converter >= 50)
            {
                min = 45;
                max = 80;
                wallConverter = rngWalls.Next(min, max);
                for (int i = 0; i < wallConverter; i++)
                {
                    wallsX = positionForWall.Next(0, converter);
                    wallsY = positionForWall.Next(0, converter);
                    Map.MapData[wallsX, wallsY] = "tile2";
                }
            }
            else if (converter < 100 && converter >= 70)
            {
                min = 51;
                max = 85;
                wallConverter = rngWalls.Next(min, max);
                for (int i = 0; i < wallConverter; i++)
                {
                    wallsX = positionForWall.Next(0, converter);
                    wallsY = positionForWall.Next(0, converter);
                    Map.MapData[wallsX, wallsY] = "tile2";
                }
            }



        }

        public void Update()
        {
            Map.Update();
        }

        public void Draw(SpriteBatch sb)
        {
            Map.Draw(sb);
        }

        public void DrawUserInterface(SpriteBatch spriteBatch)
        {
        }
    }
}
