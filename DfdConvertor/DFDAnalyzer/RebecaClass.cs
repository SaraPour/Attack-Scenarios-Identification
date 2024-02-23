using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace DFDAnalyzer
{
    public class RebecaClass
    {
        public string GuID { get; private set; }
        public string Name { get; private set; }

        private DFDDiagram diagram;
        
        public Dictionary<string,string> KnownRebecs;

        public List<FunctionParameter> Statevars { get; private set; }

        public RebecaClass(string guID, DFDDiagram diagram)
        {
            GuID = guID;
            this.diagram = diagram;
            this.Name = diagram.ProcessList[guID].Name;
            InitializeKnownRebecs();
            
        }

        private void InitializeKnownRebecs()
        {
            KnownRebecs = new Dictionary<string,string>();
            Statevars = new List<FunctionParameter>();
            List<string> rebecs = new List<string>();
            foreach (var flow in diagram.FlowList)
            {
                if (flow.Value.SourceID.Equals(GuID))
                {
                    string item = diagram.ProcessList[flow.Value.DestinationID].Name;
                    if (!rebecs.Contains(item))
                    {
                        rebecs.Add(item);
                        var name = RebecaClass.GetVariableName(item, new List<string>());
                        KnownRebecs.Add(item, name);
                    }

                    
                }

            }

            foreach (var flow in diagram.FlowList)
            {
                if (flow.Value.SourceID.Equals(GuID) || flow.Value.DestinationID.Equals(GuID))
                {
                    foreach (var par in flow.Value.Parameters)
                    {
                        if (Statevars.Any(p => p.SameAs(par)))
                            continue;
                        Statevars.Add(par);
                    }
                }

            }
        }

        private void WriteNkownRebecs(ScriptWriter sw) 
        {
            if (KnownRebecs.Count > 0)
            {
                sw.OpenBlock("knownrebecs");
                KnownRebecs.ToList().ForEach(item => sw.AppendLine(String.Format("{0} {1}", item.Key, item.Value)));
                sw.CloseBlock();
            }
        }

        private void WriteStateVars(ScriptWriter sw) 
        {
            if (Statevars.Count > 0)
            {
                sw.OpenBlock("statevars");
                Statevars.ForEach(item => sw.AppendLine(String.Format("{0} {1};", item.Type, item.LocalName)));
                sw.CloseBlock();
            }
        }

        private void WriteCustructor(ScriptWriter sw) 
        {
            sw.OpenBlock(this.Name + "()");
            Statevars.ForEach(item => sw.AppendLine(String.Format("{0} = {1};", item.LocalName, item.InitialValue)));
           
            sw.SkipLine();
            sw.AppendLine(Constants.COMMENT_TODO);
            sw.CloseBlock();
        }


        private void WriteFunctions(ScriptWriter sw)
        {
            foreach (var flow in diagram.FlowList)
            {
                if (flow.Value.SourceID.Equals(GuID) )
                {
                    sw.OpenBlock("msgsrv " + CombineNames("sen", flow.Value.Name));
                    //todo set local variables

                    var srcName = diagram.ProcessList[flow.Value.SourceID].Name;
                    var desName = diagram.ProcessList[flow.Value.DestinationID].Name;
                    sw.AppendLine( String.Format(
                        "{0}.{1} after({2});",
                        KnownRebecs[desName],  
                        CombineNames("rec", flow.Value.Name), 
                        "networkDelay_"+srcName+"_"+desName
                        )
                        );
                    sw.CloseBlock();
                       
                }
                else if (flow.Value.DestinationID.Equals(GuID)) 
                {
                    sw.OpenBlock("msgsrv " + CombineNames("rec", flow.Value.Name));
                    flow.Value.Parameters.ForEach(item => sw.AppendLine(string.Format("{0} = {1};", item.LocalName, item.Name)));
                    if (diagram.FlowList.Any(f => (f.Value.SourceID == flow.Value.DestinationID) && (f.Value.Name.Equals(flow.Value.Name))))
                        sw.AppendLine("self." + CombineNames("sen", flow.Value.Name));
                    //todo set local variables
                    sw.CloseBlock();
                }

            }
        }


        public void  WriteScript(ScriptWriter sw)
        {
            sw.Append(String.Format("reactiveclass {0}(5)", diagram.ProcessList[GuID].Name));
            sw.AppendLine("{");
            sw.IncreaseIndent();
            WriteNkownRebecs(sw);
            sw.SkipLine();
            WriteStateVars(sw);
            sw.SkipLine();
            WriteCustructor(sw);
            sw.SkipLine();
            WriteFunctions(sw);
            sw.DecreaseIndent();
            sw.AppendLine("}");
        }

   

        public static string GetVariableName(string type, List<string> forbidens) 
        {
            if (string.IsNullOrEmpty(type))
                return string.Empty;

            
            if (char.IsUpper(type[0]))
                type = string.Concat(type[0].ToString().ToLower(), type.AsSpan(1));

            if (!forbidens.Contains(type))
                return type;

            int iterator = 0;
            while (iterator < forbidens.Count)
            {
                if (!forbidens.Contains(type + "_" + iterator.ToString()))
                    return type + "_" + iterator.ToString();
            }
            throw new Exception();

        }

        public static string CombineNames ( string name1 , string name2) 
        {
            if (string.IsNullOrEmpty(name2))
                return name1;

            if (char.IsLower(name2[0]))
                name2 = string.Concat(name2[0].ToString().ToUpper(), name2.AsSpan(1));

            return name1 + name2;


        }
    }
}
