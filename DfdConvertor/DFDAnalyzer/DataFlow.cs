using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace DFDAnalyzer
{
    public class DataFlow: DFDElementBase
    {
      
        public DataFlow(XmlNode node):base(node)
        {
        }

        public string SourceID
        {
            get
            {
                return GetInfo("SourceGuid");
            }
        }

        public string DestinationID
        {
            get
            {
                return GetInfo("TargetGuid");
            }
        }

        public string PureName
        {
            get { return this.Name.Substring(0, this.Name.IndexOf("(")); }
        }

        public List<FunctionParameter> Parameters 
        {
            get
            {
                string parametersTxt = this.Name.Substring(this.Name.IndexOf("("));
                parametersTxt = parametersTxt.Substring(1, parametersTxt.IndexOf(")") -1);
                var parametersArr = parametersTxt.Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                var parametersList = new List<FunctionParameter>();
                foreach (var param in parametersArr)
                {
                    if (!string.IsNullOrWhiteSpace(param))
                        parametersList.Add(new FunctionParameter(param));
                }
                return parametersList;

            } 
        }


    }
}
