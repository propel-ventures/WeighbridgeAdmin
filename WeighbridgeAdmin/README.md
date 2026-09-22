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

Open `WeighbridgeAdmin.sln` in the repository root, or the `WeighbridgeAdmin.csproj` in this folder — both work.

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

> **The script is destructive.** It drops and recreates `ContractRates`, `CustomerContacts`,
> `Users`, `SecurityProfilePrivileges`, `SecurityProfiles`, `WeighTickets`, `Vehicles`,
> `Products`, `Customers` and `Counters` every time it runs. Re-running it wipes anything you
> have entered.

**What you get:** 15 customers, 8 products, 20 vehicles, 30 weigh tickets, a `Counters` row
that starts ticket numbering at `WB100031`, three operator logins on three security profiles
(see "Sign in" below), plus 12 customer contacts and 9 contract rates for the account screen.

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
# Build (from this folder). From the repository root use: dotnet build WeighbridgeAdmin.sln
dotnet build WeighbridgeAdmin.csproj -c Debug

# Run
.\bin\Debug\net48\WeighbridgeAdmin.exe
```

Or open `WeighbridgeAdmin.sln` (repository root) in Visual Studio and press F5. (`dotnet run` also
works, but F5 / the exe is the normal path for a WinForms app.)

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

## 4. Sign in

The first thing the application shows is a small **Sign In** dialog — before the main window,
and after the configuration and database checks. There is no password: this is a shop-floor
terminal, so the operator just picks their own name off the dropdown. The profile that name
works under appears underneath, and it is what decides which menus, toolbar buttons and
fields are available for the rest of the session. **Exit** closes without starting the app.

| Pick | Profile | What the session can do |
|---|---|---|
| **D. McGrath** | Weighbridge Operator | Customer list, add and edit customers, vehicle lookup, ticket list, raise and save weigh tickets. Delete is greyed. The price per tonne is read-only at the product's price-list rate. |
| **S. Patel** | Administrator | Everything, including deleting customers and overtyping the price per tonne. |
| **J. Reid** | Read Only | Customer list, vehicle lookup, ticket list and ticket view. New/Edit/Delete customer and New Weigh Ticket are all greyed. |

Signing in as a different operator means restarting the application — the privileges are read
once at sign-on and there is no "switch user".

If a command is greyed out, the profile does not grant it. If you reach one anyway — through a
shortcut key or a double-click that the screen forgot to disable — you get a
*"You do not have the 'X' privilege."* box instead. Both checks are deliberate.

A dialog reading *"The operator list could not be read"* on start-up means the security tables
are missing: re-run `Database\CreateDatabase.sql`.

---

## Using the application

The main window is an MDI parent with a menu bar and a status bar (operator name + live clock).
What is enabled on it depends on who signed in — see the table above.

| Menu | Item | What it does | Privilege |
|---|---|---|---|
| File | New Weigh Ticket (`Ctrl+N`) | Opens the weigh ticket wizard | `TICKET_CREATE` — **not** greyed here, only checked in the handler |
| File | Exit | Confirms, then closes | — |
| Customers | Customer List | Browsable, searchable customer grid (only one instance is ever opened) | `CUSTOMER_VIEW` |
| Customers | New Customer... | Add-customer dialog | `CUSTOMER_EDIT` |
| Vehicles | Vehicle Lookup... | Search dialog; shows the picked vehicle's details | `VEHICLE_VIEW` |
| Tickets | New Weigh Ticket | Same as File → New Weigh Ticket | `TICKET_CREATE` |
| Tickets | Ticket List (`Ctrl+L`) | Browse saved weigh tickets (only one instance is ever opened) | `TICKET_VIEW` |
| Tickets | Cascade Windows / Tile Horizontally | MDI layout | — |
| Help | About... | Version box | — |

### Weigh ticket wizard

Four tabs with Back / Next buttons; each page is validated before you can move on, and every page
is re-validated on Save.

1. **Vehicle & customer** — type a registration or press `F4` for the lookup dialog. Selecting a
   vehicle fills in the description, default tare and max gross, and selects the vehicle's customer.
2. **Weights** — gross and tare; net and net-tonnes update live. Gross over the vehicle's max gross
   shows a red warning and prompts for confirmation on Next.
3. **Charges** — product selection defaults the price per tonne. It is overtypeable only with the
   `TICKET_PRICE_OVERRIDE` privilege; without it the box is read-only and greyed at the price-list
   rate, and Save refuses any price that does not match the product default. Subtotal / GST /
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

New and Edit need `CUSTOMER_EDIT`; Delete needs `CUSTOMER_DELETE`, which only the Administrator
profile holds. The shortcut keys go through the same handlers as the buttons, so `F2` on a greyed
Edit gets the *"You do not have the..."* box rather than opening the dialog.

### Customer account

**Account** on the customer list toolbar (or `F6`) opens the full account screen for the selected
customer, as an MDI child rather than a dialog — one per customer, and you can leave it open
beside the list. It is the deepest screen in the application.

The top half is the account itself: code, name, A.B.N., contact details, credit limit, and two
address blocks — the trading address, and a postal address that can be left blank to bill to the
trading one. Under it are three tabs:

- **Contacts** — an editable grid. Type in the bottom row to add someone, select a row and press
  `Delete` to remove them. Position is a dropdown; Phone and Email are free text and the email is
  format-checked as you leave the cell. Exactly one contact has to be ticked as the primary, and
  ticking one unticks the rest.
- **Contract Rates** — an editable grid of negotiated prices per tonne, each with a product, a
  rate and a date window (leave *Effective to* blank for an open-ended rate). The rate cell only
  accepts digits. Two rates for the same product are not allowed to overlap in time, and you are
  told as soon as you leave the row if they do. The rate in force today is shown in black and
  expired ones in grey, with a totals line underneath.
- **Vehicles** — read only. Vehicles belong to the vehicle master, not to the account.

Everything on the screen saves together with one **Save**, which stays greyed until something
actually changes. Closing with unsaved changes prompts to save, discard or stay.

Contacts need `CUSTOMER_EDIT`. Contract rates need `TICKET_PRICE_OVERRIDE` — the same privilege
that lets an operator charge a weigh ticket at something other than the price-list rate — so for
the Weighbridge Operator profile the rates tab is greyed and relabelled *(read only)* while the
contacts beside it stay editable.

---

## Project layout

```
WeighbridgeAdmin.csproj      SDK-style project, net48, WinForms
app.config                   Connection string + app settings
Program.cs                   Entry point; DB reachability check, then sign-on, then the UI

Model/                       Plain data holders, no behaviour
  Customer.cs                Customer master
  Vehicle.cs                 Vehicle master (tare, max gross, owning customer)
  Product.cs                 Product + price per tonne + GST flag
  WeighTicket.cs             Ticket; also TicketStatus string constants
  WeighTicketSummary.cs      Read-only ticket row with rego/customer/product joined on
  UserAccount.cs             Operator login, with the profile name joined on
  SecurityProfile.cs         Job role (nothing constructs one yet)
  CustomerContact.cs         Someone to ring at a customer
  ContractRate.cs            Negotiated rate for one product over a date window

Security/
  Privileges.cs              One const string per privilege name
  SecurityContext.cs         Static signed-on session: SignIn, HasPrivilege, Demand

Controls/                    Reusable user controls
  AddressBlock.*             Address / suburb / state / postcode in a GroupBox
  SearchBox.*                Caption, search box, Clear button, record count
  CustomerHeaderPanel.*      Account fields plus TWO AddressBlock instances

Data/
  Repository.cs              All data access. Singleton via Repository.Current.
                             Inline SQL, one connection per call, manual reader mapping.

Forms/
  LoginForm.*                Sign-on dialog: operator dropdown, Sign In / Exit
  BaseEntryForm.*            Base class, not a screen: header, Save/Close, dirty flag
  CustomerAccountForm.*      Inherits BaseEntryForm; header panel + 3 tabs, 2 editable grids
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
- **There is authorisation but no authentication.** `LoginForm` picks an operator out of
  `dbo.Users`; `SecurityContext.SignIn` reads that profile's grants out of
  `dbo.SecurityProfilePrivileges` once into a `HashSet<string>` and holds them statically for
  the life of the process. Nothing verifies that the person at the keyboard is who they picked,
  and nothing re-reads the grants — changing a profile in the database needs a restart.
  `Repository.CurrentUserName` composes the status-bar string out of that session.
- **Every gated command is checked twice.** The control is disabled in the form's `Load` from
  `SecurityContext.HasPrivilege`, and the handler opens with `SecurityContext.Demand`, caught
  and turned into a message box. Note that `CustomerListForm.ReloadGrid` and
  `TicketListForm.ReloadGrid` re-set their toolbar buttons from the row count on every load, so
  the privilege has to be ANDed in there rather than only in `Load` — set it in `Load` alone and
  the next refresh quietly switches the button back on.
- **One rule is on a value, not a control.** `WeighTicketForm.numPrice` goes `ReadOnly` (with
  `Increment = 0`, because `ReadOnly` alone leaves the spin buttons working) without
  `TICKET_PRICE_OVERRIDE`, and `btnSave_Click` separately refuses to save a price that differs
  from the product's price-list rate. The control state is the courtesy; the value check is the
  rule.
- **`CustomerAccountForm` inherits `BaseEntryForm`.** Its Save and Close buttons, header labels,
  dirty flag and close prompt are not in its own designer file — they come from the base. The
  base declares those controls `protected` rather than `private` for exactly that reason, and
  drives the screen through three `virtual` hooks: `OnLoadRecord`, `OnValidateEntry` and
  `OnSaveRecord`. It is currently the only screen that inherits it; a second one would be the
  obvious next thing to add.
- **Save is enabled from two sources ANDed together** — `IsDirty && AllowSave`. The derived
  screen sets `AllowSave` from the operator's privileges once, and every edit flips `IsDirty`.
  Forget either and the button is wrong, which is the same trap as the toolbar buttons above.
- **Two save paths against `dbo.Customers`.** `SaveCustomer` (the old edit dialog) does not
  mention the four postal-address columns, so it cannot blank them; `SaveCustomerAccount` (the
  account screen) writes all of them. Adding the postal address to the older statement would
  look tidier and would quietly wipe data.
- **The editable grids hold the model on `DataGridViewRow.Tag`** and write it back cell by cell
  in `CellEndEdit`, so the working list stays correct even when the search box refills the grid.
  Deleted rows exist only as two `List<int>` fields of ids until Save runs. There is no
  transaction around the save: header, contacts and rates each go through their own connection,
  exactly like the rest of the application.
- **Two business rules live only in grid event handlers** — one primary contact per customer,
  and no overlapping date windows for the same product. Neither is a database constraint. If you
  are reading the schema to work out the rules, you will miss both.
