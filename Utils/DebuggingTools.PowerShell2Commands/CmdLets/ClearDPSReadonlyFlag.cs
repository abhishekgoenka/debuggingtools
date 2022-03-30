using System;
using System.Management.Automation;
using DebuggingTools.PowerShell2Commands.SolutionBuilder;

namespace DebuggingTools.PowerShell2Commands
{
    [Cmdlet("Clear", "DSPReadonlyFlag")]
    public class ClearDPSReadonlyFlag : Cmdlet
    {
        [Parameter(Position = 1, HelpMessage = "Solution path", Mandatory = true)]
        public String Path { get; set; }

        protected override void ProcessRecord()
        {
            WriteObject("Started...");
            DPS dps = new DPS {Path = Path};
            dps.ClearReadonlyFlag();
           
            WriteObject("Done...");
        }
    }
}