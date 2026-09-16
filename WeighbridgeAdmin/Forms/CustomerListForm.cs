using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using WeighbridgeAdmin.Data;
using WeighbridgeAdmin.Model;

namespace WeighbridgeAdmin.Forms
{
    /// <summary>
    /// Customer browse screen.  Straight list / toolbar / search box - all of
    /// the work happens in the control event handlers.
    /// </summary>
    public partial class CustomerListForm : Form
    {
        public CustomerListForm()
        {
            InitializeComponent();
        }

        private void CustomerListForm_Load(object sender, EventArgs e)
        {
            ReloadGrid();
        }

        /// <summary>
        /// Re-reads the customer list, applying whatever is in the search box.
        /// Called from the toolbar, after an edit and by the main form.
        /// </summary>
        public void ReloadGrid()
        {
            List<Customer> list = Repository.Current.SearchCustomers(this.txtSearch.Text);

            this.bsCustomers.DataSource = list;
            this.bsCustomers.ResetBindings(false);

            this.lblRecordCount.Text = list.Count.ToString() + " record(s)";

            // Toolbar buttons only make sense when there is a row.
            this.tbbEdit.Enabled = (list.Count > 0);
            this.tbbDelete.Enabled = (list.Count > 0);
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            // Filter as the user types - the list is small enough that we can
            // get away with re-filtering on every keystroke.
            ReloadGrid();
        }

        private Customer GetSelectedCustomer()
        {
            if (this.grdCustomers.CurrentRow == null)
            {
                return null;
            }
            return this.grdCustomers.CurrentRow.DataBoundItem as Customer;
        }

        private void tbbNew_Click(object sender, EventArgs e)
        {
            CustomerEditForm dlg = new CustomerEditForm();
            dlg.CustomerToEdit = null;
            if (dlg.ShowDialog(this) == DialogResult.OK)
            {
                Repository.Current.SaveCustomer(dlg.CustomerToEdit);
                ReloadGrid();
            }
            dlg.Dispose();
        }

        private void tbbEdit_Click(object sender, EventArgs e)
        {
            Customer selected = GetSelectedCustomer();
            if (selected == null)
            {
                MessageBox.Show(this, "Please select a customer first.", "Edit Customer",
                    MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            CustomerEditForm dlg = new CustomerEditForm();
            dlg.CustomerToEdit = selected;
            if (dlg.ShowDialog(this) == DialogResult.OK)
            {
                Repository.Current.SaveCustomer(dlg.CustomerToEdit);
                ReloadGrid();
            }
            dlg.Dispose();
        }

        private void tbbDelete_Click(object sender, EventArgs e)
        {
            Customer selected = GetSelectedCustomer();
            if (selected == null)
            {
                MessageBox.Show(this, "Please select a customer first.", "Delete Customer",
                    MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            if (Repository.Current.CustomerIsInUse(selected.Id))
            {
                MessageBox.Show(this,
                    "Customer " + selected.Code + " cannot be deleted because vehicles or weigh tickets still reference it.\r\n\r\n" +
                    "Mark the customer as inactive instead.",
                    "Delete Customer", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            }

            if (MessageBox.Show(this,
                "Delete customer " + selected.Code + " - " + selected.Name + "?\r\n\r\nThis cannot be undone.",
                "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Question,
                MessageBoxDefaultButton.Button2) != DialogResult.Yes)
            {
                return;
            }

            Repository.Current.DeleteCustomer(selected.Id);
            ReloadGrid();
        }

        private void tbbRefresh_Click(object sender, EventArgs e)
        {
            this.txtSearch.Text = "";
            ReloadGrid();
        }

        private void grdCustomers_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
            {
                // Header row was double clicked - ignore it.
                return;
            }
            tbbEdit_Click(sender, EventArgs.Empty);
        }

        private void CustomerListForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F5)
            {
                tbbRefresh_Click(sender, EventArgs.Empty);
                e.Handled = true;
            }
            else if (e.KeyCode == Keys.F2)
            {
                tbbEdit_Click(sender, EventArgs.Empty);
                e.Handled = true;
            }
            else if (e.KeyCode == Keys.Insert)
            {
                tbbNew_Click(sender, EventArgs.Empty);
                e.Handled = true;
            }
        }
    }
}
