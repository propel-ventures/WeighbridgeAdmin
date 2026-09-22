namespace WeighbridgeAdmin.Forms
{
    partial class CustomerAccountForm
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            this.ucHeader = new WeighbridgeAdmin.Controls.CustomerHeaderPanel();
            this.tabAccount = new System.Windows.Forms.TabControl();
            this.tabContacts = new System.Windows.Forms.TabPage();
            this.ucContactSearch = new WeighbridgeAdmin.Controls.SearchBox();
            this.grdContacts = new System.Windows.Forms.DataGridView();
            this.colContactName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colPosition = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.colContactPhone = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colContactEmail = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colIsPrimary = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.lblContactHint = new System.Windows.Forms.Label();
            this.tabRates = new System.Windows.Forms.TabPage();
            this.ucRateSearch = new WeighbridgeAdmin.Controls.SearchBox();
            this.grdRates = new System.Windows.Forms.DataGridView();
            this.colRateProduct = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.colRate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colEffectiveFrom = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colEffectiveTo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colRateNotes = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.lblRateTotals = new System.Windows.Forms.Label();
            this.tabVehicles = new System.Windows.Forms.TabPage();
            this.grdVehicles = new System.Windows.Forms.DataGridView();
            this.colVehRegistration = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colVehDescription = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colVehTare = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colVehMaxGross = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colVehActive = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.lblVehicleHint = new System.Windows.Forms.Label();
            this.tabAccount.SuspendLayout();
            this.tabContacts.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdContacts)).BeginInit();
            this.tabRates.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdRates)).BeginInit();
            this.tabVehicles.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdVehicles)).BeginInit();
            this.SuspendLayout();
            //
            // ucHeader
            //
            this.ucHeader.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.ucHeader.Location = new System.Drawing.Point(12, 48);
            this.ucHeader.Name = "ucHeader";
            this.ucHeader.ReadOnlyHeader = false;
            this.ucHeader.Size = new System.Drawing.Size(812, 182);
            this.ucHeader.TabIndex = 3;
            this.ucHeader.HeaderChanged += new System.EventHandler(this.ucHeader_HeaderChanged);
            //
            // tabAccount
            //
            this.tabAccount.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabAccount.Controls.Add(this.tabContacts);
            this.tabAccount.Controls.Add(this.tabRates);
            this.tabAccount.Controls.Add(this.tabVehicles);
            this.tabAccount.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tabAccount.Location = new System.Drawing.Point(12, 238);
            this.tabAccount.Name = "tabAccount";
            this.tabAccount.SelectedIndex = 0;
            this.tabAccount.Size = new System.Drawing.Size(812, 314);
            this.tabAccount.TabIndex = 4;
            this.tabAccount.SelectedIndexChanged += new System.EventHandler(this.tabAccount_SelectedIndexChanged);
            //
            // tabContacts
            //
            this.tabContacts.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.tabContacts.Controls.Add(this.lblContactHint);
            this.tabContacts.Controls.Add(this.grdContacts);
            this.tabContacts.Controls.Add(this.ucContactSearch);
            this.tabContacts.Location = new System.Drawing.Point(4, 22);
            this.tabContacts.Name = "tabContacts";
            this.tabContacts.Padding = new System.Windows.Forms.Padding(3);
            this.tabContacts.Size = new System.Drawing.Size(804, 288);
            this.tabContacts.TabIndex = 0;
            this.tabContacts.Text = "C&ontacts";
            //
            // ucContactSearch
            //
            this.ucContactSearch.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.ucContactSearch.Caption = "Find contact";
            this.ucContactSearch.Location = new System.Drawing.Point(8, 8);
            this.ucContactSearch.Name = "ucContactSearch";
            this.ucContactSearch.Size = new System.Drawing.Size(440, 28);
            this.ucContactSearch.TabIndex = 0;
            this.ucContactSearch.SearchChanged += new System.EventHandler(this.ucContactSearch_SearchChanged);
            //
            // grdContacts
            //
            this.grdContacts.AllowUserToAddRows = true;
            this.grdContacts.AllowUserToDeleteRows = true;
            this.grdContacts.AllowUserToResizeRows = false;
            this.grdContacts.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grdContacts.BackgroundColor = System.Drawing.Color.White;
            this.grdContacts.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.grdContacts.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.grdContacts.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colContactName,
            this.colPosition,
            this.colContactPhone,
            this.colContactEmail,
            this.colIsPrimary});
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(247)))), ((int)(((byte)(247)))));
            this.grdContacts.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.grdContacts.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnKeystrokeOrF2;
            this.grdContacts.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.grdContacts.Location = new System.Drawing.Point(8, 42);
            this.grdContacts.MultiSelect = false;
            this.grdContacts.Name = "grdContacts";
            this.grdContacts.RowHeadersWidth = 24;
            this.grdContacts.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            this.grdContacts.Size = new System.Drawing.Size(788, 202);
            this.grdContacts.TabIndex = 1;
            this.grdContacts.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(this.grdContacts_CellEndEdit);
            this.grdContacts.CellValidating += new System.Windows.Forms.DataGridViewCellValidatingEventHandler(this.grdContacts_CellValidating);
            this.grdContacts.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.grdContacts_CellValueChanged);
            this.grdContacts.CurrentCellDirtyStateChanged += new System.EventHandler(this.grdContacts_CurrentCellDirtyStateChanged);
            this.grdContacts.DataError += new System.Windows.Forms.DataGridViewDataErrorEventHandler(this.Grid_DataError);
            this.grdContacts.DefaultValuesNeeded += new System.Windows.Forms.DataGridViewRowEventHandler(this.grdContacts_DefaultValuesNeeded);
            this.grdContacts.UserDeletingRow += new System.Windows.Forms.DataGridViewRowCancelEventHandler(this.grdContacts_UserDeletingRow);
            //
            // colContactName
            //
            this.colContactName.HeaderText = "Name";
            this.colContactName.MaxInputLength = 60;
            this.colContactName.Name = "colContactName";
            this.colContactName.Width = 170;
            //
            // colPosition
            //
            this.colPosition.HeaderText = "Position";
            this.colPosition.Items.AddRange(new object[] {
            "Accounts",
            "Director",
            "Dispatch",
            "Operations",
            "Owner",
            "Procurement",
            "Site Manager",
            "Weighbridge"});
            this.colPosition.Name = "colPosition";
            this.colPosition.Width = 120;
            //
            // colContactPhone
            //
            this.colContactPhone.HeaderText = "Phone";
            this.colContactPhone.MaxInputLength = 20;
            this.colContactPhone.Name = "colContactPhone";
            this.colContactPhone.Width = 130;
            //
            // colContactEmail
            //
            this.colContactEmail.HeaderText = "Email";
            this.colContactEmail.MaxInputLength = 80;
            this.colContactEmail.Name = "colContactEmail";
            this.colContactEmail.Width = 250;
            //
            // colIsPrimary
            //
            this.colIsPrimary.HeaderText = "Primary";
            this.colIsPrimary.Name = "colIsPrimary";
            this.colIsPrimary.Width = 60;
            //
            // lblContactHint
            //
            this.lblContactHint.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblContactHint.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblContactHint.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.lblContactHint.Location = new System.Drawing.Point(8, 252);
            this.lblContactHint.Name = "lblContactHint";
            this.lblContactHint.Size = new System.Drawing.Size(788, 26);
            this.lblContactHint.TabIndex = 2;
            this.lblContactHint.Text = "Type in the bottom row to add a contact.  Select a row and press Delete to remove " +
                "one.  Only one contact can be the primary.";
            //
            // tabRates
            //
            this.tabRates.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.tabRates.Controls.Add(this.lblRateTotals);
            this.tabRates.Controls.Add(this.grdRates);
            this.tabRates.Controls.Add(this.ucRateSearch);
            this.tabRates.Location = new System.Drawing.Point(4, 22);
            this.tabRates.Name = "tabRates";
            this.tabRates.Padding = new System.Windows.Forms.Padding(3);
            this.tabRates.Size = new System.Drawing.Size(804, 288);
            this.tabRates.TabIndex = 1;
            this.tabRates.Text = "Contract &Rates";
            //
            // ucRateSearch
            //
            this.ucRateSearch.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.ucRateSearch.Caption = "Find product";
            this.ucRateSearch.Location = new System.Drawing.Point(8, 8);
            this.ucRateSearch.Name = "ucRateSearch";
            this.ucRateSearch.Size = new System.Drawing.Size(440, 28);
            this.ucRateSearch.TabIndex = 0;
            this.ucRateSearch.SearchChanged += new System.EventHandler(this.ucRateSearch_SearchChanged);
            //
            // grdRates
            //
            this.grdRates.AllowUserToAddRows = true;
            this.grdRates.AllowUserToDeleteRows = true;
            this.grdRates.AllowUserToResizeRows = false;
            this.grdRates.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grdRates.BackgroundColor = System.Drawing.Color.White;
            this.grdRates.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.grdRates.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.grdRates.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colRateProduct,
            this.colRate,
            this.colEffectiveFrom,
            this.colEffectiveTo,
            this.colRateNotes});
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(247)))), ((int)(((byte)(247)))));
            this.grdRates.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle2;
            this.grdRates.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnKeystrokeOrF2;
            this.grdRates.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.grdRates.Location = new System.Drawing.Point(8, 42);
            this.grdRates.MultiSelect = false;
            this.grdRates.Name = "grdRates";
            this.grdRates.RowHeadersWidth = 24;
            this.grdRates.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            this.grdRates.Size = new System.Drawing.Size(788, 202);
            this.grdRates.TabIndex = 1;
            this.grdRates.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(this.grdRates_CellEndEdit);
            this.grdRates.CellValidating += new System.Windows.Forms.DataGridViewCellValidatingEventHandler(this.grdRates_CellValidating);
            this.grdRates.CurrentCellDirtyStateChanged += new System.EventHandler(this.grdRates_CurrentCellDirtyStateChanged);
            this.grdRates.DataError += new System.Windows.Forms.DataGridViewDataErrorEventHandler(this.Grid_DataError);
            this.grdRates.DefaultValuesNeeded += new System.Windows.Forms.DataGridViewRowEventHandler(this.grdRates_DefaultValuesNeeded);
            this.grdRates.EditingControlShowing += new System.Windows.Forms.DataGridViewEditingControlShowingEventHandler(this.grdRates_EditingControlShowing);
            this.grdRates.UserDeletingRow += new System.Windows.Forms.DataGridViewRowCancelEventHandler(this.grdRates_UserDeletingRow);
            //
            // colRateProduct
            //
            this.colRateProduct.HeaderText = "Product";
            this.colRateProduct.Name = "colRateProduct";
            this.colRateProduct.Width = 210;
            //
            // colRate
            //
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle3.Format = "N2";
            this.colRate.DefaultCellStyle = dataGridViewCellStyle3;
            this.colRate.HeaderText = "Rate / tonne";
            this.colRate.MaxInputLength = 10;
            this.colRate.Name = "colRate";
            this.colRate.Width = 90;
            //
            // colEffectiveFrom
            //
            this.colEffectiveFrom.HeaderText = "Effective from";
            this.colEffectiveFrom.MaxInputLength = 10;
            this.colEffectiveFrom.Name = "colEffectiveFrom";
            this.colEffectiveFrom.Width = 100;
            //
            // colEffectiveTo
            //
            this.colEffectiveTo.HeaderText = "Effective to";
            this.colEffectiveTo.MaxInputLength = 10;
            this.colEffectiveTo.Name = "colEffectiveTo";
            this.colEffectiveTo.Width = 100;
            //
            // colRateNotes
            //
            this.colRateNotes.HeaderText = "Notes";
            this.colRateNotes.MaxInputLength = 120;
            this.colRateNotes.Name = "colRateNotes";
            this.colRateNotes.Width = 230;
            //
            // lblRateTotals
            //
            this.lblRateTotals.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblRateTotals.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblRateTotals.Location = new System.Drawing.Point(8, 252);
            this.lblRateTotals.Name = "lblRateTotals";
            this.lblRateTotals.Size = new System.Drawing.Size(788, 26);
            this.lblRateTotals.TabIndex = 2;
            this.lblRateTotals.Text = "0 rate(s)";
            //
            // tabVehicles
            //
            this.tabVehicles.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.tabVehicles.Controls.Add(this.lblVehicleHint);
            this.tabVehicles.Controls.Add(this.grdVehicles);
            this.tabVehicles.Location = new System.Drawing.Point(4, 22);
            this.tabVehicles.Name = "tabVehicles";
            this.tabVehicles.Padding = new System.Windows.Forms.Padding(3);
            this.tabVehicles.Size = new System.Drawing.Size(804, 288);
            this.tabVehicles.TabIndex = 2;
            this.tabVehicles.Text = "&Vehicles";
            //
            // grdVehicles
            //
            this.grdVehicles.AllowUserToAddRows = false;
            this.grdVehicles.AllowUserToDeleteRows = false;
            this.grdVehicles.AllowUserToResizeRows = false;
            this.grdVehicles.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grdVehicles.BackgroundColor = System.Drawing.Color.White;
            this.grdVehicles.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.grdVehicles.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.grdVehicles.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colVehRegistration,
            this.colVehDescription,
            this.colVehTare,
            this.colVehMaxGross,
            this.colVehActive});
            this.grdVehicles.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.grdVehicles.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.grdVehicles.Location = new System.Drawing.Point(8, 8);
            this.grdVehicles.MultiSelect = false;
            this.grdVehicles.Name = "grdVehicles";
            this.grdVehicles.ReadOnly = true;
            this.grdVehicles.RowHeadersVisible = false;
            this.grdVehicles.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.grdVehicles.Size = new System.Drawing.Size(788, 236);
            this.grdVehicles.TabIndex = 0;
            //
            // colVehRegistration
            //
            this.colVehRegistration.HeaderText = "Registration";
            this.colVehRegistration.Name = "colVehRegistration";
            this.colVehRegistration.ReadOnly = true;
            this.colVehRegistration.Width = 110;
            //
            // colVehDescription
            //
            this.colVehDescription.HeaderText = "Description";
            this.colVehDescription.Name = "colVehDescription";
            this.colVehDescription.ReadOnly = true;
            this.colVehDescription.Width = 280;
            //
            // colVehTare
            //
            this.colVehTare.HeaderText = "Tare (kg)";
            this.colVehTare.Name = "colVehTare";
            this.colVehTare.ReadOnly = true;
            this.colVehTare.Width = 100;
            //
            // colVehMaxGross
            //
            this.colVehMaxGross.HeaderText = "Max gross (kg)";
            this.colVehMaxGross.Name = "colVehMaxGross";
            this.colVehMaxGross.ReadOnly = true;
            this.colVehMaxGross.Width = 110;
            //
            // colVehActive
            //
            this.colVehActive.HeaderText = "Active";
            this.colVehActive.Name = "colVehActive";
            this.colVehActive.ReadOnly = true;
            this.colVehActive.Width = 60;
            //
            // lblVehicleHint
            //
            this.lblVehicleHint.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblVehicleHint.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblVehicleHint.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.lblVehicleHint.Location = new System.Drawing.Point(8, 252);
            this.lblVehicleHint.Name = "lblVehicleHint";
            this.lblVehicleHint.Size = new System.Drawing.Size(788, 26);
            this.lblVehicleHint.TabIndex = 1;
            this.lblVehicleHint.Text = "Read only.  Vehicles are maintained against the vehicle master, not the customer a" +
                "ccount.";
            //
            // CustomerAccountForm
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(834, 600);
            this.Controls.Add(this.tabAccount);
            this.Controls.Add(this.ucHeader);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.MinimumSize = new System.Drawing.Size(700, 480);
            this.Name = "CustomerAccountForm";
            this.Text = "Customer Account";
            this.Controls.SetChildIndex(this.ucHeader, 0);
            this.Controls.SetChildIndex(this.tabAccount, 0);
            this.tabAccount.ResumeLayout(false);
            this.tabContacts.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grdContacts)).EndInit();
            this.tabRates.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grdRates)).EndInit();
            this.tabVehicles.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grdVehicles)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion

        private WeighbridgeAdmin.Controls.CustomerHeaderPanel ucHeader;
        private System.Windows.Forms.TabControl tabAccount;
        private System.Windows.Forms.TabPage tabContacts;
        private WeighbridgeAdmin.Controls.SearchBox ucContactSearch;
        private System.Windows.Forms.DataGridView grdContacts;
        private System.Windows.Forms.DataGridViewTextBoxColumn colContactName;
        private System.Windows.Forms.DataGridViewComboBoxColumn colPosition;
        private System.Windows.Forms.DataGridViewTextBoxColumn colContactPhone;
        private System.Windows.Forms.DataGridViewTextBoxColumn colContactEmail;
        private System.Windows.Forms.DataGridViewCheckBoxColumn colIsPrimary;
        private System.Windows.Forms.Label lblContactHint;
        private System.Windows.Forms.TabPage tabRates;
        private WeighbridgeAdmin.Controls.SearchBox ucRateSearch;
        private System.Windows.Forms.DataGridView grdRates;
        private System.Windows.Forms.DataGridViewComboBoxColumn colRateProduct;
        private System.Windows.Forms.DataGridViewTextBoxColumn colRate;
        private System.Windows.Forms.DataGridViewTextBoxColumn colEffectiveFrom;
        private System.Windows.Forms.DataGridViewTextBoxColumn colEffectiveTo;
        private System.Windows.Forms.DataGridViewTextBoxColumn colRateNotes;
        private System.Windows.Forms.Label lblRateTotals;
        private System.Windows.Forms.TabPage tabVehicles;
        private System.Windows.Forms.DataGridView grdVehicles;
        private System.Windows.Forms.DataGridViewTextBoxColumn colVehRegistration;
        private System.Windows.Forms.DataGridViewTextBoxColumn colVehDescription;
        private System.Windows.Forms.DataGridViewTextBoxColumn colVehTare;
        private System.Windows.Forms.DataGridViewTextBoxColumn colVehMaxGross;
        private System.Windows.Forms.DataGridViewCheckBoxColumn colVehActive;
        private System.Windows.Forms.Label lblVehicleHint;
    }
}
