using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using Waddu.Types;

namespace Waddu.Core
{
    public class CookieManager
    {
        private static Dictionary<AddonSiteId, CookieContainer> _cookieDict = new Dictionary<AddonSiteId, CookieContainer>();

        public static CookieContainer GetCookies(AddonSiteId addonSiteId)
        {
            lock (_cookieDict)
            {
                if (_cookieDict.ContainsKey(addonSiteId))
                {
                    return _cookieDict[addonSiteId];
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

                _cookieDict.Add(addonSiteId, cookies);
                return _cookieDict[addonSiteId];
            }
        }

        public static void ClearCookies()
        {
            _cookieDict.Clear();
        }

        private static CookieContainer GetWowAceCookies(string loginName, string loginPassword)
        {
            System.Net.ServicePointManager.ServerCertificateValidationCallback += delegate(object sender, System.Security.Cryptography.X509Certificates.X509Certificate certificate,
                System.Security.Cryptography.X509Certificates.X509Chain chain,
                System.Net.Security.SslPolicyErrors sslPolicyErrors)
            {
                return true; // Always accept 
            };

            // Setup
            HttpWebRequest webRequest = null;
            StreamReader responseReader = null;
            string responseData = string.Empty;

            // Create an empty Cookie Container
            CookieContainer cookies = new CookieContainer();

            // Get Cookies from Base URL
            string baseUrl = "https://www.wowace.com/home/login/";
            webRequest = WebRequest.Create(baseUrl) as HttpWebRequest;
            webRequest.CookieContainer = cookies;
            responseReader = new StreamReader(webRequest.GetResponse().GetResponseStream());
            responseData = responseReader.ReadToEnd();
            responseReader.Close();

            // Build POST Data
            //string postData = String.Format("next=%2F&kind=login&username={0}&password={1}", Uri.EscapeDataString(loginName), Uri.EscapeDataString(loginPassword));
            string postData = string.Format("login-next=%2F&form_type=login&login-username={0}&login-password={1}", Uri.EscapeDataString(loginName), Uri.EscapeDataString(loginPassword));

            // Post to the Login Form
            string loginUrl = "https://www.wowace.com/home/login/";
            webRequest = WebRequest.Create(loginUrl) as HttpWebRequest;
            webRequest.Method = "POST";
            webRequest.ContentType = "application/x-www-form-urlencoded";
            webRequest.CookieContainer = cookies;
            // Write the Form Values into the Request Message
            StreamWriter requestWriter = new StreamWriter(webRequest.GetRequestStream());
            requestWriter.Write(postData);
            requestWriter.Close();

            try
            {
                // Get the Response (and the Cookies) and forget Content
                WebResponse resp = webRequest.GetResponse();
                StreamReader respStream = new StreamReader(resp.GetResponseStream());
                string respstr = respStream.ReadToEnd();
                respStream.Close();
                resp.Close();
            }
            catch (WebException webex)
            {
                Console.WriteLine("Unable to perform command: " + webRequest);

                String data = String.Empty;
                if (webex.Response != null)
                {
                    StreamReader r = new StreamReader(webex.Response.GetResponseStream());
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
            string responseData = string.Empty;

            // Create an empty Cookie Container
            CookieContainer cookies = new CookieContainer();

            // Get Cookies from Base URL
            string baseUrl = "https://www.curseforge.com/home/login/";
            webRequest = WebRequest.Create(baseUrl) as HttpWebRequest;
            webRequest.CookieContainer = cookies;
            responseReader = new StreamReader(webRequest.GetResponse().GetResponseStream());
            responseData = responseReader.ReadToEnd();
            responseReader.Close();

            // Build POST Data
            //string postData = String.Format("next=%2F&kind=login&username={0}&password={1}", loginName, loginPassword);
            string postData = string.Format("login-next=%2F&form_type=login&login-username={0}&login-password={1}", Uri.EscapeDataString(loginName), Uri.EscapeDataString(loginPassword));

            // Post to the Login Form
            string loginUrl = "https://www.curseforge.com/home/login/";
            webRequest = WebRequest.Create(loginUrl) as HttpWebRequest;
            webRequest.Method = "POST";
            webRequest.ContentType = "application/x-www-form-urlencoded";
            webRequest.CookieContainer = cookies;
            // Write the Form Values into the Request Mssage
            StreamWriter requestWriter = new StreamWriter(webRequest.GetRequestStream());
            requestWriter.Write(postData);
            requestWriter.Close();

            try
            {
                // Get the Response (and the Cookies) and forget Content
                WebResponse resp = webRequest.GetResponse();
                StreamReader respStream = new StreamReader(resp.GetResponseStream());
                string respstr = respStream.ReadToEnd();
                respStream.Close();
                resp.Close();
            }
            catch (WebException webex)
            {
                Console.WriteLine("Unable to perform command: " + webRequest);

                String data = String.Empty;
                if (webex.Response != null)
                {
                    StreamReader r = new StreamReader(webex.Response.GetResponseStream());
                    data = r.ReadToEnd();
                    r.Close();
                }
            }

            return cookies;
        }
    }
}
