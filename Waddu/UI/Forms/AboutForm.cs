﻿using System;
using System.Windows.Forms;

namespace Waddu.UI.Forms
{
    public partial class AboutForm : Form
    {
        public AboutForm()
        {
            InitializeComponent();

            lblInfo.Text = "Waddu" + " v." + GetType().Assembly.GetName().Version;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
