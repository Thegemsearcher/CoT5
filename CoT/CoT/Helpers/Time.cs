using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace CoT
{
    public static class Time
    {
        public static float DeltaTime { get; set; }
        public static GameTime GameTime { get; set; }

        public static void Update(GameTime gameTime)
        {
            DeltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
            GameTime = gameTime;
        }
    }
}
