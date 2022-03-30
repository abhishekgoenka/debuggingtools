using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using Microsoft.TeamFoundation.Client;
using Microsoft.TeamFoundation.VersionControl.Client;

namespace DebuggingTools.PowerShell2Commands
{
    [Cmdlet("Get", "TouchWorksCode")]
    public class TFSDownload : Cmdlet
    {
        private readonly List<GetRequest> requests = new List<GetRequest>();

        [Parameter(Position = 1, HelpMessage = "Solution Directory Path", Mandatory = true)]
        public String SolutionDirectory { get; set; }

        protected override void BeginProcessing()
        {
            requests.Add(new GetRequest(new ItemSpec(SolutionDirectory + "\\ServerObjects\\DataPort", RecursionType.Full),VersionSpec.Latest));
            requests.Add(new GetRequest(new ItemSpec(SolutionDirectory + "\\ServerObjects\\EEHRMessaging", RecursionType.Full), VersionSpec.Latest));
            requests.Add(new GetRequest(new ItemSpec(SolutionDirectory + "\\ServerObjects\\MomService", RecursionType.Full), VersionSpec.Latest));
            requests.Add(new GetRequest(new ItemSpec(SolutionDirectory + "\\Shared", RecursionType.Full), VersionSpec.Latest));
            requests.Add(new GetRequest(new ItemSpec(SolutionDirectory + "\\Web\\Works.NET", RecursionType.Full), VersionSpec.Latest));
        }

        protected override void ProcessRecord()
        {
            try
            {
                string workspaceName = Workstation.Current.Name;

                TfsTeamProjectCollection tfs = new TfsTeamProjectCollection(new Uri("http://pdalm-prod-app1.rd.allscripts.com:8080/tfs"), new UICredentialsProvider());
                tfs.EnsureAuthenticated(); 
                var versionControl = tfs.GetService<VersionControlServer>();
                versionControl.Getting += versionControl_Getting;     

                Workspace[] workspaces = versionControl.QueryWorkspaces(workspaceName, versionControl.AuthorizedUser, Workstation.Current.Name);
                if (workspaces.Length == 0)
                {
                    throw new Exception("Workspace not found...");
                    //WriteError(new ErrorRecord{ ErrorDetails = new ErrorDetails{"Workspace not found...", "Create new workspace and try again..."}});
                    //WriteObject("Creating new workspace...");
                    //Workspace workspace = versionControl.CreateWorkspace(Workstation.Current.Name, versionControl.AuthenticatedUser);
                    //workspace.Map(projectPath, workingDirectory);
                }
                Workspace currentWorkspace = workspaces.FirstOrDefault(e => e.Name.StartsWith(Workstation.Current.Name));
                if (currentWorkspace != null)
                {
                    requests.ForEach(e => currentWorkspace.Get(e, GetOptions.GetAll | GetOptions.Overwrite));
                }

                WriteObject("Download successfully...");    
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

        void versionControl_Getting(object sender, GettingEventArgs e)
        {
            WriteObject(e.SourceLocalItem);
        }
    }
}