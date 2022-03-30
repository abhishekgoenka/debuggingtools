using System;
using System.IO;
using System.Management.Automation;

namespace DebuggingTools.PowerShell2Commands
{
    [Cmdlet("Clear", "AssemblyCache")]
    public class ClearAssemblyCache : Cmdlet
    {
        protected override void ProcessRecord()
        {
            try
            {
                String path = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
                path += "\\assembly";
                Directory.Delete(path, true);
                WriteObject("Cache Cleared...");
            }
            catch (Exception e)
            {
                ErrorRecord record = new ErrorRecord(e, "5000", ErrorCategory.InvalidOperation, e.StackTrace);
                WriteError(record);
            }
        }
    }
}