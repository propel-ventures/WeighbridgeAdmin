using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using WeighbridgeAdmin.Data;
using WeighbridgeAdmin.Model;
using WeighbridgeAdmin.Security;

namespace WeighbridgeAdmin.Forms
{
    /// <summary>
    /// Weigh ticket browse screen.  Read only - a saved ticket is an accounting
    /// document, so nothing here edits or deletes one.  Same shape as the
    /// customer list: toolbar, filter strip, grid, and the work done in the
    /// control event handlers.
    /// </summary>
    public partial class TicketListForm : Form
    {
        // Stops the filter handlers re-querying while Form_Load is populating
        // the combos.
        private bool _loading = false;

        public TicketListForm()
        {
            InitializeComponent();
        }

        private void TicketListForm_Load(object sender, EventArgs e)
        {
            _loading = true;

            // "All customers" is a sentinel row with Id 0 - the repository
            // treats 0 as "do not filter".
            List<Customer> customers = Repository.Current.GetCustomers();
            Customer allCustomers = new Customer();
            allCustomers.Id = 0;
            allCustomers.Name = "(All customers)";
            customers.Insert(0, allCustomers);

            this.cboCustomer.DataSource = customers;
            this.cboCustomer.DisplayMember = "Name";
            this.cboCustomer.ValueMember = "Id";
            this.cboCustomer.SelectedIndex = 0;

            this.cboStatus.SelectedIndex = 0;

            // Dates start switched off so the operator sees everything on open.
            this.dtpFrom.Value = DateTime.Today.AddMonths(-1);
            this.dtpTo.Value = DateTime.Today;
            this.chkDateRange.Checked = false;
            EnableDateControls();

            _loading = false;

            ReloadGrid();
        }

        // ------------------------------------------------------------------
        // Loading
        // ------------------------------------------------------------------

        /// <summary>
        /// Re-reads the ticket list through the current filters.  Public so the
        /// main form and the weigh ticket screen can refresh an open list after
        /// a ticket is saved.
        /// </summary>
        public void ReloadGrid()
        {
            if (_loading)
            {
                return;
            }

            DateTime? fromDate = null;
            DateTime? toDate = null;
            if (this.chkDateRange.Checked)
            {
                fromDate = this.dtpFrom.Value.Date;
                toDate = this.dtpTo.Value.Date;
            }

            int customerId = 0;
            if (this.cboCustomer.SelectedValue != null)
            {
                customerId = Convert.ToInt32(this.cboCustomer.SelectedValue);
            }

            List<WeighTicketSummary> list = Repository.Current.SearchTickets(
                fromDate, toDate, customerId, GetSelectedStatus(), this.txtSearch.Text);

            this.bsTickets.DataSource = list;
            this.bsTickets.ResetBindings(false);

            this.lblRecordCount.Text = list.Count.ToString() + " ticket(s)";

            ColourRows(list);
            ShowTotals(list);

            this.tbbView.Enabled = (list.Count > 0) && SecurityContext.HasPrivilege(Privileges.TicketView);
        }

        /// <summary>Index 0 of the combo is "(All statuses)", which filters on nothing.</summary>
        private string GetSelectedStatus()
        {
            if (this.cboStatus.SelectedIndex <= 0)
            {
                return "";
            }
            return Convert.ToString(this.cboStatus.SelectedItem);
        }

        /// <summary>
        /// Void tickets grey out and open ones go amber, so a part finished day
        /// stands out from the completed work.
        /// </summary>
        private void ColourRows(List<WeighTicketSummary> list)
        {
            for (int i = 0; i < this.grdTickets.Rows.Count; i++)
            {
                WeighTicketSummary s = this.grdTickets.Rows[i].DataBoundItem as WeighTicketSummary;
                if (s == null)
                {
                    continue;
                }

                if (s.Status == TicketStatus.Void)
                {
                    this.grdTickets.Rows[i].DefaultCellStyle.ForeColor = Color.Gray;
                }
                else if (s.Status == TicketStatus.Open)
                {
                    this.grdTickets.Rows[i].DefaultCellStyle.ForeColor = Color.FromArgb(160, 80, 0);
                }
                else
                {
                    this.grdTickets.Rows[i].DefaultCellStyle.ForeColor = Color.Black;
                }
            }
        }

        /// <summary>
        /// Money totals for whatever is on screen.  Voided tickets are counted
        /// in the row count but NOT in the money - nothing was charged for them.
        /// </summary>
        private void ShowTotals(List<WeighTicketSummary> list)
        {
            int voided = 0;
            decimal tonnes = 0m;
            decimal subtotal = 0m;
            decimal gst = 0m;
            decimal total = 0m;

            for (int i = 0; i < list.Count; i++)
            {
                WeighTicketSummary s = list[i];
                if (s.Status == TicketStatus.Void)
                {
                    voided = voided + 1;
                    continue;
                }

                tonnes = tonnes + s.NetTonnes;
                subtotal = subtotal + s.Subtotal;
                gst = gst + s.Gst;
                total = total + s.Total;
            }

            string caption = "Charged totals";
            if (voided > 0)
            {
                caption = caption + " (excludes " + voided.ToString() + " voided)";
            }

            this.lblTotals.Text = caption
                + "     Net " + tonnes.ToString("N3") + " t"
                + "     Subtotal $" + subtotal.ToString("N2")
                + "     G.S.T. $" + gst.ToString("N2")
                + "     Total $" + total.ToString("N2");
        }

        // ------------------------------------------------------------------
        // Filters
        // ------------------------------------------------------------------

        private void Filter_Changed(object sender, EventArgs e)
        {
            ReloadGrid();
        }

        private void chkDateRange_CheckedChanged(object sender, EventArgs e)
        {
            EnableDateControls();
            ReloadGrid();
        }

        private void EnableDateControls()
        {
            this.dtpFrom.Enabled = this.chkDateRange.Checked;
            this.dtpTo.Enabled = this.chkDateRange.Checked;
        }

        // ------------------------------------------------------------------
        // Viewing a ticket
        // ------------------------------------------------------------------

        private WeighTicketSummary GetSelectedTicket()
        {
            if (this.grdTickets.CurrentRow == null)
            {
                return null;
            }
            return this.grdTickets.CurrentRow.DataBoundItem as WeighTicketSummary;
        }

        private void tbbView_Click(object sender, EventArgs e)
        {
            try
            {
                SecurityContext.Demand(Privileges.TicketView);
            }
            catch (UnauthorizedAccessException ex)
            {
                MessageBox.Show("You do not have the '" + ex.Message + "' privilege.", "Access denied",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            WeighTicketSummary s = GetSelectedTicket();
            if (s == null)
            {
                MessageBox.Show(this, "Please select a ticket first.", "View Ticket",
                    MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            string notes = s.Notes;
            if (notes.Length == 0)
            {
                notes = "(none)";
            }

            MessageBox.Show(this,
                "Ticket:      " + s.TicketNumber + "        " + s.Status + "\r\n" +
                "Date:        " + s.TicketDate.ToString("dd/MM/yyyy") + "\r\n\r\n" +
                "Vehicle:     " + s.Registration + "\r\n" +
                "Customer:    " + s.CustomerCode + " - " + s.CustomerName + "\r\n" +
                "Product:     " + s.ProductCode + " - " + s.ProductName + "\r\n\r\n" +
                "Gross:       " + s.GrossWeight.ToString("N0") + " kg\r\n" +
                "Tare:        " + s.TareWeight.ToString("N0") + " kg\r\n" +
                "Net:         " + s.NetWeight.ToString("N0") + " kg  ("
                                + s.NetTonnes.ToString("N3") + " t)\r\n\r\n" +
                "Rate:        $" + s.PricePerTonne.ToString("N2") + " per tonne\r\n" +
                "Subtotal:    $" + s.Subtotal.ToString("N2") + "\r\n" +
                "G.S.T.:      $" + s.Gst.ToString("N2") + "\r\n" +
                "Total:       $" + s.Total.ToString("N2") + "\r\n\r\n" +
                "Notes:       " + notes,
                "Weigh Ticket " + s.TicketNumber,
                MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void tbbRefresh_Click(object sender, EventArgs e)
        {
            ReloadGrid();
        }

        private void tbbClearFilters_Click(object sender, EventArgs e)
        {
            _loading = true;
            this.txtSearch.Text = "";
            this.cboCustomer.SelectedIndex = 0;
            this.cboStatus.SelectedIndex = 0;
            this.chkDateRange.Checked = false;
            EnableDateControls();
            _loading = false;

            ReloadGrid();
        }

        private void grdTickets_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
            {
                // Header row - ignore.
                return;
            }
            tbbView_Click(sender, EventArgs.Empty);
        }

        private void TicketListForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F5)
            {
                ReloadGrid();
                e.Handled = true;
            }
            else if (e.KeyCode == Keys.F2)
            {
                tbbView_Click(sender, EventArgs.Empty);
                e.Handled = true;
            }
        }
    }
}
