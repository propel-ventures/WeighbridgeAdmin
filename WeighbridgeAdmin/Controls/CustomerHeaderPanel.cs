using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using WeighbridgeAdmin.Model;

namespace WeighbridgeAdmin.Controls
{
    /// <summary>
    /// The top half of the customer account screen: the account fields plus
    /// two AddressBlocks, so this is a user control that is itself built out
    /// of user controls.
    ///
    /// Validation lives here rather than on the form, because the form cannot
    /// see the individual text boxes - it only has the panel.  ValidateHeader
    /// hands back a message and the control to park the cursor on, which is
    /// the same shape the weigh ticket wizard uses for its per-page checks.
    /// </summary>
    public partial class CustomerHeaderPanel : UserControl
    {
        // Raised on any change, including a change inside either address
        // block.  Stops firing while LoadFrom is filling the fields.
        public event EventHandler HeaderChanged;

        private bool _loading = false;
        private bool _readOnlyHeader = false;

        public CustomerHeaderPanel()
        {
            InitializeComponent();
        }

        [Browsable(true)]
        [Category("Weighbridge")]
        [Description("Greys the whole header, including both address blocks.")]
        [DefaultValue(false)]
        public bool ReadOnlyHeader
        {
            get { return _readOnlyHeader; }
            set
            {
                _readOnlyHeader = value;

                this.txtName.ReadOnly = value;
                this.txtAbn.ReadOnly = value;
                this.txtPhone.ReadOnly = value;
                this.txtEmail.ReadOnly = value;
                this.txtCreditLimit.ReadOnly = value;
                this.chkIsActive.Enabled = !value;

                Color back = SystemColors.Window;
                if (value)
                {
                    back = Color.FromArgb(240, 240, 240);
                }
                this.txtName.BackColor = back;
                this.txtAbn.BackColor = back;
                this.txtPhone.BackColor = back;
                this.txtEmail.BackColor = back;
                this.txtCreditLimit.BackColor = back;

                // Cascade down into the two address blocks.
                this.ucTradingAddress.ReadOnlyBlock = value;
                this.ucPostalAddress.ReadOnlyBlock = value;
            }
        }

        /// <summary>
        /// Copies the record onto the controls.  The change event is held down
        /// throughout, or simply opening a customer would mark the form dirty.
        /// </summary>
        public void LoadFrom(Customer customer)
        {
            _loading = true;

            this.txtCode.Text = customer.Code;
            this.txtName.Text = customer.Name;
            this.txtAbn.Text = customer.ABN;
            this.txtPhone.Text = customer.Phone;
            this.txtEmail.Text = customer.Email;
            this.txtCreditLimit.Text = customer.CreditLimit.ToString("F2");
            this.chkIsActive.Checked = customer.IsActive;

            this.ucTradingAddress.AddressLine = customer.Address;
            this.ucTradingAddress.Suburb = customer.Suburb;
            this.ucTradingAddress.State = customer.State;
            this.ucTradingAddress.Postcode = customer.Postcode;

            this.ucPostalAddress.AddressLine = customer.PostalAddress;
            this.ucPostalAddress.Suburb = customer.PostalSuburb;
            this.ucPostalAddress.State = customer.PostalState;
            this.ucPostalAddress.Postcode = customer.PostalPostcode;

            _loading = false;
        }

        /// <summary>
        /// Reads the controls back onto the record.  Called only after
        /// ValidateHeader has passed, so the credit limit is known to parse.
        /// </summary>
        public void ApplyTo(Customer customer)
        {
            customer.Name = this.txtName.Text.Trim();
            customer.ABN = this.txtAbn.Text.Trim();
            customer.Phone = this.txtPhone.Text.Trim();
            customer.Email = this.txtEmail.Text.Trim();
            customer.IsActive = this.chkIsActive.Checked;

            decimal creditLimit = 0m;
            decimal.TryParse(this.txtCreditLimit.Text.Trim(), out creditLimit);
            customer.CreditLimit = creditLimit;

            customer.Address = this.ucTradingAddress.AddressLine;
            customer.Suburb = this.ucTradingAddress.Suburb;
            customer.State = this.ucTradingAddress.State;
            customer.Postcode = this.ucTradingAddress.Postcode;

            customer.PostalAddress = this.ucPostalAddress.AddressLine;
            customer.PostalSuburb = this.ucPostalAddress.Suburb;
            customer.PostalState = this.ucPostalAddress.State;
            customer.PostalPostcode = this.ucPostalAddress.Postcode;
        }

        /// <summary>
        /// Returns false and fills in the message and the control to focus.
        /// The postal block is only checked when something has been keyed into
        /// it - an empty postal address is normal and means "bill to trading".
        /// </summary>
        public bool ValidateHeader(out string message, out Control focus)
        {
            message = "";
            focus = null;

            if (this.txtName.Text.Trim().Length == 0)
            {
                message = "Customer name is required.";
                focus = this.txtName;
                return false;
            }

            string email = this.txtEmail.Text.Trim();
            if (email.Length > 0)
            {
                int at = email.IndexOf('@');
                int dot = email.LastIndexOf('.');
                if (at < 1 || dot < at + 2 || dot >= email.Length - 1 || email.IndexOf(' ') >= 0)
                {
                    message = "Email address is not in a valid format.";
                    focus = this.txtEmail;
                    return false;
                }
            }

            decimal creditLimit = 0m;
            if (!decimal.TryParse(this.txtCreditLimit.Text.Trim(), out creditLimit))
            {
                message = "Credit limit must be a number.";
                focus = this.txtCreditLimit;
                return false;
            }
            if (creditLimit < 0m)
            {
                message = "Credit limit cannot be negative.";
                focus = this.txtCreditLimit;
                return false;
            }

            if (!this.ucTradingAddress.PostcodeIsValid())
            {
                message = "The trading address postcode must be 4 digits.";
                focus = this.ucTradingAddress;
                return false;
            }

            if (!this.ucPostalAddress.IsEmpty)
            {
                if (this.ucPostalAddress.Suburb.Length == 0)
                {
                    message = "A postal address has been entered without a suburb.";
                    focus = this.ucPostalAddress;
                    return false;
                }
                if (!this.ucPostalAddress.PostcodeIsValid())
                {
                    message = "The postal address postcode must be 4 digits.";
                    focus = this.ucPostalAddress;
                    return false;
                }
            }

            return true;
        }

        private void Field_Changed(object sender, EventArgs e)
        {
            if (_loading)
            {
                return;
            }
            if (this.HeaderChanged != null)
            {
                this.HeaderChanged(this, EventArgs.Empty);
            }
        }
    }
}
