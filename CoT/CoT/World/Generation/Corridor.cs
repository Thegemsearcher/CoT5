using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace CoT
{
    class Corridor
    {
        public Vector2 Position { get; set; }
        public int Length { get; set; }
        public Direction Direction { get; set; }

        public Corridor()
        {
            
        }

        public void Create(Room room, RangeInt length, RangeInt roomWidth, RangeInt roomHeight, int mapWidth, int mapHeight, bool firstCorridor)
        {
            Direction = (Direction)Game1.Random.Next(0, 4);

            Direction oppositeDirection = (Direction) (((int) room.EnteringCorridor + 2) % 4);

            if (!firstCorridor && Direction == oppositeDirection)
            {
                
            }
        }
    }
}
