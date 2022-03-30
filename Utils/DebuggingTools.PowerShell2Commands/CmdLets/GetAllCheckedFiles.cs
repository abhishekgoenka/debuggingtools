using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Management.Automation;
using Microsoft.TeamFoundation.Client;
using Microsoft.TeamFoundation.VersionControl.Client;

namespace DebuggingTools.PowerShell2Commands
{
    [Cmdlet("Get", "CheckedFiles")]
    public class GetAllCheckedFiles : Cmdlet
    {

        [Parameter(Position = 2, HelpMessage = "Enforcing days as past few days", Mandatory = true)]
        public Int32 PastDays { get; set; }

        [Parameter(Position = 1, HelpMessage = "user name followed by corporate", Mandatory = true)]
        public String User { get; set; }

        protected override void ProcessRecord()
        {
            List<String> fileList = new List<String>();

            TfsTeamProjectCollection teamProjectCollection = new TfsTeamProjectCollection(new Uri("http://pdalm-prod-app1.rd.allscripts.com:8080/tfs"), new UICredentialsProvider());
            teamProjectCollection.EnsureAuthenticated();

            var versionControl = teamProjectCollection.GetService<VersionControlServer>();

            //enforcing 3 days as "past few days":
            var deltaInDays = new TimeSpan(PastDays, 0, 0, 0);
            DateTime date = DateTime.Now - deltaInDays;

            VersionSpec versionFrom = GetDateVSpec(date);
            VersionSpec versionTo = GetDateVSpec(DateTime.Now);

            IEnumerable results = versionControl.QueryHistory("$/", VersionSpec.Latest, 0, RecursionType.Full, User, versionFrom, versionTo, int.MaxValue, true, true);
            List<Changeset> changesets = results.Cast<Changeset>().ToList();

            if (File.Exists("CheckedInFileList.txt")) File.Delete("CheckedInFileList.txt");

            if (0 < changesets.Count)
            {
                foreach (Changeset changeset in changesets)
                {
                    Change[] changes = changeset.Changes;
                    WriteObject("Files contained in " + changeset.ChangesetId + " at " + changeset.CreationDate +
                                " with comment " + changeset.Comment);

                    foreach (Change change in changes)
                    {
                        string serverItem = change.Item.ServerItem;
                        if (!fileList.Contains(serverItem))
                        {   
                            WriteObject(serverItem + "   " + change.ChangeType);
                            fileList.Add(serverItem);
                        }
                    }
                    WriteObject("");
                }
            }

            // Create a file to write to. 
            using (StreamWriter sw = File.CreateText("CheckedInFileList.txt"))
            {
                fileList.Sort();
                fileList.ForEach(sw.WriteLine);
            }

            WriteObject("Result exported to CheckedInFileList.txt. Completed successfully...");    

        }

        private static VersionSpec GetDateVSpec(DateTime date)
        {
            string dateSpec = string.Format("D{0:yyy}-{0:MM}-{0:dd}T{0:HH}:{0:mm}", date);
            return VersionSpec.ParseSingleSpec(dateSpec, "");
        }
    }
}