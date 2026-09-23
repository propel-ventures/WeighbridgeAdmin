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
of Australian-flavoured dummy data, plus three security profiles and the three
operator logins that sit on them.

Authorisation is the period-accurate kind: a `Users` table, a `SecurityProfiles`
table, a grant row per privilege, a sign-on dialog with no password, and every
gated command switched off in `Form_Load` **and** checked again inside its event
handler. See "Security model" below — it is the main thing the conversion has to
extract as a rule set rather than as UI.

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
| `dotnet build` clean from scratch | ✅ 0 warnings, 0 errors (`-t:Rebuild`, Windows) |
| `CreateDatabase.sql` runs without error | ✅ SQL Server 2022 in Docker (original schema), and re-verified against SQL Server 2025 Express after the security and account tables were added |
| `CreateDatabase.sql` is safe to re-run | ✅ run twice back to back, second pass clean, on the full schema |
| Seeded data matches the original in-memory values | ✅ all 30 tickets compared field by field |
| Every SQL statement in `Repository.cs` executes | ⚠️ the three security queries were extracted and run in isolated transactions; the ticket-summary query behind `SearchTickets` still has not been. The nine account-screen queries have not been run in isolation either, but `GetCustomerAccount`, `GetCustomerContacts`, `GetContractRates`, `GetVehiclesByCustomer` and `SaveCustomerAccount` were all exercised through the running screen. `SaveContact`, `DeleteContact`, `SaveContractRate` and `DeleteContractRate` have **not run at all**. |
| Computed `NetWeight` column agrees with `Gross - Tare` | ✅ 0 mismatching rows |
| GST-free product yields zero GST | ✅ |
| Privilege loading, `HasPrivilege`, `Demand` | ✅ all three profiles driven through `SecurityContext` against the real database; `Demand` throws `UnauthorizedAccessException` carrying the privilege name |
| Status bar text for each operator | ✅ `D. MCGRATH (Weighbridge Operator)`, `J. REID (Read Only)`, `S. PATEL (Administrator)` |
| The app starting, signing on and opening `MainForm` | ✅ launched on Windows; the sign-on dialog lists all three operators and shows the profile as you move through the combo |
| `MainForm` menu enablement per profile | ✅ read off the live menus — Read Only has *New Customer...* and *Tickets → New Weigh Ticket* greyed, the other two do not |
| `CustomerListForm` toolbar enablement per profile | ✅ read off the live toolbar — New/Edit greyed for Read Only, Delete greyed for everyone except Administrator |
| `numPrice` read-only without `TICKET_PRICE_OVERRIDE` | ❌ not verified in the running UI — the code sets it in `Form_Load`, but the weigh ticket screen was not driven far enough to read the control back |
| `WeighTicketForm.btnSave`, `CustomerEditForm.btnOK`, `TicketListForm.tbbView` enablement | ❌ not verified in the running UI |
| The "Access denied" message box actually appearing | ❌ not verified — `Demand` throwing the right exception is verified, the `MessageBox.Show` that catches it is not |
| `CustomerAccountForm` opening with its inherited chrome | ✅ opened on Windows from the customer list; the inherited header, dirty label and Save/Close strip are all present and the screen is titled from the record |
| Both `AddressBlock` instances rendering with their own data | ✅ trading address showed `14 Kessler Drive / Yatala / 4207` and postal `PO Box 417 / Beenleigh / 4207` on the same screen |
| Contacts grid rendering as editable | ✅ two contacts plus the new-row placeholder, with text cells, the `Position` combo cell and the `Primary` check box cell all live |
| The four-hop dirty chain | ✅ changing the State combo inside an `AddressBlock` lit the inherited Save button and flipped the label to "Modified - not yet saved" |
| Privilege cascade on the account screen | ✅ as Read Only the address combo could not even take focus and Save stayed disabled; as Administrator both worked |
| Account screen save round-trip | ✅ edited the trading State, saved, and the change landed in `dbo.Customers`; the dirty flag cleared and **`PostalState` was left untouched**, which is the two-save-path behaviour working |
| Contract rates tab — editing, date validation, overlap check, footer totals | ❌ not verified in the running UI. The rates grid was never driven by hand; only the contacts grid was seen rendering. |
| Contact/rate insert and delete through the grids | ❌ not verified — the save round-trip that was tested only changed a header field |
| Forms rendering, tab flow, F4 lookup, weigh ticket save round-trip | ❌ not verified |

The data layer and the privilege model are well covered; most of the UI still is not.
The sign-on path, the main menu and the customer toolbar have now been seen working on
Windows. Everything below that — the weigh ticket wizard in particular, which is the only
place a `WeighTicket` is written and the only place the price rule bites — has not been
driven by hand.

## Screen map

| Form | Purpose | Controls of note | POC requirement it exercises |
|---|---|---|---|
| **LoginForm** | Shop-floor sign on, shown modally by `Program.Main` before anything else. No password — picking the operator off a dropdown is the whole flow. | `ComboBox` of active users bound to `List<UserAccount>` with `DisplayMember = "FullName"`; a bold profile label rewritten in `SelectedIndexChanged`; Sign In / Exit with `DialogResult`; `FormBorderStyle.FixedDialog`, `MaximizeBox = false`, `StartPosition.CenterScreen`. Nothing here is in a `.resx`. | **Authentication and session bootstrap.** A modal that gates the whole app → route guard / auth provider at the React root. `SecurityContext` static session → auth context. The result is carried out on a public `SelectedUser` property, not raised as an event. |
| **MainForm** | MDI shell. Entry point for every other screen. | `MenuStrip` (File / Customers / Vehicles / Tickets / Help) with shortcut keys — Ctrl+N new ticket, Ctrl+L ticket list — and `MdiWindowListItem`; `StatusStrip` with a sunken user panel and a spring-filled clock panel; `Timer` component ticking the clock every second; `IsMdiContainer = true`. | Application shell → React app shell + routing/layout. Menu tree → nav. Status bar → persistent header/footer. MDI child windows → tabs, routes or stacked panels (the hardest structural decision in the conversion). Singleton child windows enforced by scanning `MdiChildren` → route identity. |
| **CustomerListForm** | Browse/search customers. Launch point for add, edit and delete. | `DataGridView` with `AutoGenerateColumns = false` and 8 columns declared in the Designer (incl. a `DataGridViewCheckBoxColumn`), bound through a `BindingSource`; `ToolStrip` with New/Edit/Delete/Refresh; search `TextBox` filtering on `TextChanged`; `CellDoubleClick` opens Edit; `KeyPreview` with F2/F5/Insert shortcuts. | **Simple list screen.** Data grid → React table component. `BindingSource` → client-side state/query. Designer-declared columns → a column config array. Filter-as-you-type → controlled input + derived list. Toolbar command enablement → derived UI state. |
| **CustomerEditForm** | Modal add/edit dialog, used for both New and Edit. | Labels and `TextBox`es positioned **absolutely** (no `TableLayoutPanel`/`FlowLayoutPanel`); `ComboBox` for State; `ErrorProvider` driving field-level errors; `CheckBox`; OK/Cancel with `DialogResult`; `FormBorderStyle.FixedDialog`, `MaximizeBox = false`. | **Simple add/edit form.** Absolute pixel layout → responsive/flow layout (the layout-inference problem). `ErrorProvider` → form validation library + inline error display. Modal `ShowDialog` + `DialogResult` → React modal with a promise/callback result. |
| **VehicleLookupDialog** | The reusable "F4 lookup" pattern: search, pick, return. | Search `TextBox` filtering on every keystroke; read-only `DataGridView` populated **row by row in code** with the `Vehicle` parked on `DataGridViewRow.Tag`; Select/Cancel; result handed back via the public `SelectedVehicle` property; optional `InitialSearchText` input property; inactive vehicles greyed out and confirmed before use. | **Lookup/picker dialog pattern.** Public in/out properties → component props + `onSelect` callback. Row `.Tag` object smuggling → typed row data. Reused from `WeighTicketForm`, so it proves the converted component is genuinely reusable rather than copy-pasted. |
| **WeighTicketForm** | The complex screen: a 4-step weigh ticket wizard. | `TabControl` with 4 pages plus Back/Next/Save/Cancel in a docked bottom `Panel`; `NumericUpDown` for gross/tare/price; read-only `Label`s for Net, Subtotal, GST, Total; a red overweight warning `Label`; `DateTimePicker`; three `ComboBox`es (customer, product, status); `"..."` button and F4 both opening `VehicleLookupDialog`; 13 caption/value label pairs on the Review tab; multiline Notes `TextBox`. | **Complex screen.** Wizard navigation with per-step validation → multi-step form. Cross-tab dependencies (vehicle → customer + tare; net → subtotal → GST → total) → derived state. Master-data defaulting that stays editable. Dialog-to-parent data flow. And the big one: **derived values computed in event handlers rather than held in a model** (see below). |
| **TicketListForm** | Weigh ticket browse. Read only — a saved ticket is an accounting document, so nothing here edits or deletes one. | `DataGridView` with 13 Designer-declared columns bound through a `BindingSource` to `WeighTicketSummary`; a filter strip of `CheckBox` + two `DateTimePicker`s, customer and status `ComboBox`es and a search box, all wired into one shared `Filter_Changed` handler; row fore-colour set per row after every load; a totals `Label` rebuilt as one concatenated string; View/Refresh/Clear Filters `ToolStrip`; F2/F5. | **Filtered list and reporting screen.** Multi-field filter panel → one filter state object driving one query. Sentinel rows (`Id 0` = "(All customers)", index 0 = "(All statuses)") → optional/nullable filter params. Status-driven row colouring → conditional row styling. Footer aggregates computed in the UI → derived/memoised totals. A public `ReloadGrid()` called from two other screens → shared cache invalidation. |
| **BaseEntryForm** | Not a screen — the base class `CustomerAccountForm` inherits. Owns the header labels, the docked Save/Close strip, the dirty flag and the "unsaved changes" prompt on `FormClosing`. | `protected` (not private) control fields, so the inheriting designer can reach them; three `virtual` hooks — `OnLoadRecord`, `OnValidateEntry`, `OnSaveRecord`; `Save` enabled from **two** sources ANDed together, `IsDirty && AllowSave`. | **Visual form inheritance.** The killer detail: the derived screen's `.Designer.cs` does **not** contain any of these controls, so a converter that parses designer files in isolation produces a screen missing its whole button strip and header. The inheritance has to be followed. |
| **CustomerAccountForm** | The deep screen: customer header, contacts and contract rates, all saved together. Inherits `BaseEntryForm`. Opened from the customer list toolbar (or F6) as an MDI child, one per customer. | `CustomerHeaderPanel` (a `UserControl` containing two `AddressBlock` `UserControl`s) over a nested `TabControl` of three pages; **two editable `DataGridView`s** with `AllowUserToAddRows`, a `DataGridViewComboBoxColumn`, a `DataGridViewCheckBoxColumn`, `CellValidating` with `e.Cancel`, `DefaultValuesNeeded`, `UserDeletingRow`, `EditingControlShowing` hanging a `KeyPress` filter on the editor, `CurrentCellDirtyStateChanged` + `CommitEdit`, and a `DataError` handler; two `SearchBox` `UserControl` instances; a footer total rebuilt on every cell edit. | **The hard screen.** Master–detail with in-grid editing → React form-array state that exists nowhere in the source, only across eight event handlers. Deleted rows tracked in two `List<int>` fields that are the *only* record a row existed. Three privileges landing on one screen. A user control inside a user control inside an inherited form → three levels of component extraction. |

---

## Security model

Three tables, a static session object, and privilege checks copy-pasted into the
screens. There is no password and no audit trail — this is the authorisation half
of a legacy LOB app, which is the half a conversion actually has to reproduce.

```
dbo.SecurityProfiles          ProfileId, ProfileName, Description
dbo.SecurityProfilePrivileges ProfileId (FK), PrivilegeName      -- one row per grant
dbo.Users                     UserId, UserName, FullName, ProfileId (FK), IsActive
```

The account screen added two more tables alongside them:

```
dbo.CustomerContacts  ContactId, CustomerId (FK), ContactName, Position, Phone, Email, IsPrimary
dbo.ContractRates     RateId, CustomerId (FK), ProductId (FK), RatePerTonne,
                      EffectiveFrom, EffectiveTo (NULL = open ended), Notes
```

Neither has a constraint behind the rule that matters. At most one contact per customer may
be primary, and two rate windows for the same product must not overlap — both are enforced
in grid event handlers while the operator types, and nowhere else. That is period-accurate,
and it means the conversion has to find those rules in the UI code because the schema does
not state them.

`Program.Main` shows `LoginForm` after the config and connection checks; on OK it calls
`SecurityContext.SignIn(user)`, which reads that profile's grant rows once into a
`HashSet<string>`. Nothing re-reads them, so changing a profile in the database needs a
restart. `Repository.CurrentUserName` — still the only thing the status bar knows about —
now composes `FULLNAME (Profile)` out of the session instead of returning a literal.

### Sign in as

| Operator | Profile | Can | Cannot |
|---|---|---|---|
| **D. McGrath** (`dmcgrath`) | Weighbridge Operator | Browse customers, add and edit them, look vehicles up, browse tickets, raise and save a new weigh ticket at the price-list rate | Delete a customer; change the price per tonne away from the product default |
| **S. Patel** (`sadmin`) | Administrator | Everything, including deleting customers and overriding the price per tonne | — |
| **J. Reid** (`jreid`) | Read Only | Browse customers, look vehicles up, browse and view tickets | Add or edit a customer; delete one; raise a weigh ticket; override a price |

### Privilege → control map

Every gated action is switched off up front **and** checked again inside the handler,
which is how a real system of this vintage does it — the greyed control is a courtesy,
the check is the rule.

| Privilege | Form | Control disabled in `Form_Load` | `Demand` in handler |
|---|---|---|---|
| `CUSTOMER_VIEW` | MainForm | `mnuCustomersList` | `mnuCustomersList_Click` |
| `CUSTOMER_EDIT` | MainForm | `mnuCustomersNew` | `mnuCustomersNew_Click` |
| `CUSTOMER_EDIT` | CustomerListForm | `tbbNew`, `tbbEdit` | `tbbNew_Click`, `tbbEdit_Click` |
| `CUSTOMER_EDIT` | CustomerEditForm | `btnOK` | `btnOK_Click` |
| `CUSTOMER_DELETE` | CustomerListForm | `tbbDelete` | `tbbDelete_Click` |
| `VEHICLE_VIEW` | MainForm | `mnuVehiclesLookup` | `mnuVehiclesLookup_Click` |
| `TICKET_VIEW` | MainForm | `mnuTicketsList` | `mnuTicketsList_Click` |
| `TICKET_VIEW` | TicketListForm | `tbbView` | `tbbView_Click` |
| `TICKET_CREATE` | MainForm | `mnuTicketsNew` | `mnuTicketsNew_Click` |
| `TICKET_CREATE` | WeighTicketForm | `btnSave` | `btnSave_Click` |
| `TICKET_PRICE_OVERRIDE` | WeighTicketForm | `numPrice` goes `ReadOnly` (see below) | `btnSave_Click` compares the price against the product default |
| `CUSTOMER_VIEW` | CustomerListForm | `tbbAccount` | `tbbAccount_Click` |
| `CUSTOMER_EDIT` | CustomerAccountForm | header panel goes read-only, contacts grid goes read-only, `AllowSave` | base class refuses the save |
| `TICKET_PRICE_OVERRIDE` | CustomerAccountForm | whole **rates grid** read-only, tab relabelled "(read only)" | — |

`CustomerAccountForm` is the only screen where three privileges land at once, and where a
privilege greys a whole grid rather than a button: contract rates are a pricing decision,
so they need the same privilege that lets an operator move a weigh ticket off the price
list, while the contacts beside them only need `CUSTOMER_EDIT`.

A denial that gets past the greying shows
`MessageBox.Show("You do not have the 'X' privilege.", "Access denied", ...)`.

Three things are worth knowing before the conversion reads too much into the table:

- **`CUSTOMER_VIEW`, `VEHICLE_VIEW` and `TICKET_VIEW` are granted to all three seeded
  profiles**, so those three gates never actually grey anything for the shipped users.
  They exist because the rule exists, not because anyone is currently denied. The four
  privileges that do differentiate are `CUSTOMER_EDIT`, `CUSTOMER_DELETE`,
  `TICKET_CREATE` and `TICKET_PRICE_OVERRIDE`.
- **`CustomerEditForm.btnOK` is unreachable belt-and-braces.** Every way into that dialog
  is already gated on `CUSTOMER_EDIT`, so the check there can only fire if a future
  screen opens it without checking. Left in deliberately: legacy apps are full of these.
- **`File → New Weigh Ticket` (and its `Ctrl+N` shortcut) is *not* greyed**, because it is
  a second menu item — `mnuFileNewTicket` — pointing at the same handler as the gated
  `mnuTicketsNew`. A Read Only operator can still reach it, and the `Demand` at the top of
  `mnuTicketsNew_Click` is what stops them. That gap is real legacy behaviour and is the
  clearest demonstration in the app of why the handler check has to exist at all.

### The conditional read-only field

`WeighTicketForm.numPrice` defaults from the selected product's `PricePerTonne`. Without
`TICKET_PRICE_OVERRIDE` the `Form_Load` makes it `ReadOnly`, zeroes its `Increment`
(`ReadOnly` on a `NumericUpDown` still leaves the spin buttons live — a classic way to
get this wrong) and greys its background. The figure stays visible and still flows into
subtotal/GST/total.

`btnSave_Click` then checks the **value**, not the control: if the submitted price differs
from the product default and the operator lacks the privilege, the save is blocked with a
message box naming the rule. That makes it a field-level, data-dependent rule rather than a
whole-button gate — the kind that does not survive a naive "disable the button" conversion.

---

## Structure worth converting

Most of the screens are flat: a form, some controls, event handlers. Three pieces
deliberately are not, and they are where a conversion that only parses `.Designer.cs`
files falls over.

**A user control inside a user control inside an inherited form.** `CustomerAccountForm`
inherits `BaseEntryForm` and hosts `CustomerHeaderPanel`, which itself hosts two
`AddressBlock` instances. Four files have to be read and stitched together before the
screen's real control tree is known, and two of the levels contribute controls that never
appear in the account screen's own designer file.

**Designer-serialised custom properties.** `AddressBlock.BlockTitle`,
`SearchBox.Caption` and `CustomerHeaderPanel.ReadOnlyHeader` are `[Browsable]` properties
whose *values* are written into the **consumer's** `InitializeComponent`, not the
control's. The two address blocks are the same class with different `BlockTitle` values —
one component, two instances, two prop sets. The data properties next to them are
`[DesignerSerializationVisibility(Hidden)]` precisely so a customer's address is never
baked into a form file. A converter has to tell those two kinds of property apart.

**Property cascades.** Setting `CustomerHeaderPanel.ReadOnlyHeader` greys its own fields
*and* sets `ReadOnlyBlock` on both child controls, which in turn sets `ReadOnly` and
`BackColor` on their text boxes. One assignment, three levels deep.

**Events bubbling up by hand.** There is no event aggregator. `AddressBlock` raises
`AddressChanged`; `CustomerHeaderPanel` subscribes and re-raises `HeaderChanged`; the form
subscribes to that and calls the base class's `MarkDirty()`; the base decides whether the
Save button lights up, by ANDing dirty state with the operator's privilege. Four hops for
one keystroke.

**Editable grids.** The two grids on the account screen are the only editable ones in the
application, and between them they use a combo column, a checkbox column, the new-row
placeholder, row deletion, per-cell validation that cancels the edit, a key filter hung on
the editing control, an explicit `CommitEdit` to make a checkbox commit immediately, and a
footer aggregate recomputed on every cell edit. The edited objects hang off
`DataGridViewRow.Tag` and are written back cell by cell in `CellEndEdit`; the two
`List<int>` fields of deleted ids are the only place a removed row is remembered.

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
  Program.cs                   [STAThread], EnableVisualStyles, LoginForm, Run(new MainForm())
  Properties/AssemblyInfo.cs
  Model/
    Customer.cs  Vehicle.cs  Product.cs  WeighTicket.cs   (+ TicketStatus constants)
    WeighTicketSummary.cs      read-only joined ticket row for the browse grid
    UserAccount.cs             operator login, profile name joined on
    SecurityProfile.cs         job role  (no callers yet - see "unused members")
    CustomerContact.cs         someone to ring at a customer
    ContractRate.cs            negotiated rate for one product over a date window
  Security/
    Privileges.cs              const string per privilege, C# name -> wire name
    SecurityContext.cs         static signed-on session; SignIn / HasPrivilege / Demand
  Controls/
    AddressBlock.cs        / .Designer.cs   four address fields in a GroupBox
    SearchBox.cs           / .Designer.cs   caption + box + Clear + record count
    CustomerHeaderPanel.cs / .Designer.cs   account fields + TWO AddressBlocks
  Data/
    Repository.cs              singleton, ADO.NET against SQL Server
  Database/
    CreateDatabase.sql         schema + demo data; copied next to the exe on build
  Forms/
    LoginForm.cs           / .Designer.cs               (no .resx)
    BaseEntryForm.cs       / .Designer.cs               base class, not a screen
    CustomerAccountForm.cs / .Designer.cs               inherits BaseEntryForm
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
| `UserAccount` | UserId, UserName, FullName, ProfileId, **ProfileName** (joined on, like the ticket summary), IsActive. No password field — there is no password. |
| `SecurityProfile` | ProfileId, ProfileName, Description. The privilege grants live in their own table and come back from `GetProfilePrivileges` as a plain `List<string>`, so nothing currently constructs one of these. |
| `CustomerContact` | ContactId, CustomerId, ContactName, Position, Phone, Email, IsPrimary. A contact with `ContactId == 0` has not been written yet — that is how the account screen tells a new grid row from an existing one. |
| `ContractRate` | RateId, CustomerId, ProductId, **ProductCode/ProductName** (joined on), RatePerTonne, EffectiveFrom, **EffectiveTo (nullable — null means open ended)**, Notes. Carries one piece of behaviour, `IsInForceOn(day)`, which is the only model in the app that has any. |

`Customer` also grew four postal-address columns with the account screen. They are written
**only** by `SaveCustomerAccount`; the older `SaveCustomer` that `CustomerEditForm` calls
does not mention them, so editing a customer through the old dialog leaves the postal
address alone instead of blanking it. Two save paths against one table, which is exactly
what happens when a screen gets bolted onto a system years later.

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

1. Launch — the **Sign In** dialog comes up first. Pick an operator; the profile shows
   underneath so you know what the session will be allowed to do. Sign In opens
   `MainForm` maximised, with the status bar showing that operator and a live clock;
   Exit closes without starting. Run it three times, once per operator, to see the
   menus and toolbars change.
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
