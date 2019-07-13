using Microsoft.WindowsAzure.Storage.Blob;
using System.Windows.Forms;
using WinForms.Library;

namespace BlobManager.App.Classes
{
    public class BlobItem : ListViewItem
    {
        public BlobItem(CloudBlockBlob blob, ImageList imageList) : base(blob.Name)
        {
            Blob = blob;
            ImageKey = FileSystem.AddIcon(imageList, blob.Name, FileSystem.IconSize.Small);
            SubItems.AddRange(new ListViewSubItem[]
            {
                new ListViewSubItem(this, blob.Properties.LastModified.ToString()),
                new ListViewSubItem(this, FileSystem.GetFileType(blob.Name))
            });
        }

        public CloudBlockBlob Blob { get; }
    }
}
