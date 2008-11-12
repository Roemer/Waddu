using System;
using System.Collections.Generic;
using System.IO;
using Waddu.Core.BusinessObjects;

namespace Waddu.Core
{
    public abstract class TocHelper
    {
        public static void GetSavedVariables(Addon addon)
        {
            //## SavedVariables: someVariable, someOtherVariable
        }

        public static bool GetVersion(Addon addon, out string versionString)
        {
            if (addon.IsInstalled)
            {
                versionString = GetLine(addon, "## Version:");
                if (versionString != string.Empty)
                {
                    versionString += " (TOC)";
                    return true;
                }
            }
            versionString = string.Empty;
            return false;
        }

        public static List<string> GetDependencies(Addon addon)
        {
            List<string> depList = new List<string>();
            if (addon.IsInstalled)
            {
                // Required
                string line = GetLine(addon, "## RequiredDeps:");
                foreach (string dep in line.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    depList.Add(dep.Trim() + " (Required)");
                }
                line = GetLine(addon, "## Dependencies:");
                foreach (string dep in line.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    depList.Add(dep.Trim() + " (Required)");
                }
                line = GetLine(addon, "## Dependancies:");
                foreach (string dep in line.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    depList.Add(dep.Trim() + " (Required)");
                }
                // Ace
                line = GetLine(addon, "## X-Embeds:");
                foreach (string dep in line.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    depList.Add(dep.Trim() + " (Ace)");
                }
                // Optional
                line = GetLine(addon, "## OptionalDeps:");
                foreach (string dep in line.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    depList.Add(dep.Trim() + " (Optional)");
                }
            }
            return depList;
        }

        private static string GetLine(Addon addon, string contString)
        {
            string tocFile = GetTocFile(addon);
            if (tocFile == string.Empty || !File.Exists(tocFile))
            {
                return string.Empty;
            }
            foreach (string line in File.ReadAllLines(tocFile))
            {
                if (line.Contains(contString))
                {
                    int dIndex = line.IndexOf(":");
                    if (dIndex > 0)
                    {
                        return line.Substring(dIndex + 1).Trim();
                    }
                }
            }
            return string.Empty;
        }

        private static string GetTocFile(Addon addon)
        {
            string addonFolderPath = Path.Combine(Config.Instance.WowFolderPath, @"Interface\Addons");
            addonFolderPath = Path.Combine(addonFolderPath, addon.Name);

            string tocFile = Path.Combine(addonFolderPath, addon.Name + ".toc");
            if (!File.Exists(tocFile))
            {
                // Search Toc File
                foreach (string file in Directory.GetFiles(addonFolderPath, "*.toc"))
                {
                    tocFile = file;
                    break;
                }
            }

            return tocFile;
        }
    }
}
