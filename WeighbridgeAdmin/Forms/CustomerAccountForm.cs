using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Windows.Forms;
using WeighbridgeAdmin.Data;
using WeighbridgeAdmin.Model;
using WeighbridgeAdmin.Security;

namespace WeighbridgeAdmin.Forms
{
    /// <summary>
    /// Customer account maintenance - the header, the people to ring and the
    /// negotiated rates, all saved together.
    ///
    /// This screen inherits BaseEntryForm, so the Save and Close buttons, the
    /// dirty flag and the prompt on the way out are not in this file or in
    /// this screen's designer file.  Set CustomerId before calling Show.
    ///
    /// Like the weigh ticket screen, nothing here holds a tidy edited model
    /// while the operator types: the contact and rate objects hang off
    /// DataGridViewRow.Tag and are written back cell by cell in CellEndEdit.
    /// The two lists of deleted ids are the only record that a row ever
    /// existed, and they are what makes Save able to remove anything.
    /// </summary>
    public partial class CustomerAccountForm : BaseEntryForm
    {
        private Customer _customer;
        private List<Product> _products = new List<Product>();

        // The working set.  Loaded once, edited in the grids, written on Save.
        // Re-reading these from the database on every keystroke in the search
        // box would throw away whatever had not been saved yet.
        private List<CustomerContact> _contacts = new List<CustomerContact>();
        private List<ContractRate> _rates = new List<ContractRate>();

        // Rows the operator has deleted.  Nothing else remembers them.
        private List<int> _deletedContactIds = new List<int>();
        private List<int> _deletedRateIds = new List<int>();

        // Held up while the grids are being filled, so the cell handlers do
        // not mark the form dirty before the operator has touched anything.
        private bool _loading = false;

        public CustomerAccountForm()
        {
            InitializeComponent();
        }

        /// <summary>In: the customer to open.  Set this before Show.</summary>
        public int CustomerId { get; set; }

        protected override string SavedMessage
        {
            get { return "The customer account has been saved."; }
        }

        // ------------------------------------------------------------------
        // Load
        // ------------------------------------------------------------------

        protected override void OnLoadRecord()
        {
            _loading = true;

            _customer = Repository.Current.GetCustomerAccount(this.CustomerId);
            if (_customer == null)
            {
                MessageBox.Show(this, "That customer could not be read.", "Customer Account",
                    MessageBoxButtons.OK, MessageBoxIcon.Stop);
                this.AllowSave = false;
                _loading = false;
                return;
            }

            this.Text = "Customer Account - " + _customer.Code;
            this.lblHeader.Text = _customer.Code + " - " + _customer.Name;
            this.lblHeaderSub.Text = "Account details, contacts and contract rates.  "
                + "Everything on this screen is saved together.";

            this.ucHeader.LoadFrom(_customer);

            // The rates grid picks its product out of a combo column.  The
            // items are the Product objects themselves, so the cell value is
            // the master record rather than a code that has to be looked up.
            _products = Repository.Current.GetProducts();
            this.colRateProduct.Items.Clear();
            for (int i = 0; i < _products.Count; i++)
            {
                this.colRateProduct.Items.Add(_products[i]);
            }

            _contacts = Repository.Current.GetCustomerContacts(_customer.Id);
            _rates = Repository.Current.GetContractRates(_customer.Id);

            ApplyPrivileges();

            FillContactGrid();
            FillRateGrid();
            FillVehicleGrid();

            _loading = false;
        }

        /// <summary>
        /// Three different privileges land on this one screen: the header and
        /// the contacts need CUSTOMER_EDIT, the rates need the same privilege
        /// that lets an operator move a price off the price list, and Save
        /// needs CUSTOMER_EDIT as well.
        /// </summary>
        private void ApplyPrivileges()
        {
            bool canEdit = SecurityContext.HasPrivilege(Privileges.CustomerEdit);
            bool canPrice = SecurityContext.HasPrivilege(Privileges.TicketPriceOverride);

            this.AllowSave = canEdit;

            this.ucHeader.ReadOnlyHeader = !canEdit;

            this.grdContacts.ReadOnly = !canEdit;
            this.grdContacts.AllowUserToAddRows = canEdit;
            this.grdContacts.AllowUserToDeleteRows = canEdit;

            // Rates are a pricing decision, not customer maintenance.
            this.grdRates.ReadOnly = !canPrice;
            this.grdRates.AllowUserToAddRows = canPrice;
            this.grdRates.AllowUserToDeleteRows = canPrice;

            if (!canPrice)
            {
                this.grdRates.DefaultCellStyle.BackColor = Color.FromArgb(240, 240, 240);
                this.tabRates.Text = "Contract &Rates (read only)";
            }
        }

        // ------------------------------------------------------------------
        // Contacts tab
        // ------------------------------------------------------------------

        /// <summary>
        /// Paints the working list onto the grid, applying whatever is in the
        /// search box.  The contact object is parked on the row Tag so the
        /// cell handlers can write straight back onto it.
        /// </summary>
        private void FillContactGrid()
        {
            bool wasLoading = _loading;
            _loading = true;

            this.grdContacts.Rows.Clear();

            string filter = this.ucContactSearch.SearchText.ToUpper();
            int shown = 0;

            for (int i = 0; i < _contacts.Count; i++)
            {
                CustomerContact c = _contacts[i];

                if (filter.Length > 0
                    && c.ContactName.ToUpper().IndexOf(filter) < 0
                    && c.Position.ToUpper().IndexOf(filter) < 0
                    && c.Email.ToUpper().IndexOf(filter) < 0)
                {
                    continue;
                }

                int index = this.grdContacts.Rows.Add();
                DataGridViewRow row = this.grdContacts.Rows[index];

                row.Cells[this.colContactName.Index].Value = c.ContactName;
                row.Cells[this.colPosition.Index].Value = PositionOrBlank(c.Position);
                row.Cells[this.colContactPhone.Index].Value = c.Phone;
                row.Cells[this.colContactEmail.Index].Value = c.Email;
                row.Cells[this.colIsPrimary.Index].Value = c.IsPrimary;
                row.Tag = c;

                if (c.IsPrimary)
                {
                    row.DefaultCellStyle.Font = new Font("Microsoft Sans Serif", 8.25F, FontStyle.Bold);
                }

                shown++;
            }

            this.ucContactSearch.ResultCount = shown;

            _loading = wasLoading;
        }

        /// <summary>
        /// The combo column only accepts the positions it was given in the
        /// designer.  Anything else in the database - and there is nothing
        /// stopping it - comes back blank rather than raising a data error.
        /// </summary>
        private string PositionOrBlank(string position)
        {
            if (this.colPosition.Items.Contains(position))
            {
                return position;
            }
            return "";
        }

        private void ucContactSearch_SearchChanged(object sender, EventArgs e)
        {
            FillContactGrid();
        }

        /// <summary>
        /// Fired as soon as the operator starts typing in the new bottom row.
        /// The contact object is created here and pushed into the working list
        /// straight away, so every later handler can assume row.Tag is there.
        /// </summary>
        private void grdContacts_DefaultValuesNeeded(object sender, DataGridViewRowEventArgs e)
        {
            CustomerContact fresh = new CustomerContact();
            fresh.CustomerId = this.CustomerId;

            e.Row.Cells[this.colIsPrimary.Index].Value = false;
            e.Row.Tag = fresh;

            _contacts.Add(fresh);
        }

        /// <summary>
        /// A check box in a grid does not commit until the cell loses focus,
        /// so the primary-contact rule would not fire until the operator
        /// clicked somewhere else.  Committing here is the usual way round it.
        /// </summary>
        private void grdContacts_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            if (this.grdContacts.IsCurrentCellDirty
                && this.grdContacts.CurrentCell != null
                && this.grdContacts.CurrentCell.ColumnIndex == this.colIsPrimary.Index)
            {
                this.grdContacts.CommitEdit(DataGridViewDataErrorContexts.Commit);
            }
        }

        private void grdContacts_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            if (_loading || e.RowIndex < 0)
            {
                return;
            }

            DataGridViewRow row = this.grdContacts.Rows[e.RowIndex];
            if (row.IsNewRow)
            {
                return;
            }

            string value = Convert.ToString(e.FormattedValue).Trim();

            if (e.ColumnIndex == this.colContactName.Index && value.Length == 0)
            {
                row.ErrorText = "A contact must have a name.";
                e.Cancel = true;
                return;
            }

            if (e.ColumnIndex == this.colContactEmail.Index && value.Length > 0)
            {
                int at = value.IndexOf('@');
                int dot = value.LastIndexOf('.');
                if (at < 1 || dot < at + 2 || dot >= value.Length - 1 || value.IndexOf(' ') >= 0)
                {
                    row.ErrorText = "Email address is not in a valid format.";
                    e.Cancel = true;
                    return;
                }
            }

            row.ErrorText = "";
        }

        private void grdContacts_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (_loading || e.RowIndex < 0)
            {
                return;
            }

            this.grdContacts.Rows[e.RowIndex].ErrorText = "";
            ApplyContactRow(e.RowIndex);
            MarkDirty();
        }

        /// <summary>
        /// Writes one grid row back onto the contact object hanging off it.
        /// Called from CellEndEdit rather than on save, so the working list is
        /// always up to date even when the search box refills the grid.
        /// </summary>
        private void ApplyContactRow(int rowIndex)
        {
            DataGridViewRow row = this.grdContacts.Rows[rowIndex];
            CustomerContact c = row.Tag as CustomerContact;
            if (c == null)
            {
                return;
            }

            c.ContactName = Convert.ToString(row.Cells[this.colContactName.Index].Value).Trim();
            c.Position = Convert.ToString(row.Cells[this.colPosition.Index].Value).Trim();
            c.Phone = Convert.ToString(row.Cells[this.colContactPhone.Index].Value).Trim();
            c.Email = Convert.ToString(row.Cells[this.colContactEmail.Index].Value).Trim();

            object primary = row.Cells[this.colIsPrimary.Index].Value;
            c.IsPrimary = (primary != null && Convert.ToBoolean(primary));
        }

        /// <summary>
        /// Only one contact can be the primary, so ticking one unticks the
        /// rest.  A cross-row rule living in a cell handler, which is exactly
        /// how the old system did it.
        /// </summary>
        private void grdContacts_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (_loading || e.RowIndex < 0 || e.ColumnIndex != this.colIsPrimary.Index)
            {
                return;
            }

            DataGridViewRow changed = this.grdContacts.Rows[e.RowIndex];
            object value = changed.Cells[this.colIsPrimary.Index].Value;
            bool isPrimary = (value != null && Convert.ToBoolean(value));

            ApplyContactRow(e.RowIndex);

            if (isPrimary)
            {
                CustomerContact justSet = changed.Tag as CustomerContact;

                for (int i = 0; i < _contacts.Count; i++)
                {
                    if (!object.ReferenceEquals(_contacts[i], justSet))
                    {
                        _contacts[i].IsPrimary = false;
                    }
                }

                // Push the change back onto every other row on screen.
                _loading = true;
                for (int i = 0; i < this.grdContacts.Rows.Count; i++)
                {
                    DataGridViewRow row = this.grdContacts.Rows[i];
                    if (row.IsNewRow || i == e.RowIndex)
                    {
                        continue;
                    }
                    row.Cells[this.colIsPrimary.Index].Value = false;
                }
                _loading = false;
            }

            MarkDirty();
        }

        private void grdContacts_UserDeletingRow(object sender, DataGridViewRowCancelEventArgs e)
        {
            CustomerContact c = e.Row.Tag as CustomerContact;
            if (c == null)
            {
                return;
            }

            if (MessageBox.Show(this,
                "Remove contact " + c.ContactName + " from this account?",
                "Delete Contact", MessageBoxButtons.YesNo, MessageBoxIcon.Question,
                MessageBoxDefaultButton.Button2) != DialogResult.Yes)
            {
                e.Cancel = true;
                return;
            }

            // A row that was never written just disappears.  One that came out
            // of the database has to be remembered until Save.
            if (c.ContactId > 0)
            {
                _deletedContactIds.Add(c.ContactId);
            }
            _contacts.Remove(c);

            this.ucContactSearch.ResultCount = this.grdContacts.Rows.Count - 2;
            MarkDirty();
        }

        // ------------------------------------------------------------------
        // Contract rates tab
        // ------------------------------------------------------------------

        private void FillRateGrid()
        {
            bool wasLoading = _loading;
            _loading = true;

            this.grdRates.Rows.Clear();

            string filter = this.ucRateSearch.SearchText.ToUpper();
            int shown = 0;

            for (int i = 0; i < _rates.Count; i++)
            {
                ContractRate r = _rates[i];
                Product p = FindProduct(r.ProductId);

                if (filter.Length > 0)
                {
                    string haystack = (r.ProductCode + " " + r.ProductName + " " + r.Notes).ToUpper();
                    if (haystack.IndexOf(filter) < 0)
                    {
                        continue;
                    }
                }

                int index = this.grdRates.Rows.Add();
                DataGridViewRow row = this.grdRates.Rows[index];

                row.Cells[this.colRateProduct.Index].Value = p;
                row.Cells[this.colRate.Index].Value = r.RatePerTonne.ToString("N2");
                row.Cells[this.colEffectiveFrom.Index].Value = r.EffectiveFrom.ToString("dd/MM/yyyy");
                row.Cells[this.colEffectiveTo.Index].Value = FormatOptionalDate(r.EffectiveTo);
                row.Cells[this.colRateNotes.Index].Value = r.Notes;
                row.Tag = r;

                // The rate in force today stands out; one whose window has
                // closed is greyed the way the ticket list greys voided rows.
                if (r.IsInForceOn(DateTime.Today))
                {
                    row.DefaultCellStyle.ForeColor = Color.Black;
                }
                else
                {
                    row.DefaultCellStyle.ForeColor = Color.Gray;
                }

                shown++;
            }

            this.ucRateSearch.ResultCount = shown;

            _loading = wasLoading;

            RecalculateRateTotals();
        }

        private Product FindProduct(int productId)
        {
            for (int i = 0; i < _products.Count; i++)
            {
                if (_products[i].Id == productId)
                {
                    return _products[i];
                }
            }
            return null;
        }

        private static string FormatOptionalDate(DateTime? value)
        {
            if (!value.HasValue)
            {
                return "";
            }
            return value.Value.ToString("dd/MM/yyyy");
        }

        /// <summary>
        /// Dates are keyed as text.  Australian order, and a two digit year is
        /// accepted because the operators have always typed it that way.
        /// </summary>
        private static bool TryParseDate(string text, out DateTime value)
        {
            value = DateTime.MinValue;
            if (text == null)
            {
                return false;
            }
            text = text.Trim();
            if (text.Length == 0)
            {
                return false;
            }

            string[] formats = new string[] { "dd/MM/yyyy", "d/M/yyyy", "dd/MM/yy", "d/M/yy", "dd-MM-yyyy" };
            return DateTime.TryParseExact(text, formats, CultureInfo.InvariantCulture,
                DateTimeStyles.None, out value);
        }

        private void ucRateSearch_SearchChanged(object sender, EventArgs e)
        {
            FillRateGrid();
        }

        private void grdRates_DefaultValuesNeeded(object sender, DataGridViewRowEventArgs e)
        {
            ContractRate fresh = new ContractRate();
            fresh.CustomerId = this.CustomerId;
            fresh.EffectiveFrom = DateTime.Today;

            e.Row.Cells[this.colRate.Index].Value = "0.00";
            e.Row.Cells[this.colEffectiveFrom.Index].Value = DateTime.Today.ToString("dd/MM/yyyy");
            e.Row.Tag = fresh;

            _rates.Add(fresh);
        }

        private void grdRates_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            if (this.grdRates.IsCurrentCellDirty
                && this.grdRates.CurrentCell != null
                && this.grdRates.CurrentCell.ColumnIndex == this.colRateProduct.Index)
            {
                this.grdRates.CommitEdit(DataGridViewDataErrorContexts.Commit);
            }
        }

        /// <summary>
        /// Hangs a key filter on the editing box while the rate column is
        /// being typed into, so letters never get in there in the first place.
        /// The handler has to be taken off again or it accumulates - the same
        /// editing control is handed out for every cell.
        /// </summary>
        private void grdRates_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            TextBox box = e.Control as TextBox;
            if (box == null)
            {
                return;
            }

            box.KeyPress -= new KeyPressEventHandler(RateBox_KeyPress);

            if (this.grdRates.CurrentCell != null
                && this.grdRates.CurrentCell.ColumnIndex == this.colRate.Index)
            {
                box.KeyPress += new KeyPressEventHandler(RateBox_KeyPress);
            }
        }

        private void RateBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (char.IsControl(e.KeyChar))
            {
                return;
            }
            if (char.IsDigit(e.KeyChar))
            {
                return;
            }
            if (e.KeyChar == '.')
            {
                TextBox box = sender as TextBox;
                if (box != null && box.Text.IndexOf('.') < 0)
                {
                    return;
                }
            }
            e.Handled = true;
        }

        private void grdRates_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            if (_loading || e.RowIndex < 0)
            {
                return;
            }

            DataGridViewRow row = this.grdRates.Rows[e.RowIndex];
            if (row.IsNewRow)
            {
                return;
            }

            string value = Convert.ToString(e.FormattedValue).Trim();

            if (e.ColumnIndex == this.colRate.Index)
            {
                decimal rate;
                if (!decimal.TryParse(value, out rate) || rate < 0m)
                {
                    row.ErrorText = "The rate must be a number and cannot be negative.";
                    e.Cancel = true;
                    return;
                }
            }

            if (e.ColumnIndex == this.colEffectiveFrom.Index)
            {
                DateTime from;
                if (!TryParseDate(value, out from))
                {
                    row.ErrorText = "Effective from must be a date, for example 01/07/2025.";
                    e.Cancel = true;
                    return;
                }

                DateTime existingTo;
                string toText = Convert.ToString(row.Cells[this.colEffectiveTo.Index].Value);
                if (TryParseDate(toText, out existingTo) && existingTo < from)
                {
                    row.ErrorText = "Effective from cannot be after effective to.";
                    e.Cancel = true;
                    return;
                }
            }

            if (e.ColumnIndex == this.colEffectiveTo.Index && value.Length > 0)
            {
                DateTime to;
                if (!TryParseDate(value, out to))
                {
                    row.ErrorText = "Effective to must be a date, or blank for an open ended rate.";
                    e.Cancel = true;
                    return;
                }

                DateTime existingFrom;
                string fromText = Convert.ToString(row.Cells[this.colEffectiveFrom.Index].Value);
                if (TryParseDate(fromText, out existingFrom) && to < existingFrom)
                {
                    row.ErrorText = "Effective to cannot be before effective from.";
                    e.Cancel = true;
                    return;
                }
            }

            row.ErrorText = "";
        }

        private void grdRates_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (_loading || e.RowIndex < 0)
            {
                return;
            }

            this.grdRates.Rows[e.RowIndex].ErrorText = "";
            ApplyRateRow(e.RowIndex);

            // Two rate windows for the same product must not overlap.  This
            // cannot be a cell check because it depends on the other rows, so
            // it runs once the whole row has been written back.
            string clash;
            if (RowOverlaps(e.RowIndex, out clash))
            {
                this.grdRates.Rows[e.RowIndex].ErrorText = clash;
                MessageBox.Show(this, clash, "Contract Rates",
                    MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }

            RecalculateRateTotals();
            MarkDirty();
        }

        private void ApplyRateRow(int rowIndex)
        {
            DataGridViewRow row = this.grdRates.Rows[rowIndex];
            ContractRate r = row.Tag as ContractRate;
            if (r == null)
            {
                return;
            }

            Product p = row.Cells[this.colRateProduct.Index].Value as Product;
            if (p != null)
            {
                r.ProductId = p.Id;
                r.ProductCode = p.Code;
                r.ProductName = p.Name;
            }

            decimal rate = 0m;
            decimal.TryParse(Convert.ToString(row.Cells[this.colRate.Index].Value), out rate);
            r.RatePerTonne = rate;

            DateTime from;
            if (TryParseDate(Convert.ToString(row.Cells[this.colEffectiveFrom.Index].Value), out from))
            {
                r.EffectiveFrom = from;
            }

            DateTime to;
            if (TryParseDate(Convert.ToString(row.Cells[this.colEffectiveTo.Index].Value), out to))
            {
                r.EffectiveTo = to;
            }
            else
            {
                r.EffectiveTo = null;
            }

            r.Notes = Convert.ToString(row.Cells[this.colRateNotes.Index].Value);
            if (r.Notes == null)
            {
                r.Notes = "";
            }
        }

        /// <summary>
        /// True when the row shares a product and an overlapping date window
        /// with another rate on the account.  An open ended rate runs forever,
        /// so anything starting after it overlaps it.
        /// </summary>
        private bool RowOverlaps(int rowIndex, out string message)
        {
            message = "";

            ContractRate subject = this.grdRates.Rows[rowIndex].Tag as ContractRate;
            if (subject == null || subject.ProductId == 0)
            {
                return false;
            }

            for (int i = 0; i < _rates.Count; i++)
            {
                ContractRate other = _rates[i];

                if (object.ReferenceEquals(other, subject) || other.ProductId != subject.ProductId)
                {
                    continue;
                }

                DateTime subjectTo = DateTime.MaxValue;
                if (subject.EffectiveTo.HasValue)
                {
                    subjectTo = subject.EffectiveTo.Value;
                }

                DateTime otherTo = DateTime.MaxValue;
                if (other.EffectiveTo.HasValue)
                {
                    otherTo = other.EffectiveTo.Value;
                }

                if (subject.EffectiveFrom <= otherTo && other.EffectiveFrom <= subjectTo)
                {
                    message = "There is already a rate for " + subject.ProductCode
                        + " covering " + subject.EffectiveFrom.ToString("dd/MM/yyyy")
                        + ".  Close the old rate off before starting a new one.";
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Footer line under the rates grid, rebuilt as one concatenated
        /// string the same way the ticket list builds its totals.
        /// </summary>
        private void RecalculateRateTotals()
        {
            int count = 0;
            int current = 0;
            decimal cheapest = 0m;
            decimal dearest = 0m;

            for (int i = 0; i < this.grdRates.Rows.Count; i++)
            {
                DataGridViewRow row = this.grdRates.Rows[i];
                if (row.IsNewRow)
                {
                    continue;
                }

                ContractRate r = row.Tag as ContractRate;
                if (r == null)
                {
                    continue;
                }

                if (count == 0 || r.RatePerTonne < cheapest)
                {
                    cheapest = r.RatePerTonne;
                }
                if (count == 0 || r.RatePerTonne > dearest)
                {
                    dearest = r.RatePerTonne;
                }

                count++;
                if (r.IsInForceOn(DateTime.Today))
                {
                    current++;
                }
            }

            if (count == 0)
            {
                this.lblRateTotals.Text = "No contract rates - this customer is charged the price list rate.";
                return;
            }

            this.lblRateTotals.Text = count.ToString() + " rate(s) shown    "
                + current.ToString() + " in force today    "
                + "Lowest " + cheapest.ToString("N2") + "    "
                + "Highest " + dearest.ToString("N2");
        }

        private void grdRates_UserDeletingRow(object sender, DataGridViewRowCancelEventArgs e)
        {
            ContractRate r = e.Row.Tag as ContractRate;
            if (r == null)
            {
                return;
            }

            if (MessageBox.Show(this,
                "Remove the " + r.ProductCode + " rate of " + r.RatePerTonne.ToString("N2")
                + " from this account?",
                "Delete Contract Rate", MessageBoxButtons.YesNo, MessageBoxIcon.Question,
                MessageBoxDefaultButton.Button2) != DialogResult.Yes)
            {
                e.Cancel = true;
                return;
            }

            if (r.RateId > 0)
            {
                _deletedRateIds.Add(r.RateId);
            }
            _rates.Remove(r);

            MarkDirty();
        }

        // ------------------------------------------------------------------
        // Vehicles tab - read only
        // ------------------------------------------------------------------

        private void FillVehicleGrid()
        {
            this.grdVehicles.Rows.Clear();

            List<Vehicle> vehicles = Repository.Current.GetVehiclesByCustomer(this.CustomerId);
            for (int i = 0; i < vehicles.Count; i++)
            {
                Vehicle v = vehicles[i];

                int index = this.grdVehicles.Rows.Add();
                DataGridViewRow row = this.grdVehicles.Rows[index];

                row.Cells[this.colVehRegistration.Index].Value = v.Registration;
                row.Cells[this.colVehDescription.Index].Value = v.Description;
                row.Cells[this.colVehTare.Index].Value = v.TareWeight.ToString("N0");
                row.Cells[this.colVehMaxGross.Index].Value = v.MaxGross.ToString("N0");
                row.Cells[this.colVehActive.Index].Value = v.IsActive;
                row.Tag = v;

                if (!v.IsActive)
                {
                    row.DefaultCellStyle.ForeColor = Color.Gray;
                }
            }

            this.lblVehicleHint.Text = vehicles.Count.ToString()
                + " vehicle(s) on this account.  Read only - vehicles are maintained against "
                + "the vehicle master, not the customer account.";
        }

        // ------------------------------------------------------------------
        // Shared
        // ------------------------------------------------------------------

        private void tabAccount_SelectedIndexChanged(object sender, EventArgs e)
        {
            // The rates footer is rebuilt whenever the tab comes to the front,
            // the same way the weigh ticket review page re-reads its controls.
            if (this.tabAccount.SelectedTab == this.tabRates)
            {
                RecalculateRateTotals();
            }
        }

        private void ucHeader_HeaderChanged(object sender, EventArgs e)
        {
            if (_loading)
            {
                return;
            }
            MarkDirty();
        }

        /// <summary>
        /// A combo cell whose value is not one of its items raises this rather
        /// than throwing.  It happens while rows are being rebuilt, and there
        /// is nothing useful to tell the operator, so it is swallowed.
        /// </summary>
        private void Grid_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            e.ThrowException = false;
        }

        // ------------------------------------------------------------------
        // Validate and save - called by BaseEntryForm
        // ------------------------------------------------------------------

        protected override bool OnValidateEntry(out string message, out Control focus)
        {
            message = "";
            focus = null;

            if (_customer == null)
            {
                message = "There is no customer loaded on this screen.";
                return false;
            }

            if (!this.ucHeader.ValidateHeader(out message, out focus))
            {
                return false;
            }

            // Anything the grids refused to accept is still sitting there with
            // its error icon showing.
            for (int i = 0; i < this.grdContacts.Rows.Count; i++)
            {
                if (this.grdContacts.Rows[i].ErrorText.Length > 0)
                {
                    message = "A contact row still has an error against it:\r\n\r\n"
                        + this.grdContacts.Rows[i].ErrorText;
                    focus = this.grdContacts;
                    this.tabAccount.SelectedTab = this.tabContacts;
                    return false;
                }
            }

            for (int i = 0; i < this.grdRates.Rows.Count; i++)
            {
                if (this.grdRates.Rows[i].ErrorText.Length > 0)
                {
                    message = "A contract rate row still has an error against it:\r\n\r\n"
                        + this.grdRates.Rows[i].ErrorText;
                    focus = this.grdRates;
                    this.tabAccount.SelectedTab = this.tabRates;
                    return false;
                }
            }

            // Cross-row rules that only make sense over the whole list.
            int primaries = 0;
            for (int i = 0; i < _contacts.Count; i++)
            {
                if (_contacts[i].ContactName.Trim().Length == 0)
                {
                    message = "One of the contacts has no name.  Remove the row or fill it in.";
                    focus = this.grdContacts;
                    this.tabAccount.SelectedTab = this.tabContacts;
                    return false;
                }
                if (_contacts[i].IsPrimary)
                {
                    primaries++;
                }
            }

            if (_contacts.Count > 0 && primaries == 0)
            {
                message = "One contact has to be marked as the primary contact.";
                focus = this.grdContacts;
                this.tabAccount.SelectedTab = this.tabContacts;
                return false;
            }

            for (int i = 0; i < _rates.Count; i++)
            {
                if (_rates[i].ProductId == 0)
                {
                    message = "One of the contract rates has no product against it.";
                    focus = this.grdRates;
                    this.tabAccount.SelectedTab = this.tabRates;
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Header, then contacts, then rates, then the two lists of things the
        /// operator deleted.  No transaction spans the three - each repository
        /// call opens and closes its own connection, exactly as the rest of
        /// this application does.
        /// </summary>
        protected override void OnSaveRecord()
        {
            this.ucHeader.ApplyTo(_customer);
            Repository.Current.SaveCustomerAccount(_customer);

            for (int i = 0; i < _deletedContactIds.Count; i++)
            {
                Repository.Current.DeleteContact(_deletedContactIds[i]);
            }
            _deletedContactIds.Clear();

            for (int i = 0; i < _contacts.Count; i++)
            {
                Repository.Current.SaveContact(_contacts[i]);
            }

            for (int i = 0; i < _deletedRateIds.Count; i++)
            {
                Repository.Current.DeleteContractRate(_deletedRateIds[i]);
            }
            _deletedRateIds.Clear();

            for (int i = 0; i < _rates.Count; i++)
            {
                Repository.Current.SaveContractRate(_rates[i]);
            }

            // Repaint from the working list so the new identities that came
            // back from the inserts are on the rows.
            FillContactGrid();
            FillRateGrid();

            this.lblHeader.Text = _customer.Code + " - " + _customer.Name;

            // If the customer list is open behind this, it is now stale.
            MainForm main = this.MdiParent as MainForm;
            if (main != null)
            {
                for (int i = 0; i < main.MdiChildren.Length; i++)
                {
                    CustomerListForm list = main.MdiChildren[i] as CustomerListForm;
                    if (list != null)
                    {
                        list.ReloadGrid();
                    }
                }
            }
        }
    }
}
