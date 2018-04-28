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
        public RangeInt RoomCountRange { get; set; } = new RangeInt(10, 20);
        public RangeInt RoomWidthRange { get; set; } = new RangeInt(10, 20);
        public RangeInt RoomHeightRange { get; set; } = new RangeInt(10, 20);
        public RangeInt CorridorLengthRange { get; set; } = new RangeInt(5, 10);

        public Room[] Rooms { get; set; }
        public Corridor[] Corridors { get; set; }
        public string[,] MapData { get; set; }

        public MapGenerator()
        {
            MapData = new string[MapWidth, MapHeight];

            for (int i = 0; i < MapWidth; i++)
            {
                for (int j = 0; j < MapHeight; j++)
                {
                    MapData[i, j] = "tile2";
                }
            }

            Rooms = new Room[RoomCountRange.Random];
            Corridors = new Corridor[Rooms.Length - 1];

            Rooms[0] = new Room().Create(RoomWidthRange, RoomHeightRange, MapWidth, MapHeight);
            Corridors[0] = new Corridor().Create(Rooms[0], CorridorLengthRange, RoomWidthRange, RoomHeightRange, MapWidth, MapHeight, true);

            for (int i = 1; i < Rooms.Length; i++)
            {
                Rooms[i] = new Room().Create(RoomWidthRange, RoomHeightRange, MapWidth, MapHeight, Corridors[i - 1]);

                if (i < Corridors.Length)
                {
                    Corridors[i] = new Corridor().Create(Rooms[i], CorridorLengthRange, RoomWidthRange, RoomHeightRange, MapWidth, MapHeight, false);
                }
            }

            for (int i = 0; i < Rooms.Length; i++)
            {
                Room room = Rooms[i];

                for (int j = 0; j < room.Width; j++)
                {
                    int xPos = room.Position.X + j;

                    for (int k = 0; k < room.Height; k++)
                    {
                        int yPos = room.Position.Y + k;
                        if (yPos >= 100)
                        {
                            yPos -= 20;
                        }
                        else if (xPos >= 100)
                        {
                            xPos -= 20;
                        }
                        MapData[xPos, yPos] = "tile1";
                    }
                }
            }

            for (int i = 0; i < Corridors.Length; i++)
            {
                Corridor corridor = Corridors[i];

                for (int j = 0; j < corridor.Length; j++)
                {
                    int xPos = corridor.Position.X;
                    int yPos = corridor.Position.Y;

                    switch (corridor.Direction)
                    {
                        case Direction.North:
                            yPos += j;
                            break;
                        case Direction.East:
                            xPos += j;
                            break;
                        case Direction.South:
                            yPos -= j;
                            break;
                        case Direction.West:
                            xPos -= j;
                            break;
                    }
                    if (yPos >= 100)
                    {
                        yPos -= 5;
                    }
                    else if (xPos >= 100)
                    {
                        xPos -= 5;
                    }
                    MapData[xPos, yPos] = "tile1";
                }
            }
        }
    }
}
