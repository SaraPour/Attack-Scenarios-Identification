using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace DFDAnalyzer
{
    public abstract class DFDElementBase
    {
        

        public XmlNode Data { get; private set; }

        protected XmlNode Properties
        {
            get
            {
                foreach (XmlNode node in Data.ChildNodes)
                {
                    if (node.Name.Equals("Properties"))
                        return node;
                };
                return null;
            }
        }

        protected string GetInfo(string property)
        {
            foreach (XmlNode node in Data.ChildNodes)
            {
                if (node.Name.Equals(property))
                    return node.InnerText;
            };
            return null;
        }

        protected string GetProperty(string property)
        {
            foreach (XmlNode node in Properties.ChildNodes)
            {
                if (node.FirstChild.InnerText.Equals(property))
                    return node.LastChild.InnerText;
            }
            return null;
        }

        public DFDElementBase(XmlNode node)
        {
            this.GuID = node.FirstChild.InnerText;
            this.Data = node.LastChild;
        }

        public string GuID { get; set; }

        public string Name
        {
            get
            {
                return this.GetProperty("Name").Trim();

            }
        }

        public string TypeID
        {
            get
            {
                return this.GetInfo("GenericTypeId");

            }
        }



       

       


    }
}
