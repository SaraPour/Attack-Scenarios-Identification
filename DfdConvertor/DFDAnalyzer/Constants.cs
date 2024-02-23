using System;
using System.Collections.Generic;
using System.Text;

namespace DFDAnalyzer
{
    public class Constants
    {
        public const int INITIAL_VALUE_INT = 0;
        public const string INITIAL_VALUE_STRING = "";
        public const double INITIAL_VALUE_DOUBLE = 1.0;

        public const string HeaderComment = "";
        public const string SeparatorComment = "\n//******************************************************\n";
        public const string COMMENT_TODO = "// TODO: You may change this secion by overriding auto-generated code";

        public const byte DEFAULT_NETWORK_DELAY = 1;

        public const string ENV_SECTION =
@"env int remoteControllingPeriod = 150;
env byte remoteControlRequest = 100;
env boolean acceptRequestConditions = true;
env byte reactionDelay = 5;
env byte RunCommandsDelay = 10;";

    }
}
