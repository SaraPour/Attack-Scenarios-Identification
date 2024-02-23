using System;
using System.Collections.Generic;
using System.Text;

namespace DFDAnalyzer.Vector
{
    internal class Edge
    {
        public Edge(Node source, Node destination)
        {
            Lables = new HashSet<string>();
            this.Source = source;
            this.Destination = destination;
        }

        public Node Source { get; set; }
        public Node Destination { get; set; }

        public HashSet<string> Lables { get; set; }

        public override string ToString()
        {
            return this.Source.Name + "->(" + this.Lables.Count.ToString() + ")" + this.Destination.Name;
        }
    }
}
