namespace WeighbridgeAdmin.Forms
{
    partial class CustomerListForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CustomerListForm));
            this.pnlHeader = new System.Windows.Forms.Panel();
            this.lblHeaderSub = new System.Windows.Forms.Label();
            this.lblHeader = new System.Windows.Forms.Label();
            this.tlbMain = new System.Windows.Forms.ToolStrip();
            this.tbbNew = new System.Windows.Forms.ToolStripButton();
            this.tbbEdit = new System.Windows.Forms.ToolStripButton();
            this.tbbDelete = new System.Windows.Forms.ToolStripButton();
            this.tbbSep0 = new System.Windows.Forms.ToolStripSeparator();
            this.tbbAccount = new System.Windows.Forms.ToolStripButton();
            this.tbbSep1 = new System.Windows.Forms.ToolStripSeparator();
            this.tbbRefresh = new System.Windows.Forms.ToolStripButton();
            this.pnlSearch = new System.Windows.Forms.Panel();
            this.lblRecordCount = new System.Windows.Forms.Label();
            this.txtSearch = new System.Windows.Forms.TextBox();
            this.lblSearch = new System.Windows.Forms.Label();
            this.grdCustomers = new DevExpress.XtraGrid.GridControl();
            this.gvCustomers = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.colCode = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colName = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colSuburb = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colState = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colPostcode = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colPhone = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colCreditLimit = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colIsActive = new DevExpress.XtraGrid.Columns.GridColumn();
            this.riIsActive = new DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit();
            this.bsCustomers = new System.Windows.Forms.BindingSource(this.components);
            this.pnlHeader.SuspendLayout();
            this.tlbMain.SuspendLayout();
            this.pnlSearch.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdCustomers)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvCustomers)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.riIsActive)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bsCustomers)).BeginInit();
            this.SuspendLayout();
            //
            // pnlHeader
            //
            this.pnlHeader.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.pnlHeader.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlHeader.Controls.Add(this.lblHeaderSub);
            this.pnlHeader.Controls.Add(this.lblHeader);
            this.pnlHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlHeader.Location = new System.Drawing.Point(0, 0);
            this.pnlHeader.Name = "pnlHeader";
            this.pnlHeader.Size = new System.Drawing.Size(784, 44);
            this.pnlHeader.TabIndex = 0;
            //
            // lblHeaderSub
            //
            this.lblHeaderSub.AutoSize = true;
            this.lblHeaderSub.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblHeaderSub.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.lblHeaderSub.Location = new System.Drawing.Point(14, 25);
            this.lblHeaderSub.Name = "lblHeaderSub";
            this.lblHeaderSub.Size = new System.Drawing.Size(287, 13);
            this.lblHeaderSub.TabIndex = 1;
            this.lblHeaderSub.Text = resources.GetString("lblHeaderSub.Text");
            //
            // lblHeader
            //
            this.lblHeader.AutoSize = true;
            this.lblHeader.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblHeader.ForeColor = System.Drawing.Color.Navy;
            this.lblHeader.Location = new System.Drawing.Point(12, 6);
            this.lblHeader.Name = "lblHeader";
            this.lblHeader.Size = new System.Drawing.Size(178, 18);
            this.lblHeader.TabIndex = 0;
            this.lblHeader.Text = resources.GetString("lblHeader.Text");
            //
            // tlbMain
            //
            this.tlbMain.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.tlbMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tbbNew,
            this.tbbEdit,
            this.tbbDelete,
            this.tbbSep0,
            this.tbbAccount,
            this.tbbSep1,
            this.tbbRefresh});
            this.tlbMain.Location = new System.Drawing.Point(0, 44);
            this.tlbMain.Name = "tlbMain";
            this.tlbMain.Size = new System.Drawing.Size(784, 25);
            this.tlbMain.TabIndex = 1;
            this.tlbMain.Text = "toolStrip1";
            //
            // tbbNew
            //
            this.tbbNew.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.tbbNew.Name = "tbbNew";
            this.tbbNew.Size = new System.Drawing.Size(35, 22);
            this.tbbNew.Text = "&New";
            this.tbbNew.ToolTipText = "Create a new customer (Ins)";
            this.tbbNew.Click += new System.EventHandler(this.tbbNew_Click);
            //
            // tbbEdit
            //
            this.tbbEdit.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.tbbEdit.Name = "tbbEdit";
            this.tbbEdit.Size = new System.Drawing.Size(31, 22);
            this.tbbEdit.Text = "&Edit";
            this.tbbEdit.ToolTipText = "Edit the selected customer (F2)";
            this.tbbEdit.Click += new System.EventHandler(this.tbbEdit_Click);
            //
            // tbbDelete
            //
            this.tbbDelete.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.tbbDelete.Name = "tbbDelete";
            this.tbbDelete.Size = new System.Drawing.Size(44, 22);
            this.tbbDelete.Text = "&Delete";
            this.tbbDelete.ToolTipText = "Delete the selected customer";
            this.tbbDelete.Click += new System.EventHandler(this.tbbDelete_Click);
            //
            // tbbSep0
            //
            this.tbbSep0.Name = "tbbSep0";
            this.tbbSep0.Size = new System.Drawing.Size(6, 25);
            //
            // tbbAccount
            //
            this.tbbAccount.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.tbbAccount.Name = "tbbAccount";
            this.tbbAccount.Size = new System.Drawing.Size(58, 22);
            this.tbbAccount.Text = "&Account";
            this.tbbAccount.ToolTipText = "Open the full account screen for the selected customer (F6)";
            this.tbbAccount.Click += new System.EventHandler(this.tbbAccount_Click);
            //
            // tbbSep1
            //
            this.tbbSep1.Name = "tbbSep1";
            this.tbbSep1.Size = new System.Drawing.Size(6, 25);
            //
            // tbbRefresh
            //
            this.tbbRefresh.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.tbbRefresh.Name = "tbbRefresh";
            this.tbbRefresh.Size = new System.Drawing.Size(50, 22);
            this.tbbRefresh.Text = "&Refresh";
            this.tbbRefresh.ToolTipText = "Re-read the customer list (F5)";
            this.tbbRefresh.Click += new System.EventHandler(this.tbbRefresh_Click);
            //
            // pnlSearch
            //
            this.pnlSearch.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.pnlSearch.Controls.Add(this.lblRecordCount);
            this.pnlSearch.Controls.Add(this.txtSearch);
            this.pnlSearch.Controls.Add(this.lblSearch);
            this.pnlSearch.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlSearch.Location = new System.Drawing.Point(0, 69);
            this.pnlSearch.Name = "pnlSearch";
            this.pnlSearch.Size = new System.Drawing.Size(784, 32);
            this.pnlSearch.TabIndex = 2;
            //
            // lblRecordCount
            //
            this.lblRecordCount.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblRecordCount.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblRecordCount.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.lblRecordCount.Location = new System.Drawing.Point(604, 9);
            this.lblRecordCount.Name = "lblRecordCount";
            this.lblRecordCount.Size = new System.Drawing.Size(168, 16);
            this.lblRecordCount.TabIndex = 2;
            this.lblRecordCount.Text = "0 record(s)";
            this.lblRecordCount.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            //
            // txtSearch
            //
            this.txtSearch.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtSearch.Location = new System.Drawing.Point(108, 6);
            this.txtSearch.MaxLength = 40;
            this.txtSearch.Name = "txtSearch";
            this.txtSearch.Size = new System.Drawing.Size(240, 20);
            this.txtSearch.TabIndex = 1;
            this.txtSearch.TextChanged += new System.EventHandler(this.txtSearch_TextChanged);
            //
            // lblSearch
            //
            this.lblSearch.AutoSize = true;
            this.lblSearch.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSearch.Location = new System.Drawing.Point(12, 10);
            this.lblSearch.Name = "lblSearch";
            this.lblSearch.Size = new System.Drawing.Size(93, 13);
            this.lblSearch.TabIndex = 0;
            this.lblSearch.Text = "Search Code/Name";
            //
            // grdCustomers
            //
            this.grdCustomers.DataSource = this.bsCustomers;
            this.grdCustomers.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grdCustomers.Location = new System.Drawing.Point(0, 101);
            this.grdCustomers.MainView = this.gvCustomers;
            this.grdCustomers.Name = "grdCustomers";
            this.grdCustomers.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.riIsActive});
            this.grdCustomers.Size = new System.Drawing.Size(784, 460);
            this.grdCustomers.TabIndex = 3;
            this.grdCustomers.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gvCustomers});
            //
            // gvCustomers
            //
            this.gvCustomers.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.colCode,
            this.colName,
            this.colSuburb,
            this.colState,
            this.colPostcode,
            this.colPhone,
            this.colCreditLimit,
            this.colIsActive});
            this.gvCustomers.GridControl = this.grdCustomers;
            this.gvCustomers.Name = "gvCustomers";
            this.gvCustomers.OptionsBehavior.ReadOnly = true;
            this.gvCustomers.OptionsView.ColumnAutoWidth = false;
            this.gvCustomers.OptionsView.EnableAppearanceEvenRow = true;
            this.gvCustomers.OptionsView.ShowAutoFilterRow = true;
            this.gvCustomers.OptionsView.ShowGroupPanel = true;
            this.gvCustomers.DoubleClick += new System.EventHandler(this.gvCustomers_DoubleClick);
            //
            // colCode
            //
            this.colCode.Caption = "Code";
            this.colCode.FieldName = "Code";
            this.colCode.Name = "colCode";
            this.colCode.OptionsColumn.AllowEdit = false;
            this.colCode.OptionsColumn.ReadOnly = true;
            this.colCode.Visible = true;
            this.colCode.VisibleIndex = 0;
            this.colCode.Width = 75;
            //
            // colName
            //
            this.colName.Caption = "Customer Name";
            this.colName.FieldName = "Name";
            this.colName.Name = "colName";
            this.colName.OptionsColumn.AllowEdit = false;
            this.colName.OptionsColumn.ReadOnly = true;
            this.colName.Visible = true;
            this.colName.VisibleIndex = 1;
            this.colName.Width = 230;
            //
            // colSuburb
            //
            this.colSuburb.Caption = "Suburb";
            this.colSuburb.FieldName = "Suburb";
            this.colSuburb.Name = "colSuburb";
            this.colSuburb.OptionsColumn.AllowEdit = false;
            this.colSuburb.OptionsColumn.ReadOnly = true;
            this.colSuburb.Visible = true;
            this.colSuburb.VisibleIndex = 2;
            this.colSuburb.Width = 120;
            //
            // colState
            //
            this.colState.Caption = "St";
            this.colState.FieldName = "State";
            this.colState.Name = "colState";
            this.colState.OptionsColumn.AllowEdit = false;
            this.colState.OptionsColumn.ReadOnly = true;
            this.colState.Visible = true;
            this.colState.VisibleIndex = 3;
            this.colState.Width = 40;
            //
            // colPostcode
            //
            this.colPostcode.Caption = "P/Code";
            this.colPostcode.FieldName = "Postcode";
            this.colPostcode.Name = "colPostcode";
            this.colPostcode.OptionsColumn.AllowEdit = false;
            this.colPostcode.OptionsColumn.ReadOnly = true;
            this.colPostcode.Visible = true;
            this.colPostcode.VisibleIndex = 4;
            this.colPostcode.Width = 55;
            //
            // colPhone
            //
            this.colPhone.Caption = "Phone";
            this.colPhone.FieldName = "Phone";
            this.colPhone.Name = "colPhone";
            this.colPhone.OptionsColumn.AllowEdit = false;
            this.colPhone.OptionsColumn.ReadOnly = true;
            this.colPhone.Visible = true;
            this.colPhone.VisibleIndex = 5;
            this.colPhone.Width = 105;
            //
            // colCreditLimit
            //
            this.colCreditLimit.AppearanceCell.Options.UseTextOptions = true;
            this.colCreditLimit.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.colCreditLimit.Caption = "Credit Limit";
            this.colCreditLimit.DisplayFormat.FormatString = "n2";
            this.colCreditLimit.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.colCreditLimit.FieldName = "CreditLimit";
            this.colCreditLimit.Name = "colCreditLimit";
            this.colCreditLimit.OptionsColumn.AllowEdit = false;
            this.colCreditLimit.OptionsColumn.ReadOnly = true;
            this.colCreditLimit.Visible = true;
            this.colCreditLimit.VisibleIndex = 6;
            this.colCreditLimit.Width = 85;
            //
            // colIsActive
            //
            this.colIsActive.Caption = "Active";
            this.colIsActive.ColumnEdit = this.riIsActive;
            this.colIsActive.FieldName = "IsActive";
            this.colIsActive.Name = "colIsActive";
            this.colIsActive.OptionsColumn.AllowEdit = false;
            this.colIsActive.OptionsColumn.ReadOnly = true;
            this.colIsActive.Visible = true;
            this.colIsActive.VisibleIndex = 7;
            this.colIsActive.Width = 45;
            //
            // riIsActive
            //
            this.riIsActive.AutoHeight = false;
            this.riIsActive.Name = "riIsActive";
            //
            // bsCustomers
            //
            this.bsCustomers.DataSource = typeof(WeighbridgeAdmin.Model.Customer);
            //
            // CustomerListForm
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.ClientSize = new System.Drawing.Size(784, 561);
            this.Controls.Add(this.grdCustomers);
            this.Controls.Add(this.pnlSearch);
            this.Controls.Add(this.tlbMain);
            this.Controls.Add(this.pnlHeader);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Sizable;
            this.KeyPreview = true;
            this.MinimumSize = new System.Drawing.Size(640, 400);
            this.Name = "CustomerListForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.WindowsDefaultLocation;
            this.Text = resources.GetString("$this.Text");
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.CustomerListForm_KeyDown);
            this.Load += new System.EventHandler(this.CustomerListForm_Load);
            this.pnlHeader.ResumeLayout(false);
            this.pnlHeader.PerformLayout();
            this.tlbMain.ResumeLayout(false);
            this.tlbMain.PerformLayout();
            this.pnlSearch.ResumeLayout(false);
            this.pnlSearch.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdCustomers)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvCustomers)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.riIsActive)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bsCustomers)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion

        private System.Windows.Forms.Panel pnlHeader;
        private System.Windows.Forms.Label lblHeader;
        private System.Windows.Forms.Label lblHeaderSub;
        private System.Windows.Forms.ToolStrip tlbMain;
        private System.Windows.Forms.ToolStripButton tbbNew;
        private System.Windows.Forms.ToolStripButton tbbEdit;
        private System.Windows.Forms.ToolStripButton tbbDelete;
        private System.Windows.Forms.ToolStripSeparator tbbSep0;
        private System.Windows.Forms.ToolStripButton tbbAccount;
        private System.Windows.Forms.ToolStripSeparator tbbSep1;
        private System.Windows.Forms.ToolStripButton tbbRefresh;
        private System.Windows.Forms.Panel pnlSearch;
        private System.Windows.Forms.Label lblSearch;
        private System.Windows.Forms.TextBox txtSearch;
        private System.Windows.Forms.Label lblRecordCount;
        private DevExpress.XtraGrid.GridControl grdCustomers;
        private DevExpress.XtraGrid.Views.Grid.GridView gvCustomers;
        private DevExpress.XtraGrid.Columns.GridColumn colCode;
        private DevExpress.XtraGrid.Columns.GridColumn colName;
        private DevExpress.XtraGrid.Columns.GridColumn colSuburb;
        private DevExpress.XtraGrid.Columns.GridColumn colState;
        private DevExpress.XtraGrid.Columns.GridColumn colPostcode;
        private DevExpress.XtraGrid.Columns.GridColumn colPhone;
        private DevExpress.XtraGrid.Columns.GridColumn colCreditLimit;
        private DevExpress.XtraGrid.Columns.GridColumn colIsActive;
        private DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit riIsActive;
        private System.Windows.Forms.BindingSource bsCustomers;
    }
}
