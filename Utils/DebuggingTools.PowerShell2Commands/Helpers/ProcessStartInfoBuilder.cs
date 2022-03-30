using System;
using System.Diagnostics;

namespace DebuggingTools.PowerShell2Commands.Helpers
{
    public class ProcessStartInfoBuilder
    {
        private ProcessStartInfo startInfo;
        public ProcessStartInfo GetProcessStartInfo()
        {
            return startInfo;
        }

        public void CreateProcessStartInfo(String command, String args)
        {
           
            startInfo = new ProcessStartInfo(command)
            {
                //WindowStyle = ProcessWindowStyle.Hidden,
                Arguments = args,
                CreateNoWindow = true,
                UseShellExecute = false,
                RedirectStandardOutput = true,
                RedirectStandardError = true
                //WorkingDirectory = workingDirectory
            };
        } 
    }
}