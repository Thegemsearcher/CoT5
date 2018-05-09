using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace CoT
{
    class MapGenerator
    {
        public int MapWidth { get; set; } = 100;
        public int MapHeight { get; set; } = 100;
        public RangeInt RoomCountRange { get; set; } = new RangeInt(10, 10);
        public RangeInt RoomWidthRange { get; set; } = new RangeInt(7, 15);
        public RangeInt RoomHeightRange { get; set; } = new RangeInt(7, 15);
        public RangeInt CorridorLengthRange { get; set; } = new RangeInt(5, 10);

        public Room[] Rooms { get; set; }
        public Corridor[] Corridors { get; set; }
        public string[,] MapData { get; set; }

        public Vector2 PlayerStartPosition { get; set; }

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

            PlayerStartPosition = new Vector2(Rooms[0].Position.X + 1, Rooms[0].Position.Y);

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

                        if (yPos >= MapHeight)
                        {
                            yPos = MapHeight - 1;
                        }
                        else if (xPos >= MapWidth)
                        {
                            xPos = MapWidth - 1;
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

                    if (yPos > MapHeight - 1)
                    {
                        yPos = MapHeight - 1;
                    }
                    else if (xPos > MapWidth - 1)
                    {
                        xPos = MapWidth - 1;
                    }

                    MapData[xPos, yPos] = "tile1";
                }
            }

            for (int i = 0; i < MapData.GetLength(0); i++)
            {
                for (int j = 0; j < MapData.GetLength(1); j++)
                {
                    if (MapData[i, j] == "tile2")
                    {
                        try
                        {
                            if (MapData[i + 1, j] == "tile1" || MapData[i - 1, j] == "tile1" || MapData[i, j + 1] == "tile1" || MapData[i, j - 1] == "tile1")
                            {
                                MapData[i, j] = "tile3";
                            }
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e);
                        }
                    }
                }
            }
        }
    }
}
