using BlobManager.App.Classes;
using BlobManager.App.Models;
using BlobManager.App.Services;
using BlobManager.App.WinForms;
using JsonSettings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using WinForms.Library.Models;

namespace BlobManager.App
{
    public partial class frmMain : Form
    {
        private Options _options = null;

        public frmMain()
        {
            InitializeComponent();
        }

        private void FrmMain_Load(object sender, EventArgs e)
        {
            try
            {
                _options = JsonSettingsBase.Load<Options>();
                _options.FormPosition?.Apply(this);
                FillAccounts();
            }
            catch (Exception exc)
            {
                MessageBox.Show(exc.Message);
            }
        }

        private void FillAccounts()
        {
            tvwObjects.Nodes.Clear();            

            var root = tvwObjects.Nodes.Add("root", "Azure Storage Accounts");
            root.ImageKey = "accounts.png";
            root.SelectedImageKey = "accounts.png";

            int count = _options.Accounts?.Count ?? 0;
            string text = (count != 1) ? "accounts" : "account";
            tslAccountStatus.Text = $"{count} {text}";

            if (!_options.Accounts?.Any() ?? true) return;

            foreach (var a in _options.Accounts) root.Nodes.Add(new AccountNode(a));

            root.Expand();
        }

        private void FrmMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                _options.FormPosition = FormPosition.FromForm(this);
                _options.Save();
            }
            catch (Exception exc)
            {
                MessageBox.Show(exc.Message);
            }
        }

        private void AddAccountToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                var dlg = new frmStorageAccount();
                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    if (_options.Accounts == null) _options.Accounts = new HashSet<Options.StorageAccount>();
                    _options.Accounts.Add(dlg.SelectedAccount);
                    _options.Save();
                    FillAccounts();
                }
            }
            catch (Exception exc)
            {
                MessageBox.Show(exc.Message);
            }
        }

        private void TvwObjects_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
            {
                var accountNode = tvwObjects.SelectedNode as AccountNode;
                if (accountNode != null)
                {
                    if (MessageBox.Show($"This will remove the '{accountNode.Name}' account node.", "Remove Account", MessageBoxButtons.OKCancel) == DialogResult.OK)
                    {
                        var account = _options.GetAccount[accountNode.Name];
                        _options.Accounts.Remove(account);
                        FillAccounts();
                    }
                }
            }
        }

        private void CmAccounts_Opening(object sender, System.ComponentModel.CancelEventArgs e)
        {
            var accountNode = tvwObjects.SelectedNode as AccountNode;
            editDetailsToolStripMenuItem.Enabled = (accountNode != null);
        }

        private void EditDetailsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var accountNode = tvwObjects.SelectedNode as AccountNode;
            if (accountNode != null)
            {
                var dlg = new frmStorageAccount();
                dlg.SelectedAccount = accountNode.Account;
                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    _options.Accounts.RemoveWhere(row => row.Name.Equals(dlg.SelectedAccount.Name));
                    _options.Accounts.Add(dlg.SelectedAccount);
                    _options.Save();
                }
            }            
        }

        private async void TvwObjects_BeforeExpand(object sender, TreeViewCancelEventArgs e)
        {
            try
            {
                var accountNode = e.Node as AccountNode;
                if (!accountNode?.HasContainers() ?? false)
                {
                    await DoActionAsync(tslAccountStatus, "Loading containers...", async () =>
                    {
                        var service = new BlobService(accountNode.Account.Name, accountNode.Account.Key);
                        var containers = await service.ListContainersAsync();
                        tvwObjects.BeginUpdate();
                        accountNode.LoadContainers(containers);
                        tvwObjects.EndUpdate();
                    });                                        
                }
            }
            catch (Exception exc)
            {
                MessageBox.Show(exc.Message);            
            }
        }

        private async void TvwObjects_AfterSelect(object sender, TreeViewEventArgs e)
        {
            try
            {
                var containerNode = e.Node as ContainerNode;
                if (containerNode != null)
                {
                    await DoActionAsync(tslBlobStatus, $"Listing blobs in {containerNode.Name}...", async () =>
                    {
                        var account = (containerNode.Parent as AccountNode).Account;
                        var service = new BlobService(account.Name, account.Key);
                        var results = await service.ListBlobsAsync(containerNode.Name);
                        tslBlobStatus.Text = $"{results.Blobs.Count():n0} blobs, {results.Folders.Count():n0} folders";
                        lvBlobs.BeginUpdate();
                        lvBlobs.EndUpdate();
                    }, false);
                }
            }
            catch (Exception exc)
            {
                MessageBox.Show(exc.Message);
            }
        }

        private async Task DoActionAsync(ToolStripLabel label, string statusText, Func<Task> action, bool restoreStatusText = true)
        {
            string originalStatus = label.Text;
            label.Text = statusText;
            try
            {
                await action.Invoke();
            }
            finally
            {
                if (restoreStatusText)
                {
                    label.Text = originalStatus;
                }      
            }
        }
    }
}
