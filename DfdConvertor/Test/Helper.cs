using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Test
{
    internal class Helper
    {

        private static string TryGetSolutionDirectory(string currentPath = null)
        {
            var directory = new DirectoryInfo(
                currentPath ?? Directory.GetCurrentDirectory());
            while (directory != null && !directory.GetFiles("*.sln").Any())
            {
                directory = directory.Parent;
            }
            return Path.Combine(directory.FullName, "Data");
        }


        private static string InputFileDfd
        {
            get { return "RemoteControlHxDFDV13.tm7"; }
        }

        private static string NameDfd
        {
            get
            {
                int idx = Helper.InputFileDfd.LastIndexOf(".");
                return idx < 0 ? Helper.InputFileDfd : Helper.InputFileDfd.Substring(0, idx);

            }
        }

        private static string InputFileAttackList
        {
            get { return "AttackListV15.csv"; }
        }

       


        public static string PathDfd
        {
            get
            {
                return Path.Combine(
    Helper.TryGetSolutionDirectory(),
    Helper.InputFileDfd);
            }
        }
        public static string PathAttackList
        {
            get
            {
                return Path.Combine(
    Helper.TryGetSolutionDirectory(),
    Helper.InputFileAttackList);
            }
        }


        public static string OutputFileRebecca
        {
            get
            {
                return Path.Combine(
    Helper.TryGetSolutionDirectory(),
    Helper.NameDfd + ".csv");
            }
        }

        public static string OutputFileFlows
        {
            get
            {
                return Path.Combine(
    Helper.TryGetSolutionDirectory(),
    Helper.NameDfd + ".rebeca");
            }
        }

        public static string OutputFileThreats
        {
            get
            {
                return Path.Combine(
    Helper.TryGetSolutionDirectory(),
    "Vectors.csv");
            }
        }

        public static string OutputFileExpandedThreats
        {
            get
            {
                return Path.Combine(
    Helper.TryGetSolutionDirectory(),
    "ExpandedVectors.csv");
            }
        }
    }
}
