using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows.Forms;
using Waddu.Types;

namespace Waddu.Classes
{
    public class Helpers
    {
        // Private Constructor
        private Helpers() { }

        /// <summary>
        /// Browse for a Folder
        /// </summary>
        public static string BrowseForFolder(string origPath, FolderBrowseType.Enum type)
        {
            string newPath = origPath;
            using (FolderBrowserDialog dlg = new FolderBrowserDialog())
            {
                dlg.SelectedPath = origPath;
                dlg.ShowNewFolderButton = false;
                dlg.Description = FolderBrowseType.GetDescription(type);
                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    newPath = dlg.SelectedPath;
                }
            }
            return newPath;
        }

        /// <summary>
        /// Browse for a File
        /// </summary>
        public static string BrowseForFile(string origFile)
        {
            string newFile = origFile;
            using (OpenFileDialog dlg = new OpenFileDialog())
            {
                dlg.FileName = origFile;
                string parentPath = Path.GetDirectoryName(origFile);
                if (!Path.IsPathRooted(parentPath))
                {
                    parentPath = Path.GetFullPath(parentPath);
                }
                if (Directory.Exists(parentPath))
                {
                    dlg.InitialDirectory = parentPath;
                    
                }
                else
                {
                    dlg.InitialDirectory = Application.StartupPath;
                }
                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    newFile = dlg.FileName;
                }
            }
            return newFile;
        }

        // Default Join that takes an IEnumerable list and just takes the ToString of each item
        public static string Join<T>(string separator, IEnumerable<T> list)
        {
            return Join<T>(separator, list, false);
        }
        public static string Join<T>(string separator, IEnumerable<T> list, bool skipEmpty)
        {
            return Join<T>(separator, list, skipEmpty, delegate(T o) { return o.ToString(); });
        }
        // Join that takes an IEnumerable list that uses a converter to convert the type to a string
        public static string Join<T>(string separator, IEnumerable<T> list, Converter<T, string> converter)
        {
            return Join<T>(separator, list, false, converter);
        }
        public static string Join<T>(string separator, IEnumerable<T> list, bool skipEmpty, Converter<T, string> converter)
        {
            StringBuilder sb = new StringBuilder();
            foreach (T t in list)
            {
                string converted = converter(t);
                if (skipEmpty && converted.Length <= 0) { continue; }
                if (sb.Length != 0) { sb.Append(separator); }
                sb.Append(converted);
            }
            return sb.ToString();
        }

        public static void AddIfNeeded<T>(IList<T> list, T value)
        {
            if (!list.Contains(value))
            {
                list.Add(value);
            }
        }

        public static void AddOrUpdate<T, T2>(IDictionary<T, T2> dict, T key, T2 value)
        {
            lock (dict)
            {
                if (dict.ContainsKey(key))
                {
                    dict[key] = value;
                }
                else
                {
                    dict.Add(key, value);
                }
            }
        }

        public static string FormatBytes(long bytes)
        {
            // MBytes
            if (bytes > 1024 * 1024)
            {
                return string.Format("{0:F} MB", (decimal)bytes / 1024 / 1024);
            }

            // KBytes
            if (bytes > 1024)
            {
                return string.Format("{0} KB", bytes / 1024);
            }

            // Bytes
            return string.Format("{0} B", bytes);
        }
    }
}
