using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DFDAnalyzer.Vector
{
    internal class Vector
    {
        public Vector(List<Edge> edges)
        {
            Edges = edges;
        }

        public Vector(Vector vector)
        {
            this.Edges = new List<Edge>(vector.Edges);
        }

        List<Edge> Edges { get; set; }

        public Vector tryExpand(Edge edge)
        {
            if (Edges.Last().Destination != edge.Source)
                return null;

            if (Edges.Any(e => e.Source == edge.Destination))
                return null;

            var NewVector = new Vector(this);
            NewVector.Edges.Add(edge);
            return NewVector;
        }


        public override string ToString()
        {

            StringBuilder result = new StringBuilder();
            string format =  "{0},\"{1}\",";
            Edges.ForEach(e => {
                result.Append(string.Format(format, e.Source.Name, string.Join(",", e.Lables)));
                });

            result.Append(Edges.Last().Destination.Name);

            return result.ToString();
        }

        public static string HeaderLine() 
        {
            return "ID,Node1, Threats, Node2, Threats, Node3, Threats, ..., ....";
        }

        internal List<string> ExpandString()
        {
            List<string> result = new List<string>();
            expanded(result, "", 0);
            return result;
        }

        private void expanded(List<string> result, string text, int index)
        {
            if (index == Edges.Count)
            {
                text += Edges.Last().Destination ;
                result.Add(text);
                return;
            }

            foreach (var label in Edges[index].Lables)
            {
                var textExtended = text + Edges[index].Source.Name + "," + label + ",";
                expanded(result, textExtended, index + 1);
            }

        }
    }
}
