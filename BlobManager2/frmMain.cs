using BlobManager2.Models;
using BlobManager2.WinForms;
using JsonSettings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using WinForms.Library.Models;

namespace BlobManager2
{
    public partial class frmMain : Form
    {
        private Options _options = null;

        public frmMain()
        {
            InitializeComponent();
        }

        private void FrmMain_Load(object sender, EventArgs e)
        {
            try
            {
                _options = JsonSettingsBase.Load<Options>();
                _options.FormPosition?.Apply(this);
                FillAccounts();
            }
            catch (Exception exc)
            {
                MessageBox.Show(exc.Message);
            }
        }

        private void FillAccounts()
        {
            cbAccount.Items.Clear();
            if (!_options.Accounts?.Any() ?? true) return;
            cbAccount.Items.AddRange(_options.Accounts);
        }

        private void FrmMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                _options.FormPosition = FormPosition.FromForm(this);
                _options.Save();
            }
            catch (Exception exc)
            {
                MessageBox.Show(exc.Message);
            }
        }

        private void BtnAddAccount_Click(object sender, EventArgs e)
        {
            var dlg = new frmStorageAccount();
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                var list = _options.Accounts?.ToList() ?? new List<Options.StorageAccount>();
                list.Add(new Options.StorageAccount() { Name = dlg.AccountName, Key = dlg.AccountKey });
                _options.Accounts = list.ToArray();
                _options.Save();
                FillAccounts();
            }
        }
    }
}
