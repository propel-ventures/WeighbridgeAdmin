namespace WeighbridgeAdmin.Forms
{
    partial class MainForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.mnuMain = new System.Windows.Forms.MenuStrip();
            this.mnuFile = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuFileNewTicket = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuFileSep1 = new System.Windows.Forms.ToolStripSeparator();
            this.mnuFileExit = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuCustomers = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuCustomersList = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuCustomersNew = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuVehicles = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuVehiclesLookup = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuTickets = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuTicketsNew = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuTicketsList = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuTicketsSep1 = new System.Windows.Forms.ToolStripSeparator();
            this.mnuTicketsCascade = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuTicketsTile = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuHelp = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuHelpAbout = new System.Windows.Forms.ToolStripMenuItem();
            this.statusMain = new System.Windows.Forms.StatusStrip();
            this.lblStatusUser = new System.Windows.Forms.ToolStripStatusLabel();
            this.lblStatusSpring = new System.Windows.Forms.ToolStripStatusLabel();
            this.lblStatusDate = new System.Windows.Forms.ToolStripStatusLabel();
            this.timerClock = new System.Windows.Forms.Timer(this.components);
            this.mnuMain.SuspendLayout();
            this.statusMain.SuspendLayout();
            this.SuspendLayout();
            // 
            // mnuMain
            // 
            this.mnuMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuFile,
            this.mnuCustomers,
            this.mnuVehicles,
            this.mnuTickets,
            this.mnuHelp});
            this.mnuMain.Location = new System.Drawing.Point(0, 0);
            this.mnuMain.MdiWindowListItem = this.mnuTickets;
            this.mnuMain.Name = "mnuMain";
            this.mnuMain.Size = new System.Drawing.Size(1008, 24);
            this.mnuMain.TabIndex = 0;
            this.mnuMain.Text = "menuStrip1";
            // 
            // mnuFile
            // 
            this.mnuFile.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuFileNewTicket,
            this.mnuFileSep1,
            this.mnuFileExit});
            this.mnuFile.Name = "mnuFile";
            this.mnuFile.Size = new System.Drawing.Size(37, 20);
            this.mnuFile.Text = "&File";
            // 
            // mnuFileNewTicket
            // 
            this.mnuFileNewTicket.Name = "mnuFileNewTicket";
            this.mnuFileNewTicket.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.N)));
            this.mnuFileNewTicket.Size = new System.Drawing.Size(205, 22);
            this.mnuFileNewTicket.Text = "&New Weigh Ticket";
            this.mnuFileNewTicket.Click += new System.EventHandler(this.mnuTicketsNew_Click);
            // 
            // mnuFileSep1
            // 
            this.mnuFileSep1.Name = "mnuFileSep1";
            this.mnuFileSep1.Size = new System.Drawing.Size(202, 6);
            // 
            // mnuFileExit
            // 
            this.mnuFileExit.Name = "mnuFileExit";
            this.mnuFileExit.Size = new System.Drawing.Size(205, 22);
            this.mnuFileExit.Text = "E&xit";
            this.mnuFileExit.Click += new System.EventHandler(this.mnuFileExit_Click);
            // 
            // mnuCustomers
            // 
            this.mnuCustomers.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuCustomersList,
            this.mnuCustomersNew});
            this.mnuCustomers.Name = "mnuCustomers";
            this.mnuCustomers.Size = new System.Drawing.Size(76, 20);
            this.mnuCustomers.Text = "&Customers";
            // 
            // mnuCustomersList
            // 
            this.mnuCustomersList.Name = "mnuCustomersList";
            this.mnuCustomersList.Size = new System.Drawing.Size(180, 22);
            this.mnuCustomersList.Text = "Customer &List";
            this.mnuCustomersList.Click += new System.EventHandler(this.mnuCustomersList_Click);
            // 
            // mnuCustomersNew
            // 
            this.mnuCustomersNew.Name = "mnuCustomersNew";
            this.mnuCustomersNew.Size = new System.Drawing.Size(180, 22);
            this.mnuCustomersNew.Text = "&New Customer...";
            this.mnuCustomersNew.Click += new System.EventHandler(this.mnuCustomersNew_Click);
            // 
            // mnuVehicles
            // 
            this.mnuVehicles.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuVehiclesLookup});
            this.mnuVehicles.Name = "mnuVehicles";
            this.mnuVehicles.Size = new System.Drawing.Size(61, 20);
            this.mnuVehicles.Text = "&Vehicles";
            // 
            // mnuVehiclesLookup
            // 
            this.mnuVehiclesLookup.Name = "mnuVehiclesLookup";
            this.mnuVehiclesLookup.Size = new System.Drawing.Size(180, 22);
            this.mnuVehiclesLookup.Text = "Vehicle &Lookup...";
            this.mnuVehiclesLookup.Click += new System.EventHandler(this.mnuVehiclesLookup_Click);
            // 
            // mnuTickets
            // 
            this.mnuTickets.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuTicketsNew,
            this.mnuTicketsList,
            this.mnuTicketsSep1,
            this.mnuTicketsCascade,
            this.mnuTicketsTile});
            this.mnuTickets.Name = "mnuTickets";
            this.mnuTickets.Size = new System.Drawing.Size(55, 20);
            this.mnuTickets.Text = "&Tickets";
            // 
            // mnuTicketsNew
            // 
            this.mnuTicketsNew.Name = "mnuTicketsNew";
            this.mnuTicketsNew.Size = new System.Drawing.Size(180, 22);
            this.mnuTicketsNew.Text = "&New Weigh Ticket";
            this.mnuTicketsNew.Click += new System.EventHandler(this.mnuTicketsNew_Click);
            //
            // mnuTicketsList
            //
            this.mnuTicketsList.Name = "mnuTicketsList";
            this.mnuTicketsList.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.L)));
            this.mnuTicketsList.Size = new System.Drawing.Size(180, 22);
            this.mnuTicketsList.Text = "Ticket &List";
            this.mnuTicketsList.Click += new System.EventHandler(this.mnuTicketsList_Click);
            //
            // mnuTicketsSep1
            //
            this.mnuTicketsSep1.Name = "mnuTicketsSep1";
            this.mnuTicketsSep1.Size = new System.Drawing.Size(177, 6);
            // 
            // mnuTicketsCascade
            // 
            this.mnuTicketsCascade.Name = "mnuTicketsCascade";
            this.mnuTicketsCascade.Size = new System.Drawing.Size(180, 22);
            this.mnuTicketsCascade.Text = "&Cascade Windows";
            this.mnuTicketsCascade.Click += new System.EventHandler(this.mnuTicketsCascade_Click);
            // 
            // mnuTicketsTile
            // 
            this.mnuTicketsTile.Name = "mnuTicketsTile";
            this.mnuTicketsTile.Size = new System.Drawing.Size(180, 22);
            this.mnuTicketsTile.Text = "&Tile Horizontally";
            this.mnuTicketsTile.Click += new System.EventHandler(this.mnuTicketsTile_Click);
            // 
            // mnuHelp
            // 
            this.mnuHelp.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuHelpAbout});
            this.mnuHelp.Name = "mnuHelp";
            this.mnuHelp.Size = new System.Drawing.Size(44, 20);
            this.mnuHelp.Text = "&Help";
            // 
            // mnuHelpAbout
            // 
            this.mnuHelpAbout.Name = "mnuHelpAbout";
            this.mnuHelpAbout.Size = new System.Drawing.Size(180, 22);
            this.mnuHelpAbout.Text = "&About...";
            this.mnuHelpAbout.Click += new System.EventHandler(this.mnuHelpAbout_Click);
            // 
            // statusMain
            // 
            this.statusMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.lblStatusUser,
            this.lblStatusSpring,
            this.lblStatusDate});
            this.statusMain.Location = new System.Drawing.Point(0, 639);
            this.statusMain.Name = "statusMain";
            this.statusMain.Size = new System.Drawing.Size(1008, 22);
            this.statusMain.TabIndex = 1;
            this.statusMain.Text = "statusStrip1";
            // 
            // lblStatusUser
            // 
            this.lblStatusUser.BorderSides = ((System.Windows.Forms.ToolStripStatusLabelBorderSides)((((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left | System.Windows.Forms.ToolStripStatusLabelBorderSides.Top)
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Right)
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom)));
            this.lblStatusUser.BorderStyle = System.Windows.Forms.Border3DStyle.SunkenOuter;
            this.lblStatusUser.Name = "lblStatusUser";
            this.lblStatusUser.Size = new System.Drawing.Size(50, 17);
            this.lblStatusUser.Text = "User:";
            // 
            // lblStatusSpring
            // 
            this.lblStatusSpring.Name = "lblStatusSpring";
            this.lblStatusSpring.Size = new System.Drawing.Size(779, 17);
            this.lblStatusSpring.Spring = true;
            // 
            // lblStatusDate
            // 
            this.lblStatusDate.BorderSides = ((System.Windows.Forms.ToolStripStatusLabelBorderSides)((((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left | System.Windows.Forms.ToolStripStatusLabelBorderSides.Top)
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Right)
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom)));
            this.lblStatusDate.BorderStyle = System.Windows.Forms.Border3DStyle.SunkenOuter;
            this.lblStatusDate.Name = "lblStatusDate";
            this.lblStatusDate.Size = new System.Drawing.Size(164, 17);
            this.lblStatusDate.Text = "dd/MM/yyyy";
            // 
            // timerClock
            // 
            this.timerClock.Enabled = true;
            this.timerClock.Interval = 1000;
            this.timerClock.Tick += new System.EventHandler(this.timerClock_Tick);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.ClientSize = new System.Drawing.Size(1008, 661);
            this.Controls.Add(this.statusMain);
            this.Controls.Add(this.mnuMain);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.IsMdiContainer = true;
            this.MainMenuStrip = this.mnuMain;
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = resources.GetString("$this.Text");
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.mnuMain.ResumeLayout(false);
            this.mnuMain.PerformLayout();
            this.statusMain.ResumeLayout(false);
            this.statusMain.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion

        private System.Windows.Forms.MenuStrip mnuMain;
        private System.Windows.Forms.ToolStripMenuItem mnuFile;
        private System.Windows.Forms.ToolStripMenuItem mnuFileNewTicket;
        private System.Windows.Forms.ToolStripSeparator mnuFileSep1;
        private System.Windows.Forms.ToolStripMenuItem mnuFileExit;
        private System.Windows.Forms.ToolStripMenuItem mnuCustomers;
        private System.Windows.Forms.ToolStripMenuItem mnuCustomersList;
        private System.Windows.Forms.ToolStripMenuItem mnuCustomersNew;
        private System.Windows.Forms.ToolStripMenuItem mnuVehicles;
        private System.Windows.Forms.ToolStripMenuItem mnuVehiclesLookup;
        private System.Windows.Forms.ToolStripMenuItem mnuTickets;
        private System.Windows.Forms.ToolStripMenuItem mnuTicketsNew;
        private System.Windows.Forms.ToolStripMenuItem mnuTicketsList;
        private System.Windows.Forms.ToolStripSeparator mnuTicketsSep1;
        private System.Windows.Forms.ToolStripMenuItem mnuTicketsCascade;
        private System.Windows.Forms.ToolStripMenuItem mnuTicketsTile;
        private System.Windows.Forms.ToolStripMenuItem mnuHelp;
        private System.Windows.Forms.ToolStripMenuItem mnuHelpAbout;
        private System.Windows.Forms.StatusStrip statusMain;
        private System.Windows.Forms.ToolStripStatusLabel lblStatusUser;
        private System.Windows.Forms.ToolStripStatusLabel lblStatusSpring;
        private System.Windows.Forms.ToolStripStatusLabel lblStatusDate;
        private System.Windows.Forms.Timer timerClock;
    }
}
