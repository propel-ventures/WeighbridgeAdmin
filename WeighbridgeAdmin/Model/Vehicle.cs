using System;

namespace WeighbridgeAdmin.Model
{
    /// <summary>
    /// Truck / trailer combination registered against a customer.
    /// Weights are held in kilograms.
    /// </summary>
    public class Vehicle
    {
        public int Id { get; set; }
        public string Registration { get; set; }
        public string Description { get; set; }
        public decimal TareWeight { get; set; }
        public decimal MaxGross { get; set; }
        public int CustomerId { get; set; }
        public bool IsActive { get; set; }

        public Vehicle()
        {
            this.Registration = "";
            this.Description = "";
            this.TareWeight = 0m;
            this.MaxGross = 0m;
            this.CustomerId = 0;
            this.IsActive = true;
        }

        public override string ToString()
        {
            return this.Registration;
        }
    }
}
