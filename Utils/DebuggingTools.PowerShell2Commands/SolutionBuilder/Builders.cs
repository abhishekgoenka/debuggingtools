using System;
using System.IO;
using DebuggingTools.PowerShell2Commands.Helpers;

namespace DebuggingTools.PowerShell2Commands.SolutionBuilder
{
    internal class WorksNET : Solution
    {
        public override string Construct()
        {
            return "Compiling Works.Net Solution...";
        }

        public override string Start()
        {
            //build WORKS.NET
            WorkerProcessFacade process = new WorkerProcessFacade();
            String solutionPath = Path + @"\Web\Works.NET\Works.NET.sln /target:Clean;Build /property:Configuration=Release /property:Version=11.4.2.128";
            return process.Execute(@"C:\Windows\Microsoft.NET\Framework\v4.0.30319\msbuild.exe", solutionPath);
        }
    }

    internal class DPS : Solution
    {
        public override string Construct()
        {
            return "Compiling DPS Solution...";
        }

        public override string Start()
        {
            ClearReadonlyFlag();

            //build DPS.NET
            WorkerProcessFacade process = new WorkerProcessFacade();
            String solutionPath = Path + @"\ServerObjects\DataPort\AHSDPS.sln /target:Clean;Build /property:Configuration=Release /property:Version=11.4.2.128";
            return process.Execute(@"C:\Windows\Microsoft.NET\Framework\v4.0.30319\msbuild.exe", solutionPath);
        }

        public void ClearReadonlyFlag()
        {
            if (File.Exists(Path + @"\Shared\lib\IHE\Allscripts.Ihe.Contracts.dll"))
            {
                File.SetAttributes(Path + @"\Shared\lib\IHE\Allscripts.Ihe.Contracts.dll",
                    File.GetAttributes(Path + @"\Shared\lib\IHE\Allscripts.Ihe.Contracts.dll") &
                    ~FileAttributes.ReadOnly);
            }

            if (File.Exists(Path + @"\ServerObjects\DataPort\DataPortService\Bin\Allscripts.Ihe.Contracts.dll"))
            {
                File.SetAttributes(Path + @"\ServerObjects\DataPort\DataPortService\Bin\Allscripts.Ihe.Contracts.dll",
                    File.GetAttributes(Path +
                                       @"\ServerObjects\DataPort\DataPortService\Bin\Allscripts.Ihe.Contracts.dll") &
                    ~FileAttributes.ReadOnly);
            }

            if (File.Exists(Path + @"\ServerObjects\DataPort\DataPortService\Bin\acPDFCreator.Net.dll"))
            {
                File.SetAttributes(Path + @"\ServerObjects\DataPort\DataPortService\Bin\acPDFCreator.Net.dll",
                    File.GetAttributes(Path + @"\ServerObjects\DataPort\DataPortService\Bin\acPDFCreator.Net.dll") &
                    ~FileAttributes.ReadOnly);
            }

            if (File.Exists(Path + @"\ServerObjects\DataPort\DataPortService\Bin\acPDFCreatorLib.Net.dll"))
            {
                File.SetAttributes(Path + @"\ServerObjects\DataPort\DataPortService\Bin\acPDFCreatorLib.Net.dll",
                    File.GetAttributes(Path + @"\ServerObjects\DataPort\DataPortService\Bin\acPDFCreatorLib.Net.dll") &
                    ~FileAttributes.ReadOnly);
            }

            String[] filePath = Directory.GetFiles(Path + @"\ServerObjects\DataPort\DataPortService\Bin\", "*.pdb");
            foreach (String p in filePath)
            {
                File.SetAttributes(p, File.GetAttributes(p) & ~FileAttributes.ReadOnly);
            }
        }
    }

    internal class EEHRMessagingServices : Solution
    {
        public override string Construct()
        {
            return "Compiling EEHRMessagingServices Solution...";
        }

        public override string Start()
        {
            if (File.Exists(Path + @"\SERVEROBJECTS\EEHRMessaging\EEHRMessagingServices.sln"))
            {
                //build EEHRMessagingServices
                WorkerProcessFacade process = new WorkerProcessFacade();
                String solutionPath = Path +
                                      @"\SERVEROBJECTS\EEHRMessaging\EEHRMessagingServices.sln /target:Clean;Build /property:Configuration=Release /property:Version=11.4.2.128";
                return process.Execute(@"C:\Windows\Microsoft.NET\Framework\v4.0.30319\msbuild.exe", solutionPath);
            }
            return String.Empty;
        }
    }

    internal class MessageCenter : Solution
    {
        public override string Construct()
        {
            return "Compiling MessageCenter Solution...";
        }

        public override string Start()
        {
            if (File.Exists(Path + @"\ServerObjects\MessageCenter\MessageCenter.sln"))
            {
                //build EEHRMessagingServices
                WorkerProcessFacade process = new WorkerProcessFacade();
                String solutionPath = Path +
                                      @"\ServerObjects\MessageCenter\MessageCenter.sln /target:Clean;Build /property:Configuration=Release /property:Version=11.4.2.88 /property:OutputPath=" + Path + @"\SERVEROBJECTS\MessageCenter\Build /property:PlatformTarget=x86";
                return process.Execute(@"C:\Windows\Microsoft.NET\Framework\v4.0.30319\msbuild.exe", solutionPath);
            }
            return String.Empty;
        }
    }

    internal class Authentication : Solution
    {
        public override string Construct()
        {
            return "Compiling Authentication Solution...";
        }

        public override string Start()
        {
            if (File.Exists(Path + @"\SERVEROBJECTS\AuthenticationService\Authentication.sln"))
            {
                //build EEHRMessagingServices
                WorkerProcessFacade process = new WorkerProcessFacade();
                String solutionPath = Path +
                                      @"\SERVEROBJECTS\AuthenticationService\Authentication.sln /target:Clean;Build /property:Configuration=Release /property:Version=11.4.2.128";
                return process.Execute(@"C:\Windows\Microsoft.NET\Framework\v4.0.30319\msbuild.exe", solutionPath);
            }
            return String.Empty;
        }
    }

    internal class CSSDotNet : Solution
    {
        public override string Construct()
        {
            return "Compiling CSSDotNet Solution...";
        }

        public override string Start()
        {
            if (File.Exists(Path + @"\Web\Works.NET\CSSDotNet.sln"))
            {
                //build EEHRMessagingServices
                WorkerProcessFacade process = new WorkerProcessFacade();
                String solutionPath = Path +
                                      @"\Web\Works.NET\CSSDotNet.sln /target:Clean;Rebuild /property:Configuration=Release /property:Platform=x86 /property:Version=11.4.2.128";
                return process.Execute(@"C:\Windows\Microsoft.NET\Framework\v4.0.30319\msbuild.exe", solutionPath);
            }
            return String.Empty;
        }
    }
}