﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using ICSharpCode.SharpZipLib.Zip;
using System.Windows.Forms;

namespace Waddu.Classes
{
    public class ArchiveHelper
    {
        private ArchiveHelper() { }

        public static bool Exists7z()
        {
            string appPath7z = Path.Combine(Config.Instance.PathTo7z, "7z.exe");
            string appPath7zFM = Path.Combine(Config.Instance.PathTo7z, "7zFM.exe");
            return File.Exists(appPath7z) && File.Exists(appPath7zFM);
        }

        public static bool Expand(string archiveFile, string targetFolder)
        {
            try
            {
                if (Exists7z())
                {
                    // 7z
                    string appPath = Path.Combine(Config.Instance.PathTo7z, "7z.exe");
                    string cmdArgs = "x " + archiveFile + " -o" + targetFolder + " -r -y";
                    Process ProcessObj = new Process();
                    ProcessObj.StartInfo.FileName = appPath;
                    ProcessObj.StartInfo.Arguments = cmdArgs;
                    ProcessObj.StartInfo.UseShellExecute = false;
                    ProcessObj.StartInfo.CreateNoWindow = true;
                    ProcessObj.Start();
                    ProcessObj.WaitForExit();
                }
                else
                {
                    // SharpZipLib
                    FastZip fz = new FastZip();
                    fz.ExtractZip(archiveFile, targetFolder, "");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Could not Expand");
                return false;
            }
            return true;
        }

        public static bool CheckIntegrity(string archiveFile, string addonName)
        {
            List<string> contentList = GetArchiveContent(archiveFile);
            bool baseFound = false;
            foreach (string content in contentList)
            {
                if (content.ToUpper().StartsWith(addonName.ToUpper() + "\\"))
                {
                    baseFound = true;
                    break;
                }
            }
            return baseFound;
        }

        public static void Open(string archiveFile)
        {
            string appPath = Path.Combine(Config.Instance.PathTo7z, "7zFM.exe");
            string cmdArgs = archiveFile;
            Process ProcessObj = new Process();
            ProcessObj.StartInfo.FileName = appPath;
            ProcessObj.StartInfo.Arguments = cmdArgs;
            ProcessObj.Start();
        }

        public static List<string> GetArchiveContent(string archiveFile)
        {
            string appPath = Path.Combine(Config.Instance.PathTo7z, "7z.exe");
            string cmdArgs = "l " + archiveFile;

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

            string[] strList = Result.Split(new string[] { Environment.NewLine }, StringSplitOptions.None);
            bool add = false;
            List<string> contentList = new List<string>();
            foreach (string line in strList)
            {
                if (!add && line.Contains("-------------------"))
                {
                    add = true;
                    continue;
                }
                if (add && line.Contains("-------------------"))
                {
                    add = false;
                    continue;
                }
                if (add)
                {
                    contentList.Add(line.Substring(53));
                }
            }
            return contentList;
        }
    }
}
