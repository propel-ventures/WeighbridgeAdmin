using System;

namespace WeighbridgeAdmin.Model
{
    /// <summary>
    /// One row of the ticket list.  A WeighTicket only carries the foreign key
    /// ids, so the browse screen would have to go back to the database once per
    /// row to show a registration or a customer name.  This is the same ticket
    /// with those descriptions already joined on, filled by a single query.
    ///
    /// Read only - nothing writes a summary back.  Use WeighTicket for that.
    /// </summary>
    public class WeighTicketSummary
    {
        public int Id { get; set; }
        public string TicketNumber { get; set; }
        public DateTime TicketDate { get; set; }
        public string Registration { get; set; }
        public string CustomerCode { get; set; }
        public string CustomerName { get; set; }
        public string ProductCode { get; set; }
        public string ProductName { get; set; }
        public decimal GrossWeight { get; set; }
        public decimal TareWeight { get; set; }
        public decimal NetWeight { get; set; }
        public decimal PricePerTonne { get; set; }
        public decimal Subtotal { get; set; }
        public decimal Gst { get; set; }
        public decimal Total { get; set; }
        public string Notes { get; set; }
        public string Status { get; set; }

        public WeighTicketSummary()
        {
            this.TicketNumber = "";
            this.Registration = "";
            this.CustomerCode = "";
            this.CustomerName = "";
            this.ProductCode = "";
            this.ProductName = "";
            this.Notes = "";
            this.Status = "";
        }

        /// <summary>Net weight in tonnes - what the ticket is actually charged on.</summary>
        public decimal NetTonnes
        {
            get { return this.NetWeight / 1000m; }
        }
    }
}
