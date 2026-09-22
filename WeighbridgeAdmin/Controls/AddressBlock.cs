using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace WeighbridgeAdmin.Controls
{
    /// <summary>
    /// The four address fields in a group box, so the same block can be
    /// dropped on a screen twice - once for the trading address and once for
    /// the postal one - instead of the labels and text boxes being copied.
    ///
    /// BlockTitle and ReadOnlyBlock are set in the designer, so their values
    /// end up in the InitializeComponent of whatever hosts the control rather
    /// than in this file.  The four value properties are hidden from the
    /// designer: they carry data, not layout, and serialising them would bake
    /// one customer's address into the form.
    /// </summary>
    public partial class AddressBlock : UserControl
    {
        private bool _readOnlyBlock = false;

        // Raised while the operator is typing.  The account screen uses it to
        // put the form into the dirty state.
        public event EventHandler AddressChanged;

        public AddressBlock()
        {
            InitializeComponent();
        }

        [Browsable(true)]
        [Category("Weighbridge")]
        [Description("Caption shown on the group box around the address fields.")]
        [DefaultValue("Address")]
        public string BlockTitle
        {
            get { return this.grpAddress.Text; }
            set { this.grpAddress.Text = value; }
        }

        [Browsable(true)]
        [Category("Weighbridge")]
        [Description("Greys the fields out and stops them being typed into.")]
        [DefaultValue(false)]
        public bool ReadOnlyBlock
        {
            get { return _readOnlyBlock; }
            set
            {
                _readOnlyBlock = value;

                this.txtAddress.ReadOnly = value;
                this.txtSuburb.ReadOnly = value;
                this.txtPostcode.ReadOnly = value;
                this.cboState.Enabled = !value;

                Color back = SystemColors.Window;
                if (value)
                {
                    back = Color.FromArgb(240, 240, 240);
                }
                this.txtAddress.BackColor = back;
                this.txtSuburb.BackColor = back;
                this.txtPostcode.BackColor = back;
            }
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string AddressLine
        {
            get { return this.txtAddress.Text.Trim(); }
            set { this.txtAddress.Text = value; }
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string Suburb
        {
            get { return this.txtSuburb.Text.Trim(); }
            set { this.txtSuburb.Text = value; }
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string State
        {
            get { return Convert.ToString(this.cboState.SelectedItem); }
            set
            {
                int index = this.cboState.Items.IndexOf(value);
                if (index < 0)
                {
                    // Unknown or blank state - fall back to Queensland, the
                    // way the customer dialog always has.
                    index = 3;
                }
                this.cboState.SelectedIndex = index;
            }
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string Postcode
        {
            get { return this.txtPostcode.Text.Trim(); }
            set { this.txtPostcode.Text = value; }
        }

        /// <summary>
        /// True when every field is empty.  The account screen uses it to
        /// decide whether a postal address is worth validating at all.
        /// </summary>
        public bool IsEmpty
        {
            get
            {
                return this.AddressLine.Length == 0
                    && this.Suburb.Length == 0
                    && this.Postcode.Length == 0;
            }
        }

        /// <summary>
        /// Postcode has to be four digits if it is filled in at all - the same
        /// rule the customer dialog applies, repeated here because there is
        /// nowhere shared to put it.
        /// </summary>
        public bool PostcodeIsValid()
        {
            string postcode = this.Postcode;
            if (postcode.Length == 0)
            {
                return true;
            }
            if (postcode.Length != 4)
            {
                return false;
            }
            for (int i = 0; i < postcode.Length; i++)
            {
                if (!char.IsDigit(postcode[i]))
                {
                    return false;
                }
            }
            return true;
        }

        /// <summary>Puts the cursor on the first field, for the error path.</summary>
        public void FocusFirstField()
        {
            this.txtAddress.Focus();
        }

        private void Field_Changed(object sender, EventArgs e)
        {
            if (this.AddressChanged != null)
            {
                this.AddressChanged(this, EventArgs.Empty);
            }
        }
    }
}
