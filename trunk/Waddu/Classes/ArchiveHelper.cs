using System.Diagnostics;
using ICSharpCode.SharpZipLib.Zip;
using System.IO;

namespace Waddu.Classes
{
    public class ArchiveHelper
    {
        private ArchiveHelper() { }

        public static void Unzip(string zipFile, string targetFolder)
        {
            FastZip fz = new FastZip();
            fz.ExtractZip(zipFile, targetFolder, "");
        }

        public static void Open(string zipFile)
        {
            string appPath = Path.Combine(Config.Instance.PathTo7z, "7zFM.exe");
            string cmdArgs = zipFile;

            Process ProcessObj = new Process();
            ProcessObj.StartInfo.FileName = appPath;
            ProcessObj.StartInfo.Arguments = cmdArgs;

            ProcessObj.Start();
        }

        public static string ShowContent(string zipFile)
        {
            string appPath = Path.Combine(Config.Instance.PathTo7z, "7z.exe");
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
