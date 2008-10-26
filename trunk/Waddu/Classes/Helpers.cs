using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Windows.Forms;
using ICSharpCode.SharpZipLib.Zip;
using Waddu.AddonSites;
using Waddu.Properties;
using Waddu.Types;

namespace Waddu.Classes
{
    public class Helpers
    {
        // Private Constructor
        private Helpers() { }

        /// <summary>
        /// Browse for the WoW Folder
        /// </summary>
        public static string BrowseForWoWFolder(string origPath)
        {
            string newPath = origPath;
            using (FolderBrowserDialog dlg = new FolderBrowserDialog())
            {
                dlg.SelectedPath = origPath;
                dlg.ShowNewFolderButton = false;
                dlg.Description = "Select your World of Warcraft Folder";
                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    newPath = dlg.SelectedPath;
                }
            }
            return newPath;
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

        public static string DownloadFile(string remoteFilename)
        {
            return DownloadFile(remoteFilename, null);
        }
        public static string DownloadFile(string remoteFilename, IDownloadProgress progress)
        {
            int bytesProcessed = 0;
            string fileName = remoteFilename.Substring(remoteFilename.LastIndexOf("/") + 1);
            string localFilePath = string.Empty;

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
                    if (response != null)
                    {
                        // Once the WebResponse object has been retrieved,
                        // get the stream object associated with the response's data
                        remoteStream = response.GetResponseStream();

                        // Create the local File
                        localFilePath = Path.GetTempFileName();
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
                                progress.StatusText = string.Format("{0} of {1}", FormatBytes(bytesProcessed), FormatBytes(length));
                            }
                        } while (bytesRead > 0);

                        if (progress != null)
                        {
                            progress.StatusText = string.Empty;
                        }
                    }
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
                return string.Empty;
            }
            finally
            {
                // Close the response and streams objects here
                // to make sure they're closed even if an exception
                // is thrown at some point
                if (response != null) response.Close();
                if (remoteStream != null) remoteStream.Close();
                if (localStream != null) localStream.Close();
            }

            return localFilePath;
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
