using System;
using System.Windows.Forms;
using Waddu.Classes;
using Waddu.Properties;

namespace Waddu.Forms
{
    public partial class SettingsForm : Form
    {
        public SettingsForm()
        {
            InitializeComponent();
            InitializeSettings();
        }

        private void InitializeSettings()
        {
            chkDelete.Checked = Config.Instance.DeleteBeforeUpdate;
            chkMoveToTrash.Checked = Config.Instance.MoveToTrash;
            txtWoWPath.Text = Config.Instance.WowFolderPath;
            numThreads.Value = Config.Instance.NumberOfThreads;
            txtCurseLogin.Text = Config.Instance.CurseLogin;
            txtCursePassword.Text = Config.Instance.CursePassword;
            chkSavePassword.Checked = Config.Instance.SavePassword;
            txtMappingFile.Text = Config.Instance.MappingFile;
        }

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            txtWoWPath.Text = Helpers.BrowseForWoWFolder(txtWoWPath.Text);
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            Config.Instance.DeleteBeforeUpdate = chkDelete.Checked;
            Config.Instance.MoveToTrash = chkMoveToTrash.Checked;
            Config.Instance.WowFolderPath = txtWoWPath.Text;
            Config.Instance.NumberOfThreads = Convert.ToInt32(numThreads.Value);
            Config.Instance.CurseLogin = txtCurseLogin.Text;
            Config.Instance.CursePassword = txtCursePassword.Text;
            Config.Instance.SavePassword = chkSavePassword.Checked;
            Config.Instance.MappingFile = txtMappingFile.Text;
            Config.Instance.SaveSettings();
            CookieManager.ClearCookies();
            this.Close();
        }
    }
}
