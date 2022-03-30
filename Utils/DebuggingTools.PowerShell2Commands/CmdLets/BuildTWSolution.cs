#region

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Management.Automation;
using DebuggingTools.PowerShell2Commands.SolutionBuilder;

#endregion

namespace DebuggingTools.PowerShell2Commands
{
    [Cmdlet("Invoke", "TWLocalBuild")]
    public class BuildTWSolution : Cmdlet
    {
        private List<Solution> solutions;

        [Parameter(Position = 1, HelpMessage = "Solution path", Mandatory = true)]
        public String Path { get; set; }

        protected override void BeginProcessing()
        {
            solutions = new List<Solution>
            {
                new Authentication(), // Initialize solutions : Authentication
                new EEHRMessagingServices(), // Initialize solutions : EEHRMessagingServices
                new DPS(), // Initialize solutions : DPS
                new MessageCenter(), // Initialize solutions : MessageCenter
                new WorksNET(), // Initialize solutions : Works.NET
                new CSSDotNet() // Initialize solutions : CSSDotNet.NET
            };
        }

        protected override void ProcessRecord()
        {
            try
            {
                Stopwatch sw = Stopwatch.StartNew();
                WriteObject("Started compiling...");
                Compiler compiler = new Compiler();

                int max = solutions.Count;
                for (int i = 0; i < max; i++)
                {
                    Solution solution1 = solutions[i];
                    solution1.Path = Path;
                    WriteObject(compiler.Construct(solution1));
                    WriteObject(solution1.Compile.Start());
                    
                    //update progress
                    Int32 percentage = ((i + 1) * 100) / max;
                    ProgressRecord progressRecord = new ProgressRecord(5000, solution1.ToString(), percentage + "%");
                    WriteProgress(progressRecord);
                }


                WriteObject("Finished in " + sw.ElapsedMilliseconds/60000 + "min(s)");
            }
            catch (Exception e)
            {
                ErrorRecord record = new ErrorRecord(e, "5000", ErrorCategory.InvalidOperation, e.StackTrace)
                {
                    ErrorDetails = new ErrorDetails(e.Message)
                };
                ThrowTerminatingError(record);
            }
        }
    }
}