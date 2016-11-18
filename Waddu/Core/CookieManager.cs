using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using Waddu.Types;

namespace Waddu.Core
{
    public class CookieManager
    {
        private static readonly Dictionary<AddonSiteId, CookieContainer> CookieDict = new Dictionary<AddonSiteId, CookieContainer>();

        public static CookieContainer GetCookies(AddonSiteId addonSiteId)
        {
            lock (CookieDict)
            {
                if (CookieDict.ContainsKey(addonSiteId))
                {
                    return CookieDict[addonSiteId];
                }

                CookieContainer cookies = null;
                if (addonSiteId == AddonSiteId.curseforge)
                {
                    cookies = GetCurseForgeCookies(Config.Instance.CurseLogin, Config.Instance.CursePassword);
                }
                else if (addonSiteId == AddonSiteId.wowace)
                {
                    cookies = GetWowAceCookies(Config.Instance.CurseLogin, Config.Instance.CursePassword);
                }

                CookieDict.Add(addonSiteId, cookies);
                return CookieDict[addonSiteId];
            }
        }

        public static void ClearCookies()
        {
            CookieDict.Clear();
        }

        private static CookieContainer GetWowAceCookies(string loginName, string loginPassword)
        {
            ServicePointManager.ServerCertificateValidationCallback += (sender, certificate, chain, errors) => true;

            // Setup
            HttpWebRequest webRequest = null;
            StreamReader responseReader = null;
            var responseData = String.Empty;

            // Create an empty Cookie Container
            var cookies = new CookieContainer();

            // Get Cookies from Base URL
            var baseUrl = "https://www.wowace.com/login-modal";
            webRequest = WebRequest.Create(baseUrl) as HttpWebRequest;
            webRequest.CookieContainer = cookies;
            responseReader = new StreamReader(webRequest.GetResponse().GetResponseStream());
            responseData = responseReader.ReadToEnd();
            responseReader.Close();

            // Build POST Data
            //string postData = String.Format("next=%2F&kind=login&username={0}&password={1}", Uri.EscapeDataString(loginName), Uri.EscapeDataString(loginPassword));
            var postData = string.Format("login-next=%2F&form_type=login&login-username={0}&login-password={1}", Uri.EscapeDataString(loginName), Uri.EscapeDataString(loginPassword));

            // Post to the Login Form
            var loginUrl = "https://www.wowace.com/login-modal";
            webRequest = WebRequest.Create(loginUrl) as HttpWebRequest;
            webRequest.Method = "POST";
            webRequest.ContentType = "application/x-www-form-urlencoded";
            webRequest.CookieContainer = cookies;
            // Write the Form Values into the Request Message
            var requestWriter = new StreamWriter(webRequest.GetRequestStream());
            requestWriter.Write(postData);
            requestWriter.Close();

            try
            {
                // Get the Response (and the Cookies) and forget Content
                var resp = webRequest.GetResponse();
                var respStream = new StreamReader(resp.GetResponseStream());
                var respstr = respStream.ReadToEnd();
                respStream.Close();
                resp.Close();
            }
            catch (WebException webex)
            {
                Console.WriteLine("Unable to perform command: " + webRequest);

                var data = String.Empty;
                if (webex.Response != null)
                {
                    var r = new StreamReader(webex.Response.GetResponseStream());
                    data = r.ReadToEnd();
                    r.Close();
                }
            }

            return cookies;
        }

        private static CookieContainer GetCurseForgeCookies(string loginName, string loginPassword)
        {
            // Setup
            HttpWebRequest webRequest = null;
            StreamReader responseReader = null;
            var responseData = string.Empty;

            // Create an empty Cookie Container
            var cookies = new CookieContainer();

            // Get Cookies from Base URL
            var baseUrl = "https://www.curseforge.com/login-modal";
            webRequest = WebRequest.Create(baseUrl) as HttpWebRequest;
            webRequest.CookieContainer = cookies;
            responseReader = new StreamReader(webRequest.GetResponse().GetResponseStream());
            responseData = responseReader.ReadToEnd();
            responseReader.Close();

            // Build POST Data
            //string postData = String.Format("next=%2F&kind=login&username={0}&password={1}", loginName, loginPassword);
            var postData = string.Format("login-next=%2F&form_type=login&login-username={0}&login-password={1}", Uri.EscapeDataString(loginName), Uri.EscapeDataString(loginPassword));

            // Post to the Login Form
            var loginUrl = "https://www.curseforge.com/login-modal";
            webRequest = WebRequest.Create(loginUrl) as HttpWebRequest;
            webRequest.Method = "POST";
            webRequest.ContentType = "application/x-www-form-urlencoded";
            webRequest.CookieContainer = cookies;
            // Write the Form Values into the Request Mssage
            var requestWriter = new StreamWriter(webRequest.GetRequestStream());
            requestWriter.Write(postData);
            requestWriter.Close();

            try
            {
                // Get the Response (and the Cookies) and forget Content
                var resp = webRequest.GetResponse();
                var respStream = new StreamReader(resp.GetResponseStream());
                var respstr = respStream.ReadToEnd();
                respStream.Close();
                resp.Close();
            }
            catch (WebException webex)
            {
                Console.WriteLine("Unable to perform command: " + webRequest);

                var data = String.Empty;
                if (webex.Response != null)
                {
                    var r = new StreamReader(webex.Response.GetResponseStream());
                    data = r.ReadToEnd();
                    r.Close();
                }
            }

            return cookies;
        }
    }
}
