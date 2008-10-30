using System;
using System.Windows.Forms;
using Waddu.Classes;
using Waddu.Properties;
using Waddu.Types;

namespace Waddu.Forms
{
    public partial class SettingsForm : Form
    {
        public SettingsForm()
        {
            InitializeComponent();
            InitializeSettings();
            SetChecks();
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
            chkNoLib.Checked = Config.Instance.PreferNoLib;
            txt7zPath.Text = Config.Instance.PathTo7z;
        }

        private void SetChecks()
        {
            if (chkDelete.Checked)
            {
                chkMoveToTrash.Enabled = true;
            }
            else
            {
                chkMoveToTrash.Enabled = false;
            }
        }

        private void chkDelete_CheckedChanged(object sender, EventArgs e)
        {
            SetChecks();
        }

        private void btnBrowseWoW_Click(object sender, EventArgs e)
        {
            txtWoWPath.Text = Helpers.BrowseForFolder(txtWoWPath.Text, FolderBrowseType.Enum.WoW);
        }

        private void btnBrowse7z_Click(object sender, EventArgs e)
        {
            txt7zPath.Text = Helpers.BrowseForFolder(txt7zPath.Text, FolderBrowseType.Enum.Folder7z);
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
            Config.Instance.PreferNoLib = chkNoLib.Checked;
            Config.Instance.PathTo7z = txt7zPath.Text;
            Config.Instance.SaveSettings();
            CookieManager.ClearCookies();
            this.Close();
        }
    }
}
