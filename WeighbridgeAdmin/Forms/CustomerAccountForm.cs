using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.Windows.Forms;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraGrid.Views.Base;
using DevExpress.XtraGrid.Views.Grid;
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
    /// The three grids are DevExpress GridControls bound straight to the
    /// working lists, so an edit in a cell lands on the contact or rate object
    /// as soon as the cell is posted - there is no copy-back step.  The two
    /// lists of deleted ids are the only record that a row ever existed, and
    /// they are what makes Save able to remove anything.
    /// </summary>
    public partial class CustomerAccountForm : BaseEntryForm
    {
        private Customer _customer;
        private List<Product> _products = new List<Product>();

        // The working set.  Loaded once, edited in the grids, written on Save.
        // Re-reading these from the database on every keystroke in the search
        // box would throw away whatever had not been saved yet.  The search
        // boxes filter the views, not these lists.
        private BindingList<CustomerContact> _contacts = new BindingList<CustomerContact>();
        private BindingList<ContractRate> _rates = new BindingList<ContractRate>();

        // Rows the operator has deleted.  Nothing else remembers them.
        private List<int> _deletedContactIds = new List<int>();
        private List<int> _deletedRateIds = new List<int>();

        // Held up while the grids are being filled, so the cell handlers do
        // not mark the form dirty before the operator has touched anything.
        private bool _loading = false;

        // Made once from the grid's own font the first time a primary contact
        // is painted, rather than a new Font on every paint.
        private Font _primaryFont;

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

            // The rates grid picks its product from a lookup over the product
            // master.  The cell holds the ProductId; the lookup shows the name.
            _products = Repository.Current.GetProducts();
            this.riRateProduct.DataSource = _products;

            _contacts = new BindingList<CustomerContact>(Repository.Current.GetCustomerContacts(_customer.Id));
            _rates = new BindingList<ContractRate>(Repository.Current.GetContractRates(_customer.Id));

            this.grdContacts.DataSource = _contacts;
            this.grdRates.DataSource = _rates;

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

            this.gvContacts.OptionsBehavior.Editable = canEdit;
            this.gvContacts.OptionsView.NewItemRowPosition = canEdit ? NewItemRowPosition.Bottom : NewItemRowPosition.None;

            // Rates are a pricing decision, not customer maintenance.
            this.gvRates.OptionsBehavior.Editable = canPrice;
            this.gvRates.OptionsView.NewItemRowPosition = canPrice ? NewItemRowPosition.Bottom : NewItemRowPosition.None;

            if (!canPrice)
            {
                this.gvRates.OptionsView.EnableAppearanceEvenRow = false;
                this.gvRates.Appearance.Row.BackColor = Color.FromArgb(240, 240, 240);
                this.gvRates.Appearance.Row.Options.UseBackColor = true;
                this.tabRates.Text = "Contract &Rates (read only)";
            }
        }

        // ------------------------------------------------------------------
        // Contacts tab
        // ------------------------------------------------------------------

        /// <summary>
        /// Re-applies the search box to the grid.  The grid is bound to the
        /// working list, so this only re-runs the filter and repaints.
        /// </summary>
        private void FillContactGrid()
        {
            this.gvContacts.RefreshData();
            this.ucContactSearch.ResultCount = this.gvContacts.DataRowCount;
        }

        private void gvContacts_CustomRowFilter(object sender, RowFilterEventArgs e)
        {
            string filter = this.ucContactSearch.SearchText.ToUpper();
            if (filter.Length == 0 || e.ListSourceRow < 0 || e.ListSourceRow >= _contacts.Count)
            {
                return;
            }

            CustomerContact c = _contacts[e.ListSourceRow];
            e.Visible = c.ContactName.ToUpper().IndexOf(filter) >= 0
                || c.Position.ToUpper().IndexOf(filter) >= 0
                || c.Email.ToUpper().IndexOf(filter) >= 0;
            e.Handled = true;
        }

        private void ucContactSearch_SearchChanged(object sender, EventArgs e)
        {
            FillContactGrid();
        }

        private void gvContacts_RowStyle(object sender, RowStyleEventArgs e)
        {
            CustomerContact c = this.gvContacts.GetRow(e.RowHandle) as CustomerContact;
            if (c == null || !c.IsPrimary)
            {
                return;
            }

            if (_primaryFont == null)
            {
                _primaryFont = new Font(e.Appearance.Font, FontStyle.Bold);
            }
            e.Appearance.Font = _primaryFont;
        }

        /// <summary>
        /// Fired as soon as the operator starts typing in the new bottom row.
        /// The binding list has already made the contact object; this only
        /// ties it to the account.
        /// </summary>
        private void gvContacts_InitNewRow(object sender, InitNewRowEventArgs e)
        {
            CustomerContact fresh = this.gvContacts.GetRow(e.RowHandle) as CustomerContact;
            if (fresh == null)
            {
                return;
            }
            fresh.CustomerId = this.CustomerId;
            fresh.IsPrimary = false;
        }

        /// <summary>
        /// A check box in a grid does not post until the cell loses focus,
        /// so the primary-contact rule would not fire until the operator
        /// clicked somewhere else.  Posting here is the usual way round it.
        /// </summary>
        private void riIsPrimary_EditValueChanged(object sender, EventArgs e)
        {
            this.gvContacts.PostEditor();
        }

        private void gvContacts_ValidatingEditor(object sender, BaseContainerValidateEditorEventArgs e)
        {
            if (_loading)
            {
                return;
            }

            GridColumn column = this.gvContacts.FocusedColumn;
            if (column != this.colContactName && column != this.colContactPhone && column != this.colContactEmail)
            {
                return;
            }

            string value = Convert.ToString(e.Value).Trim();
            e.Value = value;

            if (column == this.colContactName && value.Length == 0)
            {
                e.ErrorText = "A contact must have a name.";
                e.Valid = false;
                return;
            }

            if (column == this.colContactEmail && value.Length > 0)
            {
                int at = value.IndexOf('@');
                int dot = value.LastIndexOf('.');
                if (at < 1 || dot < at + 2 || dot >= value.Length - 1 || value.IndexOf(' ') >= 0)
                {
                    e.ErrorText = "Email address is not in a valid format.";
                    e.Valid = false;
                    return;
                }
            }
        }

        /// <summary>
        /// Only one contact can be the primary, so ticking one unticks the
        /// rest.  A cross-row rule living in a cell handler, which is exactly
        /// how the old system did it.
        /// </summary>
        private void gvContacts_CellValueChanged(object sender, CellValueChangedEventArgs e)
        {
            if (_loading)
            {
                return;
            }

            if (e.Column == this.colIsPrimary && e.Value != null && Convert.ToBoolean(e.Value))
            {
                CustomerContact justSet = this.gvContacts.GetRow(e.RowHandle) as CustomerContact;

                for (int i = 0; i < _contacts.Count; i++)
                {
                    if (!object.ReferenceEquals(_contacts[i], justSet))
                    {
                        _contacts[i].IsPrimary = false;
                    }
                }

                // The objects changed underneath the grid; repaint so the
                // other rows lose their tick and their bold.
                this.gvContacts.LayoutChanged();
            }

            MarkDirty();
        }

        private void grdContacts_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Delete
                || !this.gvContacts.OptionsBehavior.Editable
                || this.gvContacts.ActiveEditor != null)
            {
                return;
            }

            int handle = this.gvContacts.FocusedRowHandle;
            if (this.gvContacts.IsNewItemRow(handle) || !this.gvContacts.IsDataRow(handle))
            {
                return;
            }

            CustomerContact c = this.gvContacts.GetRow(handle) as CustomerContact;
            if (c == null)
            {
                return;
            }

            e.Handled = true;

            if (MessageBox.Show(this,
                "Remove contact " + c.ContactName + " from this account?",
                "Delete Contact", MessageBoxButtons.YesNo, MessageBoxIcon.Question,
                MessageBoxDefaultButton.Button2) != DialogResult.Yes)
            {
                return;
            }

            // A row that was never written just disappears.  One that came out
            // of the database has to be remembered until Save.
            if (c.ContactId > 0)
            {
                _deletedContactIds.Add(c.ContactId);
            }
            this.gvContacts.DeleteRow(handle);

            this.ucContactSearch.ResultCount = this.gvContacts.DataRowCount;
            MarkDirty();
        }

        // ------------------------------------------------------------------
        // Contract rates tab
        // ------------------------------------------------------------------

        private void FillRateGrid()
        {
            this.gvRates.RefreshData();
            this.ucRateSearch.ResultCount = this.gvRates.DataRowCount;

            RecalculateRateTotals();
        }

        private void gvRates_CustomRowFilter(object sender, RowFilterEventArgs e)
        {
            string filter = this.ucRateSearch.SearchText.ToUpper();
            if (filter.Length == 0 || e.ListSourceRow < 0 || e.ListSourceRow >= _rates.Count)
            {
                return;
            }

            ContractRate r = _rates[e.ListSourceRow];
            string haystack = (r.ProductCode + " " + r.ProductName + " " + r.Notes).ToUpper();
            e.Visible = haystack.IndexOf(filter) >= 0;
            e.Handled = true;
        }

        /// <summary>
        /// The rate in force today stands out; one whose window has closed is
        /// greyed the way the ticket list greys voided rows.
        /// </summary>
        private void gvRates_RowStyle(object sender, RowStyleEventArgs e)
        {
            ContractRate r = this.gvRates.GetRow(e.RowHandle) as ContractRate;
            if (r == null || this.gvRates.IsNewItemRow(e.RowHandle))
            {
                return;
            }

            if (!r.IsInForceOn(DateTime.Today))
            {
                e.Appearance.ForeColor = Color.Gray;
            }
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

        private void gvRates_InitNewRow(object sender, InitNewRowEventArgs e)
        {
            ContractRate fresh = this.gvRates.GetRow(e.RowHandle) as ContractRate;
            if (fresh == null)
            {
                return;
            }
            fresh.CustomerId = this.CustomerId;
            fresh.EffectiveFrom = DateTime.Today;
            fresh.RatePerTonne = 0m;
        }

        private void riRateProduct_EditValueChanged(object sender, EventArgs e)
        {
            this.gvRates.PostEditor();
        }

        /// <summary>
        /// Key filter on the rate editor, so letters never get in there in the
        /// first place.  It hangs off the repository item, so every editor the
        /// grid opens in that column gets it once - nothing to take off again.
        /// </summary>
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
                Control box = sender as Control;
                if (box != null && box.Text.IndexOf('.') < 0)
                {
                    return;
                }
            }
            e.Handled = true;
        }

        /// <summary>
        /// The editors hand over text; this checks it and swaps in the typed
        /// value the model property wants, so the grid never has to convert.
        /// </summary>
        private void gvRates_ValidatingEditor(object sender, BaseContainerValidateEditorEventArgs e)
        {
            if (_loading)
            {
                return;
            }

            GridColumn column = this.gvRates.FocusedColumn;
            ContractRate r = this.gvRates.GetFocusedRow() as ContractRate;

            if (column == this.colRate)
            {
                decimal rate;
                if (!decimal.TryParse(Convert.ToString(e.Value).Trim(), out rate) || rate < 0m)
                {
                    e.ErrorText = "The rate must be a number and cannot be negative.";
                    e.Valid = false;
                    return;
                }
                e.Value = rate;
                return;
            }

            if (column == this.colEffectiveFrom)
            {
                DateTime from;
                if (!TryReadDate(e.Value, out from))
                {
                    e.ErrorText = "Effective from must be a date, for example 01/07/2025.";
                    e.Valid = false;
                    return;
                }

                if (r != null && r.EffectiveTo.HasValue && r.EffectiveTo.Value < from)
                {
                    e.ErrorText = "Effective from cannot be after effective to.";
                    e.Valid = false;
                    return;
                }
                e.Value = from;
                return;
            }

            if (column == this.colEffectiveTo)
            {
                if (Convert.ToString(e.Value).Trim().Length == 0)
                {
                    // Blank is an open ended rate.
                    e.Value = null;
                    return;
                }

                DateTime to;
                if (!TryReadDate(e.Value, out to))
                {
                    e.ErrorText = "Effective to must be a date, or blank for an open ended rate.";
                    e.Valid = false;
                    return;
                }

                if (r != null && to < r.EffectiveFrom)
                {
                    e.ErrorText = "Effective to cannot be before effective from.";
                    e.Valid = false;
                    return;
                }
                e.Value = to;
                return;
            }

            if (column == this.colRateNotes && e.Value == null)
            {
                e.Value = "";
            }
        }

        /// <summary>
        /// An editor that was opened and closed again without a change still
        /// holds the DateTime it started with, rather than text.
        /// </summary>
        private static bool TryReadDate(object value, out DateTime date)
        {
            if (value is DateTime)
            {
                date = (DateTime)value;
                return true;
            }
            return TryParseDate(Convert.ToString(value), out date);
        }

        private void gvRates_CellValueChanged(object sender, CellValueChangedEventArgs e)
        {
            if (_loading)
            {
                return;
            }

            ContractRate r = this.gvRates.GetRow(e.RowHandle) as ContractRate;
            if (r == null)
            {
                return;
            }

            // The code and name ride along with the id so the search box and
            // the overlap message can use them without going back to the list.
            if (e.Column == this.colRateProduct)
            {
                Product p = FindProduct(r.ProductId);
                if (p != null)
                {
                    r.ProductCode = p.Code;
                    r.ProductName = p.Name;
                }
            }

            // Two rate windows for the same product must not overlap.  This
            // cannot be a cell check because it depends on the other rows, so
            // it runs once the value has been written back.
            string clash;
            if (RateOverlaps(r, out clash))
            {
                this.gvRates.SetColumnError(null, clash);
                MessageBox.Show(this, clash, "Contract Rates",
                    MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            else
            {
                this.gvRates.SetColumnError(null, "");
            }

            RecalculateRateTotals();
            MarkDirty();
        }

        /// <summary>
        /// True when the rate shares a product and an overlapping date window
        /// with another rate on the account.  An open ended rate runs forever,
        /// so anything starting after it overlaps it.
        /// </summary>
        private bool RateOverlaps(ContractRate subject, out string message)
        {
            message = "";

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
        /// string the same way the ticket list builds its totals.  Counts the
        /// rows the search box has left showing.
        /// </summary>
        private void RecalculateRateTotals()
        {
            int count = 0;
            int current = 0;
            decimal cheapest = 0m;
            decimal dearest = 0m;

            for (int i = 0; i < this.gvRates.DataRowCount; i++)
            {
                ContractRate r = this.gvRates.GetRow(i) as ContractRate;
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

        private void grdRates_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Delete
                || !this.gvRates.OptionsBehavior.Editable
                || this.gvRates.ActiveEditor != null)
            {
                return;
            }

            int handle = this.gvRates.FocusedRowHandle;
            if (this.gvRates.IsNewItemRow(handle) || !this.gvRates.IsDataRow(handle))
            {
                return;
            }

            ContractRate r = this.gvRates.GetRow(handle) as ContractRate;
            if (r == null)
            {
                return;
            }

            e.Handled = true;

            if (MessageBox.Show(this,
                "Remove the " + r.ProductCode + " rate of " + r.RatePerTonne.ToString("N2")
                + " from this account?",
                "Delete Contract Rate", MessageBoxButtons.YesNo, MessageBoxIcon.Question,
                MessageBoxDefaultButton.Button2) != DialogResult.Yes)
            {
                return;
            }

            if (r.RateId > 0)
            {
                _deletedRateIds.Add(r.RateId);
            }
            this.gvRates.DeleteRow(handle);

            this.ucRateSearch.ResultCount = this.gvRates.DataRowCount;
            RecalculateRateTotals();
            MarkDirty();
        }

        // ------------------------------------------------------------------
        // Vehicles tab - read only
        // ------------------------------------------------------------------

        private void FillVehicleGrid()
        {
            List<Vehicle> vehicles = Repository.Current.GetVehiclesByCustomer(this.CustomerId);
            this.grdVehicles.DataSource = vehicles;

            this.lblVehicleHint.Text = vehicles.Count.ToString()
                + " vehicle(s) on this account.  Read only - vehicles are maintained against "
                + "the vehicle master, not the customer account.";
        }

        private void gvVehicles_RowStyle(object sender, RowStyleEventArgs e)
        {
            Vehicle v = this.gvVehicles.GetRow(e.RowHandle) as Vehicle;
            if (v != null && !v.IsActive)
            {
                e.Appearance.ForeColor = Color.Gray;
            }
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
        /// A value refused by ValidatingEditor would otherwise raise a message
        /// box asking whether to correct it.  The error icon on the cell says
        /// the same thing, and the editor stays open until it is fixed or the
        /// operator presses Esc.
        /// </summary>
        private void Grid_InvalidValueException(object sender, InvalidValueExceptionEventArgs e)
        {
            e.ExceptionMode = DevExpress.XtraEditors.Controls.ExceptionMode.NoAction;
        }

        /// <summary>
        /// Posts whatever is still open in the grid - including a half typed
        /// new row - so the working list is complete before it is checked.
        /// False when the open cell still holds a value the grid refused.
        /// </summary>
        private static bool CommitGrid(GridView view, out string error)
        {
            error = "";
            if (!view.PostEditor())
            {
                if (view.ActiveEditor != null)
                {
                    error = view.ActiveEditor.ErrorText;
                }
                return false;
            }
            if (!view.UpdateCurrentRow())
            {
                error = view.GetColumnError(null);
                return false;
            }
            return true;
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
            string error;
            if (!CommitGrid(this.gvContacts, out error))
            {
                message = "A contact row still has an error against it:\r\n\r\n" + error;
                focus = this.grdContacts;
                this.tabAccount.SelectedTab = this.tabContacts;
                return false;
            }

            if (!CommitGrid(this.gvRates, out error))
            {
                message = "A contract rate row still has an error against it:\r\n\r\n" + error;
                focus = this.grdRates;
                this.tabAccount.SelectedTab = this.tabRates;
                return false;
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

                // The grid only flags an overlap on the row being keyed, and
                // that flag goes when the focus moves on, so the whole list
                // is checked again here.
                string clash;
                if (RateOverlaps(_rates[i], out clash))
                {
                    message = "A contract rate row still has an error against it:\r\n\r\n" + clash;
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
