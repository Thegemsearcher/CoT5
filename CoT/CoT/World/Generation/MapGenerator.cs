using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoT
{
    class MapGenerator
    {
        public int MapWidth { get; set; } = 100;
        public int MapHeight { get; set; } = 100;
        public RangeInt NumberOfRooms { get; set; } = new RangeInt(15, 20);
        public RangeInt RoomWidth { get; set; } = new RangeInt(3, 10);
        public RangeInt RoomHeight { get; set; } = new RangeInt(3, 10);
        public RangeInt CorridorLength { get; set; } = new RangeInt(6, 10);
        public Room[] Rooms { get; set; }
        public Corridor[] Corridors { get; set; }
        public TileType[,] Tiles { get; set; }

        public MapGenerator()
        {
            Tiles = new TileType[MapWidth, MapHeight];
            Rooms = new Room[NumberOfRooms.Random];
            Corridors = new Corridor[Rooms.Length - 1];

            Rooms[0] = new Room();
            Corridors[0] = new Corridor();

            Rooms[0].Create(RoomWidth, RoomHeight, MapWidth, MapHeight);
            Corridors[0].Create(Rooms[0], CorridorLength, RoomWidth, RoomHeight, MapWidth, MapHeight, true);
        }
    }
}
