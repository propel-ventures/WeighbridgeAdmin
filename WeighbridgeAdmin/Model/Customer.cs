using System;

namespace WeighbridgeAdmin.Model
{
    /// <summary>
    /// Customer master record. Plain data holder - no behaviour, no INotifyPropertyChanged.
    /// </summary>
    public class Customer
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string ABN { get; set; }
        public string Address { get; set; }
        public string Suburb { get; set; }
        public string State { get; set; }
        public string Postcode { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public decimal CreditLimit { get; set; }
        public bool IsActive { get; set; }

        public Customer()
        {
            this.Code = "";
            this.Name = "";
            this.ABN = "";
            this.Address = "";
            this.Suburb = "";
            this.State = "QLD";
            this.Postcode = "";
            this.Phone = "";
            this.Email = "";
            this.CreditLimit = 0m;
            this.IsActive = true;
        }

        // Used by the customer combo box on the weigh ticket screen.
        public override string ToString()
        {
            return this.Code + " - " + this.Name;
        }
    }
}
