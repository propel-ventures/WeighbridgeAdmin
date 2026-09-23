using System;

namespace WeighbridgeAdmin.Security
{
    /// <summary>
    /// Privilege names exactly as they are stored in the PrivilegeName column
    /// of dbo.SecurityProfilePrivileges.  Spelling them out once here keeps the
    /// screens from carrying the wire names around as loose string literals.
    /// </summary>
    public static class Privileges
    {
        public const string CustomerView = "CUSTOMER_VIEW";
        public const string CustomerEdit = "CUSTOMER_EDIT";
        public const string CustomerDelete = "CUSTOMER_DELETE";
        public const string VehicleView = "VEHICLE_VIEW";
        public const string TicketView = "TICKET_VIEW";
        public const string TicketCreate = "TICKET_CREATE";
        public const string TicketPriceOverride = "TICKET_PRICE_OVERRIDE";
    }
}
