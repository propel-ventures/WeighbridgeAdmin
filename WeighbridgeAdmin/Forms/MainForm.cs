using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using WeighbridgeAdmin.Data;
using WeighbridgeAdmin.Model;

namespace WeighbridgeAdmin.Forms
{
    /// <summary>
    /// MDI parent.  Owns the menu, the status bar and the clock timer.
    /// </summary>
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            // Status bar is filled in once here and then the date half is
            // refreshed by the timer.
            this.lblStatusUser.Text = "User: " + Repository.Current.CurrentUserName;
            this.lblStatusDate.Text = DateTime.Now.ToString("dddd, d MMMM yyyy  h:mm:ss tt");
        }

        private void timerClock_Tick(object sender, EventArgs e)
        {
            this.lblStatusDate.Text = DateTime.Now.ToString("dddd, d MMMM yyyy  h:mm:ss tt");
        }

        private void mnuFileExit_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show(this, "Exit Weighbridge Administration?", "Confirm Exit",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                this.Close();
            }
        }

        private void mnuCustomersList_Click(object sender, EventArgs e)
        {
            // Only ever allow one customer list open - if it is already there
            // just bring it to the front.
            for (int i = 0; i < this.MdiChildren.Length; i++)
            {
                if (this.MdiChildren[i] is CustomerListForm)
                {
                    this.MdiChildren[i].Activate();
                    return;
                }
            }

            CustomerListForm frm = new CustomerListForm();
            frm.MdiParent = this;
            frm.Show();
        }

        private void mnuCustomersNew_Click(object sender, EventArgs e)
        {
            CustomerEditForm dlg = new CustomerEditForm();
            dlg.CustomerToEdit = null;
            if (dlg.ShowDialog(this) == DialogResult.OK)
            {
                Repository.Current.SaveCustomer(dlg.CustomerToEdit);
                MessageBox.Show(this, "Customer " + dlg.CustomerToEdit.Code + " has been created.",
                    "Weighbridge Administration", MessageBoxButtons.OK, MessageBoxIcon.Information);

                // If the list happens to be open, make it pick up the new row.
                for (int i = 0; i < this.MdiChildren.Length; i++)
                {
                    CustomerListForm list = this.MdiChildren[i] as CustomerListForm;
                    if (list != null)
                    {
                        list.ReloadGrid();
                    }
                }
            }
            dlg.Dispose();
        }

        private void mnuVehiclesLookup_Click(object sender, EventArgs e)
        {
            VehicleLookupDialog dlg = new VehicleLookupDialog();
            if (dlg.ShowDialog(this) == DialogResult.OK)
            {
                Vehicle v = dlg.SelectedVehicle;
                Customer c = Repository.Current.GetCustomerById(v.CustomerId);

                string owner = "(no customer)";
                if (c != null)
                {
                    owner = c.Name;
                }

                MessageBox.Show(this,
                    "Registration: " + v.Registration + "\r\n" +
                    "Description: " + v.Description + "\r\n" +
                    "Tare: " + v.TareWeight.ToString("N0") + " kg\r\n" +
                    "Max Gross: " + v.MaxGross.ToString("N0") + " kg\r\n" +
                    "Customer: " + owner,
                    "Vehicle Details", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            dlg.Dispose();
        }

        private void mnuTicketsNew_Click(object sender, EventArgs e)
        {
            WeighTicketForm frm = new WeighTicketForm();
            frm.MdiParent = this;
            frm.Show();
        }

        private void mnuTicketsList_Click(object sender, EventArgs e)
        {
            // One ticket list at a time, same as the customer list.
            TicketListForm existing = FindTicketList();
            if (existing != null)
            {
                existing.Activate();
                existing.ReloadGrid();
                return;
            }

            TicketListForm frm = new TicketListForm();
            frm.MdiParent = this;
            frm.Show();
        }

        /// <summary>
        /// The open ticket list, or null.  Used to refresh it after a ticket is
        /// saved so the operator sees the new row straight away.
        /// </summary>
        public TicketListForm FindTicketList()
        {
            for (int i = 0; i < this.MdiChildren.Length; i++)
            {
                TicketListForm list = this.MdiChildren[i] as TicketListForm;
                if (list != null)
                {
                    return list;
                }
            }
            return null;
        }

        private void mnuTicketsCascade_Click(object sender, EventArgs e)
        {
            this.LayoutMdi(MdiLayout.Cascade);
        }

        private void mnuTicketsTile_Click(object sender, EventArgs e)
        {
            this.LayoutMdi(MdiLayout.TileHorizontal);
        }

        private void mnuHelpAbout_Click(object sender, EventArgs e)
        {
            ComponentResourceManager resources = new ComponentResourceManager(typeof(MainForm));
            MessageBox.Show(this, resources.GetString("AboutBox.Message"),
                resources.GetString("AboutBox.Caption"),
                MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}
