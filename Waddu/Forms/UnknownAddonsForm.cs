using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
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
