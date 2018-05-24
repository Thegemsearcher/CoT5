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
        public Point Position { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public Direction EnteringCorridor { get; set; }

        public Room()
        {

        }

        public Room Create(RangeInt widthRange, RangeInt heightRange, int mapWidth, int mapHeight)
        {
            Width = widthRange.Random;
            Height = heightRange.Random;

            Position = new Point(Convert.ToInt32(mapWidth / 2f - Width / 2f), Convert.ToInt32(mapHeight / 2f - Height / 2f));
            return this;
        }
        public Room Create(RangeInt widthRange, RangeInt heightRange, int mapWidth, int mapHeight, Corridor corridor)
        {
            EnteringCorridor = corridor.Direction;
            Width = widthRange.Random;
            Height = heightRange.Random;

            switch (corridor.Direction)
            {
                case Direction.North:
                    Height = MathHelper.Clamp(Height, 3, corridor.EndPosition.Y);
                    Position = new Point(MathHelper.Clamp(Game1.Random.Next(corridor.EndPosition.X - Width + 2, corridor.EndPosition.X - 1), 3, mapWidth - Width - 3), corridor.EndPosition.Y - Height + 1);
                    break;
                case Direction.East:
                    Width = MathHelper.Clamp(Width, 3, mapWidth - corridor.EndPosition.X);
                    Position = new Point(corridor.EndPosition.X, MathHelper.Clamp(Game1.Random.Next(corridor.EndPosition.Y - Height + 2, corridor.EndPosition.Y - 1), 3, mapHeight - Height - 3));
                    break;
                case Direction.South:

                    Height = MathHelper.Clamp(Height, 3, mapHeight - corridor.EndPosition.Y);
                    Position = new Point(MathHelper.Clamp(Game1.Random.Next(corridor.EndPosition.X - Width + 2, corridor.EndPosition.X - 1), 3, mapWidth - Width - 3), corridor.EndPosition.Y);
                    break;
                case Direction.West:

                    Width = MathHelper.Clamp(Width, 3, corridor.EndPosition.X);
                    Position = new Point(corridor.EndPosition.X - Width + 1, MathHelper.Clamp(Game1.Random.Next(corridor.EndPosition.Y - Height + 2, corridor.EndPosition.Y - 1), 3, mapHeight - Height - 3));
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            return this;
        }
    }
}
