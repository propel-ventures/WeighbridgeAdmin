# Weighbridge Administration System

A deliberately old-fashioned C# **WinForms** line-of-business application, built as the
**source material for a WinForms → React conversion POC**.

The domain is a weighbridge: trucks drive on, get weighed in and out, and a priced
weigh ticket is raised against a customer account. The app looks and behaves like a
2010-era internal system — MDI parent, menu strip, status bar, absolute control
positioning, modal lookup dialogs, and all of the business logic sitting inside
control event handlers.

Data lives in **SQL Server**, reached through straight ADO.NET — `SqlConnection`,
`SqlCommand`, `SqlDataReader`, inline SQL as string constants, readers mapped onto
the model classes by hand. No ORM, no Entity Framework, no unit of work, no caching.
The database ships with 15 customers, 20 vehicles, 8 products and 30 weigh tickets
of Australian-flavoured dummy data.

> This README is written for whoever is doing the conversion — what is in here, and why.
> [`WeighbridgeAdmin/README.md`](WeighbridgeAdmin/README.md) is the setup-facing guide,
> with the longer install and troubleshooting walkthrough.

---

## Building

### Database (do this first on Windows)

The create script builds the *tables* and loads the demo data — it does **not** create the
database, so create `WeighbridgeDb` first. The app will not start until the database is
reachable: `Program.Main` runs a connection test up front and shows a message box naming
the problem instead of the main window, rather than throwing an unhandled exception.

The shipped `app.config` points at a local **SQL Server Express** instance
(`localhost\SQLEXPRESS`):

```cmd
sqlcmd -S localhost\SQLEXPRESS -Q "IF DB_ID('WeighbridgeDb') IS NULL CREATE DATABASE WeighbridgeDb;"
sqlcmd -S localhost\SQLEXPRESS -d WeighbridgeDb -I -i WeighbridgeAdmin\Database\CreateDatabase.sql
```

**LocalDB** works just as well and needs no service install (it ships with Visual Studio,
or as a standalone ~50 MB installer). Point the `WeighbridgeDb` connection string in
`app.config` at it first:

```cmd
sqlcmd -S (localdb)\MSSQLLocalDB -Q "IF DB_ID('WeighbridgeDb') IS NULL CREATE DATABASE WeighbridgeDb;"
sqlcmd -S (localdb)\MSSQLLocalDB -d WeighbridgeDb -I -i WeighbridgeAdmin\Database\CreateDatabase.sql
```

Or open `Database/CreateDatabase.sql` in SSMS and run it against a `WeighbridgeDb`
database. The script drops and recreates every table, then loads the demo data, so
it is safe to re-run whenever you want a clean slate.

> The `-I` flag matters: it turns on `QUOTED_IDENTIFIER`, which SQL Server requires
> for the `PERSISTED` computed column on `WeighTickets`. The script also sets it
> internally, so SSMS and other tools are fine without the flag.

### Configuration

`app.config` carries the connection string plus two `appSettings`:

| Setting | Effect |
|---|---|
| `GstRate` | The G.S.T. rate as a **decimal fraction**, not a percentage — `0.10` means 10%. Read once, when `Repository.Current` is first touched. A missing setting falls back to 0.10; one that is present but unparseable, negative or above 1 is a startup error rather than a silent fallback, because quietly charging the wrong tax is worse. The rate flows through to the on-screen captions as well as the arithmetic. |
| `SiteName` | **Dead setting** — nothing in the codebase reads it. Left in deliberately; every legacy config file has one. |

### On macOS (or Linux) — compile only

The project is an SDK-style csproj targeting `net48`, with an explicit
`Microsoft.NETFramework.ReferenceAssemblies` package reference and
`EnableWindowsTargeting`, so it compiles on a non-Windows machine even though it
cannot run there.

```bash
dotnet build WeighbridgeAdmin.sln
```

Output lands in `WeighbridgeAdmin/bin/Debug/net48/WeighbridgeAdmin.exe`.

Verified clean: `dotnet build` from scratch reports **0 warnings, 0 errors**.

> You can build but **cannot run** the app on macOS — `System.Windows.Forms` needs
> the real .NET Framework on Windows. The SQL schema, however, *was* verified on
> macOS against SQL Server 2022 in Docker; see "What has actually been verified".

### On Windows — build and run

.NET Framework 4.8 ships with Windows 10 1903+ and Windows 11, so no runtime
install is normally needed.

```cmd
dotnet build WeighbridgeAdmin.sln
WeighbridgeAdmin\bin\Debug\net48\WeighbridgeAdmin.exe
```

Or just double-click `WeighbridgeAdmin.exe`. The solution also opens directly in
Visual Studio 2019/2022, and every form opens in the WinForms designer.

---

## What has actually been verified

Being explicit, because some of this is tested and some isn't:

| Claim | Status |
|---|---|
| `dotnet build` clean from scratch | ✅ 0 warnings, 0 errors |
| `CreateDatabase.sql` runs without error | ✅ against SQL Server 2022 in Docker |
| Seeded data matches the original in-memory values | ✅ all 30 tickets compared field by field |
| Every SQL statement in `Repository.cs` executes | ⚠️ 19 of 20 — extracted and run in isolated transactions. The ticket-summary query behind `SearchTickets` arrived later with the ticket browse screen and has not been run in isolation. |
| Computed `NetWeight` column agrees with `Gross - Tare` | ✅ 0 mismatching rows |
| GST-free product yields zero GST | ✅ |
| The app itself running end to end | ❌ **never run** — WinForms needs Windows |
| Forms rendering, tab flow, F4 lookup, save round-trip | ❌ not verified |

The data layer is well covered; the UI is not. First run on Windows is still the
real test — particularly the F4 vehicle lookup and the Save path, which is the only
place a `WeighTicket` is written.

## Screen map

| Form | Purpose | Controls of note | POC requirement it exercises |
|---|---|---|---|
| **MainForm** | MDI shell. Entry point for every other screen. | `MenuStrip` (File / Customers / Vehicles / Tickets / Help) with shortcut keys — Ctrl+N new ticket, Ctrl+L ticket list — and `MdiWindowListItem`; `StatusStrip` with a sunken user panel and a spring-filled clock panel; `Timer` component ticking the clock every second; `IsMdiContainer = true`. | Application shell → React app shell + routing/layout. Menu tree → nav. Status bar → persistent header/footer. MDI child windows → tabs, routes or stacked panels (the hardest structural decision in the conversion). Singleton child windows enforced by scanning `MdiChildren` → route identity. |
| **CustomerListForm** | Browse/search customers. Launch point for add, edit and delete. | `DataGridView` with `AutoGenerateColumns = false` and 8 columns declared in the Designer (incl. a `DataGridViewCheckBoxColumn`), bound through a `BindingSource`; `ToolStrip` with New/Edit/Delete/Refresh; search `TextBox` filtering on `TextChanged`; `CellDoubleClick` opens Edit; `KeyPreview` with F2/F5/Insert shortcuts. | **Simple list screen.** Data grid → React table component. `BindingSource` → client-side state/query. Designer-declared columns → a column config array. Filter-as-you-type → controlled input + derived list. Toolbar command enablement → derived UI state. |
| **CustomerEditForm** | Modal add/edit dialog, used for both New and Edit. | Labels and `TextBox`es positioned **absolutely** (no `TableLayoutPanel`/`FlowLayoutPanel`); `ComboBox` for State; `ErrorProvider` driving field-level errors; `CheckBox`; OK/Cancel with `DialogResult`; `FormBorderStyle.FixedDialog`, `MaximizeBox = false`. | **Simple add/edit form.** Absolute pixel layout → responsive/flow layout (the layout-inference problem). `ErrorProvider` → form validation library + inline error display. Modal `ShowDialog` + `DialogResult` → React modal with a promise/callback result. |
| **VehicleLookupDialog** | The reusable "F4 lookup" pattern: search, pick, return. | Search `TextBox` filtering on every keystroke; read-only `DataGridView` populated **row by row in code** with the `Vehicle` parked on `DataGridViewRow.Tag`; Select/Cancel; result handed back via the public `SelectedVehicle` property; optional `InitialSearchText` input property; inactive vehicles greyed out and confirmed before use. | **Lookup/picker dialog pattern.** Public in/out properties → component props + `onSelect` callback. Row `.Tag` object smuggling → typed row data. Reused from `WeighTicketForm`, so it proves the converted component is genuinely reusable rather than copy-pasted. |
| **WeighTicketForm** | The complex screen: a 4-step weigh ticket wizard. | `TabControl` with 4 pages plus Back/Next/Save/Cancel in a docked bottom `Panel`; `NumericUpDown` for gross/tare/price; read-only `Label`s for Net, Subtotal, GST, Total; a red overweight warning `Label`; `DateTimePicker`; three `ComboBox`es (customer, product, status); `"..."` button and F4 both opening `VehicleLookupDialog`; 13 caption/value label pairs on the Review tab; multiline Notes `TextBox`. | **Complex screen.** Wizard navigation with per-step validation → multi-step form. Cross-tab dependencies (vehicle → customer + tare; net → subtotal → GST → total) → derived state. Master-data defaulting that stays editable. Dialog-to-parent data flow. And the big one: **derived values computed in event handlers rather than held in a model** (see below). |
| **TicketListForm** | Weigh ticket browse. Read only — a saved ticket is an accounting document, so nothing here edits or deletes one. | `DataGridView` with 13 Designer-declared columns bound through a `BindingSource` to `WeighTicketSummary`; a filter strip of `CheckBox` + two `DateTimePicker`s, customer and status `ComboBox`es and a search box, all wired into one shared `Filter_Changed` handler; row fore-colour set per row after every load; a totals `Label` rebuilt as one concatenated string; View/Refresh/Clear Filters `ToolStrip`; F2/F5. | **Filtered list and reporting screen.** Multi-field filter panel → one filter state object driving one query. Sentinel rows (`Id 0` = "(All customers)", index 0 = "(All statuses)") → optional/nullable filter params. Status-driven row colouring → conditional row styling. Footer aggregates computed in the UI → derived/memoised totals. A public `ReloadGrid()` called from two other screens → shared cache invalidation. |

---

## The deliberately awkward bits

These are in here on purpose. They are the parts a naive conversion gets wrong, so
they are the parts worth measuring.

**No model object while editing.** `WeighTicketForm` never holds a `WeighTicket`
instance during data entry. Every value lives on a control. Net, subtotal, GST and
total are recalculated inside `numGross_ValueChanged`, `numTare_ValueChanged`,
`cboProduct_SelectedIndexChanged` and `numPrice_ValueChanged`, and written straight
onto read-only `Label`s. A `WeighTicket` is constructed for the first and only time
inside `btnSave_Click`, by reading the controls back. The Review tab likewise
re-reads the controls every time it is shown. The only things cached on the form are
two vehicle master values (`_vehicleId`, `_vehicleMaxGross`) needed for the
overweight check — the real legacy app works exactly this way, and the conversion
has to infer the implicit model that was never written down.

**Business logic in event handlers.** Validation, GST rules, price defaulting,
duplicate-code checking, delete-referential-integrity checks and the ticket list's
money totals all live in code-behind handlers. No MVP, no MVVM, no DI, no async,
no service layer.

**`MessageBox.Show` everywhere** for errors, confirmations and even success — a
blocking, imperative interaction model that has no direct React equivalent. It is
also the *only* ticket detail view: `TicketListForm` "opens" a ticket by formatting
it into a fixed-width message box body.

**A `_loading` guard flag.** Both `WeighTicketForm` and `TicketListForm` carry a
private `bool _loading`, raised while `Form_Load` populates combo boxes purely to stop
`SelectedIndexChanged` handlers firing mid-populate and re-querying or recalculating.
`TicketListForm.tbbClearFilters_Click` raises it again so resetting four filters costs
one round trip instead of four. Implicit, order-dependent state that a declarative UI
does not need, and that has to be recognised as scaffolding rather than behaviour.

**Naive data access straight from the UI.** Every keystroke in a search box goes back
to SQL Server — no debounce, no client-side filtering. `VehicleLookupDialog.FillGrid`
is worse: it calls `GetCustomerById` once **per row**, so a lookup over 20 vehicles
opens 21 connections. Realistic, and a fair thing for the conversion to fix.

**Inline SQL, hand-rolled mapping.** Every query is a string constant inside
`Repository.cs`, every result row is copied field by field onto a model object, and
a connection is opened and closed per call. Queries *are* parameterised — period-accurate
code would have concatenated `txtSearch.Text` straight into the WHERE clause, but
seeding a working SQL injection hole into a demo app isn't worth the authenticity points.

**Copy-pasted styling.** `new Font("Microsoft Sans Serif", 8.25F, ...)` is repeated on
essentially every control, `Color.FromArgb(240, 240, 240)` on every form, header
labels in bold `Color.Navy`, buttons hardcoded to 75×23. Nothing is shared. The forms
are also mildly inconsistent with each other — `VehicleLookupDialog` uses the old
Windows XP beige `Color.FromArgb(236, 233, 216)` while everything else is grey, and
grid alternating-row colours differ between the customer and ticket grids. That
inconsistency is realistic, and forces the conversion to decide what to normalise into
a design system and what to preserve.

**Text from `.resx`.** Form titles and several header labels are pulled from each
form's resource file via `resources.GetString("$this.Text")` / `"lblHeader.Text"`,
alongside the About box message in `MainForm.resx`. The rest of the captions are
hardcoded in the Designer, and `TicketListForm` — the newest screen — has no `.resx`
at all. The usual half-finished localisation, which means a converter has to handle
both sources of strings.

**Real `.Designer.cs` files.** Every form has one, with controls declared as fields
and configured inside `InitializeComponent` exactly as Visual Studio emits it —
`SuspendLayout`/`ResumeLayout`, `ISupportInitialize` around `DataGridView` and
`NumericUpDown`, `DataGridViewCellStyle` locals, reverse-order `Controls.Add` for
docking, `System.Drawing.Point`/`Size` literals. No control is created in a
constructor. This is the actual input a conversion tool has to parse.

---

## Project layout

```
WeighbridgeAdmin.sln
README.md                      this file - conversion-facing overview
.vscode/                       launch.json (type "clr"), tasks.json
WeighbridgeAdmin/
  README.md                    setup / install / troubleshooting guide
  WeighbridgeAdmin.csproj      net48, UseWindowsForms, EnableWindowsTargeting
  app.config                   supportedRuntime, connection string, appSettings
  Program.cs                   [STAThread], EnableVisualStyles, Run(new MainForm())
  Properties/AssemblyInfo.cs
  Model/
    Customer.cs  Vehicle.cs  Product.cs  WeighTicket.cs   (+ TicketStatus constants)
    WeighTicketSummary.cs      read-only joined ticket row for the browse grid
  Data/
    Repository.cs              singleton, ADO.NET against SQL Server
  Database/
    CreateDatabase.sql         schema + demo data; copied next to the exe on build
  Forms/
    MainForm.cs            / .Designer.cs / .resx
    CustomerListForm.cs    / .Designer.cs / .resx
    CustomerEditForm.cs    / .Designer.cs / .resx
    VehicleLookupDialog.cs / .Designer.cs / .resx
    WeighTicketForm.cs     / .Designer.cs / .resx
    TicketListForm.cs      / .Designer.cs            (no .resx - see above)
```

## Domain model

| Entity | Fields |
|---|---|
| `Customer` | Id, Code, Name, ABN, Address, Suburb, State, Postcode, Phone, Email, CreditLimit, IsActive |
| `Vehicle` | Id, Registration, Description, TareWeight, MaxGross, CustomerId, IsActive |
| `Product` | Id, Code, Name, PricePerTonne, GstApplicable |
| `WeighTicket` | Id, TicketNumber, TicketDate, VehicleId, CustomerId, ProductId, GrossWeight, TareWeight, **NetWeight (derived)**, PricePerTonne, Subtotal, Gst, Total, Notes, Status |
| `WeighTicketSummary` | The same ticket with Registration, CustomerCode/Name and ProductCode/Name already joined on, plus derived **NetTonnes**. Read only — nothing writes a summary back. It exists so the browse grid fills from one query instead of going back to the database once per row. |

Weights are kilograms; prices are dollars per tonne. `Subtotal = (Net / 1000) × PricePerTonne`,
`Gst = Subtotal × GstRate` (10% unless `app.config` says otherwise) when the product is taxable,
`Total = Subtotal + Gst`. `Status` is a plain string — `Open`, `Completed` or `Void` — backed by a
`varchar(10)` column with a `CHECK` constraint. `NetWeight` is a `PERSISTED` computed column in SQL
*and* a derived property in C#; the app derives its own everywhere except the browse query, which
selects the SQL one onto `WeighTicketSummary`. Ticket numbers come from a `Counters` table,
allocated with `UPDATE ... OUTPUT deleted.NextValue` so two operators cannot collide.

### One gotcha worth knowing

**C# and SQL Server round differently.** `Math.Round(x, 2)` uses banker's rounding
(ties go to the nearest even digit); T-SQL `ROUND(x, 2)` rounds half away from zero.
On this dataset they disagree by one cent on **6 of the 30 tickets** — e.g. a subtotal
of `654.585` becomes `654.58` in the app and `654.59` in SQL.

Because the app computes `Subtotal`/`Gst`/`Total` in C# and stores the results, the
seed script stores those same values as literals rather than recomputing them in SQL.
Anything that recalculates totals server-side — a report, a view, a migration — will
drift from the app unless it replicates banker's rounding.

## Walkthrough

1. Launch — `MainForm` opens maximised, status bar shows the operator and a live clock.
2. **Customers → Customer List** — type in the search box to filter, double-click a row to edit,
   try saving with a blank Code or a 3-digit postcode to see the `ErrorProvider` fire.
3. **Tickets → New Weigh Ticket** (Ctrl+N) — press **F4** (or the `...` button) in Registration to
   open the vehicle lookup. Picking a vehicle fills Description and the default Tare, and
   auto-selects the customer. Next through the tabs; on **Weights** push Gross above the vehicle's
   Max Gross to trigger the red warning; on **Charges** pick a product to default the price, then
   overtype it; **Review** shows everything read back off the controls. Save raises the next
   ticket number.
4. Try `CLNFL - Clean Fill Received` on the Charges tab — it is GST-free, so the GST line stays at 0.00.
5. **Tickets → Ticket List** (Ctrl+L) — the ticket just saved is already there; an open list is
   refreshed by `WeighTicketForm` on save. Open tickets show amber, voided ones grey. Tick the date
   range box to switch the two `DateTimePicker`s on, or narrow by customer, status and free text.
   The totals line under the grid covers whatever is on screen and **excludes voided tickets from
   the money** while still counting them in the row count. Double-click or F2 to "view" a ticket —
   which is a message box.
