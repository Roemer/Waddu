using System;
using System.Windows.Forms;

namespace Waddu.UI.Forms
{
    public partial class ErrorForm : Form
    {
        public ErrorForm(Exception ex)
        {
            InitializeComponent();

            txtError.Text = ex.Message;
            txtStackTrace.Text = ex.StackTrace;
        }

        private void btnCopy_Click(object sender, EventArgs e)
        {
            Clipboard.SetText(txtError.Text + Environment.NewLine + txtStackTrace.Text);
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
