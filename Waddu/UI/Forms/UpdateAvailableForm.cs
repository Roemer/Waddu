using System.Diagnostics;
using System.Windows.Forms;

namespace Waddu.UI.Forms
{
    public partial class UpdateAvailableForm : Form
    {
        public UpdateAvailableForm(string newestVersion)
        {
            InitializeComponent();

            lblLocalVersion.Text = GetType().Assembly.GetName().Version.ToString();
            lblNewestVersion.Text = newestVersion;

            llInfoSite.Links[0].LinkData = "http://waddu.flauschig.ch";
            llInfoSiteMirror.Links[0].LinkData = "http://code.google.com/p/waddu/downloads/list";
        }

        private void LinkLabel_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            var target = e.Link.LinkData as string;
            Process.Start(target);
        }
    }
}
