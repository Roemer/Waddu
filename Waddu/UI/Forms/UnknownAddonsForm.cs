using System;
using System.Windows.Forms;

namespace Waddu.Forms
{
    public partial class UnknownAddonsForm : Form
    {
        public string UnknownAddonsText
        {
            get { return txtUnknownAddons.Text; }
            set { txtUnknownAddons.Text = value; }
        }

        public UnknownAddonsForm()
        {
            InitializeComponent();
        }

        private void btnCopyToClipboard_Click(object sender, EventArgs e)
        {
            Clipboard.SetText(txtUnknownAddons.Text);
        }
    }
}
