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

---

## Building

### Database (do this first on Windows)

```cmd
sqlcmd -S (localdb)\MSSQLLocalDB -Q "CREATE DATABASE WeighbridgeDb"
sqlcmd -S (localdb)\MSSQLLocalDB -d WeighbridgeDb -I -i WeighbridgeAdmin\Database\CreateDatabase.sql
```

Or open `Database/CreateDatabase.sql` in SSMS and run it against a `WeighbridgeDb`
database. The script drops and recreates every table, then loads the demo data, so
it is safe to re-run whenever you want a clean slate.

> The `-I` flag matters: it turns on `QUOTED_IDENTIFIER`, which SQL Server requires
> for the `PERSISTED` computed column on `WeighTickets`. The script also sets it
> internally, so SSMS and other tools are fine without the flag.

SQL Server **LocalDB** is the easiest option (ships with Visual Studio, or as a
standalone ~50 MB installer). Express or a full instance work too — just point the
`WeighbridgeDb` connection string in `app.config` at them.

If the app cannot reach the database it shows a message box naming the problem and
exits, rather than throwing an unhandled exception.

### On macOS (or Linux) — compile only

The project is an SDK-style csproj targeting `net48`, with an explicit
`Microsoft.NETFramework.ReferenceAssemblies` package reference and
`EnableWindowsTargeting`, so it compiles on a non-Windows machine even though it
cannot run there.

```bash
cd "WeightBridge POC"
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
| Every SQL statement in `Repository.cs` executes | ✅ all 19 extracted and run in isolated transactions |
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
| **MainForm** | MDI shell. Entry point for every other screen. | `MenuStrip` (File / Customers / Vehicles / Tickets / Help) with shortcut keys and `MdiWindowListItem`; `StatusStrip` with a sunken user panel and a spring-filled clock panel; `Timer` component ticking the clock every second; `IsMdiContainer = true`. | Application shell → React app shell + routing/layout. Menu tree → nav. Status bar → persistent header/footer. MDI child windows → tabs, routes or stacked panels (the hardest structural decision in the conversion). |
| **CustomerListForm** | Browse/search customers. Launch point for add, edit and delete. | `DataGridView` with `AutoGenerateColumns = false` and 8 columns declared in the Designer (incl. a `DataGridViewCheckBoxColumn`), bound through a `BindingSource`; `ToolStrip` with New/Edit/Delete/Refresh; search `TextBox` filtering on `TextChanged`; `CellDoubleClick` opens Edit; `KeyPreview` with F2/F5/Insert shortcuts. | **Simple list screen.** Data grid → React table component. `BindingSource` → client-side state/query. Designer-declared columns → a column config array. Filter-as-you-type → controlled input + derived list. Toolbar command enablement → derived UI state. |
| **CustomerEditForm** | Modal add/edit dialog, used for both New and Edit. | Labels and `TextBox`es positioned **absolutely** (no `TableLayoutPanel`/`FlowLayoutPanel`); `ComboBox` for State; `ErrorProvider` driving field-level errors; `CheckBox`; OK/Cancel with `DialogResult`; `FormBorderStyle.FixedDialog`, `MaximizeBox = false`. | **Simple add/edit form.** Absolute pixel layout → responsive/flow layout (the layout-inference problem). `ErrorProvider` → form validation library + inline error display. Modal `ShowDialog` + `DialogResult` → React modal with a promise/callback result. |
| **VehicleLookupDialog** | The reusable "F4 lookup" pattern: search, pick, return. | Search `TextBox` filtering on every keystroke; read-only `DataGridView` populated **row by row in code** with the `Vehicle` parked on `DataGridViewRow.Tag`; Select/Cancel; result handed back via the public `SelectedVehicle` property; optional `InitialSearchText` input property; inactive vehicles greyed out and confirmed before use. | **Lookup/picker dialog pattern.** Public in/out properties → component props + `onSelect` callback. Row `.Tag` object smuggling → typed row data. Reused from `WeighTicketForm`, so it proves the converted component is genuinely reusable rather than copy-pasted. |
| **WeighTicketForm** | The complex screen: a 4-step weigh ticket wizard. | `TabControl` with 4 pages plus Back/Next/Save/Cancel in a docked bottom `Panel`; `NumericUpDown` for gross/tare/price; read-only `Label`s for Net, Subtotal, GST, Total; a red overweight warning `Label`; `DateTimePicker`; three `ComboBox`es (customer, product, status); `"..."` button and F4 both opening `VehicleLookupDialog`; 13 caption/value label pairs on the Review tab; multiline Notes `TextBox`. | **Complex screen.** Wizard navigation with per-step validation → multi-step form. Cross-tab dependencies (vehicle → customer + tare; net → subtotal → GST → total) → derived state. Master-data defaulting that stays editable. Dialog-to-parent data flow. And the big one: **derived values computed in event handlers rather than held in a model** (see below). |

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
duplicate-code checking and delete-referential-integrity checks all live in
code-behind handlers. No MVP, no MVVM, no DI, no async, no service layer.

**`MessageBox.Show` everywhere** for errors, confirmations and even success — a
blocking, imperative interaction model that has no direct React equivalent.

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
grid alternating-row colours differ between the two grids. That inconsistency is
realistic, and forces the conversion to decide what to normalise into a design system
and what to preserve.

**Text from `.resx`.** Form titles and several header labels are pulled from each
form's resource file via `resources.GetString("$this.Text")` / `"lblHeader.Text"`,
alongside the About box message in `MainForm.resx`. The rest of the captions are
hardcoded in the Designer — the usual half-finished localisation, which means a
converter has to handle both sources of strings.

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
README.md
.vscode/                       launch.json (type "clr"), tasks.json
WeighbridgeAdmin/
  WeighbridgeAdmin.csproj      net48, UseWindowsForms, EnableWindowsTargeting
  app.config                   supportedRuntime + a couple of appSettings
  Program.cs                   [STAThread], EnableVisualStyles, Run(new MainForm())
  Properties/AssemblyInfo.cs
  Model/
    Customer.cs  Vehicle.cs  Product.cs  WeighTicket.cs   (+ TicketStatus constants)
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
```

## Domain model

| Entity | Fields |
|---|---|
| `Customer` | Id, Code, Name, ABN, Address, Suburb, State, Postcode, Phone, Email, CreditLimit, IsActive |
| `Vehicle` | Id, Registration, Description, TareWeight, MaxGross, CustomerId, IsActive |
| `Product` | Id, Code, Name, PricePerTonne, GstApplicable |
| `WeighTicket` | Id, TicketNumber, TicketDate, VehicleId, CustomerId, ProductId, GrossWeight, TareWeight, **NetWeight (derived)**, PricePerTonne, Subtotal, Gst, Total, Notes, Status |

Weights are kilograms; prices are dollars per tonne. `Subtotal = (Net / 1000) × PricePerTonne`,
`Gst = Subtotal × 10%` when the product is taxable, `Total = Subtotal + Gst`.
`Status` is a plain string — `Open`, `Completed` or `Void` — backed by a `varchar(10)`
column with a `CHECK` constraint. `NetWeight` is a `PERSISTED` computed column in SQL
*and* a derived property in C#; the app never reads the SQL one. Ticket numbers come
from a `Counters` table, allocated with `UPDATE ... OUTPUT deleted.NextValue` so two
operators cannot collide.

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
3. **Tickets → New Weigh Ticket** — press **F4** (or the `...` button) in Registration to open the
   vehicle lookup. Picking a vehicle fills Description and the default Tare, and auto-selects the
   customer. Next through the tabs; on **Weights** push Gross above the vehicle's Max Gross to
   trigger the red warning; on **Charges** pick a product to default the price, then overtype it;
   **Review** shows everything read back off the controls. Save raises the next ticket number.
4. Try `CLNFL - Clean Fill Received` on the Charges tab — it is GST-free, so the GST line stays at 0.00.
