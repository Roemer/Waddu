using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Waddu.Forms
{
    public partial class UpdateAvailableForm : Form
    {
        public UpdateAvailableForm(string newestVersion)
        {
            InitializeComponent();

            lblLocalVersion.Text = this.GetType().Assembly.GetName().Version.ToString();
            lblNewestVersion.Text = newestVersion;

            llInfoSite.Links[0].LinkData = "http://waddu.flauschig.ch";
            llInfoSiteMirror.Links[0].LinkData = "http://www.red-demon.com/waddu";
        }

        private void LinkLabel_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            string target = e.Link.LinkData as string;
            System.Diagnostics.Process.Start(target);
        }
    }
}
