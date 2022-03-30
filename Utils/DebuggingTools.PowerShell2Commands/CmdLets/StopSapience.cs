using System;
using System.Management.Automation;
using DebuggingTools.PowerShell2Commands.Helpers;

namespace DebuggingTools.PowerShell2Commands
{
    [Cmdlet("Stop", "Sapience")]
    public class StopSapience : Cmdlet
    {
        protected override void ProcessRecord()
        {
            WriteObject("Stoping sapience started...");

            try
            {
                //Stop TaskScheduler
                WorkerProcessFacade process = new WorkerProcessFacade();
                WriteObject(process.Execute("schtasks.exe", "/change /tn SapienceMaintenance /DISABLE"));

                //Stop required sapience windows service
                WindowsServiceOperations service = new WindowsServiceOperations();
                WriteObject(service.StopService("SapienceUpdaterService"));
                WriteObject(service.StopService("SapienceAgentSrv"));

                //Terminate the sapience process.
                WriteObject(process.TerminateProcess("SapienceWinConnector"));
                WriteObject(process.TerminateProcess("SapienceAgentSysTray"));

                WriteObject("Sapience stoped...");
            }
            catch (Exception e)
            {
                ErrorRecord record = new ErrorRecord(e, "5000", ErrorCategory.InvalidOperation, e.StackTrace );
                WriteError(record);
            }
            
        }
    }
}