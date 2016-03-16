using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;
using ICSharpCode.SharpZipLib.Zip;

namespace Waddu.Core
{
    public class ArchiveHelper
    {
        private ArchiveHelper() { }

        public static bool Exists7z()
        {
            var appPath7z = Path.Combine(Config.Instance.PathTo7z, "7z.exe");
            var appPath7zFM = Path.Combine(Config.Instance.PathTo7z, "7zFM.exe");
            return File.Exists(appPath7z) && File.Exists(appPath7zFM);
        }

        public static bool Expand(string archiveFile, string targetFolder)
        {
            try
            {
                if (Exists7z())
                {
                    // 7z
                    var appPath = Path.Combine(Config.Instance.PathTo7z, "7z.exe");
                    var cmdArgs = string.Format(@"x ""{0}"" -o""{1}"" -r -y", archiveFile, targetFolder);
                    var processObj = new Process
                    {
                        StartInfo =
                        {
                            FileName = appPath,
                            Arguments = cmdArgs,
                            UseShellExecute = false,
                            CreateNoWindow = true
                        }
                    };
                    processObj.Start();
                    processObj.WaitForExit();
                }
                else
                {
                    // SharpZipLib
                    var fz = new FastZip();
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
            var baseFound = false;
            var folderList = GetRootFolders(archiveFile);
            foreach (var folder in folderList)
            {
                if (folder.ToUpper() == addonName.ToUpper())
                {
                    baseFound = true;
                    break;
                }
            }
            return baseFound;
        }

        public static void Open(string archiveFile)
        {
            var appPath = Path.Combine(Config.Instance.PathTo7z, "7zFM.exe");
            var cmdArgs = archiveFile;
            var processObj = new Process { StartInfo = { FileName = appPath, Arguments = cmdArgs } };
            processObj.Start();
        }

        public static List<string> GetRootFolders(string archiveFile)
        {
            var contentList = GetArchiveContent(archiveFile);
            var folderList = new List<string>();
            foreach (var content in contentList)
            {
                var folderEndIndex = content.IndexOf(@"\");
                if (folderEndIndex > 0)
                {
                    var folderName = content.Substring(0, folderEndIndex);
                    Helpers.AddIfNeeded(folderList, folderName);
                }
            }
            folderList.Sort();
            return folderList;
        }

        public static List<string> GetArchiveContent(string archiveFile)
        {
            var appPath = Path.Combine(Config.Instance.PathTo7z, "7z.exe");
            var cmdArgs = string.Format(@"l ""{0}""", archiveFile);

            // Create a new process object
            var processObj = new Process();
            processObj.StartInfo.FileName = appPath;
            processObj.StartInfo.Arguments = cmdArgs;

            // Hide DOS Window
            processObj.StartInfo.UseShellExecute = false;
            processObj.StartInfo.CreateNoWindow = true;

            // This ensures that you get the output from the DOS application
            processObj.StartInfo.RedirectStandardOutput = true;

            // Start the process
            processObj.Start();

            // Now read the output of the DOS application
            var result = processObj.StandardOutput.ReadToEnd();

            // Wait that the process exits
            processObj.WaitForExit();

            var strList = result.Split(new[] { Environment.NewLine }, StringSplitOptions.None);
            var add = false;
            var contentList = new List<string>();
            foreach (var line in strList)
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
