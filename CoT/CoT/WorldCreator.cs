using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoT
{
    class WorldCreator
    {
        Random tileCreation;

        
        public Map Map { get; set; }

        public WorldCreator(Random tileCreation)
        {
            this.tileCreation = tileCreation;
        }

        public void MakeTheTileGrid()
        {

        }
    }
}
