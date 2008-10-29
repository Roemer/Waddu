using System;
using System.Collections.Generic;
using System.Text;
using ICSharpCode.SharpZipLib.Zip;
using System.Diagnostics;

namespace Waddu.Classes
{
    public class ZipHelper
    {
        private ZipHelper() { }

        public static void Unzip(string zipFile, string targetFolder)
        {
            FastZip fz = new FastZip();
            fz.ExtractZip(zipFile, targetFolder, "");
        }

        public static void Open(string zipFile)
        {
            string appPath = @"C:\Program Files\7-Zip\7zFM.exe";
            string cmdArgs = zipFile;

            Process ProcessObj = new Process();
            ProcessObj.StartInfo.FileName = appPath;
            ProcessObj.StartInfo.Arguments = cmdArgs;

            ProcessObj.Start();
        }

        public static string ShowContent(string zipFile)
        {
            string appPath = @"C:\Program Files\7-Zip\7z.exe";
            string cmdArgs = "l " + zipFile;

            // Create a new process object
            Process ProcessObj = new Process();
            ProcessObj.StartInfo.FileName = appPath;
            ProcessObj.StartInfo.Arguments = cmdArgs;

            // Hide DOS Window
            ProcessObj.StartInfo.UseShellExecute = false;
            ProcessObj.StartInfo.CreateNoWindow = true;

            // This ensures that you get the output from the DOS application
            ProcessObj.StartInfo.RedirectStandardOutput = true;

            // Start the process
            ProcessObj.Start();

            // Now read the output of the DOS application
            string Result = ProcessObj.StandardOutput.ReadToEnd();

            // Wait that the process exits
            ProcessObj.WaitForExit();

            return Result;
        }
    }
}
