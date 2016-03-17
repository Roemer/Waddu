using System;
using System.Drawing;
using System.Windows.Forms;
using Waddu.Core;
using Waddu.Types;

namespace Waddu.UI.Forms
{
    public partial class SettingsForm : Form
    {
        public SettingsForm()
        {
            InitializeComponent();

            // Initialize Priority List
            lbPriority.AllowDrop = true;
            lbPriority.MouseDown += lbPriority_MouseDown;
            lbPriority.DragEnter += lbPriority_DragEnter;
            lbPriority.DragDrop += lbPriority_DragDrop;

            InitializeSettings();
            SetChecks();
        }

        private void lbPriority_MouseDown(object sender, MouseEventArgs e)
        {
            var lb = sender as ListBox;
            if (lb.Items.Count == 0)
            {
                return;
            }

            var index = lb.IndexFromPoint(e.X, e.Y);
            var o = lb.Items[index];
            var dde1 = DoDragDrop(o, DragDropEffects.Move);
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
            var lb = sender as ListBox;
            if (e.Data.GetDataPresent(typeof(AddonSiteId)))
            {
                var o = e.Data.GetData(typeof(AddonSiteId));
                var indexPos = lb.IndexFromPoint(lb.PointToClient(new Point(e.X, e.Y)));
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
            chkUseCustomMapping.Checked = Config.Instance.UseCustomMapping;
            txtMappingFile.Text = Config.Instance.MappingFile;
            chkNoLib.Checked = Config.Instance.PreferNoLib;
            chkUseOlderNoLib.Checked = Config.Instance.UseOlderNoLib;
            txt7zPath.Text = Config.Instance.PathTo7z;
            foreach (var addonSite in Config.Instance.AddonSites)
            {
                lbPriority.Items.Add(addonSite);
            }
            foreach (var addonName in Config.Instance.IgnoredAddons)
            {
                lvIgnored.Items.Add(addonName);
            }
            foreach (var kvp in Config.Instance.PreferredMappings)
            {
                var i = new ListViewItem(new[] { kvp.Key, kvp.Value.ToString() });
                lvPreferred.Items.Add(i);
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

            if (chkUseCustomMapping.Checked)
            {
                txtMappingFile.Enabled = true;
                btnBrowseMapping.Enabled = true;
            }
            else
            {
                txtMappingFile.Enabled = false;
                btnBrowseMapping.Enabled = false;
            }

            if (chkNoLib.Checked)
            {
                chkUseOlderNoLib.Enabled = true;
            }
            else
            {
                chkUseOlderNoLib.Enabled = false;
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

        private void chkUseCustomMapping_CheckedChanged(object sender, EventArgs e)
        {
            SetChecks();
        }

        private void btnBrowseMapping_Click(object sender, EventArgs e)
        {
            txtMappingFile.Text = Helpers.BrowseForFile(txtMappingFile.Text);
        }

        private void btnUnignore_Click(object sender, EventArgs e)
        {
            if (lvIgnored.SelectedItems.Count > 0)
            {
                lvIgnored.Items.Remove(lvIgnored.SelectedItems[0]);
            }
        }

        private void btnRemovePreferred_Click(object sender, EventArgs e)
        {
            if (lvPreferred.SelectedItems.Count > 0)
            {
                lvPreferred.Items.Remove(lvPreferred.SelectedItems[0]);
            }
        }

        private void chkNoLib_CheckedChanged(object sender, EventArgs e)
        {
            SetChecks();
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
            Config.Instance.UseCustomMapping = chkUseCustomMapping.Checked;
            Config.Instance.MappingFile = txtMappingFile.Text;
            Config.Instance.PreferNoLib = chkNoLib.Checked;
            Config.Instance.UseOlderNoLib = chkUseOlderNoLib.Checked;
            Config.Instance.PathTo7z = txt7zPath.Text;

            Config.Instance.AddonSites.Clear();
            foreach (AddonSiteId addonSite in lbPriority.Items)
            {
                Config.Instance.AddonSites.Add(addonSite);
            }

            Config.Instance.IgnoredAddons.Clear();
            foreach (ListViewItem item in lvIgnored.Items)
            {
                var addonName = item.Text;
                Config.Instance.AddIgnored(addonName);
            }

            Config.Instance.PreferredMappings.Clear();
            foreach (ListViewItem item in lvPreferred.Items)
            {
                var addonName = item.Text;
                var addonSiteId = (AddonSiteId)Enum.Parse(typeof(AddonSiteId), item.SubItems[1].Text);
                Config.Instance.SetPreferredMapping(addonName, addonSiteId);
            }

            foreach (var kvp in Config.Instance.PreferredMappings)
            {
                var i = new ListViewItem(new[] { kvp.Key, kvp.Value.ToString() });
                lvPreferred.Items.Add(i);
            }

            Config.Instance.SaveSettings();
            CookieManager.ClearCookies();
            Close();
        }
    }
}
