using System.Windows.Forms;

namespace BlobManager.App.Classes
{
    public class PlaceholderNode : TreeNode
    {
        public PlaceholderNode() : base("loading...")
        {
            ImageKey = "container.png";
            SelectedImageKey = "container.png";
        }
    }
}
