using System;

namespace DebuggingTools.PowerShell2Commands.SolutionBuilder
{
    abstract class Solution
    {
        public String Path { get; set; }

        // Abstract compile methods
        public abstract String Construct();
        public abstract String Start();


        public Solution Compile
        {
            get { return this; }
        }
    }
}