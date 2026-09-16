using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using WeighbridgeAdmin.Data;
using WeighbridgeAdmin.Model;

namespace WeighbridgeAdmin.Forms
{
    /// <summary>
    /// The standard "F4 lookup" dialog.  Caller optionally seeds InitialSearchText,
    /// calls ShowDialog and reads SelectedVehicle when the result is OK.
    /// </summary>
    public partial class VehicleLookupDialog : Form
    {
        public VehicleLookupDialog()
        {
            InitializeComponent();
        }

        /// <summary>Vehicle the operator picked.  Only valid when DialogResult is OK.</summary>
        public Vehicle SelectedVehicle { get; set; }

        /// <summary>Optional text to pre-load the search box with.</summary>
        public string InitialSearchText { get; set; }

        private void VehicleLookupDialog_Load(object sender, EventArgs e)
        {
            if (this.InitialSearchText != null)
            {
                this.txtSearch.Text = this.InitialSearchText.Trim();
            }

            FillGrid();
            this.txtSearch.Focus();
            this.txtSearch.SelectAll();
        }

        /// <summary>
        /// Clears the grid and re-adds a row per matching vehicle.  The Vehicle
        /// itself is parked on the row Tag so Select can pull it straight back out.
        /// </summary>
        private void FillGrid()
        {
            this.grdVehicles.Rows.Clear();

            List<Vehicle> matches = Repository.Current.SearchVehicles(this.txtSearch.Text);

            for (int i = 0; i < matches.Count; i++)
            {
                Vehicle v = matches[i];

                string customerName = "";
                Customer c = Repository.Current.GetCustomerById(v.CustomerId);
                if (c != null)
                {
                    customerName = c.Name;
                }

                string status = "Active";
                if (!v.IsActive)
                {
                    status = "Inactive";
                }

                int rowIndex = this.grdVehicles.Rows.Add(
                    v.Registration,
                    v.Description,
                    v.TareWeight.ToString("N0"),
                    v.MaxGross.ToString("N0"),
                    customerName,
                    status);

                this.grdVehicles.Rows[rowIndex].Tag = v;

                if (!v.IsActive)
                {
                    this.grdVehicles.Rows[rowIndex].DefaultCellStyle.ForeColor = Color.Gray;
                }
            }

            this.lblHint.Text = matches.Count.ToString()
                + " vehicle(s) found.  Double-click a row to select the vehicle.";

            this.btnSelect.Enabled = (matches.Count > 0);
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            FillGrid();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            FillGrid();
        }

        private void grdVehicles_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
            {
                return;
            }
            btnSelect_Click(sender, EventArgs.Empty);
        }

        private void btnSelect_Click(object sender, EventArgs e)
        {
            if (this.grdVehicles.CurrentRow == null)
            {
                MessageBox.Show(this, "Please select a vehicle from the list.", "Vehicle Lookup",
                    MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            Vehicle v = this.grdVehicles.CurrentRow.Tag as Vehicle;
            if (v == null)
            {
                MessageBox.Show(this, "Please select a vehicle from the list.", "Vehicle Lookup",
                    MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            if (!v.IsActive)
            {
                if (MessageBox.Show(this,
                    "Vehicle " + v.Registration + " is marked as INACTIVE.\r\n\r\nUse it anyway?",
                    "Vehicle Lookup", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation,
                    MessageBoxDefaultButton.Button2) != DialogResult.Yes)
                {
                    return;
                }
            }

            this.SelectedVehicle = v;
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.SelectedVehicle = null;
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}
