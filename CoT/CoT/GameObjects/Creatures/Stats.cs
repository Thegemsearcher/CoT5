using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoT
{
    public class Stats
    {
        public int Health { get; set; }
        public int Defense { get; set; }
        public int Attack { get; set; }

        public Stats(int health, int defense, int attack)
        {
            Health = health;
            Defense = defense;
            Attack = attack;
        }
    }
}
