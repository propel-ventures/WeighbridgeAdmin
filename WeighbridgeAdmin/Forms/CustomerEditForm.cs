using System;
using System.Drawing;
using System.Windows.Forms;
using WeighbridgeAdmin.Data;
using WeighbridgeAdmin.Model;

namespace WeighbridgeAdmin.Forms
{
    /// <summary>
    /// Add / edit customer dialog.  Set CustomerToEdit to an existing customer
    /// before calling ShowDialog, or leave it null to add a new one.  On OK the
    /// property holds the customer the caller should save.
    /// </summary>
    public partial class CustomerEditForm : Form
    {
        public CustomerEditForm()
        {
            InitializeComponent();
        }

        /// <summary>
        /// In: the customer being edited (null = add).  Out: the customer to save.
        /// </summary>
        public Customer CustomerToEdit { get; set; }

        private void CustomerEditForm_Load(object sender, EventArgs e)
        {
            if (this.CustomerToEdit == null)
            {
                // ADD MODE
                this.Text = "New Customer";
                this.lblHeader.Text = "New Customer";
                this.cboState.SelectedIndex = 3;
                this.txtCreditLimit.Text = "0.00";
                this.chkIsActive.Checked = true;
                this.txtCode.Focus();
                return;
            }

            // EDIT MODE - copy the record onto the controls.
            this.Text = "Edit Customer - " + this.CustomerToEdit.Code;
            this.lblHeader.Text = "Edit Customer";
            this.txtCode.Text = this.CustomerToEdit.Code;
            this.txtName.Text = this.CustomerToEdit.Name;
            this.txtAbn.Text = this.CustomerToEdit.ABN;
            this.txtAddress.Text = this.CustomerToEdit.Address;
            this.txtSuburb.Text = this.CustomerToEdit.Suburb;
            this.txtPostcode.Text = this.CustomerToEdit.Postcode;
            this.txtPhone.Text = this.CustomerToEdit.Phone;
            this.txtEmail.Text = this.CustomerToEdit.Email;
            this.txtCreditLimit.Text = this.CustomerToEdit.CreditLimit.ToString("F2");
            this.chkIsActive.Checked = this.CustomerToEdit.IsActive;

            int stateIndex = this.cboState.Items.IndexOf(this.CustomerToEdit.State);
            if (stateIndex >= 0)
            {
                this.cboState.SelectedIndex = stateIndex;
            }
            else
            {
                this.cboState.SelectedIndex = 3;
            }

            this.txtName.Focus();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            this.errCustomer.Clear();

            bool ok = true;
            decimal creditLimit = 0m;

            // --- Code ---------------------------------------------------
            if (this.txtCode.Text.Trim().Length == 0)
            {
                this.errCustomer.SetError(this.txtCode, "Code is required.");
                ok = false;
            }
            else
            {
                int ignoreId = 0;
                if (this.CustomerToEdit != null)
                {
                    ignoreId = this.CustomerToEdit.Id;
                }
                if (Repository.Current.CustomerCodeExists(this.txtCode.Text.Trim(), ignoreId))
                {
                    this.errCustomer.SetError(this.txtCode, "That code is already used by another customer.");
                    ok = false;
                }
            }

            // --- Name ---------------------------------------------------
            if (this.txtName.Text.Trim().Length == 0)
            {
                this.errCustomer.SetError(this.txtName, "Name is required.");
                ok = false;
            }

            // --- Postcode -----------------------------------------------
            string postcode = this.txtPostcode.Text.Trim();
            if (postcode.Length > 0)
            {
                bool postcodeOk = (postcode.Length == 4);
                for (int i = 0; i < postcode.Length; i++)
                {
                    if (!char.IsDigit(postcode[i]))
                    {
                        postcodeOk = false;
                    }
                }
                if (!postcodeOk)
                {
                    this.errCustomer.SetError(this.txtPostcode, "Postcode must be 4 digits.");
                    ok = false;
                }
            }

            // --- Email --------------------------------------------------
            string email = this.txtEmail.Text.Trim();
            if (email.Length > 0)
            {
                int at = email.IndexOf('@');
                int dot = email.LastIndexOf('.');
                if (at < 1 || dot < at + 2 || dot >= email.Length - 1 || email.IndexOf(' ') >= 0)
                {
                    this.errCustomer.SetError(this.txtEmail, "Email address is not in a valid format.");
                    ok = false;
                }
            }

            // --- Credit limit -------------------------------------------
            if (!decimal.TryParse(this.txtCreditLimit.Text.Trim(), out creditLimit))
            {
                this.errCustomer.SetError(this.txtCreditLimit, "Credit limit must be a number.");
                ok = false;
            }
            else if (creditLimit < 0m)
            {
                this.errCustomer.SetError(this.txtCreditLimit, "Credit limit cannot be negative.");
                ok = false;
            }

            if (!ok)
            {
                MessageBox.Show(this,
                    "One or more fields are not valid.  Hover over the red icons for details.",
                    "Validation", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            // Passed validation - write the controls back onto the record.
            if (this.CustomerToEdit == null)
            {
                this.CustomerToEdit = new Customer();
            }

            this.CustomerToEdit.Code = this.txtCode.Text.Trim();
            this.CustomerToEdit.Name = this.txtName.Text.Trim();
            this.CustomerToEdit.ABN = this.txtAbn.Text.Trim();
            this.CustomerToEdit.Address = this.txtAddress.Text.Trim();
            this.CustomerToEdit.Suburb = this.txtSuburb.Text.Trim();
            this.CustomerToEdit.State = Convert.ToString(this.cboState.SelectedItem);
            this.CustomerToEdit.Postcode = postcode;
            this.CustomerToEdit.Phone = this.txtPhone.Text.Trim();
            this.CustomerToEdit.Email = email;
            this.CustomerToEdit.CreditLimit = creditLimit;
            this.CustomerToEdit.IsActive = this.chkIsActive.Checked;

            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}
