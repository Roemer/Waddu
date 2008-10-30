using System;
using System.Windows.Forms;
using Waddu.Classes;
using System.Diagnostics;

namespace Waddu.Forms
{
    public partial class ArchiveContentForm : Form
    {
        private string _zipFile = string.Empty;

        public ArchiveContentForm(string zipFile)
        {
            InitializeComponent();

            _zipFile = zipFile;

            string str = ArchiveHelper.ShowContent(zipFile);
            string[] lines = str.Split(new string[] { Environment.NewLine }, StringSplitOptions.None);
            string ret = string.Empty;
            bool add = false;
            foreach (string line in lines)
            {
                if (!add && line.Contains("-------------------"))
                {
                    add = true;
                    continue;
                }
                if (add && line.Contains("-------------------"))
                {
                    add = false;
                    continue;
                }
                if (add)
                {
                    ret += line.Substring(53) + Environment.NewLine;
                }
            }
            txtContent.Text = ret;
        }

        private void btnOpen_Click(object sender, EventArgs e)
        {
            ArchiveHelper.Open(_zipFile);
        }
    }
}
