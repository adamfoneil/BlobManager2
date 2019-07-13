namespace BlobManager.App
{
    partial class frmMain
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmMain));
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.tvwObjects = new System.Windows.Forms.TreeView();
            this.cmAccounts = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.addAccountToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.editDetailsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.imlSmallIcons = new System.Windows.Forms.ImageList(this.components);
            this.toolStrip2 = new System.Windows.Forms.ToolStrip();
            this.tbSearchContainers = new System.Windows.Forms.ToolStripTextBox();
            this.toolStripLabel2 = new System.Windows.Forms.ToolStripLabel();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.tslAccountStatus = new System.Windows.Forms.ToolStripStatusLabel();
            this.lvBlobs = new System.Windows.Forms.ListView();
            this.colHame = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colDateModified = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colType = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colSize = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colContentType = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.tbSearchBlobs = new System.Windows.Forms.ToolStripTextBox();
            this.toolStripLabel1 = new System.Windows.Forms.ToolStripLabel();
            this.statusStrip2 = new System.Windows.Forms.StatusStrip();
            this.tslBlobStatus = new System.Windows.Forms.ToolStripStatusLabel();
            this.tslCurrentPath = new System.Windows.Forms.ToolStripLabel();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.cmAccounts.SuspendLayout();
            this.toolStrip2.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.statusStrip2.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.tvwObjects);
            this.splitContainer1.Panel1.Controls.Add(this.toolStrip2);
            this.splitContainer1.Panel1.Controls.Add(this.statusStrip1);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.lvBlobs);
            this.splitContainer1.Panel2.Controls.Add(this.toolStrip1);
            this.splitContainer1.Panel2.Controls.Add(this.statusStrip2);
            this.splitContainer1.Size = new System.Drawing.Size(817, 294);
            this.splitContainer1.SplitterDistance = 271;
            this.splitContainer1.TabIndex = 2;
            // 
            // tvwObjects
            // 
            this.tvwObjects.ContextMenuStrip = this.cmAccounts;
            this.tvwObjects.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tvwObjects.FullRowSelect = true;
            this.tvwObjects.HideSelection = false;
            this.tvwObjects.ImageIndex = 0;
            this.tvwObjects.ImageList = this.imlSmallIcons;
            this.tvwObjects.Location = new System.Drawing.Point(0, 25);
            this.tvwObjects.Name = "tvwObjects";
            this.tvwObjects.SelectedImageIndex = 0;
            this.tvwObjects.ShowLines = false;
            this.tvwObjects.Size = new System.Drawing.Size(271, 247);
            this.tvwObjects.TabIndex = 0;
            this.tvwObjects.BeforeExpand += new System.Windows.Forms.TreeViewCancelEventHandler(this.TvwObjects_BeforeExpand);
            this.tvwObjects.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.TvwObjects_AfterSelect);
            this.tvwObjects.KeyDown += new System.Windows.Forms.KeyEventHandler(this.TvwObjects_KeyDown);
            // 
            // cmAccounts
            // 
            this.cmAccounts.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.addAccountToolStripMenuItem,
            this.editDetailsToolStripMenuItem});
            this.cmAccounts.Name = "cmAccounts";
            this.cmAccounts.Size = new System.Drawing.Size(154, 48);
            this.cmAccounts.Opening += new System.ComponentModel.CancelEventHandler(this.CmAccounts_Opening);
            // 
            // addAccountToolStripMenuItem
            // 
            this.addAccountToolStripMenuItem.Name = "addAccountToolStripMenuItem";
            this.addAccountToolStripMenuItem.Size = new System.Drawing.Size(153, 22);
            this.addAccountToolStripMenuItem.Text = "Add Account...";
            this.addAccountToolStripMenuItem.Click += new System.EventHandler(this.AddAccountToolStripMenuItem_Click);
            // 
            // editDetailsToolStripMenuItem
            // 
            this.editDetailsToolStripMenuItem.Name = "editDetailsToolStripMenuItem";
            this.editDetailsToolStripMenuItem.Size = new System.Drawing.Size(153, 22);
            this.editDetailsToolStripMenuItem.Text = "Edit Details...";
            this.editDetailsToolStripMenuItem.Click += new System.EventHandler(this.EditDetailsToolStripMenuItem_Click);
            // 
            // imlSmallIcons
            // 
            this.imlSmallIcons.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imlSmallIcons.ImageStream")));
            this.imlSmallIcons.TransparentColor = System.Drawing.Color.Transparent;
            this.imlSmallIcons.Images.SetKeyName(0, "account.png");
            this.imlSmallIcons.Images.SetKeyName(1, "accounts.png");
            this.imlSmallIcons.Images.SetKeyName(2, "container.png");
            this.imlSmallIcons.Images.SetKeyName(3, "folder.png");
            // 
            // toolStrip2
            // 
            this.toolStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tbSearchContainers,
            this.toolStripLabel2});
            this.toolStrip2.Location = new System.Drawing.Point(0, 0);
            this.toolStrip2.Name = "toolStrip2";
            this.toolStrip2.Size = new System.Drawing.Size(271, 25);
            this.toolStrip2.TabIndex = 2;
            this.toolStrip2.Text = "toolStrip2";
            // 
            // tbSearchContainers
            // 
            this.tbSearchContainers.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.tbSearchContainers.Name = "tbSearchContainers";
            this.tbSearchContainers.Size = new System.Drawing.Size(120, 25);
            this.tbSearchContainers.KeyDown += new System.Windows.Forms.KeyEventHandler(this.TbSearchContainers_KeyDown);
            // 
            // toolStripLabel2
            // 
            this.toolStripLabel2.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.toolStripLabel2.Name = "toolStripLabel2";
            this.toolStripLabel2.Size = new System.Drawing.Size(105, 22);
            this.toolStripLabel2.Text = "Search Containers:";
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tslAccountStatus});
            this.statusStrip1.Location = new System.Drawing.Point(0, 272);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(271, 22);
            this.statusStrip1.SizingGrip = false;
            this.statusStrip1.TabIndex = 1;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // tslAccountStatus
            // 
            this.tslAccountStatus.Name = "tslAccountStatus";
            this.tslAccountStatus.Size = new System.Drawing.Size(256, 17);
            this.tslAccountStatus.Spring = true;
            this.tslAccountStatus.Text = "0 accounts";
            this.tslAccountStatus.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lvBlobs
            // 
            this.lvBlobs.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.colHame,
            this.colDateModified,
            this.colType,
            this.colSize,
            this.colContentType});
            this.lvBlobs.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lvBlobs.Location = new System.Drawing.Point(0, 25);
            this.lvBlobs.Name = "lvBlobs";
            this.lvBlobs.Size = new System.Drawing.Size(542, 247);
            this.lvBlobs.SmallImageList = this.imlSmallIcons;
            this.lvBlobs.TabIndex = 1;
            this.lvBlobs.UseCompatibleStateImageBehavior = false;
            this.lvBlobs.View = System.Windows.Forms.View.Details;
            // 
            // colHame
            // 
            this.colHame.Text = "Name";
            this.colHame.Width = 150;
            // 
            // colDateModified
            // 
            this.colDateModified.Text = "Date Modified";
            this.colDateModified.Width = 105;
            // 
            // colType
            // 
            this.colType.Text = "Type";
            // 
            // colSize
            // 
            this.colSize.Text = "Size";
            this.colSize.Width = 83;
            // 
            // colContentType
            // 
            this.colContentType.Text = "Content Type";
            this.colContentType.Width = 123;
            // 
            // toolStrip1
            // 
            this.toolStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tbSearchBlobs,
            this.toolStripLabel1,
            this.tslCurrentPath});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(542, 25);
            this.toolStrip1.TabIndex = 2;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // tbSearchBlobs
            // 
            this.tbSearchBlobs.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.tbSearchBlobs.Name = "tbSearchBlobs";
            this.tbSearchBlobs.Size = new System.Drawing.Size(120, 25);
            // 
            // toolStripLabel1
            // 
            this.toolStripLabel1.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.toolStripLabel1.Name = "toolStripLabel1";
            this.toolStripLabel1.Size = new System.Drawing.Size(77, 22);
            this.toolStripLabel1.Text = "Search Blobs:";
            // 
            // statusStrip2
            // 
            this.statusStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tslBlobStatus});
            this.statusStrip2.Location = new System.Drawing.Point(0, 272);
            this.statusStrip2.Name = "statusStrip2";
            this.statusStrip2.Size = new System.Drawing.Size(542, 22);
            this.statusStrip2.TabIndex = 0;
            this.statusStrip2.Text = "statusStrip2";
            // 
            // tslBlobStatus
            // 
            this.tslBlobStatus.Name = "tslBlobStatus";
            this.tslBlobStatus.Size = new System.Drawing.Size(45, 17);
            this.tslBlobStatus.Text = "0 blobs";
            // 
            // tslCurrentPath
            // 
            this.tslCurrentPath.Name = "tslCurrentPath";
            this.tslCurrentPath.Size = new System.Drawing.Size(78, 22);
            this.tslCurrentPath.Text = "Selected Path";
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(817, 294);
            this.Controls.Add(this.splitContainer1);
            this.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.KeyPreview = true;
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "frmMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Helloblob";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FrmMain_FormClosing);
            this.Load += new System.EventHandler(this.FrmMain_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.FrmMain_KeyDown);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.PerformLayout();
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.cmAccounts.ResumeLayout(false);
            this.toolStrip2.ResumeLayout(false);
            this.toolStrip2.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.statusStrip2.ResumeLayout(false);
            this.statusStrip2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.TreeView tvwObjects;
        private System.Windows.Forms.ImageList imlSmallIcons;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel tslAccountStatus;
        private System.Windows.Forms.ContextMenuStrip cmAccounts;
        private System.Windows.Forms.ToolStripMenuItem addAccountToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem editDetailsToolStripMenuItem;
        private System.Windows.Forms.StatusStrip statusStrip2;
        private System.Windows.Forms.ToolStripStatusLabel tslBlobStatus;
        private System.Windows.Forms.ListView lvBlobs;
        private System.Windows.Forms.ColumnHeader colHame;
        private System.Windows.Forms.ColumnHeader colDateModified;
        private System.Windows.Forms.ColumnHeader colType;
        private System.Windows.Forms.ColumnHeader colSize;
        private System.Windows.Forms.ColumnHeader colContentType;
        private System.Windows.Forms.ToolStrip toolStrip2;
        private System.Windows.Forms.ToolStripTextBox tbSearchContainers;
        private System.Windows.Forms.ToolStripLabel toolStripLabel2;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripTextBox tbSearchBlobs;
        private System.Windows.Forms.ToolStripLabel toolStripLabel1;
        private System.Windows.Forms.ToolStripLabel tslCurrentPath;
    }
}

