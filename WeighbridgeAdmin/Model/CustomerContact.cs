using System;

namespace WeighbridgeAdmin.Model
{
    /// <summary>
    /// Someone to ring at a customer.  Plain data holder.  A contact with
    /// ContactId 0 has not been written yet - the account screen leaves new
    /// grid rows that way until Save.
    /// </summary>
    public class CustomerContact
    {
        public int ContactId { get; set; }
        public int CustomerId { get; set; }
        public string ContactName { get; set; }
        public string Position { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public bool IsPrimary { get; set; }

        public CustomerContact()
        {
            this.ContactName = "";
            this.Position = "";
            this.Phone = "";
            this.Email = "";
            this.IsPrimary = false;
        }

        public override string ToString()
        {
            return this.ContactName;
        }
    }
}
