using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Astar {
    public class Map {
        public List<Node> Nodes { get; set; } = new List<Node>();

        public Node StartNode { get; set; }

        public Node EndNode { get; set; }

        public List<Node> ShortestPath { get; set; } = new List<Node>();

        public void CreateNodes(int nodeCountX, int nodeCountY, int start, int end) {
            for (int i = 0; i < nodeCountX; i++) {
                for (int j = 0; j < nodeCountY; j++) {
                    var newNode = new Node(i, j);
                    Nodes.Add(newNode);
                }
            }
            foreach (var node in Nodes)
                node.ConnectClosestNodes(Nodes, 8);

            EndNode = Nodes[end];
            StartNode = Nodes[start];
        }
    }
}

