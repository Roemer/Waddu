using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Waddu.Forms
{
    public partial class AboutForm : Form
    {
        public AboutForm()
        {
            InitializeComponent();

            lblInfo.Text = "Waddu" + " v." + this.GetType().Assembly.GetName().Version;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
