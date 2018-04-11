using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace CoT
{
    public class FloatRectangle
    {
        public Vector2 Position { get; set; }
        public Vector2 Size { get; set; }

        public FloatRectangle(Vector2 position, Vector2 size)
        {
            Position = position;
            Size = size;
        }

        public bool Contains(Vector2 position) => (position.X >= Position.X && position.X < Position.X + Size.X &&
                                                     position.Y >= Position.Y && position.Y < Position.Y + Size.Y);

        public bool Intersects(FloatRectangle vr) => (vr.Position.X < (Position.X + Size.X) && (vr.Position.X + vr.Size.X) > Position.X &&
                                                      vr.Position.Y < (Position.Y + Size.Y) && (vr.Position.Y + vr.Size.Y) > Position.Y);
    }
}
