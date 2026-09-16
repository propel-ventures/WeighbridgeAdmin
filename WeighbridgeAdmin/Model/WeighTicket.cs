using System;

namespace WeighbridgeAdmin.Model
{
    /// <summary>
    /// A single weighbridge transaction.  NOTE: this object is only ever built at
    /// save time from whatever the controls happen to contain - the weigh ticket
    /// screen does NOT keep an instance of this around while the user is editing.
    /// </summary>
    public class WeighTicket
    {
        public int Id { get; set; }
        public string TicketNumber { get; set; }
        public DateTime TicketDate { get; set; }
        public int VehicleId { get; set; }
        public int CustomerId { get; set; }
        public int ProductId { get; set; }
        public decimal GrossWeight { get; set; }
        public decimal TareWeight { get; set; }
        public decimal PricePerTonne { get; set; }
        public decimal Subtotal { get; set; }
        public decimal Gst { get; set; }
        public decimal Total { get; set; }
        public string Notes { get; set; }
        public string Status { get; set; }

        public WeighTicket()
        {
            this.TicketNumber = "";
            this.TicketDate = DateTime.Today;
            this.Notes = "";
            this.Status = TicketStatus.Open;
        }

        /// <summary>Derived - net weight in kilograms.</summary>
        public decimal NetWeight
        {
            get { return this.GrossWeight - this.TareWeight; }
        }
    }

    /// <summary>
    /// Status values.  Stored as plain strings on the ticket, the way the old
    /// database column did it.
    /// </summary>
    public class TicketStatus
    {
        public const string Open = "Open";
        public const string Completed = "Completed";
        public const string Void = "Void";
    }
}
