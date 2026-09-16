using System;

namespace WeighbridgeAdmin.Model
{
    /// <summary>
    /// Sellable material (quarry product, waste stream, etc).
    /// </summary>
    public class Product
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public decimal PricePerTonne { get; set; }
        public bool GstApplicable { get; set; }

        public Product()
        {
            this.Code = "";
            this.Name = "";
            this.PricePerTonne = 0m;
            this.GstApplicable = true;
        }

        public override string ToString()
        {
            return this.Code + " - " + this.Name;
        }
    }
}
