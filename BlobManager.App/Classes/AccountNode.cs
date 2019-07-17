using BlobManager.App.Interfaces;
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using static BlobManager.App.Models.Options;

namespace BlobManager.App.Classes
{
    public class AccountNode : TreeNode, IBreadcrumbItem, IFolderNode
    {
        public AccountNode(StorageAccount account) : base(account.Name, new PlaceholderNode[] { new PlaceholderNode() })
        {
            Account = account;
            ImageKey = "account.png";
            SelectedImageKey = "account.png";            
        }

        public new string Name { get { return Account.Name; } }

        public StorageAccount Account { get; }

        public string Prefix => throw new NotImplementedException();

        public string GetBreadcrumbText()
        {
            return Name;
        }

        public bool HasChildren()
        {
            try
            {
                var containerNode = Nodes[0] as ContainerNode;
                return (containerNode != null);
            }
            catch
            {
                return false;
            }
        }

        public void LoadChildren(IEnumerable<string> children)
        {
            IFolderHelper.RemovePlaceholder(this);
            foreach (var child in children) Nodes.Add(new ContainerNode(child));
        }        
    }
}
