using System;
using System.Collections.Generic;
using System.Text;
using ICSharpCode.SharpZipLib.Zip;

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
    }
}
/*
// This snippet needs the "System.Diagnostics"
// library
 
// Application path and command line arguments
string ApplicationPath = "C:\\example.exe";
string ApplicationArguments = "-c -x";
 
// Create a new process object
Process ProcessObj = new Process();
 
// StartInfo contains the startup information of
// the new process
ProcessObj.StartInfo.FileName = ApplicationPath;
ProcessObj.StartInfo.Arguments = ApplicationArguments;
 
// These two optional flags ensure that no DOS window
// appears
ProcessObj.StartInfo.UseShellExecute = false;
ProcessObj.StartInfo.CreateNoWindow = true;
 
// If this option is set the DOS window appears again :-/
// ProcessObj.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
 
// This ensures that you get the output from the DOS application
ProcessObj.StartInfo.RedirectStandardOutput = true;
 
// Start the process
ProcessObj.Start();
 
// Wait that the process exits
ProcessObj.WaitForExit();
 
// Now read the output of the DOS application
string Result = ProcessObj.StandardOutput.ReadToEnd();
*/