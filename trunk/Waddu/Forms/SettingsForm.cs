using System;
using System.Windows.Forms;
using Waddu.Classes;
using Waddu.Properties;
using Waddu.Types;
using System.Drawing;

namespace Waddu.Forms
{
    public partial class SettingsForm : Form
    {
        public SettingsForm()
        {
            InitializeComponent();

            // Initialize Priority List
            lbPriority.AllowDrop = true;
            lbPriority.MouseDown += new MouseEventHandler(lbPriority_MouseDown);
            lbPriority.DragEnter += new DragEventHandler(lbPriority_DragEnter);
            lbPriority.DragDrop += new DragEventHandler(lbPriority_DragDrop);

            InitializeSettings();
            SetChecks();
        }

        private void lbPriority_MouseDown(object sender, MouseEventArgs e)
        {
            ListBox lb = sender as ListBox;
            if (lb.Items.Count == 0)
            {
                return;
            }

            int index = lb.IndexFromPoint(e.X, e.Y);
            object o = lb.Items[index];
            DragDropEffects dde1 = DoDragDrop(o, DragDropEffects.Move);
            if (dde1 == DragDropEffects.Move)
            {
                if (o == ((ListBox)sender).Items[index])
                {
                    ((ListBox)sender).Items.RemoveAt(index);
                }
                else
                {
                    ((ListBox)sender).Items.RemoveAt(index + 1);
                }
            }
        }

        private void lbPriority_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(typeof(AddonSiteId)))
                e.Effect = DragDropEffects.Move;
            else
                e.Effect = DragDropEffects.None;
        }

        private void lbPriority_DragDrop(object sender, DragEventArgs e)
        {
            ListBox lb = sender as ListBox;
            if (e.Data.GetDataPresent(typeof(AddonSiteId)))
            {
                object o = e.Data.GetData(typeof(AddonSiteId));
                int indexPos = lb.IndexFromPoint(lb.PointToClient(new Point(e.X, e.Y)));
                if (indexPos > -1)
                {
                    lb.Items.Insert(indexPos, o);
                }
                else
                {
                    lb.Items.Add(o);
                }
            }
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
            foreach (AddonSiteId addonSite in Config.Instance.AddonSites)
            {
                lbPriority.Items.Add(addonSite);
            }
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

            Config.Instance.AddonSites.Clear();
            foreach (AddonSiteId addonSite in lbPriority.Items)
            {
                Config.Instance.AddonSites.Add(addonSite);
            }

            Config.Instance.SaveSettings();
            CookieManager.ClearCookies();
            this.Close();
        }
    }
}
