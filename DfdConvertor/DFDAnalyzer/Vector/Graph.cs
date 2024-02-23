using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace DFDAnalyzer.Vector
{
    public class Graph
    {
        private List<Node> Nodes { get; set; }
        private List<Edge> Edges { get; set; }

        private List<Vector> Vectors { get; set; }

        public Graph(string path) 
        {
            Nodes = new List<Node>();
            Edges = new List<Edge>();
            Vectors= new List<Vector>();

            Load(path);
            CreateVectors();

        }

        private void Load(string path) 
        {
            using (StreamReader reader = new StreamReader(path))
            {
                string Headerline = reader.ReadLine();

                while (!reader.EndOfStream)
                {
                    Regex CSVParser = new Regex(",(?=(?:[^\"]*\"[^\"]*\")*(?![^\"]*\"))");
                    string[] line = CSVParser.Split(reader.ReadLine());         //reader.ReadLine().Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);

                    bool repeated = bool.Parse(line[7]);
                    if (repeated)
                        continue;

                    string label = line[9].Trim();
                    string source = line[11].Trim();
                    string destination = line[13].Trim();

                    if (source.Equals(destination))
                        continue;

                    if (!Nodes.Any(node => node.Name.Equals(source)))
                        Nodes.Add(new Node(source));

                    if (!Nodes.Any(node => node.Name.Equals(destination)))
                        Nodes.Add(new Node(destination));

                    var srcNode = Nodes.FirstOrDefault(node => node.Name.Equals(source));
                    var dstNode = Nodes.FirstOrDefault(node => node.Name.Equals(destination));

                    if (!Edges.Any(edge => edge.Source == srcNode && edge.Destination == dstNode))
                        Edges.Add(new Edge(srcNode, dstNode));

                    var edge = Edges.FirstOrDefault(edge => edge.Source == srcNode && edge.Destination == dstNode);
                    edge.Lables.Add(label);

                }
            }
        }
        private void CreateVectors()
        {
            var tempVectors = new Queue<Vector>();

            Edges.ForEach(Edge => tempVectors.Enqueue(new Vector(new List<Edge> { Edge })));

            while (tempVectors.Count > 0)
            {
                var vector = tempVectors.Dequeue();


                Edges.ForEach(e =>
                {
                    var newVector = vector.tryExpand(e);
                    if (newVector != null)
                        tempVectors.Enqueue(newVector);
                });

                Vectors.Add(new Vector(vector));

            }
        }

        public void ExportVectors(string path) 
        {
            using (StreamWriter sw = new StreamWriter(path))
            {
                sw.WriteLine(Vector.HeaderLine());
                int id = 0;
                Vectors.ForEach(v => {
                    sw.Write(++id);
                    sw.Write(",");
                    sw.WriteLine(v);
                    });
            }
        }

        public void ExportPaths(string path)
        {
            using (StreamWriter sw = new StreamWriter(path))
            {
                sw.WriteLine(Vector.HeaderLine());
                int id = 0;
                Vectors.ForEach(v => {
                    var results = v.ExpandString();
                    results.ForEach(res =>
                    {
                        sw.Write(++id);
                        sw.Write(",");
                        sw.WriteLine(res);
                    });
                });

            }
        }
    }

   
}
