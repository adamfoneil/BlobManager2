using System.Windows.Forms;
using static BlobManager.App.Models.Options;

namespace BlobManager.App.Classes
{
    public class AccountNode : TreeNode
    {
        public AccountNode(StorageAccount account) : base(account.Name)
        {
            Account = account;
            ImageKey = "account.png";
            SelectedImageKey = "account.png";
        }

        public new string Name { get { return Account.Name; } }

        public StorageAccount Account { get; }
    }
}
