using BlobManager.App.Classes;
using BlobManager.App.Controls;
using BlobManager.App.Interfaces;
using BlobManager.App.Models;
using BlobManager.App.Services;
using BlobManager.App.WinForms;
using JsonSettings;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using WinForms.Library.Models;
using static BlobManager.App.Services.BlobService;

namespace BlobManager.App
{
    public partial class frmMain : Form
    {
        private Options _options = null;
        private ListViewColumnSizer _columnSizer;
        private readonly ToolStripBreadcrumb _breadcrumb;
        private readonly FileDropManager _dropHandler;

        static int searchFieldCycle = 0;

        public frmMain()
        {
            InitializeComponent();
            _breadcrumb = new ToolStripBreadcrumb(blobToolstrip);
            _dropHandler = new FileDropManager(lvBlobs, () => GetCurrentContainer() != null);
            _dropHandler.DropStarted += DropStarted;
            _dropHandler.FileHandling += UploadFileAsync;
            _dropHandler.DropCompleted += UploadComplete;
        }

        private void DropStarted(IEnumerable<string> fileNames)
        {
            pbUpload.Visible = true;
        }

        private async Task<ListViewItem> UploadFileAsync(FileHandlingEventArgs e)
        {
            BlobItem result = null;
            await DoActionAsync(tslBlobStatus, $"Uploading {Path.GetFileName(e.FullPath)}...", async () =>
            {
                var containerNode = GetCurrentContainer();
                var account = (containerNode.Parent as AccountNode).Account;
                var service = new BlobService(account.Name, account.Key);
                var blob = await service.UploadFileAsync(containerNode.Name, e.FullPath);
                result = new BlobItem(blob, imlSmallIcons);
            });
            return result;
        }

        private void UploadComplete(object sender, EventArgs e)
        {
            // update status bar blob and folder counts
            pbUpload.Visible = false;
        }

        private void FrmMain_Load(object sender, EventArgs e)
        {
            try
            {
                _columnSizer = new ListViewColumnSizer(lvBlobs);
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

            TreeNode root = AddRootNode();

            int count = _options.Accounts?.Count ?? 0;
            string text = (count != 1) ? "accounts" : "account";
            tslAccountStatus.Text = $"{count} {text}";

            if (!_options.Accounts?.Any() ?? true) return;

            foreach (var a in _options.Accounts) root.Nodes.Add(new AccountNode(a));

            root.Expand();
        }

        private TreeNode AddRootNode()
        {
            var root = tvwObjects.Nodes.Add("root", "Azure Storage Accounts");
            root.ImageKey = "accounts.png";
            root.SelectedImageKey = "accounts.png";
            return root;
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
                        _options.Save();
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
                if (!accountNode?.HasChildren() ?? false)
                {
                    await LoadContainersAsync(accountNode);
                }

                var containerNode = e.Node as ContainerNode;
                if (!containerNode?.HasChildren() ?? false)
                {
                    await LoadFoldersAsync(containerNode);
                }

                var folderNode = e.Node as FolderNode;
                if (!folderNode?.HasChildren() ?? false)
                {
                    await LoadFoldersAsync(folderNode);
                }
            }
            catch (Exception exc)
            {
                MessageBox.Show(exc.Message);            
            }
        }

        private async Task LoadFoldersAsync<T>(T folderNode) where T : TreeNode, IFolderNode
        {
            await DoActionAsync(tslAccountStatus, "Finding directories...", async () =>
            {
                var account = CurrentAccount;
                string container = CurrentContainer.Name;
                var service = new BlobService(account.Name, account.Key);
                var folders = await service.ListDirectoriesAsync(container, folderNode.Prefix);
                tvwObjects.BeginUpdate();
                IFolderHelper.RemovePlaceholder(folderNode);
                foreach (var dir in folders) folderNode.Nodes.Add(new FolderNode(dir));
                tvwObjects.EndUpdate();
            });
        }

        private async Task LoadContainersAsync(AccountNode accountNode)
        {
            await DoActionAsync(tslAccountStatus, "Loading containers...", async () =>
            {
                var service = new BlobService(accountNode.Account.Name, accountNode.Account.Key);
                var containers = await service.ListContainersAsync();
                tvwObjects.BeginUpdate();
                accountNode.LoadChildren(containers);
                tvwObjects.EndUpdate();
            });
        }

        private async void TvwObjects_AfterSelect(object sender, TreeViewEventArgs e)
        {
            try
            {
                var containerNode = e.Node as ContainerNode;
                if (containerNode != null)
                {
                    await LoadBlobListViewAsync(containerNode);
                }
            }
            catch (Exception exc)
            {
                MessageBox.Show(exc.Message);
            }
        }

        private async Task LoadBlobListViewAsync(ContainerNode containerNode, string directory = null)
        {
            if (containerNode == null) throw new NullReferenceException("No container selected.");

            var accountNode = containerNode.Parent as AccountNode;

            _breadcrumb.Clear();
            _breadcrumb.Add(accountNode);
            _breadcrumb.Add(containerNode, async (node) => await LoadBlobListViewAsync(node));
            if (!string.IsNullOrEmpty(directory))
            {
                _breadcrumb.AddPath(directory, async (folder) => await LoadBlobListViewAsync(containerNode, folder.FullPath));
            }
            
            await DoActionAsync(tslBlobStatus, $"Listing blobs in {containerNode.Name}...", async () =>
            {                
                var service = new BlobService(accountNode.Name, accountNode.Account.Key);
                var results = await service.ListBlobsAndDirectoriesAsync(containerNode.Name, directory);
                tslBlobStatus.Text = $"{results.Blobs.Count():n0} blobs, {results.Directories.Count():n0} folders";
                lvBlobs.BeginUpdate();
                lvBlobs.Items.Clear();
                lvBlobs.Items.AddRange(results.Directories.Select(folder => new FolderItem(folder)).ToArray());
                lvBlobs.Items.AddRange(results.Blobs.Select(blob => new BlobItem(blob, imlSmallIcons)).ToArray());
                lvBlobs.EndUpdate();
            }, false);
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

        private async void TbSearchContainers_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                string searchText = tbSearchContainers.Text;
                if (e.KeyCode == Keys.Enter)
                {
                    if (!string.IsNullOrEmpty(searchText))
                    {
                        await DoActionAsync(tslAccountStatus, "Searching containers...", async () =>
                        {
                            tvwObjects.Nodes.Clear();
                            var root = AddRootNode();
                            foreach (var acc in _options.Accounts)
                            {
                                var service = new BlobService(acc.Name, acc.Key);
                                var containers = await service.ListContainersAsync(searchText);
                                if (containers.Any())
                                {
                                    var accountNode = new AccountNode(acc);
                                    root.Nodes.Add(accountNode);
                                    tvwObjects.BeginUpdate();
                                    accountNode.LoadChildren(containers);
                                    tvwObjects.EndUpdate();
                                    accountNode.Expand();
                                }
                            }
                            root.Expand();
                        });
                    }
                    else
                    {
                        FillAccounts();
                    }
                }
                
            }
            catch (Exception exc)
            {
                MessageBox.Show(exc.Message);
            }
        }

        private void FrmMain_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F3)
            {
                searchFieldCycle++;
                var fields = new Dictionary<int, ToolStripTextBox>()
                {
                    { 0, tbSearchContainers },
                    { 1, tbSearchBlobs }
                };

                fields[searchFieldCycle % 2].Focus();
            }
        }

        private async void LvBlobs_DoubleClick(object sender, EventArgs e)
        {
            try
            {
                var folder = GetSelectedFolder();
                var container = GetCurrentContainer();
                if (folder != null && container != null)
                {
                    await LoadBlobListViewAsync(container, folder.Directory.Prefix);
                }
            }
            catch (Exception exc)
            {
                MessageBox.Show(exc.Message);
            }
        }

        private FolderItem GetSelectedFolder()
        {
            try
            {
                return lvBlobs.SelectedItems[0] as FolderItem;
            }
            catch 
            {
                return null;
            }
        }

        private ContainerNode GetCurrentContainer()
        {
            try
            {
                return tvwObjects.SelectedNode as ContainerNode;
            }
            catch
            {
                return null;
            }
        }

        private void LvBlobs_DragDrop(object sender, DragEventArgs e)
        {
            
        }

        private void LvBlobs_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.C)
            {
                //Clipboard.SetDataObject()
            }            
        }

        private async void TbSearchBlobs_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                string searchText = tbSearchBlobs.Text;
                if (e.KeyCode == Keys.Enter)
                {
                    if (!string.IsNullOrEmpty(searchText))
                    {
                        lvBlobs.Items.Clear();
                        var accountNodes = GetAccountNodes();
                        foreach (var accountNode in accountNodes)
                        {
                            var service = new BlobService(accountNode.Account.Name, accountNode.Account.Key);
                            var containers = await GetContainersAsync(accountNode);                            
                            foreach (var container in containers)
                            {                                
                                Progress<SearchProgress> progress = new Progress<SearchProgress>(ReportProgress);

                                var results = await service.SearchBlobsAsync(container, searchText, progress);
                                lvBlobs.BeginUpdate();
                                lvBlobs.Items.AddRange(results.Select(blob => new BlobItem(blob, imlSmallIcons, showPath: true)).ToArray());
                                lvBlobs.EndUpdate();
                                tslBlobStatus.Text = $"{lvBlobs.Items.Count:n0} blobs";
                            }
                        }
                        _breadcrumb.Clear();
                    }
                    else
                    {
                        await LoadBlobListViewAsync(CurrentContainer);
                    }
                }
            }
            catch (Exception exc)
            {
                MessageBox.Show(exc.Message);
            }
        }

        private void ReportProgress(SearchProgress obj)
        {
            tslBlobStatus.Text = $"Searching {obj.AccountName}.{obj.ContainerName} {obj.Path}...";
        }

        private IEnumerable<AccountNode> GetAccountNodes()
        {
            var accountNode = tvwObjects.SelectedNode as AccountNode;
            if (accountNode != null) return new AccountNode[] { accountNode };

            var containerNode = CurrentContainer;
            if (containerNode != null) return new AccountNode[] { containerNode.Parent as AccountNode };

            return tvwObjects.Nodes[0].Nodes.OfType<AccountNode>();
        }

        private async Task<IEnumerable<string>> GetContainersAsync(AccountNode accountNode)
        {            
            if (!accountNode.HasChildren()) await LoadContainersAsync(accountNode);
            return accountNode.Nodes.OfType<ContainerNode>().Select(node => node.Name);            
        }

        public Options.StorageAccount CurrentAccount
        {
            get
            {
                var containerNode = tvwObjects.SelectedNode as ContainerNode;
                if (containerNode != null) return (containerNode.Parent as AccountNode).Account;

                var accountNode = tvwObjects.SelectedNode as AccountNode;
                if (accountNode != null) return accountNode.Account;

                var folderNode = tvwObjects.SelectedNode as FolderNode;
                if (folderNode != null) return FindParentNode<AccountNode>(folderNode).Account;

                throw new NullReferenceException("No current account selected.");
            }
        }

        private T FindParentNode<T>(TreeNode node) where T : TreeNode
        {
            TreeNode result = node.Parent;

            var check = result as T;
            while (check == null)
            {
                result = result.Parent;
                check = result as T;
            }

            return check;
        }

        public ContainerNode CurrentContainer
        {
            get
            {
                var node = tvwObjects.SelectedNode as ContainerNode;
                if (node != null) return node;

                var folderNode = tvwObjects.SelectedNode as FolderNode;
                if (folderNode != null) return FindParentNode<ContainerNode>(folderNode);
                
                throw new NullReferenceException("No current container selected.");
            }
        }        
    }
}
