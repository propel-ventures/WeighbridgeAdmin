using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using WeighbridgeAdmin.Model;
using WeighbridgeAdmin.Security;

namespace WeighbridgeAdmin.Data
{
    /// <summary>
    /// Data access layer.  Straight ADO.NET against SQL Server - a connection is
    /// opened per call, the SQL sits inline as string constants and the readers
    /// are mapped onto the model classes by hand.  No ORM, no unit of work, no
    /// caching.  Forms reach in through Repository.Current.
    /// </summary>
    public class Repository
    {
        /// <summary>Rate used when the GstRate app setting is not present.</summary>
        private const decimal DefaultGstRate = 0.10m;

        private static Repository _current;

        private string _connectionString;
        private decimal _gstRate;

        private Repository()
        {
            ConnectionStringSettings setting = ConfigurationManager.ConnectionStrings["WeighbridgeDb"];
            if (setting == null)
            {
                throw new ConfigurationErrorsException(
                    "Connection string 'WeighbridgeDb' is missing from the configuration file.");
            }
            _connectionString = setting.ConnectionString;
            _gstRate = ReadGstRate();
        }

        public static Repository Current
        {
            get
            {
                if (_current == null)
                {
                    _current = new Repository();
                }
                return _current;
            }
        }

        /// <summary>
        /// The signed on operator, the way the status bar has always shown it -
        /// NAME in capitals with the security profile in brackets after it.
        /// Empty until the sign on dialog has been through, which only happens
        /// if something asks before Program.Main has got that far.
        /// </summary>
        public string CurrentUserName
        {
            get
            {
                if (SecurityContext.CurrentUser == null)
                {
                    return "(not signed on)";
                }
                return SecurityContext.FullName.ToUpper() + " (" + SecurityContext.ProfileName + ")";
            }
        }

        /// <summary>
        /// G.S.T. rate as a fraction - 0.10 means 10 per cent.  Read once from
        /// the GstRate app setting when the repository is created.
        /// </summary>
        public decimal GstRate
        {
            get { return _gstRate; }
        }

        /// <summary>
        /// The rate the way it reads on screen, e.g. "10%".  Trailing zeroes are
        /// dropped so 0.10 shows as 10% rather than 10.00%.
        /// </summary>
        public string GstRateCaption
        {
            get { return (_gstRate * 100m).ToString("0.##", CultureInfo.CurrentCulture) + "%"; }
        }

        /// <summary>
        /// A missing setting falls back to the long standing 10 per cent, so
        /// configuration files written before the setting existed still work.
        /// A setting that IS present but unusable is an error rather than a
        /// silent fallback - quietly charging the wrong tax would be worse.
        /// </summary>
        private static decimal ReadGstRate()
        {
            string value = ConfigurationManager.AppSettings["GstRate"];
            if (string.IsNullOrEmpty(value) || value.Trim().Length == 0)
            {
                return DefaultGstRate;
            }

            decimal rate;
            if (!decimal.TryParse(value.Trim(), NumberStyles.Number, CultureInfo.InvariantCulture, out rate)
                || rate < 0m || rate > 1m)
            {
                throw new ConfigurationErrorsException(
                    "The GstRate app setting must be a decimal fraction between 0 and 1 - " +
                    "for example 0.10 for 10 per cent.  Found: '" + value + "'.");
            }

            return rate;
        }

        // ------------------------------------------------------------------
        // Plumbing
        // ------------------------------------------------------------------

        private SqlConnection OpenConnection()
        {
            SqlConnection cn = new SqlConnection(_connectionString);
            cn.Open();
            return cn;
        }

        /// <summary>
        /// Called from Program.Main before the main form is shown, so the user
        /// gets a sensible message instead of an unhandled exception dialog.
        /// </summary>
        public bool TestConnection(out string errorMessage)
        {
            errorMessage = "";
            try
            {
                using (SqlConnection cn = OpenConnection())
                {
                    SqlCommand cmd = new SqlCommand(
                        "SELECT COUNT(*) FROM dbo.Customers", cn);
                    cmd.ExecuteScalar();
                }
                return true;
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                return false;
            }
        }

        private static string GetString(SqlDataReader dr, string column)
        {
            object value = dr[column];
            if (value == DBNull.Value)
            {
                return "";
            }
            return Convert.ToString(value);
        }

        // ------------------------------------------------------------------
        // Customers
        // ------------------------------------------------------------------

        private const string CustomerColumns =
            "Id, Code, Name, ABN, Address, Suburb, State, Postcode, Phone, Email, CreditLimit, IsActive";

        /// <summary>
        /// The same customer with the postal address on the end.  Only the
        /// account screen reads these four, and only SaveCustomerAccount
        /// writes them - SaveCustomer, which the older edit dialog calls,
        /// leaves them alone rather than blanking them.
        /// </summary>
        private const string CustomerAccountColumns =
            CustomerColumns + ", PostalAddress, PostalSuburb, PostalState, PostalPostcode";

        private static Customer ReadCustomer(SqlDataReader dr)
        {
            Customer c = new Customer();
            c.Id = Convert.ToInt32(dr["Id"]);
            c.Code = GetString(dr, "Code");
            c.Name = GetString(dr, "Name");
            c.ABN = GetString(dr, "ABN");
            c.Address = GetString(dr, "Address");
            c.Suburb = GetString(dr, "Suburb");
            c.State = GetString(dr, "State");
            c.Postcode = GetString(dr, "Postcode");
            c.Phone = GetString(dr, "Phone");
            c.Email = GetString(dr, "Email");
            c.CreditLimit = Convert.ToDecimal(dr["CreditLimit"]);
            c.IsActive = Convert.ToBoolean(dr["IsActive"]);
            return c;
        }

        private static Customer ReadCustomerAccount(SqlDataReader dr)
        {
            Customer c = ReadCustomer(dr);
            c.PostalAddress = GetString(dr, "PostalAddress");
            c.PostalSuburb = GetString(dr, "PostalSuburb");
            c.PostalState = GetString(dr, "PostalState");
            c.PostalPostcode = GetString(dr, "PostalPostcode");
            return c;
        }

        public List<Customer> Customers
        {
            get { return GetCustomers(); }
        }

        public List<Customer> GetCustomers()
        {
            List<Customer> results = new List<Customer>();

            using (SqlConnection cn = OpenConnection())
            {
                SqlCommand cmd = new SqlCommand(
                    "SELECT " + CustomerColumns + " FROM dbo.Customers ORDER BY Code", cn);

                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        results.Add(ReadCustomer(dr));
                    }
                }
            }
            return results;
        }

        public Customer GetCustomerById(int id)
        {
            using (SqlConnection cn = OpenConnection())
            {
                SqlCommand cmd = new SqlCommand(
                    "SELECT " + CustomerColumns + " FROM dbo.Customers WHERE Id = @Id", cn);
                cmd.Parameters.AddWithValue("@Id", id);

                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    if (dr.Read())
                    {
                        return ReadCustomer(dr);
                    }
                }
            }
            return null;
        }

        public List<Customer> SearchCustomers(string text)
        {
            if (text == null)
            {
                text = "";
            }
            text = text.Trim();

            List<Customer> results = new List<Customer>();

            using (SqlConnection cn = OpenConnection())
            {
                SqlCommand cmd = new SqlCommand(
                    "SELECT " + CustomerColumns + " FROM dbo.Customers " +
                    " WHERE (@Text = '' OR Code LIKE @Like OR Name LIKE @Like) " +
                    " ORDER BY Code", cn);
                cmd.Parameters.AddWithValue("@Text", text);
                cmd.Parameters.AddWithValue("@Like", "%" + text + "%");

                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        results.Add(ReadCustomer(dr));
                    }
                }
            }
            return results;
        }

        public bool CustomerCodeExists(string code, int ignoreId)
        {
            using (SqlConnection cn = OpenConnection())
            {
                SqlCommand cmd = new SqlCommand(
                    "SELECT COUNT(*) FROM dbo.Customers WHERE Code = @Code AND Id <> @IgnoreId", cn);
                cmd.Parameters.AddWithValue("@Code", code);
                cmd.Parameters.AddWithValue("@IgnoreId", ignoreId);

                return Convert.ToInt32(cmd.ExecuteScalar()) > 0;
            }
        }

        public void SaveCustomer(Customer customer)
        {
            using (SqlConnection cn = OpenConnection())
            {
                SqlCommand cmd;

                if (customer.Id == 0)
                {
                    cmd = new SqlCommand(
                        "INSERT INTO dbo.Customers " +
                        " (Code, Name, ABN, Address, Suburb, State, Postcode, Phone, Email, CreditLimit, IsActive) " +
                        " VALUES " +
                        " (@Code, @Name, @ABN, @Address, @Suburb, @State, @Postcode, @Phone, @Email, @CreditLimit, @IsActive); " +
                        "SELECT CAST(SCOPE_IDENTITY() AS INT);", cn);
                }
                else
                {
                    cmd = new SqlCommand(
                        "UPDATE dbo.Customers SET " +
                        "  Code = @Code, Name = @Name, ABN = @ABN, Address = @Address, " +
                        "  Suburb = @Suburb, State = @State, Postcode = @Postcode, " +
                        "  Phone = @Phone, Email = @Email, CreditLimit = @CreditLimit, " +
                        "  IsActive = @IsActive " +
                        " WHERE Id = @Id", cn);
                    cmd.Parameters.AddWithValue("@Id", customer.Id);
                }

                cmd.Parameters.AddWithValue("@Code", customer.Code);
                cmd.Parameters.AddWithValue("@Name", customer.Name);
                cmd.Parameters.AddWithValue("@ABN", customer.ABN);
                cmd.Parameters.AddWithValue("@Address", customer.Address);
                cmd.Parameters.AddWithValue("@Suburb", customer.Suburb);
                cmd.Parameters.AddWithValue("@State", customer.State);
                cmd.Parameters.AddWithValue("@Postcode", customer.Postcode);
                cmd.Parameters.AddWithValue("@Phone", customer.Phone);
                cmd.Parameters.AddWithValue("@Email", customer.Email);
                cmd.Parameters.AddWithValue("@CreditLimit", customer.CreditLimit);
                cmd.Parameters.AddWithValue("@IsActive", customer.IsActive);

                if (customer.Id == 0)
                {
                    customer.Id = Convert.ToInt32(cmd.ExecuteScalar());
                }
                else
                {
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void DeleteCustomer(int id)
        {
            using (SqlConnection cn = OpenConnection())
            {
                SqlCommand cmd = new SqlCommand(
                    "DELETE FROM dbo.Customers WHERE Id = @Id", cn);
                cmd.Parameters.AddWithValue("@Id", id);
                cmd.ExecuteNonQuery();
            }
        }

        /// <summary>True when the customer is referenced by a vehicle or a ticket.</summary>
        public bool CustomerIsInUse(int customerId)
        {
            using (SqlConnection cn = OpenConnection())
            {
                SqlCommand cmd = new SqlCommand(
                    "SELECT " +
                    "  (SELECT COUNT(*) FROM dbo.Vehicles     WHERE CustomerId = @Id) + " +
                    "  (SELECT COUNT(*) FROM dbo.WeighTickets WHERE CustomerId = @Id)", cn);
                cmd.Parameters.AddWithValue("@Id", customerId);

                return Convert.ToInt32(cmd.ExecuteScalar()) > 0;
            }
        }

        // ------------------------------------------------------------------
        // Vehicles
        // ------------------------------------------------------------------

        private const string VehicleColumns =
            "Id, Registration, Description, TareWeight, MaxGross, CustomerId, IsActive";

        private static Vehicle ReadVehicle(SqlDataReader dr)
        {
            Vehicle v = new Vehicle();
            v.Id = Convert.ToInt32(dr["Id"]);
            v.Registration = GetString(dr, "Registration");
            v.Description = GetString(dr, "Description");
            v.TareWeight = Convert.ToDecimal(dr["TareWeight"]);
            v.MaxGross = Convert.ToDecimal(dr["MaxGross"]);
            v.CustomerId = Convert.ToInt32(dr["CustomerId"]);
            v.IsActive = Convert.ToBoolean(dr["IsActive"]);
            return v;
        }

        public List<Vehicle> Vehicles
        {
            get { return GetVehicles(); }
        }

        public List<Vehicle> GetVehicles()
        {
            List<Vehicle> results = new List<Vehicle>();

            using (SqlConnection cn = OpenConnection())
            {
                SqlCommand cmd = new SqlCommand(
                    "SELECT " + VehicleColumns + " FROM dbo.Vehicles ORDER BY Registration", cn);

                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        results.Add(ReadVehicle(dr));
                    }
                }
            }
            return results;
        }

        public Vehicle GetVehicleById(int id)
        {
            using (SqlConnection cn = OpenConnection())
            {
                SqlCommand cmd = new SqlCommand(
                    "SELECT " + VehicleColumns + " FROM dbo.Vehicles WHERE Id = @Id", cn);
                cmd.Parameters.AddWithValue("@Id", id);

                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    if (dr.Read())
                    {
                        return ReadVehicle(dr);
                    }
                }
            }
            return null;
        }

        public List<Vehicle> SearchVehicles(string text)
        {
            if (text == null)
            {
                text = "";
            }
            text = text.Trim();

            List<Vehicle> results = new List<Vehicle>();

            using (SqlConnection cn = OpenConnection())
            {
                SqlCommand cmd = new SqlCommand(
                    "SELECT " + VehicleColumns + " FROM dbo.Vehicles " +
                    " WHERE (@Text = '' OR Registration LIKE @Like OR Description LIKE @Like) " +
                    " ORDER BY Registration", cn);
                cmd.Parameters.AddWithValue("@Text", text);
                cmd.Parameters.AddWithValue("@Like", "%" + text + "%");

                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        results.Add(ReadVehicle(dr));
                    }
                }
            }
            return results;
        }

        // ------------------------------------------------------------------
        // Products
        // ------------------------------------------------------------------

        private const string ProductColumns = "Id, Code, Name, PricePerTonne, GstApplicable";

        private static Product ReadProduct(SqlDataReader dr)
        {
            Product p = new Product();
            p.Id = Convert.ToInt32(dr["Id"]);
            p.Code = GetString(dr, "Code");
            p.Name = GetString(dr, "Name");
            p.PricePerTonne = Convert.ToDecimal(dr["PricePerTonne"]);
            p.GstApplicable = Convert.ToBoolean(dr["GstApplicable"]);
            return p;
        }

        public List<Product> Products
        {
            get { return GetProducts(); }
        }

        public List<Product> GetProducts()
        {
            List<Product> results = new List<Product>();

            using (SqlConnection cn = OpenConnection())
            {
                SqlCommand cmd = new SqlCommand(
                    "SELECT " + ProductColumns + " FROM dbo.Products ORDER BY Code", cn);

                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        results.Add(ReadProduct(dr));
                    }
                }
            }
            return results;
        }

        public Product GetProductById(int id)
        {
            using (SqlConnection cn = OpenConnection())
            {
                SqlCommand cmd = new SqlCommand(
                    "SELECT " + ProductColumns + " FROM dbo.Products WHERE Id = @Id", cn);
                cmd.Parameters.AddWithValue("@Id", id);

                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    if (dr.Read())
                    {
                        return ReadProduct(dr);
                    }
                }
            }
            return null;
        }

        // ------------------------------------------------------------------
        // Weigh tickets
        // ------------------------------------------------------------------

        /// <summary>
        /// NetWeight is a computed column in the database and a derived property
        /// on the model, so it is never selected or written here.
        /// </summary>
        private const string TicketColumns =
            "Id, TicketNumber, TicketDate, VehicleId, CustomerId, ProductId, " +
            "GrossWeight, TareWeight, PricePerTonne, Subtotal, Gst, Total, Notes, Status";

        private static WeighTicket ReadTicket(SqlDataReader dr)
        {
            WeighTicket t = new WeighTicket();
            t.Id = Convert.ToInt32(dr["Id"]);
            t.TicketNumber = GetString(dr, "TicketNumber");
            t.TicketDate = Convert.ToDateTime(dr["TicketDate"]);
            t.VehicleId = Convert.ToInt32(dr["VehicleId"]);
            t.CustomerId = Convert.ToInt32(dr["CustomerId"]);
            t.ProductId = Convert.ToInt32(dr["ProductId"]);
            t.GrossWeight = Convert.ToDecimal(dr["GrossWeight"]);
            t.TareWeight = Convert.ToDecimal(dr["TareWeight"]);
            t.PricePerTonne = Convert.ToDecimal(dr["PricePerTonne"]);
            t.Subtotal = Convert.ToDecimal(dr["Subtotal"]);
            t.Gst = Convert.ToDecimal(dr["Gst"]);
            t.Total = Convert.ToDecimal(dr["Total"]);
            t.Notes = GetString(dr, "Notes");
            t.Status = GetString(dr, "Status");
            return t;
        }

        public List<WeighTicket> Tickets
        {
            get { return GetTickets(); }
        }

        public List<WeighTicket> GetTickets()
        {
            List<WeighTicket> results = new List<WeighTicket>();

            using (SqlConnection cn = OpenConnection())
            {
                SqlCommand cmd = new SqlCommand(
                    "SELECT " + TicketColumns + " FROM dbo.WeighTickets ORDER BY TicketNumber", cn);

                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        results.Add(ReadTicket(dr));
                    }
                }
            }
            return results;
        }

        // ------------------------------------------------------------------
        // Weigh ticket browse
        // ------------------------------------------------------------------

        /// <summary>
        /// Ticket rows with the vehicle, customer and product descriptions
        /// joined on, for the browse screen.  One query, not one per row.
        /// </summary>
        private const string TicketSummarySql =
            "SELECT t.Id, t.TicketNumber, t.TicketDate, " +
            "       v.Registration, " +
            "       c.Code AS CustomerCode, c.Name AS CustomerName, " +
            "       p.Code AS ProductCode, p.Name AS ProductName, " +
            "       t.GrossWeight, t.TareWeight, t.NetWeight, " +
            "       t.PricePerTonne, t.Subtotal, t.Gst, t.Total, t.Notes, t.Status " +
            "  FROM dbo.WeighTickets t " +
            "  JOIN dbo.Vehicles  v ON v.Id = t.VehicleId " +
            "  JOIN dbo.Customers c ON c.Id = t.CustomerId " +
            "  JOIN dbo.Products  p ON p.Id = t.ProductId ";

        private static WeighTicketSummary ReadTicketSummary(SqlDataReader dr)
        {
            WeighTicketSummary s = new WeighTicketSummary();
            s.Id = Convert.ToInt32(dr["Id"]);
            s.TicketNumber = GetString(dr, "TicketNumber");
            s.TicketDate = Convert.ToDateTime(dr["TicketDate"]);
            s.Registration = GetString(dr, "Registration");
            s.CustomerCode = GetString(dr, "CustomerCode");
            s.CustomerName = GetString(dr, "CustomerName");
            s.ProductCode = GetString(dr, "ProductCode");
            s.ProductName = GetString(dr, "ProductName");
            s.GrossWeight = Convert.ToDecimal(dr["GrossWeight"]);
            s.TareWeight = Convert.ToDecimal(dr["TareWeight"]);
            s.NetWeight = Convert.ToDecimal(dr["NetWeight"]);
            s.PricePerTonne = Convert.ToDecimal(dr["PricePerTonne"]);
            s.Subtotal = Convert.ToDecimal(dr["Subtotal"]);
            s.Gst = Convert.ToDecimal(dr["Gst"]);
            s.Total = Convert.ToDecimal(dr["Total"]);
            s.Notes = GetString(dr, "Notes");
            s.Status = GetString(dr, "Status");
            return s;
        }

        /// <summary>
        /// Filtered ticket list, newest first.  Every filter is optional:
        /// pass null dates for all dates, 0 for all customers, "" for all
        /// statuses and "" for no text search.
        ///
        /// The To date is inclusive - a ticket stamped any time on that day is
        /// returned, which is why the comparison is against the following
        /// midnight rather than the date itself.
        /// </summary>
        public List<WeighTicketSummary> SearchTickets(
            DateTime? fromDate, DateTime? toDate, int customerId, string status, string text)
        {
            if (status == null)
            {
                status = "";
            }
            if (text == null)
            {
                text = "";
            }
            text = text.Trim();

            List<WeighTicketSummary> results = new List<WeighTicketSummary>();

            using (SqlConnection cn = OpenConnection())
            {
                SqlCommand cmd = new SqlCommand(
                    TicketSummarySql +
                    " WHERE (@FromDate IS NULL OR t.TicketDate >= @FromDate) " +
                    "   AND (@ToDate   IS NULL OR t.TicketDate <  @ToDate) " +
                    "   AND (@CustomerId = 0 OR t.CustomerId = @CustomerId) " +
                    "   AND (@Status = '' OR t.Status = @Status) " +
                    "   AND (@Text = '' OR t.TicketNumber LIKE @Like " +
                    "                   OR v.Registration LIKE @Like " +
                    "                   OR c.Name         LIKE @Like) " +
                    " ORDER BY t.TicketDate DESC, t.Id DESC", cn);

                if (fromDate.HasValue)
                {
                    cmd.Parameters.AddWithValue("@FromDate", fromDate.Value.Date);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@FromDate", DBNull.Value);
                }

                if (toDate.HasValue)
                {
                    cmd.Parameters.AddWithValue("@ToDate", toDate.Value.Date.AddDays(1));
                }
                else
                {
                    cmd.Parameters.AddWithValue("@ToDate", DBNull.Value);
                }

                cmd.Parameters.AddWithValue("@CustomerId", customerId);
                cmd.Parameters.AddWithValue("@Status", status);
                cmd.Parameters.AddWithValue("@Text", text);
                cmd.Parameters.AddWithValue("@Like", "%" + text + "%");

                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        results.Add(ReadTicketSummary(dr));
                    }
                }
            }
            return results;
        }

        /// <summary>
        /// Looks at the ticket counter WITHOUT consuming it, so the weigh ticket
        /// screen can show the operator what number they are about to get.
        /// </summary>
        public string PeekNextTicketNumber()
        {
            using (SqlConnection cn = OpenConnection())
            {
                SqlCommand cmd = new SqlCommand(
                    "SELECT NextValue FROM dbo.Counters WHERE CounterName = 'TICKET'", cn);

                object value = cmd.ExecuteScalar();
                if (value == null)
                {
                    return "WB100001";
                }
                return "WB" + Convert.ToInt32(value).ToString();
            }
        }

        public void SaveTicket(WeighTicket ticket)
        {
            using (SqlConnection cn = OpenConnection())
            {
                SqlTransaction tx = cn.BeginTransaction();
                try
                {
                    if (ticket.Id == 0)
                    {
                        // Take the next document number.  The OUTPUT clause hands
                        // back the value we consumed in the same statement, so two
                        // operators cannot land on the same ticket number.
                        SqlCommand counter = new SqlCommand(
                            "UPDATE dbo.Counters SET NextValue = NextValue + 1 " +
                            " OUTPUT deleted.NextValue " +
                            " WHERE CounterName = 'TICKET'", cn, tx);

                        ticket.TicketNumber = "WB" + Convert.ToInt32(counter.ExecuteScalar()).ToString();

                        SqlCommand cmd = new SqlCommand(
                            "INSERT INTO dbo.WeighTickets " +
                            " (TicketNumber, TicketDate, VehicleId, CustomerId, ProductId, " +
                            "  GrossWeight, TareWeight, PricePerTonne, Subtotal, Gst, Total, Notes, Status) " +
                            " VALUES " +
                            " (@TicketNumber, @TicketDate, @VehicleId, @CustomerId, @ProductId, " +
                            "  @GrossWeight, @TareWeight, @PricePerTonne, @Subtotal, @Gst, @Total, @Notes, @Status); " +
                            "SELECT CAST(SCOPE_IDENTITY() AS INT);", cn, tx);

                        AddTicketParameters(cmd, ticket);
                        ticket.Id = Convert.ToInt32(cmd.ExecuteScalar());
                    }
                    else
                    {
                        SqlCommand cmd = new SqlCommand(
                            "UPDATE dbo.WeighTickets SET " +
                            "  TicketNumber = @TicketNumber, TicketDate = @TicketDate, " +
                            "  VehicleId = @VehicleId, CustomerId = @CustomerId, ProductId = @ProductId, " +
                            "  GrossWeight = @GrossWeight, TareWeight = @TareWeight, " +
                            "  PricePerTonne = @PricePerTonne, Subtotal = @Subtotal, " +
                            "  Gst = @Gst, Total = @Total, Notes = @Notes, Status = @Status " +
                            " WHERE Id = @Id", cn, tx);

                        AddTicketParameters(cmd, ticket);
                        cmd.Parameters.AddWithValue("@Id", ticket.Id);
                        cmd.ExecuteNonQuery();
                    }

                    tx.Commit();
                }
                catch
                {
                    tx.Rollback();
                    throw;
                }
            }
        }

        private static void AddTicketParameters(SqlCommand cmd, WeighTicket ticket)
        {
            cmd.Parameters.AddWithValue("@TicketNumber", ticket.TicketNumber);
            cmd.Parameters.AddWithValue("@TicketDate", ticket.TicketDate);
            cmd.Parameters.AddWithValue("@VehicleId", ticket.VehicleId);
            cmd.Parameters.AddWithValue("@CustomerId", ticket.CustomerId);
            cmd.Parameters.AddWithValue("@ProductId", ticket.ProductId);
            cmd.Parameters.AddWithValue("@GrossWeight", ticket.GrossWeight);
            cmd.Parameters.AddWithValue("@TareWeight", ticket.TareWeight);
            cmd.Parameters.AddWithValue("@PricePerTonne", ticket.PricePerTonne);
            cmd.Parameters.AddWithValue("@Subtotal", ticket.Subtotal);
            cmd.Parameters.AddWithValue("@Gst", ticket.Gst);
            cmd.Parameters.AddWithValue("@Total", ticket.Total);
            cmd.Parameters.AddWithValue("@Notes", ticket.Notes);
            cmd.Parameters.AddWithValue("@Status", ticket.Status);
        }

        // ------------------------------------------------------------------
        // Users and security profiles
        // ------------------------------------------------------------------

        /// <summary>
        /// The profile name is joined on rather than looked up per row - the
        /// sign on combo and the status bar both want it straight away.
        /// </summary>
        private const string UserColumns =
            "u.UserId, u.UserName, u.FullName, u.ProfileId, u.IsActive, p.ProfileName";

        private const string UserFrom =
            " FROM dbo.Users u JOIN dbo.SecurityProfiles p ON p.ProfileId = u.ProfileId ";

        private static UserAccount ReadUser(SqlDataReader dr)
        {
            UserAccount u = new UserAccount();
            u.UserId = Convert.ToInt32(dr["UserId"]);
            u.UserName = GetString(dr, "UserName");
            u.FullName = GetString(dr, "FullName");
            u.ProfileId = Convert.ToInt32(dr["ProfileId"]);
            u.ProfileName = GetString(dr, "ProfileName");
            u.IsActive = Convert.ToBoolean(dr["IsActive"]);
            return u;
        }

        /// <summary>Operators offered by the sign on dialog.</summary>
        public List<UserAccount> GetActiveUsers()
        {
            List<UserAccount> results = new List<UserAccount>();

            using (SqlConnection cn = OpenConnection())
            {
                SqlCommand cmd = new SqlCommand(
                    "SELECT " + UserColumns + UserFrom +
                    " WHERE u.IsActive = 1 " +
                    " ORDER BY u.FullName", cn);

                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        results.Add(ReadUser(dr));
                    }
                }
            }
            return results;
        }

        public UserAccount GetUserByName(string userName)
        {
            if (userName == null)
            {
                userName = "";
            }

            using (SqlConnection cn = OpenConnection())
            {
                SqlCommand cmd = new SqlCommand(
                    "SELECT " + UserColumns + UserFrom +
                    " WHERE u.UserName = @UserName", cn);
                cmd.Parameters.AddWithValue("@UserName", userName.Trim());

                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    if (dr.Read())
                    {
                        return ReadUser(dr);
                    }
                }
            }
            return null;
        }

        /// <summary>
        /// The privilege names granted to a profile.  A privilege the profile
        /// does not hold simply has no row, so this is the whole grant list.
        /// </summary>
        public List<string> GetProfilePrivileges(int profileId)
        {
            List<string> results = new List<string>();

            using (SqlConnection cn = OpenConnection())
            {
                SqlCommand cmd = new SqlCommand(
                    "SELECT PrivilegeName FROM dbo.SecurityProfilePrivileges " +
                    " WHERE ProfileId = @ProfileId " +
                    " ORDER BY PrivilegeName", cn);
                cmd.Parameters.AddWithValue("@ProfileId", profileId);

                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        results.Add(GetString(dr, "PrivilegeName"));
                    }
                }
            }
            return results;
        }

        // ------------------------------------------------------------------
        // Customer account - header, contacts and contract rates
        // ------------------------------------------------------------------

        /// <summary>
        /// The customer with the postal address as well, for the account
        /// screen.  GetCustomerById deliberately still returns the short shape
        /// so nothing else has to change.
        /// </summary>
        public Customer GetCustomerAccount(int id)
        {
            using (SqlConnection cn = OpenConnection())
            {
                SqlCommand cmd = new SqlCommand(
                    "SELECT " + CustomerAccountColumns + " FROM dbo.Customers WHERE Id = @Id", cn);
                cmd.Parameters.AddWithValue("@Id", id);

                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    if (dr.Read())
                    {
                        return ReadCustomerAccount(dr);
                    }
                }
            }
            return null;
        }

        /// <summary>
        /// Writes the header half of the account screen.  Update only - the
        /// account screen never creates a customer, that is still the job of
        /// the edit dialog.
        /// </summary>
        public void SaveCustomerAccount(Customer customer)
        {
            using (SqlConnection cn = OpenConnection())
            {
                SqlCommand cmd = new SqlCommand(
                    "UPDATE dbo.Customers SET " +
                    "  Name = @Name, ABN = @ABN, Address = @Address, " +
                    "  Suburb = @Suburb, State = @State, Postcode = @Postcode, " +
                    "  Phone = @Phone, Email = @Email, CreditLimit = @CreditLimit, " +
                    "  IsActive = @IsActive, " +
                    "  PostalAddress = @PostalAddress, PostalSuburb = @PostalSuburb, " +
                    "  PostalState = @PostalState, PostalPostcode = @PostalPostcode " +
                    " WHERE Id = @Id", cn);

                cmd.Parameters.AddWithValue("@Id", customer.Id);
                cmd.Parameters.AddWithValue("@Name", customer.Name);
                cmd.Parameters.AddWithValue("@ABN", customer.ABN);
                cmd.Parameters.AddWithValue("@Address", customer.Address);
                cmd.Parameters.AddWithValue("@Suburb", customer.Suburb);
                cmd.Parameters.AddWithValue("@State", customer.State);
                cmd.Parameters.AddWithValue("@Postcode", customer.Postcode);
                cmd.Parameters.AddWithValue("@Phone", customer.Phone);
                cmd.Parameters.AddWithValue("@Email", customer.Email);
                cmd.Parameters.AddWithValue("@CreditLimit", customer.CreditLimit);
                cmd.Parameters.AddWithValue("@IsActive", customer.IsActive);
                cmd.Parameters.AddWithValue("@PostalAddress", customer.PostalAddress);
                cmd.Parameters.AddWithValue("@PostalSuburb", customer.PostalSuburb);
                cmd.Parameters.AddWithValue("@PostalState", customer.PostalState);
                cmd.Parameters.AddWithValue("@PostalPostcode", customer.PostalPostcode);

                cmd.ExecuteNonQuery();
            }
        }

        // ---------------------------- contacts ----------------------------

        private const string ContactColumns =
            "ContactId, CustomerId, ContactName, Position, Phone, Email, IsPrimary";

        private static CustomerContact ReadContact(SqlDataReader dr)
        {
            CustomerContact c = new CustomerContact();
            c.ContactId = Convert.ToInt32(dr["ContactId"]);
            c.CustomerId = Convert.ToInt32(dr["CustomerId"]);
            c.ContactName = GetString(dr, "ContactName");
            c.Position = GetString(dr, "Position");
            c.Phone = GetString(dr, "Phone");
            c.Email = GetString(dr, "Email");
            c.IsPrimary = Convert.ToBoolean(dr["IsPrimary"]);
            return c;
        }

        public List<CustomerContact> GetCustomerContacts(int customerId)
        {
            List<CustomerContact> results = new List<CustomerContact>();

            using (SqlConnection cn = OpenConnection())
            {
                SqlCommand cmd = new SqlCommand(
                    "SELECT " + ContactColumns + " FROM dbo.CustomerContacts " +
                    " WHERE CustomerId = @CustomerId " +
                    " ORDER BY IsPrimary DESC, ContactName", cn);
                cmd.Parameters.AddWithValue("@CustomerId", customerId);

                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        results.Add(ReadContact(dr));
                    }
                }
            }
            return results;
        }

        public void SaveContact(CustomerContact contact)
        {
            using (SqlConnection cn = OpenConnection())
            {
                SqlCommand cmd;

                if (contact.ContactId == 0)
                {
                    cmd = new SqlCommand(
                        "INSERT INTO dbo.CustomerContacts " +
                        " (CustomerId, ContactName, Position, Phone, Email, IsPrimary) " +
                        " VALUES " +
                        " (@CustomerId, @ContactName, @Position, @Phone, @Email, @IsPrimary); " +
                        "SELECT CAST(SCOPE_IDENTITY() AS INT);", cn);
                }
                else
                {
                    cmd = new SqlCommand(
                        "UPDATE dbo.CustomerContacts SET " +
                        "  ContactName = @ContactName, Position = @Position, " +
                        "  Phone = @Phone, Email = @Email, IsPrimary = @IsPrimary " +
                        " WHERE ContactId = @ContactId", cn);
                    cmd.Parameters.AddWithValue("@ContactId", contact.ContactId);
                }

                cmd.Parameters.AddWithValue("@CustomerId", contact.CustomerId);
                cmd.Parameters.AddWithValue("@ContactName", contact.ContactName);
                cmd.Parameters.AddWithValue("@Position", contact.Position);
                cmd.Parameters.AddWithValue("@Phone", contact.Phone);
                cmd.Parameters.AddWithValue("@Email", contact.Email);
                cmd.Parameters.AddWithValue("@IsPrimary", contact.IsPrimary);

                if (contact.ContactId == 0)
                {
                    contact.ContactId = Convert.ToInt32(cmd.ExecuteScalar());
                }
                else
                {
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void DeleteContact(int contactId)
        {
            using (SqlConnection cn = OpenConnection())
            {
                SqlCommand cmd = new SqlCommand(
                    "DELETE FROM dbo.CustomerContacts WHERE ContactId = @ContactId", cn);
                cmd.Parameters.AddWithValue("@ContactId", contactId);
                cmd.ExecuteNonQuery();
            }
        }

        // -------------------------- contract rates -------------------------

        /// <summary>
        /// Rates with the product code and name joined on, so the rates grid
        /// fills from one query rather than one per row.
        /// </summary>
        private const string RateSql =
            "SELECT r.RateId, r.CustomerId, r.ProductId, " +
            "       p.Code AS ProductCode, p.Name AS ProductName, " +
            "       r.RatePerTonne, r.EffectiveFrom, r.EffectiveTo, r.Notes " +
            "  FROM dbo.ContractRates r " +
            "  JOIN dbo.Products p ON p.Id = r.ProductId ";

        private static ContractRate ReadRate(SqlDataReader dr)
        {
            ContractRate r = new ContractRate();
            r.RateId = Convert.ToInt32(dr["RateId"]);
            r.CustomerId = Convert.ToInt32(dr["CustomerId"]);
            r.ProductId = Convert.ToInt32(dr["ProductId"]);
            r.ProductCode = GetString(dr, "ProductCode");
            r.ProductName = GetString(dr, "ProductName");
            r.RatePerTonne = Convert.ToDecimal(dr["RatePerTonne"]);
            r.EffectiveFrom = Convert.ToDateTime(dr["EffectiveFrom"]);
            if (dr["EffectiveTo"] == DBNull.Value)
            {
                r.EffectiveTo = null;
            }
            else
            {
                r.EffectiveTo = Convert.ToDateTime(dr["EffectiveTo"]);
            }
            r.Notes = GetString(dr, "Notes");
            return r;
        }

        public List<ContractRate> GetContractRates(int customerId)
        {
            List<ContractRate> results = new List<ContractRate>();

            using (SqlConnection cn = OpenConnection())
            {
                SqlCommand cmd = new SqlCommand(
                    RateSql +
                    " WHERE r.CustomerId = @CustomerId " +
                    " ORDER BY p.Code, r.EffectiveFrom DESC", cn);
                cmd.Parameters.AddWithValue("@CustomerId", customerId);

                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        results.Add(ReadRate(dr));
                    }
                }
            }
            return results;
        }

        public void SaveContractRate(ContractRate rate)
        {
            using (SqlConnection cn = OpenConnection())
            {
                SqlCommand cmd;

                if (rate.RateId == 0)
                {
                    cmd = new SqlCommand(
                        "INSERT INTO dbo.ContractRates " +
                        " (CustomerId, ProductId, RatePerTonne, EffectiveFrom, EffectiveTo, Notes) " +
                        " VALUES " +
                        " (@CustomerId, @ProductId, @RatePerTonne, @EffectiveFrom, @EffectiveTo, @Notes); " +
                        "SELECT CAST(SCOPE_IDENTITY() AS INT);", cn);
                }
                else
                {
                    cmd = new SqlCommand(
                        "UPDATE dbo.ContractRates SET " +
                        "  ProductId = @ProductId, RatePerTonne = @RatePerTonne, " +
                        "  EffectiveFrom = @EffectiveFrom, EffectiveTo = @EffectiveTo, " +
                        "  Notes = @Notes " +
                        " WHERE RateId = @RateId", cn);
                    cmd.Parameters.AddWithValue("@RateId", rate.RateId);
                }

                cmd.Parameters.AddWithValue("@CustomerId", rate.CustomerId);
                cmd.Parameters.AddWithValue("@ProductId", rate.ProductId);
                cmd.Parameters.AddWithValue("@RatePerTonne", rate.RatePerTonne);
                cmd.Parameters.AddWithValue("@EffectiveFrom", rate.EffectiveFrom);

                if (rate.EffectiveTo.HasValue)
                {
                    cmd.Parameters.AddWithValue("@EffectiveTo", rate.EffectiveTo.Value);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@EffectiveTo", DBNull.Value);
                }

                cmd.Parameters.AddWithValue("@Notes", rate.Notes);

                if (rate.RateId == 0)
                {
                    rate.RateId = Convert.ToInt32(cmd.ExecuteScalar());
                }
                else
                {
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void DeleteContractRate(int rateId)
        {
            using (SqlConnection cn = OpenConnection())
            {
                SqlCommand cmd = new SqlCommand(
                    "DELETE FROM dbo.ContractRates WHERE RateId = @RateId", cn);
                cmd.Parameters.AddWithValue("@RateId", rateId);
                cmd.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// The vehicles on a customer's account, for the account screen's
        /// third tab.  SearchVehicles cannot do this - it filters on text.
        /// </summary>
        public List<Vehicle> GetVehiclesByCustomer(int customerId)
        {
            List<Vehicle> results = new List<Vehicle>();

            using (SqlConnection cn = OpenConnection())
            {
                SqlCommand cmd = new SqlCommand(
                    "SELECT " + VehicleColumns + " FROM dbo.Vehicles " +
                    " WHERE CustomerId = @CustomerId " +
                    " ORDER BY Registration", cn);
                cmd.Parameters.AddWithValue("@CustomerId", customerId);

                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        results.Add(ReadVehicle(dr));
                    }
                }
            }
            return results;
        }
    }
}
