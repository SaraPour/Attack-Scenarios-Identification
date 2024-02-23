using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;

namespace DFDAnalyzer
{
    public class DFDDiagram
    {
        public XmlDocument Doc { get; set; }

        public XmlNode DrawingSurfaceNode
        {
            get
            {
                foreach (XmlNode node in Doc.FirstChild.ChildNodes)
                {
                    if (node.Name.Equals("DrawingSurfaceList"))
                        return node.FirstChild;
                };
                return null;
            }
        }

        public XmlNode BordersNode
        {
            get
            {
                foreach (XmlNode node in DrawingSurfaceNode.ChildNodes)
                {
                    if (node.Name.Equals("Borders"))
                        return node;
                };
                return null;
            }
        }

        public XmlNode LinesNode
        {
            get
            {
                foreach (XmlNode node in DrawingSurfaceNode.ChildNodes)
                {
                    if (node.Name.Equals("Lines"))
                        return node;
                };
                return null;
            }
        }

        public string Path { get; set; }

        public Dictionary<string, GeneralProcess> ProcessList { get; set; }
        public Dictionary<string, DataFlow> FlowList { get; set; }

        public DFDDiagram(string path)
        {
            Path = path;
            Doc = new XmlDocument();
            Doc.Load(this.Path);

            ProcessList = new Dictionary<string, GeneralProcess>();
            foreach (XmlNode node in BordersNode)
            {
                var item = new GeneralProcess(node);
                ProcessList.Add(item.GuID, item);
            }

            FlowList = new Dictionary<string, DataFlow>();
            foreach (XmlNode node in LinesNode)
            {
                var item = new DataFlow(node);
                FlowList.Add(item.GuID, item);
            }
        }
      

        public void ExportFlows(string path) 
        {
            int counter = 0;
            StreamWriter writer = new StreamWriter(path);

            foreach (var flowitem in FlowList) 
            {
                counter++;
                var flow =  flowitem.Value;
                var source = this.ProcessList[flow.SourceID];
                var destination = this.ProcessList[flow.DestinationID];
                writer.Write(string.Format("{0},{1},{2},{3},{4},{5}\n", counter, flow.Name.Replace(',',';'), source.Name, source.TypeID, destination.Name, destination.TypeID));

            }
            writer.Flush();
            writer.Close();
        }



    }
}
