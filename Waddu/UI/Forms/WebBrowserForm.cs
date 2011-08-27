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
        static extern IntPtr SendMessage(IntPtr hWnd, uint msg, IntPtr wParam, IntPtr lParam);

        [DllImport("user32.dll", SetLastError = true)]
        static extern IntPtr GetWindow(IntPtr hWnd, uint uCmd);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        static extern int GetClassName(IntPtr hWnd, StringBuilder lpClassName, int nMaxCount);

        public string DownloadUrl = string.Empty;
        public bool UseFile = false;
        public string FileUrl = string.Empty;

        private readonly AddonSiteId _addonSiteId;

        public WebBrowserForm(string url, AddonSiteId addonSiteId, string addonName)
        {
            _addonSiteId = addonSiteId;

            InitializeComponent();

            ChangeUserAgent();

            // Set the Title
            Text = "DL: " + addonName;

            // Initialize Browser
            btnBack.Enabled = false;
            btnForward.Enabled = false;
            webBrowser1.CanGoBackChanged += new EventHandler(webBrowser1_CanGoBackChanged);
            webBrowser1.CanGoForwardChanged += new EventHandler(webBrowser1_CanGoForwardChanged);
            webBrowser1.ScriptErrorsSuppressed = true;
            webBrowser1.NewWindow += new System.ComponentModel.CancelEventHandler(webBrowser1_NewWindow);

            // Navigate
            webBrowser1.Navigate(url);
        }

        private void webBrowser1_CanGoBackChanged(object sender, EventArgs e)
        {
            WebBrowser browser = sender as WebBrowser;
            btnBack.Enabled = browser.CanGoBack;
        }

        private void webBrowser1_CanGoForwardChanged(object sender, EventArgs e)
        {
            WebBrowser browser = sender as WebBrowser;
            btnForward.Enabled = browser.CanGoForward;
        }

        private void DoClick(int x, int y)
        {
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
                IntPtr lParam = (IntPtr)((y << 16) | x); // X and Y coordinates of the click
                IntPtr wParam = IntPtr.Zero; // change this if you want to simulate Ctrl-Click and such
                const uint downCode = 0x201; // these codes are for single left clicks
                const uint upCode = 0x202;
                SendMessage(handle, downCode, wParam, lParam); // mousedown
                SendMessage(handle, upCode, wParam, lParam); // mouseup
            }
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            webBrowser1.GoBack();
        }

        private void btnForward_Click(object sender, EventArgs e)
        {
            webBrowser1.GoForward();
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            webBrowser1.Refresh(WebBrowserRefreshOption.Completely);
        }

        private void btnGo_Click(object sender, EventArgs e)
        {
            webBrowser1.Navigate(txtUrl.Text);
        }

        private void webBrowser1_Navigating(object sender, WebBrowserNavigatingEventArgs e)
        {
            WebBrowser browser = sender as WebBrowser;

            if (e.Url.AbsoluteUri.Contains(".zip") || e.Url.AbsoluteUri.Contains(".rar") || e.Url.AbsoluteUri.Contains(".7z"))
            {
                DownloadUrl = e.Url.AbsoluteUri;

                e.Cancel = true;
                DialogResult = DialogResult.OK;
            }
            else
            {
                txtUrl.Text = browser.Url.AbsoluteUri;
            }
        }

        private void webBrowser1_Navigated(object sender, WebBrowserNavigatedEventArgs e)
        {
            Console.WriteLine("webBrowser1_Navigated");
        }

        private void webBrowser1_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            WebBrowser browser = sender as WebBrowser;

            if (browser != null && browser.Document != null)
            {
                // Search the Element
                int foundElement = -1;
                for (int i = 0; i < browser.Document.All.Count; i++)
                {
                    HtmlElement tempEl = browser.Document.All[i];
                    if (tempEl.InnerHtml != null)
                    {
                        // WoWAce / CurseForge
                        //if (tempEl.InnerHtml.Contains("download.swf") && i > 100)
                        //{
                        //    string temp = string.Format("Element {0}: {1}", i, tempEl.InnerHtml);
                        //}
                        //// Curse
                        //if (tempEl.InnerHtml.Contains("DownloadButton.swf"))
                        //{
                        //    string temp = string.Format("Element {0}: {1}", i, tempEl.InnerHtml);
                        //}

                        if (_addonSiteId == AddonSiteId.wowace && tempEl.InnerHtml.StartsWith("<LI class=\"user-action user-action-download\""))
                        {
                            foundElement = i;
                            break;
                        }
                        else if (_addonSiteId == AddonSiteId.curseforge && tempEl.InnerHtml.StartsWith("<LI class=\"user-action user-action-download\""))
                        {
                            foundElement = i + 1;
                            break;
                        }
                        else if (_addonSiteId == AddonSiteId.curse && tempEl.InnerHtml.StartsWith("\r\n<embed src=\"/Themes/Common/v6/flash/DownloadButton.swf"))
                        {
                            foundElement = i;
                            break;
                        }
                    }
                }

                // Scroll the Flash to the Screen
                if (foundElement >= 0)
                {
                    HtmlElement el2 = browser.Document.All[foundElement];
                    if (el2 != null)
                    {
                        // Scroll the Element into the View
                        el2.ScrollIntoView(true);

                        // Set the Right Position
                        int x = 5;
                        int y = 5;

                        // Fix Position
                        if (_addonSiteId == AddonSiteId.wowace || _addonSiteId == AddonSiteId.curseforge)
                        {
                            x = 60;
                            y = 30;
                        }

                        // Perform the Click
                        DoClick(x, y);
                    }
                }
            }
        }

        private void webBrowser1_FileDownload(object sender, EventArgs e)
        {
            Console.WriteLine("webBrowser1_FileDownload");
        }

        private void webBrowser1_NewWindow(object sender, System.ComponentModel.CancelEventArgs e)
        {
            // Possibility to block Popups?
            //e.Cancel = true;
            Console.WriteLine("webBrowser1_NewWindow");
        }

        private void llInfoText_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            using (OpenFileDialog dlg = new OpenFileDialog())
            {
                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    UseFile = true;
                    FileUrl = dlg.FileName;
                    DialogResult = DialogResult.OK;
                }
            }
        }

        #region Override UserAgent
        [DllImport("urlmon.dll", CharSet = CharSet.Ansi)]
        private static extern int UrlMkSetSessionOption(int dwOption, string pBuffer, int dwBufferLength, int dwReserved);

        const int URLMON_OPTION_USERAGENT = 0x10000001;

        public void ChangeUserAgent()
        {
            string ua = "Mozilla/4.0 (compatible; MSIE 8.0; Windows NT 6.0; Trident/4.0; SLCC1; .NET CLR 2.0.50727; Media Center PC 5.0; .NET CLR 3.0.30729; .NET CLR 3.5.30729; Creative AutoUpdate v1.40.01)";
            UrlMkSetSessionOption(URLMON_OPTION_USERAGENT, ua, ua.Length, 0);
        }
        #endregion
    }
}
