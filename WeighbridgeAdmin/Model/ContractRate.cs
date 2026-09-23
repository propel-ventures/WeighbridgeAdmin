using System;

namespace WeighbridgeAdmin.Model
{
    /// <summary>
    /// A negotiated price per tonne for one customer and one product over a
    /// date window.  EffectiveTo is null for an open ended rate.  ProductCode
    /// and ProductName are joined on by the repository so the rates grid does
    /// not have to go back per row.
    /// </summary>
    public class ContractRate
    {
        public int RateId { get; set; }
        public int CustomerId { get; set; }
        public int ProductId { get; set; }
        public string ProductCode { get; set; }
        public string ProductName { get; set; }
        public decimal RatePerTonne { get; set; }
        public DateTime EffectiveFrom { get; set; }
        public DateTime? EffectiveTo { get; set; }
        public string Notes { get; set; }

        public ContractRate()
        {
            this.ProductCode = "";
            this.ProductName = "";
            this.RatePerTonne = 0m;
            this.EffectiveFrom = DateTime.Today;
            this.EffectiveTo = null;
            this.Notes = "";
        }

        /// <summary>
        /// True when this rate is the one in force on the given day.  Used by
        /// the account screen to colour the current row and by the overlap
        /// check while the operator is keying.
        /// </summary>
        public bool IsInForceOn(DateTime day)
        {
            if (day.Date < this.EffectiveFrom.Date)
            {
                return false;
            }
            if (this.EffectiveTo.HasValue && day.Date > this.EffectiveTo.Value.Date)
            {
                return false;
            }
            return true;
        }

        public override string ToString()
        {
            return this.ProductCode + " @ " + this.RatePerTonne.ToString("N2");
        }
    }
}
