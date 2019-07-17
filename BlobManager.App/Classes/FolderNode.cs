using BlobManager.App.Interfaces;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace BlobManager.App.Classes
{
    public class FolderNode : TreeNode, IFolderNode
    {
        public FolderNode(CloudBlobDirectory directory) : base(LastFolder(directory.Prefix), new PlaceholderNode[] { new PlaceholderNode() })
        {
            Directory = directory;
            Name = Path.GetFileName(directory.Prefix);
            Prefix = directory.Prefix;
            ImageKey = "folder.png";
            SelectedImageKey = "folder.png";
        }

        private static string LastFolder(string path)
        {
            return path.Split(new char[] { '/' }, StringSplitOptions.RemoveEmptyEntries).Last();
        }

        public CloudBlobDirectory Directory { get; }

        public new string Name { get; }

        public string Prefix { get; }

        public bool HasChildren() => IFolderHelper.HasFolders(this);

        public void LoadChildren(IEnumerable<IFolderNode> children)
        {
            IFolderHelper.RemovePlaceholder(this);
        }

        public void LoadChildren(IEnumerable<string> children)
        {
            throw new System.NotImplementedException();
        }
    }
}
