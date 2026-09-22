using System;
using System.Drawing;
using System.Windows.Forms;

namespace WeighbridgeAdmin.Forms
{
    /// <summary>
    /// Shared shell for the maintenance screens: a header, a docked strip of
    /// Save and Close buttons, a dirty flag and the "you have unsaved changes"
    /// prompt on the way out.  A screen inherits this form in the designer and
    /// fills in the middle.
    ///
    /// The three OnXxx methods are the whole contract.  The base decides WHEN
    /// they run - on load, on Save, on close - and the derived screen decides
    /// WHAT they do.  None of that is visible in the derived form's designer
    /// file, which is the point: half of that screen's behaviour lives here.
    /// </summary>
    public partial class BaseEntryForm : Form
    {
        private bool _dirty = false;
        private bool _allowSave = true;

        public BaseEntryForm()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Set by the derived screen from the operator's privileges.  Save is
        /// only ever enabled when the record has been changed AND the operator
        /// is allowed to write it, so both have to be true - clearing the
        /// dirty flag after a save switches the button off again.
        /// </summary>
        protected bool AllowSave
        {
            get { return _allowSave; }
            set
            {
                _allowSave = value;
                UpdateSaveButton();
            }
        }

        protected bool IsDirty
        {
            get { return _dirty; }
        }

        /// <summary>Fill the controls from the database.  Runs once, on load.</summary>
        protected virtual void OnLoadRecord()
        {
        }

        /// <summary>
        /// Check the controls before a save.  Return false with a message and
        /// the control to park the cursor on.
        /// </summary>
        protected virtual bool OnValidateEntry(out string message, out Control focus)
        {
            message = "";
            focus = null;
            return true;
        }

        /// <summary>Write the controls back.  Only runs once validation passes.</summary>
        protected virtual void OnSaveRecord()
        {
        }

        /// <summary>What the message box says after a successful save.</summary>
        protected virtual string SavedMessage
        {
            get { return "Changes have been saved."; }
        }

        /// <summary>
        /// Called by the derived screen whenever anything on it changes.  Every
        /// route into this - a text box, a grid cell, a user control's own
        /// event - ends up here.
        /// </summary>
        protected void MarkDirty()
        {
            if (_dirty)
            {
                return;
            }
            _dirty = true;
            this.lblDirty.Text = "Modified - not yet saved";
            this.lblDirty.ForeColor = Color.Maroon;
            UpdateSaveButton();
        }

        protected void ClearDirty()
        {
            _dirty = false;
            this.lblDirty.Text = "No changes";
            this.lblDirty.ForeColor = Color.FromArgb(64, 64, 64);
            UpdateSaveButton();
        }

        private void UpdateSaveButton()
        {
            this.btnSave.Enabled = _dirty && _allowSave;
        }

        private void BaseEntryForm_Load(object sender, EventArgs e)
        {
            OnLoadRecord();

            // Loading the record fires every change handler the derived screen
            // has, so the dirty flag is cleared afterwards rather than before.
            ClearDirty();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            SaveRecord();
        }

        /// <summary>
        /// The save path, shared by the button and by the "save on the way
        /// out" prompt.  Returns false when the operator has to fix something.
        /// </summary>
        private bool SaveRecord()
        {
            string message;
            Control focus;

            if (!OnValidateEntry(out message, out focus))
            {
                MessageBox.Show(this, message, "Validation",
                    MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                if (focus != null)
                {
                    focus.Focus();
                }
                return false;
            }

            OnSaveRecord();
            ClearDirty();

            MessageBox.Show(this, this.SavedMessage, this.Text,
                MessageBoxButtons.OK, MessageBoxIcon.Information);
            return true;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void BaseEntryForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!_dirty)
            {
                return;
            }

            DialogResult answer = MessageBox.Show(this,
                "There are unsaved changes on this screen.\r\n\r\nSave them before closing?",
                this.Text, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question,
                MessageBoxDefaultButton.Button1);

            if (answer == DialogResult.Cancel)
            {
                e.Cancel = true;
                return;
            }

            if (answer == DialogResult.Yes)
            {
                if (!_allowSave)
                {
                    // Nothing to do - they were never allowed to save.  Tell
                    // them rather than closing as though it worked.
                    MessageBox.Show(this,
                        "Your security profile does not allow this record to be saved, " +
                        "so the changes have been discarded.",
                        this.Text, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (!SaveRecord())
                {
                    // Validation failed - stay open so it can be fixed.
                    e.Cancel = true;
                }
            }
        }
    }
}
