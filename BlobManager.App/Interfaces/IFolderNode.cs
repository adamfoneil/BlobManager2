using BlobManager.App.Classes;
using System.Collections.Generic;
using System.Windows.Forms;

namespace BlobManager.App.Interfaces
{
    /// <summary>
    /// Describes Container and Folder nodes in that they can be recursively expanded
    /// </summary>
    public interface IFolderNode
    {
        string Name { get; }
        string Prefix { get; }
        bool HasChildren();
        void LoadChildren(IEnumerable<string> children);
    }

    public static class IFolderHelper
    {
        public static bool HasFolders(TreeNode parentNode)
        {
            try
            {
                var child = parentNode.Nodes[0] as IFolderNode;
                return (child != null);
            }
            catch
            {
                return false;
            }
        }

        public static void RemovePlaceholder(TreeNode node)
        {
            try
            {
                var placeholder = node.Nodes[0] as PlaceholderNode;
                if (placeholder != null) node.Nodes.Remove(placeholder);
            }
            catch 
            {
                // do nothing
            }
        }
    }
}
