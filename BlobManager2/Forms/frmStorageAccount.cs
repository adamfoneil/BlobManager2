using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BlobManager2.WinForms
{
	public partial class frmStorageAccount : Form
	{
		public frmStorageAccount()
		{
			InitializeComponent();
		}

		public string AccountName { get { return tbName.Text; } }
		public string AccountKey { get { return tbKey.Text; } }

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