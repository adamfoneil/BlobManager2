using Microsoft.WindowsAzure.Storage.Blob;
using System.Windows.Forms;

namespace BlobManager.App.Classes
{
    public class FolderItem : ListViewItem
    {        
        public FolderItem(CloudBlobDirectory directory) : base(directory.Prefix.Substring(0, directory.Prefix.Length - 1))
        {
            ImageKey = "folder.png";
            Directory = directory;
            SubItems.AddRange(new ListViewSubItem[] {
                new ListViewSubItem(), // spacer
                new ListViewSubItem(this, "File Folder")
            });
        }

        public CloudBlobDirectory Directory { get; }
    }
}
