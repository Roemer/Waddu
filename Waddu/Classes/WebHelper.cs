﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Windows.Forms;
using Waddu.Types;

namespace Waddu.Classes
{
    public abstract class WebHelper
    {
        public static bool GetString(string url, out string returnValue)
        {
            returnValue = string.Empty;
            try
            {
                WebClient cl = new WebClient();
                returnValue = cl.DownloadString(url);
                return true;
            }
            catch
            {
                return false;
            }
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
            List<string> lineList = new List<string>();
            try
            {
                HttpWebRequest webRequest = WebRequest.Create(url) as HttpWebRequest;
                if (cookies != null)
                {
                    webRequest.CookieContainer = cookies;
                }
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
    }
}