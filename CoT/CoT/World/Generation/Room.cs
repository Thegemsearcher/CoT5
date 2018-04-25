using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace CoT
{
    class Room
    {
        public Vector2 Position { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public Direction EnteringCorridor { get; set; }

        public Room()
        {
            
        }

        public void Create(RangeInt roomWidth, RangeInt roomHeight, int mapWidth, int mapHeight)
        {
            Width = roomWidth.Random;
            Height = roomHeight.Random;

            Position = new Vector2(mapWidth / 2f - Width / 2f, mapHeight / 2f - Height / 2f);
        }
        public void Create(RangeInt roomWidth, RangeInt roomHeight, int mapWidth, int mapHeight, Corridor corridor)
        {

        }
    }
}
