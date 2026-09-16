using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using WeighbridgeAdmin.Data;
using WeighbridgeAdmin.Model;

namespace WeighbridgeAdmin.Forms
{
    /// <summary>
    /// Weigh ticket entry.
    ///
    /// IMPORTANT (and deliberate): there is no WeighTicket instance hanging
    /// around while the operator is keying.  Every value lives on a control and
    /// the derived figures (net, subtotal, GST, total) are recalculated inside
    /// the ValueChanged / SelectedIndexChanged handlers.  A WeighTicket is only
    /// constructed at the moment Save is pressed, straight off the controls.
    /// The only things cached on the form are the couple of vehicle master
    /// values the screen needs for the overweight check.
    /// </summary>
    public partial class WeighTicketForm : Form
    {
        // Cached bits of the vehicle master record - NOT ticket state.
        private int _vehicleId = 0;
        private decimal _vehicleMaxGross = 0m;

        // Stops the ValueChanged/SelectedIndexChanged handlers firing while
        // the combos are being populated in Form_Load.
        private bool _loading = false;

        public WeighTicketForm()
        {
            InitializeComponent();
        }

        private void WeighTicketForm_Load(object sender, EventArgs e)
        {
            _loading = true;

            this.lblTicketNumber.Text = "Ticket No. " + Repository.Current.PeekNextTicketNumber();
            this.txtOperator.Text = Repository.Current.CurrentUserName;
            this.dtpTicketDate.Value = DateTime.Today;

            // The designer lays this caption out with the old fixed 10%.
            this.lblGstCaption.Text = "G.S.T. (" + Repository.Current.GstRateCaption + ")";

            this.cboCustomer.DataSource = Repository.Current.GetCustomers();
            this.cboCustomer.DisplayMember = "Name";
            this.cboCustomer.ValueMember = "Id";
            this.cboCustomer.SelectedIndex = -1;

            this.cboProduct.DataSource = Repository.Current.GetProducts();
            this.cboProduct.DisplayMember = "Name";
            this.cboProduct.ValueMember = "Id";
            this.cboProduct.SelectedIndex = -1;

            this.cboStatus.SelectedIndex = 0;

            _loading = false;

            RecalculateWeights();
            RecalculateCharges();
            UpdateWizardButtons();

            this.txtRegistration.Focus();
        }

        // ------------------------------------------------------------------
        // Tab 1 - vehicle & customer
        // ------------------------------------------------------------------

        private void btnLookup_Click(object sender, EventArgs e)
        {
            VehicleLookupDialog dlg = new VehicleLookupDialog();
            dlg.InitialSearchText = this.txtRegistration.Text;

            if (dlg.ShowDialog(this) == DialogResult.OK)
            {
                ApplyVehicle(dlg.SelectedVehicle);
            }
            dlg.Dispose();
        }

        private void txtRegistration_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F4)
            {
                btnLookup_Click(sender, EventArgs.Empty);
                e.Handled = true;
                e.SuppressKeyPress = true;
            }
        }

        private void txtRegistration_Leave(object sender, EventArgs e)
        {
            // If the operator typed a rego straight in, try to resolve it
            // without making them open the lookup.
            string rego = this.txtRegistration.Text.Trim();
            if (rego.Length == 0)
            {
                ClearVehicle();
                return;
            }

            List<Vehicle> matches = Repository.Current.SearchVehicles(rego);
            if (matches.Count == 1)
            {
                ApplyVehicle(matches[0]);
            }
            else if (matches.Count == 0)
            {
                ClearVehicle();
            }
        }

        /// <summary>Copies the vehicle master onto the controls.</summary>
        private void ApplyVehicle(Vehicle v)
        {
            if (v == null)
            {
                return;
            }

            _vehicleId = v.Id;
            _vehicleMaxGross = v.MaxGross;

            this.txtRegistration.Text = v.Registration;
            this.txtDescription.Text = v.Description;
            this.txtDefaultTare.Text = v.TareWeight.ToString("N0");
            this.txtMaxGross.Text = v.MaxGross.ToString("N0");

            // Default the tare on the weights tab from the vehicle master.
            if (v.TareWeight <= this.numTare.Maximum)
            {
                this.numTare.Value = v.TareWeight;
            }

            // The vehicle drags its customer along with it.
            Customer c = Repository.Current.GetCustomerById(v.CustomerId);
            if (c != null)
            {
                this.cboCustomer.SelectedValue = c.Id;
            }

            RecalculateWeights();
            RecalculateCharges();
        }

        private void ClearVehicle()
        {
            _vehicleId = 0;
            _vehicleMaxGross = 0m;
            this.txtDescription.Text = "";
            this.txtDefaultTare.Text = "";
            this.txtMaxGross.Text = "";
            RecalculateWeights();
        }

        // ------------------------------------------------------------------
        // Tab 2 - weights
        // ------------------------------------------------------------------

        private void numGross_ValueChanged(object sender, EventArgs e)
        {
            if (_loading)
            {
                return;
            }
            RecalculateWeights();
            RecalculateCharges();
        }

        private void numTare_ValueChanged(object sender, EventArgs e)
        {
            if (_loading)
            {
                return;
            }
            RecalculateWeights();
            RecalculateCharges();
        }

        /// <summary>
        /// Works the net out of whatever is currently on the two spinners and
        /// pushes it onto the read-only label.
        /// </summary>
        private void RecalculateWeights()
        {
            decimal gross = this.numGross.Value;
            decimal tare = this.numTare.Value;
            decimal net = gross - tare;

            this.lblNetValue.Text = net.ToString("N0");
            if (net < 0m)
            {
                this.lblNetValue.ForeColor = Color.Red;
            }
            else
            {
                this.lblNetValue.ForeColor = Color.Black;
            }

            this.lblTonnes.Text = "Net tonnes: " + (net / 1000m).ToString("N3") + " t";

            if (_vehicleMaxGross > 0m && gross > _vehicleMaxGross)
            {
                this.lblWarning.Text = "*** GROSS " + gross.ToString("N0")
                    + " kg EXCEEDS VEHICLE MAXIMUM OF " + _vehicleMaxGross.ToString("N0") + " kg ***";
                this.lblWarning.Visible = true;
            }
            else
            {
                this.lblWarning.Visible = false;
            }
        }

        // ------------------------------------------------------------------
        // Tab 3 - charges
        // ------------------------------------------------------------------

        private void cboProduct_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_loading)
            {
                return;
            }

            Product p = GetSelectedProduct();
            if (p != null)
            {
                // Price defaults from the product but the operator may overtype it.
                if (p.PricePerTonne <= this.numPrice.Maximum)
                {
                    this.numPrice.Value = p.PricePerTonne;
                }

                if (p.GstApplicable)
                {
                    this.lblGstFlag.Text = "G.S.T. applies to this product ("
                        + Repository.Current.GstRateCaption + ").";
                }
                else
                {
                    this.lblGstFlag.Text = "This product is GST FREE - no tax will be charged.";
                }
            }

            RecalculateCharges();
        }

        private void numPrice_ValueChanged(object sender, EventArgs e)
        {
            if (_loading)
            {
                return;
            }
            RecalculateCharges();
        }

        private Product GetSelectedProduct()
        {
            if (this.cboProduct.SelectedIndex < 0)
            {
                return null;
            }
            return this.cboProduct.SelectedItem as Product;
        }

        private Customer GetSelectedCustomer()
        {
            if (this.cboCustomer.SelectedIndex < 0)
            {
                return null;
            }
            return this.cboCustomer.SelectedItem as Customer;
        }

        /// <summary>
        /// Subtotal / GST / Total off the controls.  Called from every handler
        /// that can change one of the inputs.
        /// </summary>
        private void RecalculateCharges()
        {
            decimal net = this.numGross.Value - this.numTare.Value;
            if (net < 0m)
            {
                net = 0m;
            }

            decimal tonnes = net / 1000m;
            decimal price = this.numPrice.Value;
            decimal subtotal = Math.Round(tonnes * price, 2);

            decimal gst = 0m;
            Product p = GetSelectedProduct();
            if (p != null && p.GstApplicable)
            {
                gst = Math.Round(subtotal * Repository.Current.GstRate, 2);
            }

            decimal total = subtotal + gst;

            this.lblSubtotalValue.Text = subtotal.ToString("N2");
            this.lblGstValue.Text = gst.ToString("N2");
            this.lblTotalValue.Text = total.ToString("N2");
        }

        // ------------------------------------------------------------------
        // Tab 4 - review
        // ------------------------------------------------------------------

        /// <summary>
        /// Re-reads every control on the first three tabs and paints the
        /// summary labels.  Done every time the tab is shown.
        /// </summary>
        private void RefreshReviewTab()
        {
            decimal net = this.numGross.Value - this.numTare.Value;

            string customerName = "-";
            Customer c = GetSelectedCustomer();
            if (c != null)
            {
                customerName = c.Code + " - " + c.Name;
            }

            string productName = "-";
            Product p = GetSelectedProduct();
            if (p != null)
            {
                productName = p.Code + " - " + p.Name;
            }

            this.lblRevTicketNoValue.Text = Repository.Current.PeekNextTicketNumber();
            this.lblRevDateValue.Text = this.dtpTicketDate.Value.ToString("dd/MM/yyyy");
            this.lblRevRegoValue.Text = this.txtRegistration.Text;
            this.lblRevVehicleValue.Text = this.txtDescription.Text;
            this.lblRevCustomerValue.Text = customerName;
            this.lblRevProductValue.Text = productName;
            this.lblRevGrossValue.Text = this.numGross.Value.ToString("N0") + " kg";
            this.lblRevTareValue.Text = this.numTare.Value.ToString("N0") + " kg";
            this.lblRevNetValue.Text = net.ToString("N0") + " kg  (" + (net / 1000m).ToString("N3") + " t)";
            this.lblRevPriceValue.Text = "$ " + this.numPrice.Value.ToString("N2");
            this.lblRevSubtotalValue.Text = "$ " + this.lblSubtotalValue.Text;
            this.lblRevGstValue.Text = "$ " + this.lblGstValue.Text;
            this.lblRevTotalValue.Text = "$ " + this.lblTotalValue.Text;
        }

        // ------------------------------------------------------------------
        // Wizard navigation
        // ------------------------------------------------------------------

        private void tabTicket_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.tabTicket.SelectedIndex == 3)
            {
                RefreshReviewTab();
            }
            UpdateWizardButtons();
        }

        private void UpdateWizardButtons()
        {
            int index = this.tabTicket.SelectedIndex;

            this.btnBack.Enabled = (index > 0);
            this.btnNext.Enabled = (index < 3);

            this.lblStepHint.Text = "Step " + (index + 1).ToString() + " of 4  -  "
                + this.tabTicket.TabPages[index].Text.Replace("&&", "&");
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            if (this.tabTicket.SelectedIndex > 0)
            {
                this.tabTicket.SelectedIndex = this.tabTicket.SelectedIndex - 1;
            }
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            if (this.tabTicket.SelectedIndex >= 3)
            {
                return;
            }

            if (!ValidateTab(this.tabTicket.SelectedIndex))
            {
                return;
            }

            this.tabTicket.SelectedIndex = this.tabTicket.SelectedIndex + 1;
        }

        /// <summary>
        /// Page level validation.  Pops a message box and parks the cursor on
        /// the offending control, the way the old screen did.
        /// </summary>
        private bool ValidateTab(int index)
        {
            if (index == 0)
            {
                if (this.txtRegistration.Text.Trim().Length == 0 || _vehicleId == 0)
                {
                    MessageBox.Show(this,
                        "A valid vehicle must be selected before continuing.\r\n\r\nPress F4 to look one up.",
                        "Weigh Ticket", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    this.tabTicket.SelectedIndex = 0;
                    this.txtRegistration.Focus();
                    return false;
                }

                if (this.cboCustomer.SelectedIndex < 0)
                {
                    MessageBox.Show(this, "Please select the customer to charge.",
                        "Weigh Ticket", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    this.tabTicket.SelectedIndex = 0;
                    this.cboCustomer.Focus();
                    return false;
                }

                return true;
            }

            if (index == 1)
            {
                if (this.numGross.Value <= 0m)
                {
                    MessageBox.Show(this, "Gross weight must be greater than zero.",
                        "Weigh Ticket", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    this.tabTicket.SelectedIndex = 1;
                    this.numGross.Focus();
                    return false;
                }

                if (this.numTare.Value >= this.numGross.Value)
                {
                    MessageBox.Show(this,
                        "Tare weight must be less than the gross weight - the net would be zero or negative.",
                        "Weigh Ticket", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    this.tabTicket.SelectedIndex = 1;
                    this.numTare.Focus();
                    return false;
                }

                if (_vehicleMaxGross > 0m && this.numGross.Value > _vehicleMaxGross)
                {
                    if (MessageBox.Show(this,
                        "Gross weight of " + this.numGross.Value.ToString("N0")
                        + " kg exceeds the vehicle maximum of " + _vehicleMaxGross.ToString("N0") + " kg.\r\n\r\n"
                        + "Continue anyway?",
                        "Overweight", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation,
                        MessageBoxDefaultButton.Button2) != DialogResult.Yes)
                    {
                        this.tabTicket.SelectedIndex = 1;
                        this.numGross.Focus();
                        return false;
                    }
                }

                return true;
            }

            if (index == 2)
            {
                if (this.cboProduct.SelectedIndex < 0)
                {
                    MessageBox.Show(this, "Please select the product being weighed.",
                        "Weigh Ticket", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    this.tabTicket.SelectedIndex = 2;
                    this.cboProduct.Focus();
                    return false;
                }

                if (this.numPrice.Value <= 0m)
                {
                    if (MessageBox.Show(this,
                        "The price per tonne is zero, so this ticket will not be charged.\r\n\r\nIs that correct?",
                        "Zero Price", MessageBoxButtons.YesNo, MessageBoxIcon.Question,
                        MessageBoxDefaultButton.Button2) != DialogResult.Yes)
                    {
                        this.tabTicket.SelectedIndex = 2;
                        this.numPrice.Focus();
                        return false;
                    }
                }

                return true;
            }

            if (index == 3)
            {
                if (this.cboStatus.SelectedIndex < 0)
                {
                    MessageBox.Show(this, "Please choose a ticket status.",
                        "Weigh Ticket", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    this.tabTicket.SelectedIndex = 3;
                    this.cboStatus.Focus();
                    return false;
                }

                return true;
            }

            return true;
        }

        // ------------------------------------------------------------------
        // Save / cancel
        // ------------------------------------------------------------------

        private void btnSave_Click(object sender, EventArgs e)
        {
            // Re-run every page, not just the current one.
            for (int i = 0; i <= 3; i++)
            {
                if (!ValidateTab(i))
                {
                    return;
                }
            }

            if (MessageBox.Show(this, "Save this weigh ticket?", "Confirm Save",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
            {
                return;
            }

            // Everything is read back off the controls here - this is the first
            // and only time a WeighTicket object exists on this screen.
            decimal gross = this.numGross.Value;
            decimal tare = this.numTare.Value;
            decimal net = gross - tare;
            decimal price = this.numPrice.Value;
            decimal subtotal = Math.Round((net / 1000m) * price, 2);

            decimal gst = 0m;
            Product p = GetSelectedProduct();
            if (p != null && p.GstApplicable)
            {
                gst = Math.Round(subtotal * Repository.Current.GstRate, 2);
            }

            WeighTicket ticket = new WeighTicket();
            ticket.TicketDate = this.dtpTicketDate.Value.Date;
            ticket.VehicleId = _vehicleId;
            ticket.CustomerId = Convert.ToInt32(this.cboCustomer.SelectedValue);
            ticket.ProductId = Convert.ToInt32(this.cboProduct.SelectedValue);
            ticket.GrossWeight = gross;
            ticket.TareWeight = tare;
            ticket.PricePerTonne = price;
            ticket.Subtotal = subtotal;
            ticket.Gst = gst;
            ticket.Total = subtotal + gst;
            ticket.Notes = this.txtNotes.Text.Trim();
            ticket.Status = Convert.ToString(this.cboStatus.SelectedItem);

            Repository.Current.SaveTicket(ticket);

            // If the ticket list happens to be open, make it pick up the new row.
            MainForm main = this.MdiParent as MainForm;
            if (main != null)
            {
                TicketListForm list = main.FindTicketList();
                if (list != null)
                {
                    list.ReloadGrid();
                }
            }

            MessageBox.Show(this,
                "Weigh ticket " + ticket.TicketNumber + " saved.\r\n\r\n"
                + "Net: " + ticket.NetWeight.ToString("N0") + " kg\r\n"
                + "Total: $" + ticket.Total.ToString("N2"),
                "Weigh Ticket", MessageBoxButtons.OK, MessageBoxIcon.Information);

            this.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show(this, "Abandon this weigh ticket?  Nothing will be saved.",
                "Confirm Cancel", MessageBoxButtons.YesNo, MessageBoxIcon.Question,
                MessageBoxDefaultButton.Button2) != DialogResult.Yes)
            {
                return;
            }

            this.Close();
        }
    }
}
