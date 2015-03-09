using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;
using Waddu.Types;

namespace Waddu.Core
{
    public static class Helpers
    {
        /// <summary>
        /// Browse for a Folder
        /// </summary>
        public static string BrowseForFolder(string origPath, FolderBrowseType.Enum type)
        {
            var newPath = origPath;
            using (var dlg = new FolderBrowserDialog())
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
            var newFile = origFile;
            using (var dlg = new OpenFileDialog())
            {
                dlg.FileName = origFile;
                var parentPath = Path.GetDirectoryName(origFile);
                if (!Path.IsPathRooted(parentPath))
                {
                    parentPath = Path.GetFullPath(parentPath);
                }
                dlg.InitialDirectory = Directory.Exists(parentPath) ? parentPath : Application.StartupPath;
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
            return Join(separator, list, false);
        }
        public static string Join<T>(string separator, IEnumerable<T> list, bool skipEmpty)
        {
            return Join(separator, list, skipEmpty, delegate(T o) { return o.ToString(); });
        }
        // Join that takes an IEnumerable list that uses a converter to convert the type to a string
        public static string Join<T>(string separator, IEnumerable<T> list, Converter<T, string> converter)
        {
            return Join(separator, list, false, converter);
        }
        public static string Join<T>(string separator, IEnumerable<T> list, bool skipEmpty, Converter<T, string> converter)
        {
            var sb = new StringBuilder();
            foreach (var t in list)
            {
                var converted = converter(t);
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
                dict[key] = value;
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

        public static void CenterFormTo(Form form, Form containerForm)
        {
            Point point = new Point(); Size formSize = form.Size;
            Rectangle workingArea = Screen.GetWorkingArea(containerForm);
            Rectangle rect = containerForm.Bounds;
            point.X = ((rect.Left + rect.Right) - formSize.Width) / 2;
            if (point.X < workingArea.X) point.X = workingArea.X;
            else if ((point.X + formSize.Width) > (workingArea.X + workingArea.Width))
                point.X = (workingArea.X + workingArea.Width) - formSize.Width;
            point.Y = ((rect.Top + rect.Bottom) - formSize.Height) / 2;
            if (point.Y < workingArea.Y) point.Y = workingArea.Y;
            else if ((point.Y + formSize.Height) > (workingArea.Y + workingArea.Height))
                point.Y = (workingArea.Y + workingArea.Height) - formSize.Height;
            form.Location = point;
        }
    }
}
