using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Astar
{
    public class Edge
    {//Längden mellan 2 noder.
        public double Length { get; set; }
        public Node ConnectedNode { get; set; }
    }
}
