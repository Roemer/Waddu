using System;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using Waddu.Types;

namespace Waddu.UI.Forms
{
    public partial class WebBrowserForm : Form
    {
        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = false)]
        static extern IntPtr SendMessage(IntPtr hWnd, uint Msg, IntPtr wParam, IntPtr lParam);

        [DllImport("user32.dll", SetLastError = true)]
        static extern IntPtr GetWindow(IntPtr hWnd, uint uCmd);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        static extern int GetClassName(IntPtr hWnd, StringBuilder lpClassName, int nMaxCount);

        public string DownloadUrl = string.Empty;

        private AddonSiteId _addonSiteId;

        public WebBrowserForm(string url, AddonSiteId addonSiteId)
        {
            InitializeComponent();

            _addonSiteId = addonSiteId;

            webBrowser1.Navigate(url);
        }

        private void webBrowser1_Navigating(object sender, WebBrowserNavigatingEventArgs e)
        {
            if (e.Url.AbsoluteUri.Contains(".zip") || e.Url.AbsoluteUri.Contains(".rar") || e.Url.AbsoluteUri.Contains(".7z"))
            {
                //Console.WriteLine("Download detected");
                //WebClient wc = new WebClient();
                //string filename = @"c:\test.zip";
                //wc.DownloadFile(e.Url.AbsoluteUri, filename);
                //Console.WriteLine("file downloaded");

                DownloadUrl = e.Url.AbsoluteUri;

                e.Cancel = true;
                DialogResult = DialogResult.OK;
            }
        }

        private void webBrowser1_Navigated(object sender, WebBrowserNavigatedEventArgs e)
        {
            // Search the Element
            //for (int i = 0; i < webBrowser1.Document.All.Count; i++)
            //{
            //    HtmlElement tempEl = webBrowser1.Document.All[i];
            //    if (tempEl.InnerHtml != null && tempEl.InnerHtml.Contains(@"<param name=""movie"" value=""/Themes/Common/v6/flash/DownloadButton.swf"))
            //    {
            //        Console.WriteLine(i);
            //    }

            //    if (tempEl.InnerHtml != null && tempEl.InnerHtml.Contains("download.swf") && i > 100)
            //    {
            //        Console.WriteLine("Element {0}: {1}", i, tempEl.InnerHtml ?? "");
            //    }
            //}

            // Scroll the Flash to the Screen
            HtmlElement el2 = null;
            if (_addonSiteId == AddonSiteId.wowace)
            {
                el2 = webBrowser1.Document.All[776];
            }
            else if (_addonSiteId == AddonSiteId.curse)
            {
                el2 = webBrowser1.Document.All[81];
            }
            else if (_addonSiteId == AddonSiteId.curseforge)
            {
                el2 = webBrowser1.Document.All[154];
            }
            if (el2 != null)
            {
                el2.ScrollIntoView(true);
            }

            // Set the Right Position
            int x = 0;
            int y = 0;
            if (_addonSiteId == AddonSiteId.wowace)
            {
                x = 5;
                y = 5;
            }
            else if (_addonSiteId == AddonSiteId.curse)
            {
                x = 5;
                y = 5;
            }
            else if (_addonSiteId == AddonSiteId.curseforge)
            {
                x = 5;
                y = 5;
            }

            // Emulate the Click
            IntPtr handle = webBrowser1.Handle;
            StringBuilder className = new StringBuilder(100);
            while (className.ToString() != "Internet Explorer_Server") // your mileage may vary with this classname
            {
                handle = GetWindow(handle, 5); // 5 == child
                if (handle == IntPtr.Zero)
                {
                    break;
                }
                GetClassName(handle, className, className.Capacity);
            }
            if (handle != IntPtr.Zero)
            {
                IntPtr lParam = (IntPtr) ((y << 16) | x); // X and Y coordinates of the click
                IntPtr wParam = IntPtr.Zero; // change this if you want to simulate Ctrl-Click and such
                const uint downCode = 0x201; // these codes are for single left clicks
                const uint upCode = 0x202;
                SendMessage(handle, downCode, wParam, lParam); // mousedown
                SendMessage(handle, upCode, wParam, lParam); // mouseup
            }
        }
    }
}
