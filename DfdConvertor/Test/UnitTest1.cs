using DFDAnalyzer;
using System.IO;
using Xunit;
using dfdquery;

namespace Test
{
    public class UnitTest1
    {

        [Fact]
        public void Test1()
        {


            DFDDiagram diagram = new DFDDiagram(Helper.PathDfd);
            var x = diagram.ProcessList;
            Assert.True(true);

        }

        [Fact]
        public void Test_ExportFlows() 
        {
            Program.Main(new string[] { "Export", Helper.PathDfd, Helper.OutputFileFlows });

        }

        [Fact]
        public void Test_ExportRebeca()
        {
            Program.Main(new string[] { "Convert", Helper.PathDfd, Helper.OutputFileFlows });
            //DFDDiagram diagram = new DFDDiagram(filePath);
            //RebecaCode code = new RebecaCode(diagram);
            //code.ExportToFile(this.RebecaFilePath);
        }


        [Fact]
        public void Test_ExportThreats()
        {
            Program.Main(new string[] { "Threat", Helper.PathAttackList, Helper.OutputFileThreats, Helper.OutputFileExpandedThreats });
        }


    }
}
