using System;
using System.IO;
using System.Management.Automation;
using System.Runtime.InteropServices;

namespace DebuggingTools.PowerShell2Commands.CmdLets
{

    [Cmdlet(VerbsCommon.Clear, "System")]
    public class SystemCleanup : Cmdlet
    {
        [DllImport("Shell32.dll", CharSet = CharSet.Unicode)]
        static extern uint SHEmptyRecycleBin(IntPtr hwnd, string pszRootPath, RecycleFlags dwFlags);

        enum RecycleFlags : uint
        {
            SHERB_NOCONFIRMATION = 0x00000001
        }
        protected override void ProcessRecord()
        {
            //Total freespace
            GetTotalFreeSpace();

            //Clear Recyclebin
            ClearRecycleBin();

            //clear temp file
            DeleteFiles(Environment.GetFolderPath(Environment.SpecialFolder.Windows) + "\\temp");
            DeleteFiles(Environment.GetFolderPath(Environment.SpecialFolder.Windows) + "\\Prefetch");

            //Total freespace
            GetTotalFreeSpace();
            
        }

        /// <summary>
        /// Empty Recycle bin
        /// </summary>
        private void ClearRecycleBin()
        {
            try
            {
                SHEmptyRecycleBin(IntPtr.Zero, null, RecycleFlags.SHERB_NOCONFIRMATION);
                Console.WriteLine("Recyclebin Cleared");
            }
            catch (Exception exception)
            {
                WriteObject(exception.Message);
            }
        }

        /// <summary>
        /// Delete file of folder
        /// </summary>
        private void DeleteFiles(String path)
        {
            if (Directory.Exists(path))
            {
                String[] files = Directory.GetFiles(path, "*.*", SearchOption.AllDirectories);
                foreach (String file in files)
                {
                    try
                    {
                        File.Delete(file);
                    }
                    // ReSharper disable once EmptyGeneralCatchClause
                    catch (Exception)
                    {
                        //ok to shallow
                    }

                }
                WriteObject(String.Format("Folder {0} deleted", path));
            }
        }


        /// <summary>
        /// Show total free space in GB
        /// </summary>
        private void GetTotalFreeSpace()
        {
            foreach (DriveInfo drive in DriveInfo.GetDrives())
            {
                if (drive.IsReady && drive.Name == "C:\\")
                {
                    WriteObject(String.Format("Total free space : {0}GB", drive.TotalFreeSpace / 1073751824));
                }
            }
        }
    }
}
