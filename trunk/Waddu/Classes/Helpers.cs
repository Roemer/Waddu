﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
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

        public static List<string> GetHtml(string url)
        {
            return GetHtml(url, AddonSiteId.wowspecial);
        }
        public static List<string> GetHtml(string url, AddonSiteId addonSiteId)
        {
            List<string> lineList = new List<string>();
            try
            {
                HttpWebRequest webRequest = WebRequest.Create(url) as HttpWebRequest;
                webRequest.CookieContainer = CookieManager.GetCookies(addonSiteId);
                WebResponse responseHtml = webRequest.GetResponse();
                StreamReader reader = new StreamReader(responseHtml.GetResponseStream());

                string line = null;
                while ((line = reader.ReadLine()) != null)
                {
                    lineList.Add(line);
                }
                reader.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(string.Format("Getting URL {0} failed:{1}{2}", url, Environment.NewLine, ex.Message));
            }
            return lineList;
        }

        public static string DownloadFileToTemp(string remoteFilename)
        {
            return DownloadFileToTemp(remoteFilename, null);
        }
        public static string DownloadFileToTemp(string remoteFilename, IDownloadProgress progress)
        {
            string localFilePath = Path.GetTempFileName();
            if (DownloadFile(remoteFilename, localFilePath, progress))
            {
                return localFilePath;
            }
            return string.Empty;
        }
        public static bool DownloadFile(string remoteFilename, string localFilePath)
        {
            return DownloadFile(remoteFilename, localFilePath, null);
        }
        public static bool DownloadFile(string remoteFilename, string localFilePath, IDownloadProgress progress)
        {
            if (progress != null)
            {
                progress.DownloadStatusChanged(-1, -1);
            }

            int bytesProcessed = 0;
            string fileName = remoteFilename.Substring(remoteFilename.LastIndexOf("/") + 1);

            // Assign values to these objects here so that they can
            // be referenced in the finally block
            Stream remoteStream = null;
            Stream localStream = null;
            WebResponse response = null;

            // Use a try/catch/finally block as both the WebRequest and Stream
            // classes throw exceptions upon error
            try
            {
                // Create a request for the specified remote file name
                WebRequest request = WebRequest.Create(remoteFilename);
                if (request != null)
                {
                    // Send the request to the server and retrieve the
                    // WebResponse object
                    response = request.GetResponse();
                    long length = response.ContentLength;
                    if (progress != null)
                    {
                        progress.DownloadStatusChanged(-1, length);
                    }
                    if (response != null)
                    {
                        // Once the WebResponse object has been retrieved,
                        // get the stream object associated with the response's data
                        remoteStream = response.GetResponseStream();

                        // Create the local File
                        localStream = File.Create(localFilePath);

                        // Allocate a 1k buffer
                        byte[] buffer = new byte[1024];
                        int bytesRead;

                        // Simple do/while loop to read from stream until
                        // no bytes are returned
                        do
                        {
                            // Read data (up to 1k) from the stream
                            bytesRead = remoteStream.Read(buffer, 0, buffer.Length);

                            // Write the data to the local file
                            localStream.Write(buffer, 0, bytesRead);

                            // Increment total bytes processed
                            bytesProcessed += bytesRead;

                            if (progress != null)
                            {
                                progress.DownloadStatusChanged(bytesProcessed, length);
                            }
                        } while (bytesRead > 0);
                    }
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
                return false;
            }
            finally
            {
                // Close the response and streams objects here
                // to make sure they're closed even if an exception
                // is thrown at some point
                if (response != null) response.Close();
                if (remoteStream != null) remoteStream.Close();
                if (localStream != null) localStream.Close();

                if (progress != null)
                {
                    progress.DownloadStatusChanged(-1, -1);
                }
            }

            return true;
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
