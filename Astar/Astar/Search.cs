using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Astar {
    static public class Search {
        static public Map MapOfNodes { get; set; } = new Map();
        static public Node Start { get; set; }
        static public Node End { get; set; }
        static public double ShortestPathLength { get; set; }

        static public void SearchInit(int countX, int countY) {
            MapOfNodes.CreateNodes(countX, countY, 4, 440);
            Start = MapOfNodes.StartNode;
            End = MapOfNodes.EndNode;
            foreach (var node in MapOfNodes.Nodes)
                node.StraightLineDistanceToEnd = node.StraightLineDistanceTo(End);
        }
        static public List<Node> Pathing() {
            AstarSearch();
            var shortestPath = new List<Node>();
            shortestPath.Add(End);
            BuildShortestPath(shortestPath, End);
            shortestPath.Reverse();
            return shortestPath;
        }
        static private void AstarSearch() {
            Start.MinCostToStart = 0;
            var prioQueue = new List<Node>();
            prioQueue.Add(Start);
            do {
                prioQueue = prioQueue.OrderBy(x => x.MinCostToStart + x.StraightLineDistanceToEnd).ToList();
                var node = prioQueue.First();
                prioQueue.Remove(node);
                
                foreach (var cnn in node.Connections.OrderBy(x => x.Length)) {
                    var childNode = cnn.ConnectedNode;
                    if (childNode.Visited)
                        continue;
                    if (childNode.MinCostToStart == null) {

                        childNode.MinCostToStart = node.MinCostToStart;
                        childNode.NearestToStart = node;
                        if (!prioQueue.Contains(childNode))
                            prioQueue.Add(childNode);
                    }
                }
                node.Visited = true;
                if (node == End)
                    return;
            } while (prioQueue.Any());
        }
        static private void BuildShortestPath(List<Node> list, Node node) {
            if (node.NearestToStart == null)
                return;
            list.Add(node.NearestToStart);
            ShortestPathLength += node.Connections.Single(x => x.ConnectedNode == node.NearestToStart).Length;
            BuildShortestPath(list, node.NearestToStart);
        }
    }
}
