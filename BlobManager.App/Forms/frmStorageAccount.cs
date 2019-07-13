using BlobManager.App.Models;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BlobManager.App.WinForms
{
    public partial class frmStorageAccount : Form
	{
		public frmStorageAccount()
		{
			InitializeComponent();
		}

        public Options.StorageAccount SelectedAccount
        {
            set { tbName.Text = value.Name; tbKey.Text = value.Key; }
            get { return new Options.StorageAccount() { Name = tbName.Text, Key = tbKey.Text }; }            
        }

		private async void btnOK_Click(object sender, EventArgs e)
		{
			try
			{
				await ValidateConnectionAsync(tbName.Text, tbKey.Text);				
				DialogResult = DialogResult.OK;				
			}
			catch (Exception exc)
			{
				MessageBox.Show(exc.Message);
			}
		}

		private async Task ValidateConnectionAsync(string accountName, string accountKey)
		{			
			var account = new CloudStorageAccount(new StorageCredentials(accountName, accountKey), true);
			var client = account.CreateCloudBlobClient();
			var token = new BlobContinuationToken();
			var containers = await client.ListContainersSegmentedAsync(token);			
		}
	}
}