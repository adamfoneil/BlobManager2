using BlobManager.App.Interfaces;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace BlobManager.App.Classes
{
    public class ContainerNode : TreeNode, IBreadcrumbItem, IFolderNode
    {        
        public ContainerNode(string name) : base(name, new PlaceholderNode[] { new PlaceholderNode() })
        {
            Name = name;
            SelectedImageKey = "container.png";
            ImageKey = "container.png";
        }

        public new string Name { get; }

        public string Prefix => null;

        public string GetBreadcrumbText()
        {
            return Name;
        }

        public bool HasChildren() => IFolderHelper.HasFolders(this);

        public void LoadChildren(IEnumerable<IFolderNode> children)
        {
            
        }

        public void LoadChildren(IEnumerable<string> children)
        {
            throw new NotImplementedException();
        }
    }
}
