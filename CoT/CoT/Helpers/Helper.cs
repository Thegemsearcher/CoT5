using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CoT
{
    public static class Helper
    {
        public static void Serialize(string fileName, object obj)
        {
            using (var fs = new FileStream(fileName, FileMode.Create))
            {
                var formatter = new BinaryFormatter();
                formatter.Serialize(fs, obj);
            }
        }

        public static object Deserialize(string fileName)
        {
            using (var fs = new FileStream(fileName, FileMode.Open))
            {
                var formatter = new BinaryFormatter();
                return formatter.Deserialize(fs);
            }
        }

        public static Vector2 RandomDirection(float inputFromInclusive = -1, float inputToExclusive = 1.1f)
        {
            Vector2 vec = new Vector2(Game1.Random.NextFloat(inputFromInclusive, inputToExclusive), Game1.Random.NextFloat(inputFromInclusive, inputToExclusive));
            //vec = Vector2.Normalize(vec);
            return vec;
        }
    }
}
