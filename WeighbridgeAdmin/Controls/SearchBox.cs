using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace WeighbridgeAdmin.Controls
{
    /// <summary>
    /// Caption, search box, Clear button and a record count, which is the same
    /// strip that sits above every list in this application.  The account
    /// screen puts one on the contacts tab and another on the rates tab, so
    /// the two behave identically without the code being written twice.
    ///
    /// The control does no filtering itself - it raises SearchChanged and the
    /// host decides what that means.
    /// </summary>
    public partial class SearchBox : UserControl
    {
        private bool _suppressChange = false;

        public event EventHandler SearchChanged;

        public SearchBox()
        {
            InitializeComponent();
        }

        [Browsable(true)]
        [Category("Weighbridge")]
        [Description("Caption shown to the left of the search box.")]
        [DefaultValue("Search")]
        public string Caption
        {
            get { return this.lblCaption.Text; }
            set { this.lblCaption.Text = value; }
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string SearchText
        {
            get { return this.txtSearch.Text.Trim(); }
            set { this.txtSearch.Text = value; }
        }

        /// <summary>
        /// Set by the host after it has refilled its list.  Writing it does
        /// not raise SearchChanged, or every refill would start another one.
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public int ResultCount
        {
            set { this.lblCount.Text = value.ToString() + " record(s)"; }
        }

        /// <summary>
        /// Empties the box without raising SearchChanged, for a host that is
        /// about to reload anyway and does not want two round trips.
        /// </summary>
        public void ClearQuietly()
        {
            _suppressChange = true;
            this.txtSearch.Text = "";
            _suppressChange = false;
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            if (_suppressChange)
            {
                return;
            }
            if (this.SearchChanged != null)
            {
                this.SearchChanged(this, EventArgs.Empty);
            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            this.txtSearch.Text = "";
            this.txtSearch.Focus();
        }
    }
}
