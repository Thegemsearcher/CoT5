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
        public Point Position { get; set; }
        public int Length { get; set; }
        public Direction Direction { get; set; }

        public Point EndPosition
        {
            get
            {
                switch (Direction)
                {
                    case Direction.North:
                        return new Point(Position.X, Position.Y + Length - 1);
                    case Direction.East:
                        return new Point(Position.X + Length - 1, Position.Y);
                    case Direction.South:
                        return new Point(Position.X, Position.Y - Length + 1);
                    case Direction.West:
                        return new Point(Position.X - Length + 1, Position.Y);
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }

        public Corridor()
        {
        }

        public Corridor Create(Room room, RangeInt length, RangeInt roomWidth, RangeInt roomHeight, int mapWidth, int mapHeight, bool firstCorridor)
        {
            Direction = (Direction)Game1.Random.Next(0, 4);

            Direction oppositeDirection = (Direction)(((int)room.EnteringCorridor + 2) % 4);

            if (!firstCorridor && Direction == oppositeDirection)
            {
                int directionInt = (int)Direction;
                directionInt++;
                directionInt = directionInt % 4;
                Direction = (Direction)directionInt;
            }

            Length = length.Random;

            int maxLength = length.Max;

            switch (Direction)
            {
                case Direction.North:
                    Position = new Point(Game1.Random.Next(room.Position.X, room.Position.X + room.Width - 1), room.Position.Y + room.Height);
                    maxLength = mapHeight - Position.Y - roomHeight.Min;
                    break;
                case Direction.East:
                    Position = new Point(room.Position.X + room.Width, Game1.Random.Next(room.Position.Y, room.Position.Y + room.Height - 1));
                    maxLength = mapWidth - Position.X - roomWidth.Min;
                    break;
                case Direction.South:
                    Position = new Point(Game1.Random.Next(room.Position.X, room.Position.X + room.Width), room.Position.Y);
                    maxLength = Position.Y - roomHeight.Min;
                    break;
                case Direction.West:
                    Position = new Point(room.Position.X, Game1.Random.Next(room.Position.Y, room.Position.Y + room.Height));
                    maxLength = Position.X - roomWidth.Min;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            Length = MathHelper.Clamp(Length, 1, maxLength);

            return this;
        }
    }
}
