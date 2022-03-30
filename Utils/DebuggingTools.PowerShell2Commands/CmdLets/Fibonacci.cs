using System;
using System.Management.Automation;

namespace DebuggingTools.PowerShell2Commands
{
    [Cmdlet("Get", "Fibonacci")]
    public class Fibonacci : Cmdlet
    {
        [Parameter(Position = 1, HelpMessage = "Count of fibonacci numbers", Mandatory = true)]
        public Int32 Count { get; set; }
        protected override void ProcessRecord()
        {
            for (int i = 0; i < Count; i++)
            {
                WriteObject(CalculateFibonacci(i));
            }
        }

        private static int CalculateFibonacci(int n)
        {
            int a = 0;
            int b = 1;

            // In N steps compute Fibonacci sequence iteratively.
            for (int i = 0; i < n; i++)
            {
                int temp = a;
                a = b;
                b = temp + b;
            }
            return a;
        }
    }
}