using System.Windows.Forms;
using static BlobManager2.Models.Options;

namespace BlobManager2.Classes
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
