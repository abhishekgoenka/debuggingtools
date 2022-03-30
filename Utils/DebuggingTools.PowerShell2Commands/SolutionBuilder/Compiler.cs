using System;

namespace DebuggingTools.PowerShell2Commands.SolutionBuilder
{
    internal class Compiler
    {

        public String Construct(Solution solution)
        {
            return solution.Construct();
        }

        // Builder uses a complex series of steps
        public String Compile(Solution solution)
        {
            return solution.Start();
        }
         
    }
}