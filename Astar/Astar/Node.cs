using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Astar
{
    public class Node
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public Vector2 Position { get; set; }
        public List<Edge> Connections { get; set; } = new List<Edge>();

        public double? MinCostToStart { get; set; }
        public Node NearestToStart { get; set; }
        public bool Visited { get; set; }
        public double StraightLineDistanceToEnd { get; set; }

        public Node(int countX, int countY)
        {
            Position = new Vector2(countX * 15, countY * 15);
            Id = Guid.NewGuid();
        }

        internal void ConnectClosestNodes(List<Node> nodes, int branching)
        {
            var connections = new List<Edge>();
            foreach (var node in nodes)
            {
                if (node.Id == this.Id)
                    continue;

                var dist = Math.Sqrt(Math.Pow(Position.X - node.Position.X, 2) + Math.Pow(Position.Y - node.Position.Y, 2));
                connections.Add(new Edge
                {
                    ConnectedNode = node,
                    Length = dist,
                });
            }
            connections = connections.OrderBy(x => x.Length).ToList();
            var count = 0;
            foreach (var cnn in connections)
            {
                //Connect three closes nodes that are not connected.
                if (!Connections.Any(c => c.ConnectedNode == cnn.ConnectedNode))
                    Connections.Add(cnn);
                count++;

                //Make it a two way connection if not already connected.
                if (!cnn.ConnectedNode.Connections.Any(cc => cc.ConnectedNode == this))
                {
                    var backConnection = new Edge { ConnectedNode = this, Length = cnn.Length };
                    cnn.ConnectedNode.Connections.Add(backConnection);
                }
                if (count == branching)
                    return;
            }
        }

        public double StraightLineDistanceTo(Node end)
        {
            return Math.Sqrt(Math.Pow(Position.X - end.Position.X, 2) + Math.Pow(Position.Y - end.Position.Y, 2));
        }
        public override string ToString()
        {
            return Name;
        }
    }
}
