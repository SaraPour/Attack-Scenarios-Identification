using System;
using System.Collections.Generic;
using System.Text;

namespace DFDAnalyzer.Vector
{
    internal class Node
    {
        public Node(string name)
        {
            this.Name = name;
        }

        public static bool Any { get; internal set; }
        public string Name { get; set; }

        public override string ToString()
        {
            return this.Name;
        }
    }
}
