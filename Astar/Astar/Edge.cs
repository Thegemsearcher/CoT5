using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Astar {
    public class Edge {
        public double Length { get; set; }
        public Node ConnectedNode { get; set; }
    }
}
