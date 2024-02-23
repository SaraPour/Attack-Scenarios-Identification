using System;
using System.Collections.Generic;
using System.Text;

namespace DFDAnalyzer
{
    public class FunctionParameter
    {
        public string Name;
        public string Type;
        public FunctionParameter(string name, string type)
        {
            Name = name;
            Type = type;
        }
        public FunctionParameter(string definition)
        {
            var parametersArr = definition.Split(" ".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
            this.Type = parametersArr[0];
            this.Name = parametersArr[1];
        }

        public  bool SameAs(object obj)
        {
            if ( !(obj is FunctionParameter))
                return base.Equals(obj);

            else 
            {
                
                return Name.Equals((obj as FunctionParameter).Name) && Type.Equals((obj as FunctionParameter).Type);
            }
        }

        public string InitialValue { get 
            {
                if (Type.Equals("int", StringComparison.InvariantCultureIgnoreCase))
                    return Constants.INITIAL_VALUE_INT.ToString();

                else if (Type.Equals("double", StringComparison.InvariantCultureIgnoreCase))
                    return Constants.INITIAL_VALUE_DOUBLE.ToString();

                else if (Type.Equals("int", StringComparison.InvariantCultureIgnoreCase))
                    return Constants.INITIAL_VALUE_STRING.ToString();

                return string.Empty;
            }
        }

        public string LocalName { 
            get {
                return "self" + string.Concat(Name[0].ToString().ToUpper(), Name.AsSpan(1));
            } 
        }
    }
}
