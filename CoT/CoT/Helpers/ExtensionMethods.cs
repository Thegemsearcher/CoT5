using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace CoT
{
    public static class ExtensionMethods
    {
        public static Vector2 ToIsometric(this Vector2 v1)
        {
            return new Vector2(v1.X - v1.Y, (v1.X + v1.Y) / 2);
        }

        public static Vector2 ToCartesian(this Vector2 v1)
        {
            return new Vector2((2 * v1.Y + v1.X) / 2, (2 * v1.Y - v1.X) / 2);
        }

        public static Vector2 ScreenToWorld(this Vector2 v1)
        {
            return Vector2.Transform(v1, Matrix.Invert(Camera.Transform));
        }

        public static Vector2 WorldToScreen(this Vector2 v1)
        {
            return Vector2.Transform(v1, Camera.Transform);
        }

        public static T ConvertValue<T>(this object value)
        {
            return (T)Convert.ChangeType(value, typeof(T));
        }

        public static void ForEach<T>(this IEnumerable<T> result, Action<T> action)
        {
            foreach (T item in result) action(item);
        }

        public static float NextFloat(this Random random, float min, float max)
        {
            return min + ((float)random.NextDouble() * (max - min));
        }
    }
}
