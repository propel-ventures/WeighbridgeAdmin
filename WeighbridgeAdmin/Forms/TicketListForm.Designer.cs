namespace WeighbridgeAdmin.Forms
{
    partial class TicketListForm
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
            this.pnlHeader = new System.Windows.Forms.Panel();
            this.lblHeaderSub = new System.Windows.Forms.Label();
            this.lblHeader = new System.Windows.Forms.Label();
            this.tlbMain = new System.Windows.Forms.ToolStrip();
            this.tbbView = new System.Windows.Forms.ToolStripButton();
            this.tbbSep1 = new System.Windows.Forms.ToolStripSeparator();
            this.tbbRefresh = new System.Windows.Forms.ToolStripButton();
            this.tbbClearFilters = new System.Windows.Forms.ToolStripButton();
            this.pnlFilter = new System.Windows.Forms.Panel();
            this.lblRecordCount = new System.Windows.Forms.Label();
            this.txtSearch = new System.Windows.Forms.TextBox();
            this.lblSearch = new System.Windows.Forms.Label();
            this.cboCustomer = new System.Windows.Forms.ComboBox();
            this.lblCustomer = new System.Windows.Forms.Label();
            this.cboStatus = new System.Windows.Forms.ComboBox();
            this.lblStatus = new System.Windows.Forms.Label();
            this.dtpTo = new System.Windows.Forms.DateTimePicker();
            this.lblTo = new System.Windows.Forms.Label();
            this.dtpFrom = new System.Windows.Forms.DateTimePicker();
            this.chkDateRange = new System.Windows.Forms.CheckBox();
            this.pnlTotals = new System.Windows.Forms.Panel();
            this.lblTotals = new System.Windows.Forms.Label();
            this.grdTickets = new DevExpress.XtraGrid.GridControl();
            this.gvTickets = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.colTicketNumber = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colTicketDate = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colRegistration = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colCustomerName = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colProductName = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colGross = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colTare = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colNet = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colRate = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colSubtotal = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colGst = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colTotal = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colStatus = new DevExpress.XtraGrid.Columns.GridColumn();
            this.bsTickets = new System.Windows.Forms.BindingSource(this.components);
            this.pnlHeader.SuspendLayout();
            this.tlbMain.SuspendLayout();
            this.pnlFilter.SuspendLayout();
            this.pnlTotals.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdTickets)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvTickets)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bsTickets)).BeginInit();
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
            this.pnlHeader.Size = new System.Drawing.Size(1000, 44);
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
            this.lblHeaderSub.Text = "Saved weigh tickets.  Double-click a row, or press F2, to see the full ticket.";
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
            this.lblHeader.Text = "Weigh Ticket List";
            //
            // tlbMain
            //
            this.tlbMain.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.tlbMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tbbView,
            this.tbbSep1,
            this.tbbRefresh,
            this.tbbClearFilters});
            this.tlbMain.Location = new System.Drawing.Point(0, 44);
            this.tlbMain.Name = "tlbMain";
            this.tlbMain.Size = new System.Drawing.Size(1000, 25);
            this.tlbMain.TabIndex = 1;
            this.tlbMain.Text = "toolStrip1";
            //
            // tbbView
            //
            this.tbbView.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.tbbView.Name = "tbbView";
            this.tbbView.Size = new System.Drawing.Size(39, 22);
            this.tbbView.Text = "&View";
            this.tbbView.ToolTipText = "Show the selected ticket in full (F2)";
            this.tbbView.Click += new System.EventHandler(this.tbbView_Click);
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
            this.tbbRefresh.ToolTipText = "Re-read the ticket list (F5)";
            this.tbbRefresh.Click += new System.EventHandler(this.tbbRefresh_Click);
            //
            // tbbClearFilters
            //
            this.tbbClearFilters.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.tbbClearFilters.Name = "tbbClearFilters";
            this.tbbClearFilters.Size = new System.Drawing.Size(72, 22);
            this.tbbClearFilters.Text = "&Clear Filters";
            this.tbbClearFilters.ToolTipText = "Show all tickets again";
            this.tbbClearFilters.Click += new System.EventHandler(this.tbbClearFilters_Click);
            //
            // pnlFilter
            //
            this.pnlFilter.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.pnlFilter.Controls.Add(this.lblRecordCount);
            this.pnlFilter.Controls.Add(this.txtSearch);
            this.pnlFilter.Controls.Add(this.lblSearch);
            this.pnlFilter.Controls.Add(this.cboCustomer);
            this.pnlFilter.Controls.Add(this.lblCustomer);
            this.pnlFilter.Controls.Add(this.cboStatus);
            this.pnlFilter.Controls.Add(this.lblStatus);
            this.pnlFilter.Controls.Add(this.dtpTo);
            this.pnlFilter.Controls.Add(this.lblTo);
            this.pnlFilter.Controls.Add(this.dtpFrom);
            this.pnlFilter.Controls.Add(this.chkDateRange);
            this.pnlFilter.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlFilter.Location = new System.Drawing.Point(0, 69);
            this.pnlFilter.Name = "pnlFilter";
            this.pnlFilter.Size = new System.Drawing.Size(1000, 68);
            this.pnlFilter.TabIndex = 2;
            //
            // lblRecordCount
            //
            this.lblRecordCount.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblRecordCount.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblRecordCount.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.lblRecordCount.Location = new System.Drawing.Point(812, 41);
            this.lblRecordCount.Name = "lblRecordCount";
            this.lblRecordCount.Size = new System.Drawing.Size(176, 16);
            this.lblRecordCount.TabIndex = 10;
            this.lblRecordCount.Text = "0 ticket(s)";
            this.lblRecordCount.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            //
            // txtSearch
            //
            this.txtSearch.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtSearch.Location = new System.Drawing.Point(180, 38);
            this.txtSearch.MaxLength = 40;
            this.txtSearch.Name = "txtSearch";
            this.txtSearch.Size = new System.Drawing.Size(240, 20);
            this.txtSearch.TabIndex = 9;
            this.txtSearch.TextChanged += new System.EventHandler(this.Filter_Changed);
            //
            // lblSearch
            //
            this.lblSearch.AutoSize = true;
            this.lblSearch.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSearch.Location = new System.Drawing.Point(12, 41);
            this.lblSearch.Name = "lblSearch";
            this.lblSearch.Size = new System.Drawing.Size(155, 13);
            this.lblSearch.TabIndex = 8;
            this.lblSearch.Text = "Search No. / Rego / Customer";
            //
            // cboCustomer
            //
            this.cboCustomer.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboCustomer.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cboCustomer.FormattingEnabled = true;
            this.cboCustomer.Location = new System.Drawing.Point(588, 7);
            this.cboCustomer.Name = "cboCustomer";
            this.cboCustomer.Size = new System.Drawing.Size(220, 21);
            this.cboCustomer.TabIndex = 7;
            this.cboCustomer.SelectedIndexChanged += new System.EventHandler(this.Filter_Changed);
            //
            // lblCustomer
            //
            this.lblCustomer.AutoSize = true;
            this.lblCustomer.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCustomer.Location = new System.Drawing.Point(526, 11);
            this.lblCustomer.Name = "lblCustomer";
            this.lblCustomer.Size = new System.Drawing.Size(54, 13);
            this.lblCustomer.TabIndex = 6;
            this.lblCustomer.Text = "Customer";
            //
            // cboStatus
            //
            this.cboStatus.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboStatus.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cboStatus.FormattingEnabled = true;
            this.cboStatus.Items.AddRange(new object[] {
            "(All statuses)",
            "Open",
            "Completed",
            "Void"});
            this.cboStatus.Location = new System.Drawing.Point(400, 7);
            this.cboStatus.Name = "cboStatus";
            this.cboStatus.Size = new System.Drawing.Size(110, 21);
            this.cboStatus.TabIndex = 5;
            this.cboStatus.SelectedIndexChanged += new System.EventHandler(this.Filter_Changed);
            //
            // lblStatus
            //
            this.lblStatus.AutoSize = true;
            this.lblStatus.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblStatus.Location = new System.Drawing.Point(356, 11);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(37, 13);
            this.lblStatus.TabIndex = 4;
            this.lblStatus.Text = "Status";
            //
            // dtpTo
            //
            this.dtpTo.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dtpTo.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpTo.Location = new System.Drawing.Point(238, 7);
            this.dtpTo.Name = "dtpTo";
            this.dtpTo.Size = new System.Drawing.Size(100, 20);
            this.dtpTo.TabIndex = 3;
            this.dtpTo.ValueChanged += new System.EventHandler(this.Filter_Changed);
            //
            // lblTo
            //
            this.lblTo.AutoSize = true;
            this.lblTo.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTo.Location = new System.Drawing.Point(214, 11);
            this.lblTo.Name = "lblTo";
            this.lblTo.Size = new System.Drawing.Size(16, 13);
            this.lblTo.TabIndex = 2;
            this.lblTo.Text = "to";
            //
            // dtpFrom
            //
            this.dtpFrom.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dtpFrom.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpFrom.Location = new System.Drawing.Point(108, 7);
            this.dtpFrom.Name = "dtpFrom";
            this.dtpFrom.Size = new System.Drawing.Size(100, 20);
            this.dtpFrom.TabIndex = 1;
            this.dtpFrom.ValueChanged += new System.EventHandler(this.Filter_Changed);
            //
            // chkDateRange
            //
            this.chkDateRange.AutoSize = true;
            this.chkDateRange.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkDateRange.Location = new System.Drawing.Point(14, 10);
            this.chkDateRange.Name = "chkDateRange";
            this.chkDateRange.Size = new System.Drawing.Size(82, 17);
            this.chkDateRange.TabIndex = 0;
            this.chkDateRange.Text = "&Date range";
            this.chkDateRange.UseVisualStyleBackColor = true;
            this.chkDateRange.CheckedChanged += new System.EventHandler(this.chkDateRange_CheckedChanged);
            //
            // pnlTotals
            //
            this.pnlTotals.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.pnlTotals.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlTotals.Controls.Add(this.lblTotals);
            this.pnlTotals.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlTotals.Location = new System.Drawing.Point(0, 535);
            this.pnlTotals.Name = "pnlTotals";
            this.pnlTotals.Size = new System.Drawing.Size(1000, 26);
            this.pnlTotals.TabIndex = 4;
            //
            // lblTotals
            //
            this.lblTotals.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblTotals.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTotals.ForeColor = System.Drawing.Color.Navy;
            this.lblTotals.Location = new System.Drawing.Point(0, 0);
            this.lblTotals.Name = "lblTotals";
            this.lblTotals.Padding = new System.Windows.Forms.Padding(12, 0, 0, 0);
            this.lblTotals.Size = new System.Drawing.Size(998, 24);
            this.lblTotals.TabIndex = 0;
            this.lblTotals.Text = "Charged totals";
            this.lblTotals.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            //
            // grdTickets
            //
            this.grdTickets.DataSource = this.bsTickets;
            this.grdTickets.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grdTickets.Location = new System.Drawing.Point(0, 137);
            this.grdTickets.MainView = this.gvTickets;
            this.grdTickets.Name = "grdTickets";
            this.grdTickets.Size = new System.Drawing.Size(1000, 398);
            this.grdTickets.TabIndex = 3;
            this.grdTickets.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gvTickets});
            //
            // gvTickets
            //
            this.gvTickets.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.colTicketNumber,
            this.colTicketDate,
            this.colRegistration,
            this.colCustomerName,
            this.colProductName,
            this.colGross,
            this.colTare,
            this.colNet,
            this.colRate,
            this.colSubtotal,
            this.colGst,
            this.colTotal,
            this.colStatus});
            this.gvTickets.GridControl = this.grdTickets;
            this.gvTickets.Name = "gvTickets";
            this.gvTickets.OptionsBehavior.ReadOnly = true;
            this.gvTickets.OptionsView.ColumnAutoWidth = false;
            this.gvTickets.OptionsView.EnableAppearanceEvenRow = true;
            this.gvTickets.OptionsView.ShowAutoFilterRow = true;
            this.gvTickets.OptionsView.ShowGroupPanel = true;
            this.gvTickets.DoubleClick += new System.EventHandler(this.gvTickets_DoubleClick);
            this.gvTickets.RowStyle += new DevExpress.XtraGrid.Views.Grid.RowStyleEventHandler(this.gvTickets_RowStyle);
            //
            // colTicketNumber
            //
            this.colTicketNumber.Caption = "Ticket No.";
            this.colTicketNumber.FieldName = "TicketNumber";
            this.colTicketNumber.Name = "colTicketNumber";
            this.colTicketNumber.OptionsColumn.AllowEdit = false;
            this.colTicketNumber.OptionsColumn.ReadOnly = true;
            this.colTicketNumber.Visible = true;
            this.colTicketNumber.VisibleIndex = 0;
            this.colTicketNumber.Width = 80;
            //
            // colTicketDate
            //
            this.colTicketDate.AppearanceCell.Options.UseTextOptions = true;
            this.colTicketDate.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.colTicketDate.Caption = "Date";
            this.colTicketDate.DisplayFormat.FormatString = "dd/MM/yyyy";
            this.colTicketDate.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            this.colTicketDate.FieldName = "TicketDate";
            this.colTicketDate.Name = "colTicketDate";
            this.colTicketDate.OptionsColumn.AllowEdit = false;
            this.colTicketDate.OptionsColumn.ReadOnly = true;
            this.colTicketDate.Visible = true;
            this.colTicketDate.VisibleIndex = 1;
            this.colTicketDate.Width = 78;
            //
            // colRegistration
            //
            this.colRegistration.Caption = "Rego";
            this.colRegistration.FieldName = "Registration";
            this.colRegistration.Name = "colRegistration";
            this.colRegistration.OptionsColumn.AllowEdit = false;
            this.colRegistration.OptionsColumn.ReadOnly = true;
            this.colRegistration.Visible = true;
            this.colRegistration.VisibleIndex = 2;
            this.colRegistration.Width = 72;
            //
            // colCustomerName
            //
            this.colCustomerName.Caption = "Customer";
            this.colCustomerName.FieldName = "CustomerName";
            this.colCustomerName.Name = "colCustomerName";
            this.colCustomerName.OptionsColumn.AllowEdit = false;
            this.colCustomerName.OptionsColumn.ReadOnly = true;
            this.colCustomerName.Visible = true;
            this.colCustomerName.VisibleIndex = 3;
            this.colCustomerName.Width = 165;
            //
            // colProductName
            //
            this.colProductName.Caption = "Product";
            this.colProductName.FieldName = "ProductName";
            this.colProductName.Name = "colProductName";
            this.colProductName.OptionsColumn.AllowEdit = false;
            this.colProductName.OptionsColumn.ReadOnly = true;
            this.colProductName.Visible = true;
            this.colProductName.VisibleIndex = 4;
            this.colProductName.Width = 130;
            //
            // colGross
            //
            this.colGross.AppearanceCell.Options.UseTextOptions = true;
            this.colGross.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.colGross.Caption = "Gross kg";
            this.colGross.DisplayFormat.FormatString = "n0";
            this.colGross.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.colGross.FieldName = "GrossWeight";
            this.colGross.Name = "colGross";
            this.colGross.OptionsColumn.AllowEdit = false;
            this.colGross.OptionsColumn.ReadOnly = true;
            this.colGross.Visible = true;
            this.colGross.VisibleIndex = 5;
            this.colGross.Width = 68;
            //
            // colTare
            //
            this.colTare.AppearanceCell.Options.UseTextOptions = true;
            this.colTare.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.colTare.Caption = "Tare kg";
            this.colTare.DisplayFormat.FormatString = "n0";
            this.colTare.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.colTare.FieldName = "TareWeight";
            this.colTare.Name = "colTare";
            this.colTare.OptionsColumn.AllowEdit = false;
            this.colTare.OptionsColumn.ReadOnly = true;
            this.colTare.Visible = true;
            this.colTare.VisibleIndex = 6;
            this.colTare.Width = 64;
            //
            // colNet
            //
            this.colNet.AppearanceCell.Options.UseTextOptions = true;
            this.colNet.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.colNet.Caption = "Net kg";
            this.colNet.DisplayFormat.FormatString = "n0";
            this.colNet.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.colNet.FieldName = "NetWeight";
            this.colNet.Name = "colNet";
            this.colNet.OptionsColumn.AllowEdit = false;
            this.colNet.OptionsColumn.ReadOnly = true;
            this.colNet.Visible = true;
            this.colNet.VisibleIndex = 7;
            this.colNet.Width = 64;
            //
            // colRate
            //
            this.colRate.AppearanceCell.Options.UseTextOptions = true;
            this.colRate.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.colRate.Caption = "Rate";
            this.colRate.DisplayFormat.FormatString = "n2";
            this.colRate.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.colRate.FieldName = "PricePerTonne";
            this.colRate.Name = "colRate";
            this.colRate.OptionsColumn.AllowEdit = false;
            this.colRate.OptionsColumn.ReadOnly = true;
            this.colRate.Visible = true;
            this.colRate.VisibleIndex = 8;
            this.colRate.Width = 58;
            //
            // colSubtotal
            //
            this.colSubtotal.AppearanceCell.Options.UseTextOptions = true;
            this.colSubtotal.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.colSubtotal.Caption = "Subtotal";
            this.colSubtotal.DisplayFormat.FormatString = "n2";
            this.colSubtotal.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.colSubtotal.FieldName = "Subtotal";
            this.colSubtotal.Name = "colSubtotal";
            this.colSubtotal.OptionsColumn.AllowEdit = false;
            this.colSubtotal.OptionsColumn.ReadOnly = true;
            this.colSubtotal.Visible = true;
            this.colSubtotal.VisibleIndex = 9;
            this.colSubtotal.Width = 72;
            //
            // colGst
            //
            this.colGst.AppearanceCell.Options.UseTextOptions = true;
            this.colGst.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.colGst.Caption = "G.S.T.";
            this.colGst.DisplayFormat.FormatString = "n2";
            this.colGst.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.colGst.FieldName = "Gst";
            this.colGst.Name = "colGst";
            this.colGst.OptionsColumn.AllowEdit = false;
            this.colGst.OptionsColumn.ReadOnly = true;
            this.colGst.Visible = true;
            this.colGst.VisibleIndex = 10;
            this.colGst.Width = 62;
            //
            // colTotal
            //
            this.colTotal.AppearanceCell.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold);
            this.colTotal.AppearanceCell.Options.UseFont = true;
            this.colTotal.AppearanceCell.Options.UseTextOptions = true;
            this.colTotal.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.colTotal.Caption = "Total";
            this.colTotal.DisplayFormat.FormatString = "n2";
            this.colTotal.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.colTotal.FieldName = "Total";
            this.colTotal.Name = "colTotal";
            this.colTotal.OptionsColumn.AllowEdit = false;
            this.colTotal.OptionsColumn.ReadOnly = true;
            this.colTotal.Visible = true;
            this.colTotal.VisibleIndex = 11;
            this.colTotal.Width = 78;
            //
            // colStatus
            //
            this.colStatus.Caption = "Status";
            this.colStatus.FieldName = "Status";
            this.colStatus.Name = "colStatus";
            this.colStatus.OptionsColumn.AllowEdit = false;
            this.colStatus.OptionsColumn.ReadOnly = true;
            this.colStatus.Visible = true;
            this.colStatus.VisibleIndex = 12;
            this.colStatus.Width = 70;
            //
            // bsTickets
            //
            this.bsTickets.DataSource = typeof(WeighbridgeAdmin.Model.WeighTicketSummary);
            //
            // TicketListForm
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.ClientSize = new System.Drawing.Size(1000, 561);
            this.Controls.Add(this.grdTickets);
            this.Controls.Add(this.pnlTotals);
            this.Controls.Add(this.pnlFilter);
            this.Controls.Add(this.tlbMain);
            this.Controls.Add(this.pnlHeader);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Sizable;
            this.KeyPreview = true;
            this.MinimumSize = new System.Drawing.Size(700, 420);
            this.Name = "TicketListForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.WindowsDefaultLocation;
            this.Text = "Weigh Ticket List";
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.TicketListForm_KeyDown);
            this.Load += new System.EventHandler(this.TicketListForm_Load);
            this.pnlHeader.ResumeLayout(false);
            this.pnlHeader.PerformLayout();
            this.tlbMain.ResumeLayout(false);
            this.tlbMain.PerformLayout();
            this.pnlFilter.ResumeLayout(false);
            this.pnlFilter.PerformLayout();
            this.pnlTotals.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grdTickets)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvTickets)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bsTickets)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion

        private System.Windows.Forms.Panel pnlHeader;
        private System.Windows.Forms.Label lblHeader;
        private System.Windows.Forms.Label lblHeaderSub;
        private System.Windows.Forms.ToolStrip tlbMain;
        private System.Windows.Forms.ToolStripButton tbbView;
        private System.Windows.Forms.ToolStripSeparator tbbSep1;
        private System.Windows.Forms.ToolStripButton tbbRefresh;
        private System.Windows.Forms.ToolStripButton tbbClearFilters;
        private System.Windows.Forms.Panel pnlFilter;
        private System.Windows.Forms.CheckBox chkDateRange;
        private System.Windows.Forms.DateTimePicker dtpFrom;
        private System.Windows.Forms.Label lblTo;
        private System.Windows.Forms.DateTimePicker dtpTo;
        private System.Windows.Forms.Label lblStatus;
        private System.Windows.Forms.ComboBox cboStatus;
        private System.Windows.Forms.Label lblCustomer;
        private System.Windows.Forms.ComboBox cboCustomer;
        private System.Windows.Forms.Label lblSearch;
        private System.Windows.Forms.TextBox txtSearch;
        private System.Windows.Forms.Label lblRecordCount;
        private System.Windows.Forms.Panel pnlTotals;
        private System.Windows.Forms.Label lblTotals;
        private DevExpress.XtraGrid.GridControl grdTickets;
        private DevExpress.XtraGrid.Views.Grid.GridView gvTickets;
        private DevExpress.XtraGrid.Columns.GridColumn colTicketNumber;
        private DevExpress.XtraGrid.Columns.GridColumn colTicketDate;
        private DevExpress.XtraGrid.Columns.GridColumn colRegistration;
        private DevExpress.XtraGrid.Columns.GridColumn colCustomerName;
        private DevExpress.XtraGrid.Columns.GridColumn colProductName;
        private DevExpress.XtraGrid.Columns.GridColumn colGross;
        private DevExpress.XtraGrid.Columns.GridColumn colTare;
        private DevExpress.XtraGrid.Columns.GridColumn colNet;
        private DevExpress.XtraGrid.Columns.GridColumn colRate;
        private DevExpress.XtraGrid.Columns.GridColumn colSubtotal;
        private DevExpress.XtraGrid.Columns.GridColumn colGst;
        private DevExpress.XtraGrid.Columns.GridColumn colTotal;
        private DevExpress.XtraGrid.Columns.GridColumn colStatus;
        private System.Windows.Forms.BindingSource bsTickets;
    }
}
