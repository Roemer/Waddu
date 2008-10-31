using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Waddu.Classes;

namespace Waddu.Forms
{
    public partial class ArchiveContentForm : Form
    {
        private string _zipFile = string.Empty;

        public ArchiveContentForm(string zipFile)
        {
            InitializeComponent();

            _zipFile = zipFile;

            List<string> lines = ArchiveHelper.GetArchiveContent(zipFile);
            txtContent.Text = Helpers.Join<string>(Environment.NewLine, lines);

            tvContent.BeginUpdate();
            tvContent.Nodes.Clear();
            tvContent.Nodes.Add("Archive", "Archive");
            foreach (string line in lines)
            {
                TreeNode node = tvContent.Nodes[0];
                string[] nameList = line.Split(new string[] { "\\" }, StringSplitOptions.RemoveEmptyEntries);
                for (int i=0; i<nameList.Length; i++)
                {
                    string name = nameList[i];
                    if (!node.Nodes.ContainsKey(name))
                    {
                        node.Nodes.Add(name, name);
                    }
                    node = node.Nodes[name];
                }
            }
            tvContent.EndUpdate();
        }

        private void btnOpen_Click(object sender, EventArgs e)
        {
            ArchiveHelper.Open(_zipFile);
        }
    }
}
