using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace CoT
{
    [DataContract]
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

        public static implicit operator FloatRectangle(Rectangle rectangle)
        {
            return new FloatRectangle(new Vector2(rectangle.X, rectangle.Y), new Vector2(rectangle.Width, rectangle.Height));
        }

        public static implicit operator Rectangle(FloatRectangle floatRectangle)
        {
            return new Rectangle((int)floatRectangle.Position.X, (int)floatRectangle.Position.Y, (int)floatRectangle.Size.X, (int)floatRectangle.Size.Y);
        }
    }
}
