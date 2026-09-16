# Weighbridge Administration System

A Windows Forms (MDI) desktop application for a quarry / landfill weighbridge office. Operators
look up vehicles, key a weigh ticket through a four-step wizard, browse and total the tickets
already raised, and maintain the customer master file. Data lives in SQL Server and is reached
through plain ADO.NET.

- **Target framework:** .NET Framework 4.8 (`net48`), SDK-style project
- **UI:** Windows Forms, MDI parent + child windows
- **Database:** SQL Server 2012 or later (LocalDB, Express, or full)
- **Platform:** Windows only

---

## Prerequisites

| Requirement | Notes |
|---|---|
| Windows | WinForms + `net48`; will not run on Linux/macOS |
| .NET SDK (6.0 or later) **or** Visual Studio 2019+ | Building via `dotnet build` works — the project sets `EnableWindowsTargeting` and pulls in `Microsoft.NETFramework.ReferenceAssemblies`, so the .NET Framework 4.8 *targeting pack* is not required separately |
| .NET Framework 4.8 runtime | Needed to **run** the exe. Ships with Windows 10 1903+ / Windows 11 |
| SQL Server | The shipped `app.config` points at a local **SQL Server Express** instance (`localhost\SQLEXPRESS`). LocalDB works just as well — see the alternative below |
| `sqlcmd` or SQL Server Management Studio (SSMS) | To run the create script |

There is no `.sln` file — open the folder or `WeighbridgeAdmin.csproj` directly.

---

## 1. Set up the database

The application will **not start** until the database is reachable — `Program.Main` runs a
connection test up front and shows an error dialog instead of the main window if it fails.

`Database/CreateDatabase.sql` creates the *tables* and loads demo data, but it does **not** create
the database itself. Create `WeighbridgeDb` first, then run the script against it.

### Using SQL Server Express (what the shipped config expects)

```powershell
# 1. Check the instance is running
Get-Service 'MSSQL$SQLEXPRESS'

# 2. Create the database
sqlcmd -S "localhost\SQLEXPRESS" -Q "IF DB_ID('WeighbridgeDb') IS NULL CREATE DATABASE WeighbridgeDb;"

# 3. Create the tables and load the demo data
sqlcmd -S "localhost\SQLEXPRESS" -d WeighbridgeDb -i Database\CreateDatabase.sql
```

### Alternative: LocalDB

LocalDB needs no service install and is fine for development. Use it instead of step 1 above, then
point the connection string at `(localdb)\MSSQLLocalDB` (see the next section).

```powershell
sqllocaldb create MSSQLLocalDB
sqllocaldb start  MSSQLLocalDB
sqlcmd -S "(localdb)\MSSQLLocalDB" -Q "IF DB_ID('WeighbridgeDb') IS NULL CREATE DATABASE WeighbridgeDb;"
sqlcmd -S "(localdb)\MSSQLLocalDB" -d WeighbridgeDb -i Database\CreateDatabase.sql
```

### Using SSMS

Create a database named `WeighbridgeDb`, then open `Database/CreateDatabase.sql` and execute it
with that database selected.

> **The script is destructive.** It drops and recreates `WeighTickets`, `Vehicles`, `Products`,
> `Customers` and `Counters` every time it runs. Re-running it wipes anything you have entered.

**What you get:** 15 customers, 8 products, 20 vehicles, 30 weigh tickets, and a `Counters` row
that starts ticket numbering at `WB100031`.

---

## 2. Configure the connection string

Connection settings live in `app.config`, which is copied to
`bin\Debug\net48\WeighbridgeAdmin.exe.config` on build.

```xml
<connectionStrings>
  <add name="WeighbridgeDb"
       connectionString="Data Source=localhost\SQLEXPRESS;Initial Catalog=WeighbridgeDb;Integrated Security=True;Connect Timeout=15"
       providerName="System.Data.SqlClient" />
</connectionStrings>
```

Change the `Data Source` to match your instance:

| Instance | `Data Source` |
|---|---|
| Local SQL Express (default) | `localhost\SQLEXPRESS` — `.\SQLEXPRESS` is equivalent |
| LocalDB | `(localdb)\MSSQLLocalDB` |
| Site server, SQL login | `WBSERVER01\SQLEXPRESS` plus `User ID=wbapp;Password=...` in place of `Integrated Security=True` |

Edit `app.config` and **rebuild** — the build copies it over `bin\Debug\net48\WeighbridgeAdmin.exe.config`,
so editing only the `bin` copy is lost on the next build. Edit the `bin` copy directly only to
re-point an already-deployed install.

> **Watch the spelling of the server name.** A typo in the host part (`locahost` for `localhost`,
> say) fails DNS and reports *"error: 26 - Error Locating Server/Instance Specified"*, which reads
> as though SQL Server is missing or stopped when the instance is actually running fine.

### App settings

```xml
<appSettings>
  <add key="SiteName" value="Riverstone Quarries - Site 04" />
  <add key="GstRate"  value="0.10" />
</appSettings>
```

`GstRate` is the G.S.T. rate as a **decimal fraction**, not a percentage — `0.10` means 10%. It
drives the tax on every weigh ticket and the on-screen captions ("G.S.T. (10%)"), which relabel
themselves to match. Products flagged as GST-free are unaffected by it.

- Must parse as a decimal between `0` and `1`. A value outside that range, or one written as `10`
  or `10%`, is rejected at startup with a message naming the bad value — rather than silently
  charging the wrong tax.
- If the key is **absent** the application falls back to 10%, so older configuration files written
  before the setting existed keep working.
- The rate is read once at startup. Changing it requires a restart, and it does not retrospectively
  alter tickets already saved — each ticket stores the GST amount that was calculated at the time.

> **Note:** `SiteName` is still not read by any code.

---

## 3. Build and run

```powershell
# Build
dotnet build WeighbridgeAdmin.csproj -c Debug

# Run
.\bin\Debug\net48\WeighbridgeAdmin.exe
```

Or open `WeighbridgeAdmin.csproj` in Visual Studio and press F5. (`dotnet run` also works, but F5 /
the exe is the normal path for a WinForms app.)

Release build:

```powershell
dotnet build WeighbridgeAdmin.csproj -c Release
```

### If it does not start

A dialog reading *"The configuration file is not valid"* means a setting could not be read — most
often a malformed `GstRate`. The message names the offending value; fix it in the config file.

A dialog reading *"Cannot connect to the weighbridge database"* means the connection test in
`Program.cs` failed. Check, in order:

1. The server name in the connection string is spelled correctly — see the warning above. This
   produces an error that looks like a missing instance.
2. The instance is running — `Get-Service 'MSSQL$SQLEXPRESS'`, or `sqllocaldb info MSSQLLocalDB` for LocalDB.
3. The `WeighbridgeDb` database exists.
4. `Database\CreateDatabase.sql` has been run — the test query is `SELECT COUNT(*) FROM dbo.Customers`, so it fails if the tables are missing.
5. You are reading the right file: the app uses `bin\Debug\net48\WeighbridgeAdmin.exe.config`, which the build regenerates from `app.config`.

A quick way to test a connection string without launching the app:

```powershell
$cn = New-Object System.Data.SqlClient.SqlConnection "Data Source=localhost\SQLEXPRESS;Initial Catalog=WeighbridgeDb;Integrated Security=True;Connect Timeout=5"
$cn.Open(); $cn.State; $cn.Close()
```

---

## Using the application

The main window is an MDI parent with a menu bar and a status bar (operator name + live clock).

| Menu | Item | What it does |
|---|---|---|
| File | New Weigh Ticket (`Ctrl+N`) | Opens the weigh ticket wizard |
| File | Exit | Confirms, then closes |
| Customers | Customer List | Browsable, searchable customer grid (only one instance is ever opened) |
| Customers | New Customer... | Add-customer dialog |
| Vehicles | Vehicle Lookup... | Search dialog; shows the picked vehicle's details |
| Tickets | New Weigh Ticket | Same as File → New Weigh Ticket |
| Tickets | Ticket List (`Ctrl+L`) | Browse saved weigh tickets (only one instance is ever opened) |
| Tickets | Cascade Windows / Tile Horizontally | MDI layout |
| Help | About... | Version box |

### Weigh ticket wizard

Four tabs with Back / Next buttons; each page is validated before you can move on, and every page
is re-validated on Save.

1. **Vehicle & customer** — type a registration or press `F4` for the lookup dialog. Selecting a
   vehicle fills in the description, default tare and max gross, and selects the vehicle's customer.
2. **Weights** — gross and tare; net and net-tonnes update live. Gross over the vehicle's max gross
   shows a red warning and prompts for confirmation on Next.
3. **Charges** — product selection defaults the price per tonne (overtypeable). Subtotal / GST /
   total recalculate on every change. GST is charged at the configured `GstRate` and only on
   products flagged `GstApplicable`.
4. **Review** — read-only summary, notes and ticket status (`Open` / `Completed` / `Void`).

The ticket number shown during entry is a *peek* at the counter. The real number is allocated
inside a transaction on save (`UPDATE ... OUTPUT deleted.NextValue`), so two operators can't collide.

### Ticket list

**Tickets → Ticket List** (`Ctrl+L`) browses saved tickets, newest first. Filters combine (they AND
together) and re-query on every change:

- **Date range** — off by default, so the screen opens showing everything. Tick it to enable the two
  date pickers. The *to* date is inclusive.
- **Status** — all, `Open`, `Completed` or `Void`.
- **Customer** — all, or one customer.
- **Search** — matches ticket number, registration or customer name.

`F5` refresh, `F2` or double-click to see the full ticket. *Clear Filters* returns to showing
everything.

Void rows are grey and open rows amber, so unfinished work stands out. The footer totals cover
whatever is currently on screen and **exclude voided tickets from the money** (nothing was charged
for them) while still counting them in the row count.

The screen is read-only by design — a saved ticket is an accounting document, so there is no edit
or delete. Correcting one means voiding it and re-weighing, which is what the demo data shows
(`WB100006` voided, reweighed as `WB100007`).

### Customer list

`F5` refresh, `F2` edit, `Insert` new, double-click a row to edit. The search box filters on every
keystroke against code and name. Deleting a customer is blocked if any vehicle or weigh ticket
still references it — mark it inactive instead.

---

## Project layout

```
WeighbridgeAdmin.csproj      SDK-style project, net48, WinForms
app.config                   Connection string + app settings
Program.cs                   Entry point; DB reachability check before showing the UI

Model/                       Plain data holders, no behaviour
  Customer.cs                Customer master
  Vehicle.cs                 Vehicle master (tare, max gross, owning customer)
  Product.cs                 Product + price per tonne + GST flag
  WeighTicket.cs             Ticket; also TicketStatus string constants
  WeighTicketSummary.cs      Read-only ticket row with rego/customer/product joined on

Data/
  Repository.cs              All data access. Singleton via Repository.Current.
                             Inline SQL, one connection per call, manual reader mapping.

Forms/
  MainForm.*                 MDI parent: menu, status bar, clock timer
  CustomerListForm.*         Customer browse grid + toolbar + search
  CustomerEditForm.*         Add/edit customer dialog with ErrorProvider validation
  VehicleLookupDialog.*      The "F4 lookup" dialog
  WeighTicketForm.*          Four-tab weigh ticket wizard
  TicketListForm.*           Read-only ticket browse grid with filters and totals

Database/
  CreateDatabase.sql         Drop/create all tables + demo data (copied to output)

Properties/AssemblyInfo.cs   Assembly metadata (version 3.2.1.0)
```

### Architecture notes

These are deliberate characteristics of the code, worth knowing before you change anything:

- **No ORM, no unit of work, no caching.** `Repository` opens a connection per call, holds SQL as
  inline string constants, and maps `SqlDataReader` rows onto model classes by hand. Parameters are
  used throughout, so the queries are not injectable.
- **Forms reach the data layer directly** through the `Repository.Current` singleton. There is no
  service or view-model layer.
- **The weigh ticket screen holds no model instance while editing.** Every value lives on a control;
  derived figures are recalculated in the `ValueChanged` / `SelectedIndexChanged` handlers. A
  `WeighTicket` is constructed exactly once, at the moment Save is pressed. Only a couple of cached
  vehicle-master values (`_vehicleId`, `_vehicleMaxGross`) are kept on the form.
- **`NetWeight`** is a persisted computed column in SQL *and* a derived property in C#. It is never
  written. `TicketColumns` (the `WeighTicket` read/write path) does not select it either — that
  model derives it. The ticket-list query does select it, because `WeighTicketSummary` is read-only
  and takes the value SQL already computed.
- **Two shapes of ticket.** `WeighTicket` is the read/write model, carrying only foreign keys.
  `WeighTicketSummary` is the read-only browse row, with registration, customer and product
  descriptions joined on by `SearchTickets` in one query. `VehicleLookupDialog` takes the other
  approach and calls `GetCustomerById` per row; that is fine for a short vehicle list but would be
  ~90 round trips for the ticket grid, which is why the list screen does not copy it.
- **Some repository members have no callers** — the `Customers` / `Vehicles` / `Products` /
  `Tickets` convenience properties, plus `GetVehicles()` and `GetTickets()`. Forms call the
  `Get…()` and `Search…()` methods directly. They are harmless, but don't assume a member is used
  just because it exists.
- **Rounding.** Stored `Subtotal` / `Gst` / `Total` are whatever C# `Math.Round` produced (banker's
  rounding). The demo data stores those exact values rather than recomputing in T-SQL, because
  `ROUND()` rounds half away from zero and disagrees by one cent on 6 of the 30 seeded rows. Don't
  "fix" the seed totals in SQL.
- **The logged-in operator is hard-coded** — `Repository.CurrentUserName` returns a fixed string.
  There is no authentication.
