using System.Windows.Forms;
using Waddu.Classes;

namespace Waddu.Forms
{
    public partial class ArchiveContentForm : Form
    {
        public ArchiveContentForm(string zipFile)
        {
            InitializeComponent();

            string str = ZipHelper.ShowContent(zipFile);
            txtContent.Text = str;
        }
    }
}
