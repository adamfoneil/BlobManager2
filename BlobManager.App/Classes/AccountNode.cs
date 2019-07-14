using BlobManager.App.Interfaces;
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using static BlobManager.App.Models.Options;

namespace BlobManager.App.Classes
{
    public class AccountNode : TreeNode, IBreadcrumbItem
    {
        public AccountNode(StorageAccount account) : base(account.Name, new PlaceholderNode[] { new PlaceholderNode() })
        {
            Account = account;
            ImageKey = "account.png";
            SelectedImageKey = "account.png";            
        }

        public new string Name { get { return Account.Name; } }

        public StorageAccount Account { get; }

        public string GetBreadcrumbText()
        {
            return Name;
        }

        public bool HasContainers()
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

        public void LoadContainers(IEnumerable<string> containers)
        {
            RemovePlaceholder();            
            foreach (string name in containers) Nodes.Add(new ContainerNode(name));
        }

        private void RemovePlaceholder()
        {
            try
            {
                var placeholder = Nodes[0] as PlaceholderNode;
                if (placeholder != null) Nodes.Remove(placeholder);
            }
            catch 
            {
                // do nothing
            }
        }
    }
}
