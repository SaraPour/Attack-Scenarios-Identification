using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace DFDAnalyzer
{
    public class RebecaCode
    {


        List<RebecaClass> classes;
        DFDDiagram diagram;

        public RebecaCode(DFDDiagram diagram)
        {
            classes = new List<RebecaClass>();
            this.diagram = diagram;
            foreach (var cls in diagram.ProcessList)
            {
                if (!cls.Value.TypeID.StartsWith("GE.TB"))
                    classes.Add(new RebecaClass(cls.Key, diagram));
            }
        }

        private void WriteEnvVariables(ScriptWriter sw)
        {
            List<string> seenflowss = new List<string>();

            foreach (var item in diagram.FlowList)
            {
                if (
                    !seenflowss.Contains(item.Value.SourceID + item.Value.DestinationID) &&
                    !seenflowss.Contains(item.Value.DestinationID + item.Value.SourceID)
                    )
                {
                    sw.AppendLine(String.Format(
                        "env byte networkDelay_{0}_{1} = {2}",
                        diagram.ProcessList[item.Value.SourceID].Name,
                        diagram.ProcessList[item.Value.DestinationID].Name, 
                        Constants.DEFAULT_NETWORK_DELAY.ToString())
                        );
                    seenflowss.Add(item.Value.SourceID + item.Value.DestinationID);
                    seenflowss.Add(item.Value.DestinationID + item.Value.SourceID);

                }
            }

            sw.SkipLine();
            sw.AppendLine(Constants.ENV_SECTION);
        }

        private void WriteMain(ScriptWriter sw)
        {
            sw.AppendLine("main {");
            sw.IncreaseIndent();
            foreach (var cls in classes) {
                sw.AppendLine(
                    String.Format(
                        "{0} {1}({2}):();",
                        cls.Name,
                        RebecaClass.GetVariableName(cls.Name, new List<string>()),
                        string.Join(',', cls.KnownRebecs.Values.ToList())
                        )
                );
            }
            sw.DecreaseIndent();
            sw.AppendLine("}");
        }

        public void WriteScript(ScriptWriter sw)
        {
            sw.AppendLine(Constants.HeaderComment);
            WriteEnvVariables(sw);
            sw.SkipLine();

            foreach (var cls in classes)
            {
                cls.WriteScript(sw);
                sw.AppendLine(Constants.SeparatorComment);
            }
            sw.SkipLine(2);
            WriteMain(sw);
        }

        public void ExportToFile(string path)
        {
            using (StreamWriter writer = new StreamWriter(path))
            {
                ScriptWriter sw = new ScriptWriter();
                WriteScript(sw);
                writer.Write(sw);
                writer.Flush();
                writer.Close();
            }
                
        }



    }
}
