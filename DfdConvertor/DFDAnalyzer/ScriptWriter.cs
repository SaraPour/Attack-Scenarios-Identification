using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DFDAnalyzer
{
    public  class ScriptWriter
    {
        public int Indention { get; private set; }

        public string Inention_Text { get; set; } 

        public string Block_Open_Text { get; set; }

        public string Block_Close_Text { get; set; }

        public StringBuilder Builder;

        private bool Consider_Indent = true;

        public ScriptWriter( )
        {
            Indention = 0;
            Inention_Text = "\t";
            Block_Open_Text = "{";
            Block_Close_Text = "}";

            Builder = new StringBuilder();
            Consider_Indent = true;
        }

        public void IncreaseIndent () => Indention ++;

        public void DecreaseIndent()
        {
            if (Indention > 0)
                Indention--;
        }

        public void Append(string text) 
        {
            Builder.Append((Consider_Indent?String.Concat(Enumerable.Repeat(Inention_Text, Indention)):String.Empty) + text);
            Consider_Indent = false;
        }

        public void AppendLine(string text) { 
            this.Append(text + "\n");
            Consider_Indent = true;
        }

        public void SkipLine(int skipCnt =1) => Builder.Append(String.Concat(Enumerable.Repeat("\n", skipCnt)));

        public void OpenBlock(string name = "") 
        {
            this.AppendLine(name  + this.Block_Open_Text);
            IncreaseIndent();
        }

        public void CloseBlock() 
        {
            this.DecreaseIndent();
            AppendLine(Block_Close_Text);
            SkipLine();
        }

        public override string ToString()
        {
            return Builder.ToString();
        }


    }
}
