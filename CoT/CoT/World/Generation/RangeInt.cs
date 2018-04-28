using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoT
{
    class RangeInt
    {
        public int Min { get; private set; }
        public int Max { get; private set; }

        public RangeInt(int min, int max)
        {
            Min = min;
            Max = max;
        }

        public int Interval => Math.Abs(Min - Max);
        public int Random => Game1.Random.Next(Min, Max + 1);
    }
}