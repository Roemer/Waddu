using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using Waddu.Interfaces;
using Waddu.Types;

namespace Waddu.Core
{
    public abstract class WebHelper
    {
        public static bool GetString(string url, out string returnValue)
        {
            returnValue = string.Empty;
            try
            {
                var cl = new WebClient();
                returnValue = cl.DownloadString(url);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public static Match GetMatch(List<string> pageLines, string regex)
        {
            for (var i = 0; i < pageLines.Count; i++)
            {
                var line = pageLines[i];
                var m = Regex.Match(line, regex);
                if (m.Success)
                {
                    return m;
                }
            }
            return null;
        }

        public static string GetContainingLine(List<string> pageLines, string stringToContain)
        {
            for (var i = 0; i < pageLines.Count; i++)
            {
                var line = pageLines[i];
                if (line.ToUpper().Contains(stringToContain.ToUpper()))
                {
                    return line;
                }
            }
            return string.Empty;
        }

        public static List<string> GetHtml(string url)
        {
            return GetHtml(url, null);
        }
        public static List<string> GetHtml(string url, AddonSiteId addonSiteId)
        {
            return GetHtml(url, CookieManager.GetCookies(addonSiteId));
        }
        public static List<string> GetHtml(string url, CookieContainer cookies)
        {
            var lineList = new List<string>();
            try
            {
                var webRequest = WebRequest.Create(url) as HttpWebRequest;
                if (cookies != null)
                {
                    webRequest.CookieContainer = cookies;
                }
                var responseHtml = webRequest.GetResponse();
                var reader = new StreamReader(responseHtml.GetResponseStream());

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
            var localFilePath = Path.GetTempFileName();
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

            var bytesProcessed = 0;
            var fileName = remoteFilename.Substring(remoteFilename.LastIndexOf("/") + 1);

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
                var request = WebRequest.Create(remoteFilename);
                if (request != null)
                {
                    // Send the request to the server and retrieve the
                    // WebResponse object
                    response = request.GetResponse();
                    var length = response.ContentLength;
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
                        var buffer = new byte[1024];
                        int bytesRead;

                        // Simple do/while Loop to read from Stream until no Bytes are returned
                        do
                        {
                            // Read Data (up to 1k) from the Stream
                            bytesRead = remoteStream.Read(buffer, 0, buffer.Length);

                            // Write the Data to the local File
                            localStream.Write(buffer, 0, bytesRead);

                            // Increment total Bytes processed
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
    }
}
