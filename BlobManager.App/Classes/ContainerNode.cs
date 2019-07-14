using BlobManager.App.Interfaces;
using System.Windows.Forms;

namespace BlobManager.App.Classes
{
    public class ContainerNode : TreeNode, IBreadcrumbItem
    {        
        public ContainerNode(string name) : base(name)
        {
            Name = name;
            SelectedImageKey = "container.png";
            ImageKey = "container.png";
        }

        public new string Name { get; }

        public string GetBreadcrumbText()
        {
            return Name;
        }
    }
}
