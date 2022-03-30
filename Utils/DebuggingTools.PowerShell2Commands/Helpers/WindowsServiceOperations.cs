using System;
using System.ServiceProcess;

namespace DebuggingTools.PowerShell2Commands.Helpers
{
    public class WindowsServiceOperations
    {
        public String StopService(String serviceName)
        {
            ServiceController service = new ServiceController(serviceName);
            if (service.Status == ServiceControllerStatus.Stopped)
            {
                return "Service already stoped...";
            }
            service.Stop();
            service.WaitForStatus(ServiceControllerStatus.Stopped, TimeSpan.FromSeconds(60));

            return serviceName + " stoped...";
        }
    }
}