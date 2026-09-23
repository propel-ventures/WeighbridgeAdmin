using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using WeighbridgeAdmin.Data;
using WeighbridgeAdmin.Model;

namespace WeighbridgeAdmin.Forms
{
    /// <summary>
    /// Sign on dialog.  Shown by Program.Main before the main window.  There is
    /// no password - the weighbridge office is a shop floor terminal and picking
    /// the operator off the list is the whole flow.  On OK the SelectedUser
    /// property holds the account the caller should sign in.
    /// </summary>
    public partial class LoginForm : Form
    {
        public LoginForm()
        {
            InitializeComponent();
        }

        /// <summary>Out: the operator that was picked.  Null unless OK.</summary>
        public UserAccount SelectedUser { get; set; }

        private void LoginForm_Load(object sender, EventArgs e)
        {
            List<UserAccount> users;
            try
            {
                users = Repository.Current.GetActiveUsers();
            }
            catch (Exception ex)
            {
                MessageBox.Show(this,
                    "The operator list could not be read.\r\n\r\n" +
                    ex.Message + "\r\n\r\n" +
                    "Make sure Database\\CreateDatabase.sql has been run against this database.",
                    "Sign In", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                this.btnSignIn.Enabled = false;
                return;
            }

            if (users.Count == 0)
            {
                MessageBox.Show(this,
                    "There are no active operators set up, so nobody can sign in.",
                    "Sign In", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                this.btnSignIn.Enabled = false;
                return;
            }

            this.cboUser.DataSource = users;
            this.cboUser.DisplayMember = "FullName";
            this.cboUser.ValueMember = "UserId";
            this.cboUser.SelectedIndex = 0;

            ShowProfile();
            this.cboUser.Focus();
        }

        private void cboUser_SelectedIndexChanged(object sender, EventArgs e)
        {
            ShowProfile();
        }

        /// <summary>
        /// Shows the profile the highlighted operator works under, so it is
        /// obvious before signing in what the session will be able to do.
        /// </summary>
        private void ShowProfile()
        {
            UserAccount user = this.cboUser.SelectedItem as UserAccount;
            if (user == null)
            {
                this.lblProfileValue.Text = "-";
                return;
            }
            this.lblProfileValue.Text = user.ProfileName;
        }

        private void btnSignIn_Click(object sender, EventArgs e)
        {
            UserAccount user = this.cboUser.SelectedItem as UserAccount;
            if (user == null)
            {
                MessageBox.Show(this, "Please select an operator.", "Sign In",
                    MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            this.SelectedUser = user;
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}
