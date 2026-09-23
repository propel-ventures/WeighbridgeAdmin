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
            this.ucHeader = new WeighbridgeAdmin.Controls.CustomerHeaderPanel();
            this.tabAccount = new System.Windows.Forms.TabControl();
            this.tabContacts = new System.Windows.Forms.TabPage();
            this.ucContactSearch = new WeighbridgeAdmin.Controls.SearchBox();
            this.grdContacts = new DevExpress.XtraGrid.GridControl();
            this.gvContacts = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.colContactName = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colPosition = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colContactPhone = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colContactEmail = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colIsPrimary = new DevExpress.XtraGrid.Columns.GridColumn();
            this.riContactName = new DevExpress.XtraEditors.Repository.RepositoryItemTextEdit();
            this.riPosition = new DevExpress.XtraEditors.Repository.RepositoryItemComboBox();
            this.riContactPhone = new DevExpress.XtraEditors.Repository.RepositoryItemTextEdit();
            this.riContactEmail = new DevExpress.XtraEditors.Repository.RepositoryItemTextEdit();
            this.riIsPrimary = new DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit();
            this.lblContactHint = new System.Windows.Forms.Label();
            this.tabRates = new System.Windows.Forms.TabPage();
            this.ucRateSearch = new WeighbridgeAdmin.Controls.SearchBox();
            this.grdRates = new DevExpress.XtraGrid.GridControl();
            this.gvRates = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.colRateProduct = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colRate = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colEffectiveFrom = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colEffectiveTo = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colRateNotes = new DevExpress.XtraGrid.Columns.GridColumn();
            this.riRateProduct = new DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit();
            this.riRate = new DevExpress.XtraEditors.Repository.RepositoryItemTextEdit();
            this.riRateDate = new DevExpress.XtraEditors.Repository.RepositoryItemTextEdit();
            this.riRateNotes = new DevExpress.XtraEditors.Repository.RepositoryItemTextEdit();
            this.lblRateTotals = new System.Windows.Forms.Label();
            this.tabVehicles = new System.Windows.Forms.TabPage();
            this.grdVehicles = new DevExpress.XtraGrid.GridControl();
            this.gvVehicles = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.colVehRegistration = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colVehDescription = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colVehTare = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colVehMaxGross = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colVehActive = new DevExpress.XtraGrid.Columns.GridColumn();
            this.riVehActive = new DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit();
            this.lblVehicleHint = new System.Windows.Forms.Label();
            this.tabAccount.SuspendLayout();
            this.tabContacts.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdContacts)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvContacts)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.riContactName)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.riPosition)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.riContactPhone)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.riContactEmail)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.riIsPrimary)).BeginInit();
            this.tabRates.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdRates)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvRates)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.riRateProduct)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.riRate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.riRateDate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.riRateNotes)).BeginInit();
            this.tabVehicles.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdVehicles)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvVehicles)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.riVehActive)).BeginInit();
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
            this.grdContacts.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grdContacts.Location = new System.Drawing.Point(8, 42);
            this.grdContacts.MainView = this.gvContacts;
            this.grdContacts.Name = "grdContacts";
            this.grdContacts.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.riContactName,
            this.riPosition,
            this.riContactPhone,
            this.riContactEmail,
            this.riIsPrimary});
            this.grdContacts.Size = new System.Drawing.Size(788, 202);
            this.grdContacts.TabIndex = 1;
            this.grdContacts.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gvContacts});
            this.grdContacts.KeyDown += new System.Windows.Forms.KeyEventHandler(this.grdContacts_KeyDown);
            //
            // gvContacts
            //
            this.gvContacts.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.colContactName,
            this.colPosition,
            this.colContactPhone,
            this.colContactEmail,
            this.colIsPrimary});
            this.gvContacts.GridControl = this.grdContacts;
            this.gvContacts.Name = "gvContacts";
            this.gvContacts.OptionsView.ColumnAutoWidth = false;
            this.gvContacts.OptionsView.EnableAppearanceEvenRow = true;
            this.gvContacts.OptionsView.NewItemRowPosition = DevExpress.XtraGrid.Views.Grid.NewItemRowPosition.Bottom;
            this.gvContacts.OptionsView.ShowGroupPanel = false;
            this.gvContacts.CustomRowFilter += new DevExpress.XtraGrid.Views.Base.RowFilterEventHandler(this.gvContacts_CustomRowFilter);
            this.gvContacts.RowStyle += new DevExpress.XtraGrid.Views.Grid.RowStyleEventHandler(this.gvContacts_RowStyle);
            this.gvContacts.InitNewRow += new DevExpress.XtraGrid.Views.Grid.InitNewRowEventHandler(this.gvContacts_InitNewRow);
            this.gvContacts.CellValueChanged += new DevExpress.XtraGrid.Views.Base.CellValueChangedEventHandler(this.gvContacts_CellValueChanged);
            this.gvContacts.ValidatingEditor += new DevExpress.XtraEditors.Controls.BaseContainerValidateEditorEventHandler(this.gvContacts_ValidatingEditor);
            this.gvContacts.InvalidValueException += new DevExpress.XtraEditors.Controls.InvalidValueExceptionEventHandler(this.Grid_InvalidValueException);
            //
            // colContactName
            //
            this.colContactName.Caption = "Name";
            this.colContactName.ColumnEdit = this.riContactName;
            this.colContactName.FieldName = "ContactName";
            this.colContactName.Name = "colContactName";
            this.colContactName.Visible = true;
            this.colContactName.VisibleIndex = 0;
            this.colContactName.Width = 170;
            //
            // colPosition
            //
            this.colPosition.Caption = "Position";
            this.colPosition.ColumnEdit = this.riPosition;
            this.colPosition.FieldName = "Position";
            this.colPosition.Name = "colPosition";
            this.colPosition.Visible = true;
            this.colPosition.VisibleIndex = 1;
            this.colPosition.Width = 120;
            //
            // colContactPhone
            //
            this.colContactPhone.Caption = "Phone";
            this.colContactPhone.ColumnEdit = this.riContactPhone;
            this.colContactPhone.FieldName = "Phone";
            this.colContactPhone.Name = "colContactPhone";
            this.colContactPhone.Visible = true;
            this.colContactPhone.VisibleIndex = 2;
            this.colContactPhone.Width = 130;
            //
            // colContactEmail
            //
            this.colContactEmail.Caption = "Email";
            this.colContactEmail.ColumnEdit = this.riContactEmail;
            this.colContactEmail.FieldName = "Email";
            this.colContactEmail.Name = "colContactEmail";
            this.colContactEmail.Visible = true;
            this.colContactEmail.VisibleIndex = 3;
            this.colContactEmail.Width = 250;
            //
            // colIsPrimary
            //
            this.colIsPrimary.Caption = "Primary";
            this.colIsPrimary.ColumnEdit = this.riIsPrimary;
            this.colIsPrimary.FieldName = "IsPrimary";
            this.colIsPrimary.Name = "colIsPrimary";
            this.colIsPrimary.Visible = true;
            this.colIsPrimary.VisibleIndex = 4;
            this.colIsPrimary.Width = 60;
            //
            // riContactName
            //
            this.riContactName.AutoHeight = false;
            this.riContactName.MaxLength = 60;
            this.riContactName.Name = "riContactName";
            //
            // riPosition
            //
            this.riPosition.AutoHeight = false;
            this.riPosition.Items.AddRange(new object[] {
            "Accounts",
            "Director",
            "Dispatch",
            "Operations",
            "Owner",
            "Procurement",
            "Site Manager",
            "Weighbridge"});
            this.riPosition.Name = "riPosition";
            this.riPosition.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            //
            // riContactPhone
            //
            this.riContactPhone.AutoHeight = false;
            this.riContactPhone.MaxLength = 20;
            this.riContactPhone.Name = "riContactPhone";
            //
            // riContactEmail
            //
            this.riContactEmail.AutoHeight = false;
            this.riContactEmail.MaxLength = 80;
            this.riContactEmail.Name = "riContactEmail";
            //
            // riIsPrimary
            //
            this.riIsPrimary.AutoHeight = false;
            this.riIsPrimary.Name = "riIsPrimary";
            this.riIsPrimary.EditValueChanged += new System.EventHandler(this.riIsPrimary_EditValueChanged);
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
            this.grdRates.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grdRates.Location = new System.Drawing.Point(8, 42);
            this.grdRates.MainView = this.gvRates;
            this.grdRates.Name = "grdRates";
            this.grdRates.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.riRateProduct,
            this.riRate,
            this.riRateDate,
            this.riRateNotes});
            this.grdRates.Size = new System.Drawing.Size(788, 202);
            this.grdRates.TabIndex = 1;
            this.grdRates.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gvRates});
            this.grdRates.KeyDown += new System.Windows.Forms.KeyEventHandler(this.grdRates_KeyDown);
            //
            // gvRates
            //
            this.gvRates.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.colRateProduct,
            this.colRate,
            this.colEffectiveFrom,
            this.colEffectiveTo,
            this.colRateNotes});
            this.gvRates.GridControl = this.grdRates;
            this.gvRates.Name = "gvRates";
            this.gvRates.OptionsView.ColumnAutoWidth = false;
            this.gvRates.OptionsView.EnableAppearanceEvenRow = true;
            this.gvRates.OptionsView.NewItemRowPosition = DevExpress.XtraGrid.Views.Grid.NewItemRowPosition.Bottom;
            this.gvRates.OptionsView.ShowGroupPanel = false;
            this.gvRates.CustomRowFilter += new DevExpress.XtraGrid.Views.Base.RowFilterEventHandler(this.gvRates_CustomRowFilter);
            this.gvRates.RowStyle += new DevExpress.XtraGrid.Views.Grid.RowStyleEventHandler(this.gvRates_RowStyle);
            this.gvRates.InitNewRow += new DevExpress.XtraGrid.Views.Grid.InitNewRowEventHandler(this.gvRates_InitNewRow);
            this.gvRates.CellValueChanged += new DevExpress.XtraGrid.Views.Base.CellValueChangedEventHandler(this.gvRates_CellValueChanged);
            this.gvRates.ValidatingEditor += new DevExpress.XtraEditors.Controls.BaseContainerValidateEditorEventHandler(this.gvRates_ValidatingEditor);
            this.gvRates.InvalidValueException += new DevExpress.XtraEditors.Controls.InvalidValueExceptionEventHandler(this.Grid_InvalidValueException);
            //
            // colRateProduct
            //
            this.colRateProduct.Caption = "Product";
            this.colRateProduct.ColumnEdit = this.riRateProduct;
            this.colRateProduct.FieldName = "ProductId";
            this.colRateProduct.Name = "colRateProduct";
            this.colRateProduct.Visible = true;
            this.colRateProduct.VisibleIndex = 0;
            this.colRateProduct.Width = 210;
            //
            // colRate
            //
            this.colRate.AppearanceCell.Options.UseTextOptions = true;
            this.colRate.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.colRate.Caption = "Rate / tonne";
            this.colRate.ColumnEdit = this.riRate;
            this.colRate.DisplayFormat.FormatString = "n2";
            this.colRate.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.colRate.FieldName = "RatePerTonne";
            this.colRate.Name = "colRate";
            this.colRate.Visible = true;
            this.colRate.VisibleIndex = 1;
            this.colRate.Width = 90;
            //
            // colEffectiveFrom
            //
            this.colEffectiveFrom.Caption = "Effective from";
            this.colEffectiveFrom.ColumnEdit = this.riRateDate;
            this.colEffectiveFrom.DisplayFormat.FormatString = "dd/MM/yyyy";
            this.colEffectiveFrom.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            this.colEffectiveFrom.FieldName = "EffectiveFrom";
            this.colEffectiveFrom.Name = "colEffectiveFrom";
            this.colEffectiveFrom.Visible = true;
            this.colEffectiveFrom.VisibleIndex = 2;
            this.colEffectiveFrom.Width = 100;
            //
            // colEffectiveTo
            //
            this.colEffectiveTo.Caption = "Effective to";
            this.colEffectiveTo.ColumnEdit = this.riRateDate;
            this.colEffectiveTo.DisplayFormat.FormatString = "dd/MM/yyyy";
            this.colEffectiveTo.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            this.colEffectiveTo.FieldName = "EffectiveTo";
            this.colEffectiveTo.Name = "colEffectiveTo";
            this.colEffectiveTo.Visible = true;
            this.colEffectiveTo.VisibleIndex = 3;
            this.colEffectiveTo.Width = 100;
            //
            // colRateNotes
            //
            this.colRateNotes.Caption = "Notes";
            this.colRateNotes.ColumnEdit = this.riRateNotes;
            this.colRateNotes.FieldName = "Notes";
            this.colRateNotes.Name = "colRateNotes";
            this.colRateNotes.Visible = true;
            this.colRateNotes.VisibleIndex = 4;
            this.colRateNotes.Width = 230;
            //
            // riRateProduct
            //
            this.riRateProduct.AutoHeight = false;
            this.riRateProduct.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.riRateProduct.Columns.AddRange(new DevExpress.XtraEditors.Controls.LookUpColumnInfo[] {
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("Code", "Code", 60),
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("Name", "Name", 150)});
            this.riRateProduct.DisplayMember = "Name";
            this.riRateProduct.Name = "riRateProduct";
            this.riRateProduct.NullText = "";
            this.riRateProduct.ShowFooter = false;
            this.riRateProduct.ShowHeader = false;
            this.riRateProduct.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            this.riRateProduct.ValueMember = "Id";
            this.riRateProduct.EditValueChanged += new System.EventHandler(this.riRateProduct_EditValueChanged);
            //
            // riRate
            //
            this.riRate.AutoHeight = false;
            this.riRate.MaxLength = 10;
            this.riRate.Name = "riRate";
            this.riRate.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.RateBox_KeyPress);
            //
            // riRateDate
            //
            this.riRateDate.AutoHeight = false;
            this.riRateDate.EditFormat.FormatString = "dd/MM/yyyy";
            this.riRateDate.EditFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            this.riRateDate.MaxLength = 10;
            this.riRateDate.Name = "riRateDate";
            //
            // riRateNotes
            //
            this.riRateNotes.AutoHeight = false;
            this.riRateNotes.MaxLength = 120;
            this.riRateNotes.Name = "riRateNotes";
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
            this.grdVehicles.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grdVehicles.Location = new System.Drawing.Point(8, 8);
            this.grdVehicles.MainView = this.gvVehicles;
            this.grdVehicles.Name = "grdVehicles";
            this.grdVehicles.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.riVehActive});
            this.grdVehicles.Size = new System.Drawing.Size(788, 236);
            this.grdVehicles.TabIndex = 0;
            this.grdVehicles.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gvVehicles});
            //
            // gvVehicles
            //
            this.gvVehicles.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.colVehRegistration,
            this.colVehDescription,
            this.colVehTare,
            this.colVehMaxGross,
            this.colVehActive});
            this.gvVehicles.GridControl = this.grdVehicles;
            this.gvVehicles.Name = "gvVehicles";
            this.gvVehicles.OptionsBehavior.Editable = false;
            this.gvVehicles.OptionsView.ColumnAutoWidth = false;
            this.gvVehicles.OptionsView.EnableAppearanceEvenRow = true;
            this.gvVehicles.OptionsView.ShowGroupPanel = false;
            this.gvVehicles.OptionsView.ShowIndicator = false;
            this.gvVehicles.RowStyle += new DevExpress.XtraGrid.Views.Grid.RowStyleEventHandler(this.gvVehicles_RowStyle);
            //
            // colVehRegistration
            //
            this.colVehRegistration.Caption = "Registration";
            this.colVehRegistration.FieldName = "Registration";
            this.colVehRegistration.Name = "colVehRegistration";
            this.colVehRegistration.Visible = true;
            this.colVehRegistration.VisibleIndex = 0;
            this.colVehRegistration.Width = 110;
            //
            // colVehDescription
            //
            this.colVehDescription.Caption = "Description";
            this.colVehDescription.FieldName = "Description";
            this.colVehDescription.Name = "colVehDescription";
            this.colVehDescription.Visible = true;
            this.colVehDescription.VisibleIndex = 1;
            this.colVehDescription.Width = 280;
            //
            // colVehTare
            //
            this.colVehTare.AppearanceCell.Options.UseTextOptions = true;
            this.colVehTare.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.colVehTare.Caption = "Tare (kg)";
            this.colVehTare.DisplayFormat.FormatString = "n0";
            this.colVehTare.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.colVehTare.FieldName = "TareWeight";
            this.colVehTare.Name = "colVehTare";
            this.colVehTare.Visible = true;
            this.colVehTare.VisibleIndex = 2;
            this.colVehTare.Width = 100;
            //
            // colVehMaxGross
            //
            this.colVehMaxGross.AppearanceCell.Options.UseTextOptions = true;
            this.colVehMaxGross.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.colVehMaxGross.Caption = "Max gross (kg)";
            this.colVehMaxGross.DisplayFormat.FormatString = "n0";
            this.colVehMaxGross.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.colVehMaxGross.FieldName = "MaxGross";
            this.colVehMaxGross.Name = "colVehMaxGross";
            this.colVehMaxGross.Visible = true;
            this.colVehMaxGross.VisibleIndex = 3;
            this.colVehMaxGross.Width = 110;
            //
            // colVehActive
            //
            this.colVehActive.Caption = "Active";
            this.colVehActive.ColumnEdit = this.riVehActive;
            this.colVehActive.FieldName = "IsActive";
            this.colVehActive.Name = "colVehActive";
            this.colVehActive.Visible = true;
            this.colVehActive.VisibleIndex = 4;
            this.colVehActive.Width = 60;
            //
            // riVehActive
            //
            this.riVehActive.AutoHeight = false;
            this.riVehActive.Name = "riVehActive";
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
            ((System.ComponentModel.ISupportInitialize)(this.gvContacts)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.riContactName)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.riPosition)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.riContactPhone)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.riContactEmail)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.riIsPrimary)).EndInit();
            this.tabRates.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grdRates)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvRates)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.riRateProduct)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.riRate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.riRateDate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.riRateNotes)).EndInit();
            this.tabVehicles.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grdVehicles)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvVehicles)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.riVehActive)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion

        private WeighbridgeAdmin.Controls.CustomerHeaderPanel ucHeader;
        private System.Windows.Forms.TabControl tabAccount;
        private System.Windows.Forms.TabPage tabContacts;
        private WeighbridgeAdmin.Controls.SearchBox ucContactSearch;
        private DevExpress.XtraGrid.GridControl grdContacts;
        private DevExpress.XtraGrid.Views.Grid.GridView gvContacts;
        private DevExpress.XtraGrid.Columns.GridColumn colContactName;
        private DevExpress.XtraGrid.Columns.GridColumn colPosition;
        private DevExpress.XtraGrid.Columns.GridColumn colContactPhone;
        private DevExpress.XtraGrid.Columns.GridColumn colContactEmail;
        private DevExpress.XtraGrid.Columns.GridColumn colIsPrimary;
        private DevExpress.XtraEditors.Repository.RepositoryItemTextEdit riContactName;
        private DevExpress.XtraEditors.Repository.RepositoryItemComboBox riPosition;
        private DevExpress.XtraEditors.Repository.RepositoryItemTextEdit riContactPhone;
        private DevExpress.XtraEditors.Repository.RepositoryItemTextEdit riContactEmail;
        private DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit riIsPrimary;
        private System.Windows.Forms.Label lblContactHint;
        private System.Windows.Forms.TabPage tabRates;
        private WeighbridgeAdmin.Controls.SearchBox ucRateSearch;
        private DevExpress.XtraGrid.GridControl grdRates;
        private DevExpress.XtraGrid.Views.Grid.GridView gvRates;
        private DevExpress.XtraGrid.Columns.GridColumn colRateProduct;
        private DevExpress.XtraGrid.Columns.GridColumn colRate;
        private DevExpress.XtraGrid.Columns.GridColumn colEffectiveFrom;
        private DevExpress.XtraGrid.Columns.GridColumn colEffectiveTo;
        private DevExpress.XtraGrid.Columns.GridColumn colRateNotes;
        private DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit riRateProduct;
        private DevExpress.XtraEditors.Repository.RepositoryItemTextEdit riRate;
        private DevExpress.XtraEditors.Repository.RepositoryItemTextEdit riRateDate;
        private DevExpress.XtraEditors.Repository.RepositoryItemTextEdit riRateNotes;
        private System.Windows.Forms.Label lblRateTotals;
        private System.Windows.Forms.TabPage tabVehicles;
        private DevExpress.XtraGrid.GridControl grdVehicles;
        private DevExpress.XtraGrid.Views.Grid.GridView gvVehicles;
        private DevExpress.XtraGrid.Columns.GridColumn colVehRegistration;
        private DevExpress.XtraGrid.Columns.GridColumn colVehDescription;
        private DevExpress.XtraGrid.Columns.GridColumn colVehTare;
        private DevExpress.XtraGrid.Columns.GridColumn colVehMaxGross;
        private DevExpress.XtraGrid.Columns.GridColumn colVehActive;
        private DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit riVehActive;
        private System.Windows.Forms.Label lblVehicleHint;
    }
}
