using System;
using System.Windows.Forms;
using Waddu.Core;

namespace Waddu.UI.Forms
{
    public partial class ArchiveContentForm : Form
    {
        private string _zipFile = string.Empty;

        public ArchiveContentForm(string zipFile)
        {
            InitializeComponent();

            _zipFile = zipFile;

            var lines = ArchiveHelper.GetArchiveContent(zipFile);
            txtContent.Text = Helpers.Join<string>(Environment.NewLine, lines);

            tvContent.BeginUpdate();
            tvContent.Nodes.Clear();
            tvContent.Nodes.Add("Archive", "Archive");
            foreach (var line in lines)
            {
                var node = tvContent.Nodes[0];
                var nameList = line.Split(new string[] { "\\" }, StringSplitOptions.RemoveEmptyEntries);
                for (var i=0; i<nameList.Length; i++)
                {
                    var name = nameList[i];
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
