using System;
using System.Diagnostics;

namespace DebuggingTools.PowerShell2Commands.Helpers
{
    public class WorkerProcessFacade
    {
        public String Execute(String command, String args)
        {
            String output;
            String error;
            var builder = new ProcessStartInfoBuilder();
            builder.CreateProcessStartInfo(command, args);
            ProcessStartInfo startInfo = builder.GetProcessStartInfo();

            // Start the child process.
            using (Process process = Start(startInfo))
            {
                output = process.StandardOutput.ReadToEnd();
                error = process.StandardError.ReadToEnd();
                process.WaitForExit();

                if (process.HasExited)
                {
                    output += "Exit Code : " + process.ExitCode;
                    if (process.ExitCode != 0)
                    {
                        throw new Exception(output);
                    }
                }
            }
            if (!String.IsNullOrEmpty(error))
            {
                throw new Exception(error);
            }

            return output;
        }


        public String TerminateProcess(String processName)
        {
            Process [] proc = Process.GetProcessesByName(processName);
            
            // ReSharper disable once ForCanBeConvertedToForeach
            for (int i = 0; i < proc.Length; i++)
            {
                proc[i].Kill();        
            }

            return "Killed process " + processName;
        }

        private Process Start(ProcessStartInfo startInfo)
        {
            return Process.Start(startInfo);
        }
    }
}