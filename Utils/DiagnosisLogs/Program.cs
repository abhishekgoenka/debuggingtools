using System;
using System.IO;

namespace DiagnosisLogs
{
    internal class Program
    {
        private const String NET_FRAMEWORK_2 = @"C:\Windows\Microsoft.NET\Framework\v2.0.50727";
        private static readonly String FolderPath = LogFolderPath();
        private static readonly String ZipFileName = AppDomain.CurrentDomain.BaseDirectory + "TWDiagnosisLogs.zip";

        private static void Main(string[] args)
        {
            try
            {
                Console.WriteLine("This utility extracts the diagnosis logs from the system where it is executed.");

                //copy WinDBG supporting files
                CopyWinDBGRequiredFiles();

                //zip the content
                Zip();

                //delete the temp folder
                Directory.Delete(LogFolderPath(), true);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                Console.ReadKey();
            }

        }

        private static String LogFolderPath()
        {
            String folder = AppDomain.CurrentDomain.BaseDirectory + "\\Data";
            if (Directory.Exists(folder)) return folder;
            Directory.CreateDirectory(folder);
            return folder;
        }

        private static void CopyWinDBGRequiredFiles()
        {
            //copy mscordacwks.dll file
            File.Copy(NET_FRAMEWORK_2 + "\\mscordacwks.dll", FolderPath + "\\mscordacwks.dll");

            //copy mscorwks.dll file
            File.Copy(NET_FRAMEWORK_2 + "\\mscorwks.dll", FolderPath + "\\mscorwks.dll");

            //copy SOS.dll file
            File.Copy(NET_FRAMEWORK_2 + "\\SOS.dll", FolderPath + "\\SOS.dll");
        }

        private static void Zip()
        {
            DirectoryInfo di = new DirectoryInfo(FolderPath + "\\");
            if (File.Exists(ZipFileName)) File.Delete(ZipFileName);

            using (ZipStorer zip = ZipStorer.Create(ZipFileName, "TW Diagnosis Logs"))
            {
                // Compress the directory's files.
                foreach (FileInfo fi in di.GetFiles())
                {
                    zip.AddFile(ZipStorer.Compression.Store, fi.FullName, fi.Name, "");
                }

            }
        }
    }
}