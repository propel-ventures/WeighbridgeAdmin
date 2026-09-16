/* ===================================================================
   Weighbridge Administration System - database create script
   Target: SQL Server 2012 or later (LocalDB, Express or full)

   Drops and recreates every table, then loads the standard demo data
   (15 customers, 20 vehicles, 8 products, 30 weigh tickets).

   Run in SSMS against the WeighbridgeDb database, or from a prompt:
     sqlcmd -S (localdb)\MSSQLLocalDB -d WeighbridgeDb -i CreateDatabase.sql
   =================================================================== */

SET NOCOUNT ON;
SET ANSI_NULLS ON;
SET QUOTED_IDENTIFIER ON;
GO

/* ---------- drop in dependency order ---------- */
IF OBJECT_ID('dbo.WeighTickets', 'U') IS NOT NULL DROP TABLE dbo.WeighTickets;
IF OBJECT_ID('dbo.Vehicles', 'U') IS NOT NULL DROP TABLE dbo.Vehicles;
IF OBJECT_ID('dbo.Products', 'U') IS NOT NULL DROP TABLE dbo.Products;
IF OBJECT_ID('dbo.Customers', 'U') IS NOT NULL DROP TABLE dbo.Customers;
IF OBJECT_ID('dbo.Counters', 'U') IS NOT NULL DROP TABLE dbo.Counters;
GO

/* ---------- Customers ---------- */
CREATE TABLE dbo.Customers (
    Id          INT IDENTITY(1,1)  NOT NULL,
    Code        VARCHAR(10)        NOT NULL,
    Name        VARCHAR(60)        NOT NULL,
    ABN         VARCHAR(14)            NULL,
    Address     VARCHAR(60)            NULL,
    Suburb      VARCHAR(40)            NULL,
    State       VARCHAR(3)             NULL,
    Postcode    VARCHAR(4)             NULL,
    Phone       VARCHAR(20)            NULL,
    Email       VARCHAR(80)            NULL,
    CreditLimit DECIMAL(12,2)      NOT NULL CONSTRAINT DF_Customers_CreditLimit DEFAULT (0),
    IsActive    BIT                NOT NULL CONSTRAINT DF_Customers_IsActive    DEFAULT (1),
    CONSTRAINT PK_Customers      PRIMARY KEY CLUSTERED (Id),
    CONSTRAINT UQ_Customers_Code UNIQUE (Code)
);
GO

/* ---------- Products ---------- */
CREATE TABLE dbo.Products (
    Id            INT IDENTITY(1,1) NOT NULL,
    Code          VARCHAR(10)       NOT NULL,
    Name          VARCHAR(60)       NOT NULL,
    PricePerTonne DECIMAL(10,2)     NOT NULL CONSTRAINT DF_Products_Price DEFAULT (0),
    GstApplicable BIT               NOT NULL CONSTRAINT DF_Products_Gst   DEFAULT (1),
    CONSTRAINT PK_Products      PRIMARY KEY CLUSTERED (Id),
    CONSTRAINT UQ_Products_Code UNIQUE (Code)
);
GO

/* ---------- Vehicles ---------- */
CREATE TABLE dbo.Vehicles (
    Id           INT IDENTITY(1,1) NOT NULL,
    Registration VARCHAR(10)       NOT NULL,
    Description  VARCHAR(60)           NULL,
    TareWeight   DECIMAL(10,2)     NOT NULL CONSTRAINT DF_Vehicles_Tare   DEFAULT (0),
    MaxGross     DECIMAL(10,2)     NOT NULL CONSTRAINT DF_Vehicles_Max    DEFAULT (0),
    CustomerId   INT               NOT NULL,
    IsActive     BIT               NOT NULL CONSTRAINT DF_Vehicles_Active DEFAULT (1),
    CONSTRAINT PK_Vehicles           PRIMARY KEY CLUSTERED (Id),
    CONSTRAINT FK_Vehicles_Customers FOREIGN KEY (CustomerId) REFERENCES dbo.Customers (Id)
);
GO
CREATE INDEX IX_Vehicles_Registration ON dbo.Vehicles (Registration);
GO

SET ANSI_NULLS ON;
SET QUOTED_IDENTIFIER ON;
GO

/* ---------- WeighTickets ---------- */
/* NetWeight is a persisted computed column - the app still derives it in
   code as well, exactly as the original did. */
CREATE TABLE dbo.WeighTickets (
    Id            INT IDENTITY(1,1) NOT NULL,
    TicketNumber  VARCHAR(12)       NOT NULL,
    TicketDate    DATETIME          NOT NULL,
    VehicleId     INT               NOT NULL,
    CustomerId    INT               NOT NULL,
    ProductId     INT               NOT NULL,
    GrossWeight   DECIMAL(10,2)     NOT NULL CONSTRAINT DF_Tickets_Gross DEFAULT (0),
    TareWeight    DECIMAL(10,2)     NOT NULL CONSTRAINT DF_Tickets_Tare  DEFAULT (0),
    NetWeight     AS (GrossWeight - TareWeight) PERSISTED,
    PricePerTonne DECIMAL(10,2)     NOT NULL CONSTRAINT DF_Tickets_Price DEFAULT (0),
    Subtotal      DECIMAL(12,2)     NOT NULL CONSTRAINT DF_Tickets_Sub   DEFAULT (0),
    Gst           DECIMAL(12,2)     NOT NULL CONSTRAINT DF_Tickets_Gst   DEFAULT (0),
    Total         DECIMAL(12,2)     NOT NULL CONSTRAINT DF_Tickets_Total DEFAULT (0),
    Notes         VARCHAR(250)          NULL,
    Status        VARCHAR(10)       NOT NULL CONSTRAINT DF_Tickets_Status DEFAULT ('Open'),
    CONSTRAINT PK_WeighTickets        PRIMARY KEY CLUSTERED (Id),
    CONSTRAINT UQ_WeighTickets_Number UNIQUE (TicketNumber),
    CONSTRAINT FK_Tickets_Vehicles    FOREIGN KEY (VehicleId)  REFERENCES dbo.Vehicles  (Id),
    CONSTRAINT FK_Tickets_Customers   FOREIGN KEY (CustomerId) REFERENCES dbo.Customers (Id),
    CONSTRAINT FK_Tickets_Products    FOREIGN KEY (ProductId)  REFERENCES dbo.Products  (Id),
    CONSTRAINT CK_Tickets_Status      CHECK (Status IN ('Open','Completed','Void'))
);
GO

/* ---------- Counters (document number allocation) ---------- */
CREATE TABLE dbo.Counters (
    CounterName VARCHAR(30) NOT NULL,
    NextValue   INT         NOT NULL,
    CONSTRAINT PK_Counters PRIMARY KEY CLUSTERED (CounterName)
);
GO

/* =================== demo data =================== */

SET IDENTITY_INSERT dbo.Customers ON;
INSERT INTO dbo.Customers (Id, Code, Name, ABN, Address, Suburb, State, Postcode, Phone, Email, CreditLimit, IsActive) VALUES (1, 'ANDCON', 'Anderson Concrete Pty Ltd', '51 824 753 556', '14 Kessler Drive', 'Yatala', 'QLD', '4207', '(07) 3807 4411', 'accounts@andersonconcrete.com.au', 25000, 1);
INSERT INTO dbo.Customers (Id, Code, Name, ABN, Address, Suburb, State, Postcode, Phone, Email, CreditLimit, IsActive) VALUES (2, 'BALEXC', 'Ballantyne Excavations', '38 902 116 447', 'Unit 3 / 88 Kremzow Road', 'Brendale', 'QLD', '4500', '(07) 3205 8890', 'kim@ballantyneexc.com.au', 12000, 1);
INSERT INTO dbo.Customers (Id, Code, Name, ABN, Address, Suburb, State, Postcode, Phone, Email, CreditLimit, IsActive) VALUES (3, 'CARHAU', 'Carmody Haulage Pty Ltd', '72 118 995 021', '220 Bilsen Road', 'Geebung', 'QLD', '4034', '(07) 3865 2200', 'dispatch@carmodyhaulage.com.au', 60000, 1);
INSERT INTO dbo.Customers (Id, Code, Name, ABN, Address, Suburb, State, Postcode, Phone, Email, CreditLimit, IsActive) VALUES (4, 'DARCIV', 'Darling Downs Civil', '19 447 802 613', '1145 Ruthven Street', 'Toowoomba', 'QLD', '4350', '(07) 4634 7712', 'admin@ddcivil.com.au', 45000, 1);
INSERT INTO dbo.Customers (Id, Code, Name, ABN, Address, Suburb, State, Postcode, Phone, Email, CreditLimit, IsActive) VALUES (5, 'ESKQUA', 'Esk Quarry Supplies', '64 220 118 904', 'Lot 7 Gallanani Road', 'Esk', 'QLD', '4312', '(07) 5424 1180', 'orders@eskquarry.com.au', 8000, 1);
INSERT INTO dbo.Customers (Id, Code, Name, ABN, Address, Suburb, State, Postcode, Phone, Email, CreditLimit, IsActive) VALUES (6, 'FRANKS', 'Franklin & Sons Earthmoving', '86 335 770 218', '42 Hartley Street', 'Smithfield', 'NSW', '2164', '(02) 9604 3318', 'peter@franklinandsons.com.au', 30000, 1);
INSERT INTO dbo.Customers (Id, Code, Name, ABN, Address, Suburb, State, Postcode, Phone, Email, CreditLimit, IsActive) VALUES (7, 'GOLDCO', 'Goldfields Concrete', '27 660 449 113', '9 Broadwood Street', 'Kalgoorlie', 'WA', '6430', '(08) 9021 7766', 'sales@goldfieldsconcrete.com.au', 18000, 0);
INSERT INTO dbo.Customers (Id, Code, Name, ABN, Address, Suburb, State, Postcode, Phone, Email, CreditLimit, IsActive) VALUES (8, 'HUNTRA', 'Hunter Valley Transport', '45 118 277 630', '3 Racecourse Road', 'Rutherford', 'NSW', '2320', '(02) 4932 5510', 'ops@huntervalleytransport.com.au', 75000, 1);
INSERT INTO dbo.Customers (Id, Code, Name, ABN, Address, Suburb, State, Postcode, Phone, Email, CreditLimit, IsActive) VALUES (9, 'IRONBK', 'Ironbark Landscaping Supplies', '90 774 315 228', '58 Beaudesert Road', 'Moorooka', 'QLD', '4105', '(07) 3892 1044', 'shop@ironbarksupplies.com.au', 5000, 1);
INSERT INTO dbo.Customers (Id, Code, Name, ABN, Address, Suburb, State, Postcode, Phone, Email, CreditLimit, IsActive) VALUES (10, 'JARVIS', 'Jarvis Bros Contracting', '33 209 668 471', '17 Enterprise Close', 'Breakwater', 'VIC', '3219', '(03) 5221 9987', 'office@jarvisbros.com.au', 22000, 1);
INSERT INTO dbo.Customers (Id, Code, Name, ABN, Address, Suburb, State, Postcode, Phone, Email, CreditLimit, IsActive) VALUES (11, 'KEMPSE', 'Kempsey Sand & Gravel', '58 441 903 226', '265 Macleay Valley Way', 'Kempsey', 'NSW', '2440', '(02) 6562 3311', 'kempseysand@bigpond.com', 15000, 1);
INSERT INTO dbo.Customers (Id, Code, Name, ABN, Address, Suburb, State, Postcode, Phone, Email, CreditLimit, IsActive) VALUES (12, 'LOGANC', 'Logan City Civil Works', '11 337 552 809', '150 Wembley Road', 'Logan Central', 'QLD', '4114', '(07) 3412 6600', 'procurement@logancivil.com.au', 100000, 1);
INSERT INTO dbo.Customers (Id, Code, Name, ABN, Address, Suburb, State, Postcode, Phone, Email, CreditLimit, IsActive) VALUES (13, 'MURRAY', 'Murray Bridge Bulk Haulage', '74 006 219 553', '12 Maurice Road', 'Murray Bridge', 'SA', '5253', '(08) 8532 4477', 'bookings@mbbulk.com.au', 40000, 1);
INSERT INTO dbo.Customers (Id, Code, Name, ABN, Address, Suburb, State, Postcode, Phone, Email, CreditLimit, IsActive) VALUES (14, 'NORTHW', 'Northwest Tipper Hire', '22 885 601 174', '6 Dampier Road', 'Karratha', 'WA', '6714', '(08) 9185 2299', 'hire@nwtipper.com.au', 0, 0);
INSERT INTO dbo.Customers (Id, Code, Name, ABN, Address, Suburb, State, Postcode, Phone, Email, CreditLimit, IsActive) VALUES (15, 'OAKLEY', 'Oakley Road Construction', '69 513 228 947', '88 Anzac Avenue', 'Redcliffe', 'QLD', '4020', '(07) 3284 7150', 'accounts@oakleyroad.com.au', 55000, 1);
SET IDENTITY_INSERT dbo.Customers OFF;
GO

SET IDENTITY_INSERT dbo.Products ON;
INSERT INTO dbo.Products (Id, Code, Name, PricePerTonne, GstApplicable) VALUES (1, 'AGG10', 'Aggregate 10mm', 42.50, 1);
INSERT INTO dbo.Products (Id, Code, Name, PricePerTonne, GstApplicable) VALUES (2, 'AGG20', 'Aggregate 20mm', 39.75, 1);
INSERT INTO dbo.Products (Id, Code, Name, PricePerTonne, GstApplicable) VALUES (3, 'ROADB', 'Road Base CBR45', 28.90, 1);
INSERT INTO dbo.Products (Id, Code, Name, PricePerTonne, GstApplicable) VALUES (4, 'SAND', 'Washed Concrete Sand', 33.00, 1);
INSERT INTO dbo.Products (Id, Code, Name, PricePerTonne, GstApplicable) VALUES (5, 'TOPSL', 'Screened Topsoil', 47.50, 1);
INSERT INTO dbo.Products (Id, Code, Name, PricePerTonne, GstApplicable) VALUES (6, 'SCALP', 'Quarry Scalpings', 18.25, 1);
INSERT INTO dbo.Products (Id, Code, Name, PricePerTonne, GstApplicable) VALUES (7, 'GRNWA', 'Green Waste Received', 96.00, 1);
INSERT INTO dbo.Products (Id, Code, Name, PricePerTonne, GstApplicable) VALUES (8, 'CLNFL', 'Clean Fill Received', 12.00, 0);
SET IDENTITY_INSERT dbo.Products OFF;
GO

SET IDENTITY_INSERT dbo.Vehicles ON;
INSERT INTO dbo.Vehicles (Id, Registration, Description, TareWeight, MaxGross, CustomerId, IsActive) VALUES (1, '471 XKR', 'Kenworth T409 Tandem Tipper', 11200, 23000, 1, 1);
INSERT INTO dbo.Vehicles (Id, Registration, Description, TareWeight, MaxGross, CustomerId, IsActive) VALUES (2, '882 QLD', 'Isuzu FVZ 1400 Tipper', 9400, 22500, 1, 1);
INSERT INTO dbo.Vehicles (Id, Registration, Description, TareWeight, MaxGross, CustomerId, IsActive) VALUES (3, 'XBN 335', 'Hino 700 Series Body Truck', 10100, 24000, 2, 1);
INSERT INTO dbo.Vehicles (Id, Registration, Description, TareWeight, MaxGross, CustomerId, IsActive) VALUES (4, '104 TRK', 'Volvo FM Truck & Dog', 22600, 50500, 3, 1);
INSERT INTO dbo.Vehicles (Id, Registration, Description, TareWeight, MaxGross, CustomerId, IsActive) VALUES (5, '105 TRK', 'Volvo FH Truck & Dog', 23100, 50500, 3, 1);
INSERT INTO dbo.Vehicles (Id, Registration, Description, TareWeight, MaxGross, CustomerId, IsActive) VALUES (6, '106 TRK', 'Mack Trident Semi Tipper', 18800, 42500, 3, 1);
INSERT INTO dbo.Vehicles (Id, Registration, Description, TareWeight, MaxGross, CustomerId, IsActive) VALUES (7, 'DDC 118', 'Western Star 4800 Semi', 19250, 42500, 4, 1);
INSERT INTO dbo.Vehicles (Id, Registration, Description, TareWeight, MaxGross, CustomerId, IsActive) VALUES (8, 'DDC 119', 'Caterpillar CT610 Semi', 19700, 42500, 4, 0);
INSERT INTO dbo.Vehicles (Id, Registration, Description, TareWeight, MaxGross, CustomerId, IsActive) VALUES (9, 'ESK 220', 'Isuzu FTR 900 Single Axle', 7350, 16500, 5, 1);
INSERT INTO dbo.Vehicles (Id, Registration, Description, TareWeight, MaxGross, CustomerId, IsActive) VALUES (10, 'BF 77 QL', 'Freightliner Argosy B-Double', 27400, 62500, 6, 1);
INSERT INTO dbo.Vehicles (Id, Registration, Description, TareWeight, MaxGross, CustomerId, IsActive) VALUES (11, 'GFC 041', 'Mercedes Actros Agitator', 14900, 26000, 7, 0);
INSERT INTO dbo.Vehicles (Id, Registration, Description, TareWeight, MaxGross, CustomerId, IsActive) VALUES (12, 'HVT 612', 'Kenworth K200 B-Double', 28100, 68000, 8, 1);
INSERT INTO dbo.Vehicles (Id, Registration, Description, TareWeight, MaxGross, CustomerId, IsActive) VALUES (13, 'HVT 613', 'Kenworth C509 Road Train', 31500, 79000, 8, 1);
INSERT INTO dbo.Vehicles (Id, Registration, Description, TareWeight, MaxGross, CustomerId, IsActive) VALUES (14, '1QZ 4RT', 'Fuso Canter Tray Truck', 3850, 8500, 9, 1);
INSERT INTO dbo.Vehicles (Id, Registration, Description, TareWeight, MaxGross, CustomerId, IsActive) VALUES (15, 'JB 419 V', 'Scania R560 Truck & Dog', 22950, 50500, 10, 1);
INSERT INTO dbo.Vehicles (Id, Registration, Description, TareWeight, MaxGross, CustomerId, IsActive) VALUES (16, 'KSG 208', 'Hino 500 FG Tipper', 8950, 16500, 11, 1);
INSERT INTO dbo.Vehicles (Id, Registration, Description, TareWeight, MaxGross, CustomerId, IsActive) VALUES (17, 'LCC 900', 'UD Quon CW Tipper', 12300, 26000, 12, 1);
INSERT INTO dbo.Vehicles (Id, Registration, Description, TareWeight, MaxGross, CustomerId, IsActive) VALUES (18, 'MBH 344', 'Kenworth T659 Road Train', 30800, 79000, 13, 1);
INSERT INTO dbo.Vehicles (Id, Registration, Description, TareWeight, MaxGross, CustomerId, IsActive) VALUES (19, 'NWT 155', 'Isuzu Giga Tipper', 13100, 26000, 14, 0);
INSERT INTO dbo.Vehicles (Id, Registration, Description, TareWeight, MaxGross, CustomerId, IsActive) VALUES (20, 'ORC 077', 'Volvo FMX Truck & Dog', 23400, 50500, 15, 1);
SET IDENTITY_INSERT dbo.Vehicles OFF;
GO

/* Tickets inherit CustomerId from the vehicle, as the seed code did.
   Subtotal / Gst / Total are stored exactly as the application computed
   them.  They are NOT recomputed in SQL: C# Math.Round uses banker's
   rounding and T-SQL ROUND() rounds half away from zero, which disagree
   by one cent on 6 of these 30 rows. */
SET IDENTITY_INSERT dbo.WeighTickets ON;
INSERT INTO dbo.WeighTickets (Id, TicketNumber, TicketDate, VehicleId, CustomerId, ProductId, GrossWeight, TareWeight, PricePerTonne, Subtotal, Gst, Total, Status, Notes)
  SELECT 1, 'WB100001', '2025-07-01', 1, v.CustomerId, 2, 22150, 11200, 39.75, 435.26, 43.53, 478.79, 'Completed', '' FROM dbo.Vehicles v WHERE v.Id = 1;
INSERT INTO dbo.WeighTickets (Id, TicketNumber, TicketDate, VehicleId, CustomerId, ProductId, GrossWeight, TareWeight, PricePerTonne, Subtotal, Gst, Total, Status, Notes)
  SELECT 2, 'WB100002', '2025-07-01', 4, v.CustomerId, 3, 49800, 22600, 28.90, 786.08, 78.61, 864.69, 'Completed', 'Weighbridge 2' FROM dbo.Vehicles v WHERE v.Id = 4;
INSERT INTO dbo.WeighTickets (Id, TicketNumber, TicketDate, VehicleId, CustomerId, ProductId, GrossWeight, TareWeight, PricePerTonne, Subtotal, Gst, Total, Status, Notes)
  SELECT 3, 'WB100003', '2025-07-02', 12, v.CustomerId, 1, 66900, 28100, 42.50, 1649.00, 164.90, 1813.90, 'Completed', '' FROM dbo.Vehicles v WHERE v.Id = 12;
INSERT INTO dbo.WeighTickets (Id, TicketNumber, TicketDate, VehicleId, CustomerId, ProductId, GrossWeight, TareWeight, PricePerTonne, Subtotal, Gst, Total, Status, Notes)
  SELECT 4, 'WB100004', '2025-07-02', 9, v.CustomerId, 6, 16200, 7350, 18.25, 161.51, 16.15, 177.66, 'Completed', 'Cash sale - paid at gate' FROM dbo.Vehicles v WHERE v.Id = 9;
INSERT INTO dbo.WeighTickets (Id, TicketNumber, TicketDate, VehicleId, CustomerId, ProductId, GrossWeight, TareWeight, PricePerTonne, Subtotal, Gst, Total, Status, Notes)
  SELECT 5, 'WB100005', '2025-07-03', 3, v.CustomerId, 4, 23700, 10100, 33.00, 448.80, 44.88, 493.68, 'Completed', '' FROM dbo.Vehicles v WHERE v.Id = 3;
INSERT INTO dbo.WeighTickets (Id, TicketNumber, TicketDate, VehicleId, CustomerId, ProductId, GrossWeight, TareWeight, PricePerTonne, Subtotal, Gst, Total, Status, Notes)
  SELECT 6, 'WB100006', '2025-07-03', 7, v.CustomerId, 3, 41900, 19250, 28.90, 654.58, 65.46, 720.04, 'Void', 'Operator error - reweighed on WB100012' FROM dbo.Vehicles v WHERE v.Id = 7;
INSERT INTO dbo.WeighTickets (Id, TicketNumber, TicketDate, VehicleId, CustomerId, ProductId, GrossWeight, TareWeight, PricePerTonne, Subtotal, Gst, Total, Status, Notes)
  SELECT 7, 'WB100007', '2025-07-03', 7, v.CustomerId, 3, 42050, 19250, 28.90, 658.92, 65.89, 724.81, 'Completed', 'Reweigh of voided ticket' FROM dbo.Vehicles v WHERE v.Id = 7;
INSERT INTO dbo.WeighTickets (Id, TicketNumber, TicketDate, VehicleId, CustomerId, ProductId, GrossWeight, TareWeight, PricePerTonne, Subtotal, Gst, Total, Status, Notes)
  SELECT 8, 'WB100008', '2025-07-04', 15, v.CustomerId, 2, 50100, 22950, 39.75, 1079.21, 107.92, 1187.13, 'Completed', '' FROM dbo.Vehicles v WHERE v.Id = 15;
INSERT INTO dbo.WeighTickets (Id, TicketNumber, TicketDate, VehicleId, CustomerId, ProductId, GrossWeight, TareWeight, PricePerTonne, Subtotal, Gst, Total, Status, Notes)
  SELECT 9, 'WB100009', '2025-07-07', 5, v.CustomerId, 5, 49650, 23100, 47.50, 1261.12, 126.11, 1387.23, 'Completed', 'Tail lift required at delivery' FROM dbo.Vehicles v WHERE v.Id = 5;
INSERT INTO dbo.WeighTickets (Id, TicketNumber, TicketDate, VehicleId, CustomerId, ProductId, GrossWeight, TareWeight, PricePerTonne, Subtotal, Gst, Total, Status, Notes)
  SELECT 10, 'WB100010', '2025-07-08', 17, v.CustomerId, 3, 25800, 12300, 26.50, 357.75, 35.78, 393.53, 'Completed', 'Contract rate applied' FROM dbo.Vehicles v WHERE v.Id = 17;
INSERT INTO dbo.WeighTickets (Id, TicketNumber, TicketDate, VehicleId, CustomerId, ProductId, GrossWeight, TareWeight, PricePerTonne, Subtotal, Gst, Total, Status, Notes)
  SELECT 11, 'WB100011', '2025-07-08', 14, v.CustomerId, 8, 8200, 3850, 12.00, 52.20, 0.00, 52.20, 'Completed', 'Clean fill received - no GST' FROM dbo.Vehicles v WHERE v.Id = 14;
INSERT INTO dbo.WeighTickets (Id, TicketNumber, TicketDate, VehicleId, CustomerId, ProductId, GrossWeight, TareWeight, PricePerTonne, Subtotal, Gst, Total, Status, Notes)
  SELECT 12, 'WB100012', '2025-07-09', 13, v.CustomerId, 1, 77400, 31500, 42.50, 1950.75, 195.08, 2145.83, 'Completed', 'Road train - two trailers' FROM dbo.Vehicles v WHERE v.Id = 13;
INSERT INTO dbo.WeighTickets (Id, TicketNumber, TicketDate, VehicleId, CustomerId, ProductId, GrossWeight, TareWeight, PricePerTonne, Subtotal, Gst, Total, Status, Notes)
  SELECT 13, 'WB100013', '2025-07-10', 2, v.CustomerId, 6, 21900, 9400, 18.25, 228.12, 22.81, 250.93, 'Completed', '' FROM dbo.Vehicles v WHERE v.Id = 2;
INSERT INTO dbo.WeighTickets (Id, TicketNumber, TicketDate, VehicleId, CustomerId, ProductId, GrossWeight, TareWeight, PricePerTonne, Subtotal, Gst, Total, Status, Notes)
  SELECT 14, 'WB100014', '2025-07-10', 20, v.CustomerId, 4, 49100, 23400, 33.00, 848.10, 84.81, 932.91, 'Completed', '' FROM dbo.Vehicles v WHERE v.Id = 20;
INSERT INTO dbo.WeighTickets (Id, TicketNumber, TicketDate, VehicleId, CustomerId, ProductId, GrossWeight, TareWeight, PricePerTonne, Subtotal, Gst, Total, Status, Notes)
  SELECT 15, 'WB100015', '2025-07-11', 10, v.CustomerId, 2, 61800, 27400, 39.75, 1367.40, 136.74, 1504.14, 'Completed', 'B-double - split load' FROM dbo.Vehicles v WHERE v.Id = 10;
INSERT INTO dbo.WeighTickets (Id, TicketNumber, TicketDate, VehicleId, CustomerId, ProductId, GrossWeight, TareWeight, PricePerTonne, Subtotal, Gst, Total, Status, Notes)
  SELECT 16, 'WB100016', '2025-07-14', 16, v.CustomerId, 7, 15750, 8950, 96.00, 652.80, 65.28, 718.08, 'Completed', 'Green waste tipped bay 4' FROM dbo.Vehicles v WHERE v.Id = 16;
INSERT INTO dbo.WeighTickets (Id, TicketNumber, TicketDate, VehicleId, CustomerId, ProductId, GrossWeight, TareWeight, PricePerTonne, Subtotal, Gst, Total, Status, Notes)
  SELECT 17, 'WB100017', '2025-07-15', 6, v.CustomerId, 3, 41700, 18800, 28.90, 661.81, 66.18, 727.99, 'Completed', '' FROM dbo.Vehicles v WHERE v.Id = 6;
INSERT INTO dbo.WeighTickets (Id, TicketNumber, TicketDate, VehicleId, CustomerId, ProductId, GrossWeight, TareWeight, PricePerTonne, Subtotal, Gst, Total, Status, Notes)
  SELECT 18, 'WB100018', '2025-07-15', 1, v.CustomerId, 1, 22800, 11200, 42.50, 493.00, 49.30, 542.30, 'Completed', 'Overweight warning acknowledged' FROM dbo.Vehicles v WHERE v.Id = 1;
INSERT INTO dbo.WeighTickets (Id, TicketNumber, TicketDate, VehicleId, CustomerId, ProductId, GrossWeight, TareWeight, PricePerTonne, Subtotal, Gst, Total, Status, Notes)
  SELECT 19, 'WB100019', '2025-07-16', 18, v.CustomerId, 2, 78200, 30800, 39.75, 1884.15, 188.42, 2072.57, 'Completed', '' FROM dbo.Vehicles v WHERE v.Id = 18;
INSERT INTO dbo.WeighTickets (Id, TicketNumber, TicketDate, VehicleId, CustomerId, ProductId, GrossWeight, TareWeight, PricePerTonne, Subtotal, Gst, Total, Status, Notes)
  SELECT 20, 'WB100020', '2025-07-17', 4, v.CustomerId, 6, 48900, 22600, 18.25, 479.98, 48.00, 527.98, 'Completed', '' FROM dbo.Vehicles v WHERE v.Id = 4;
INSERT INTO dbo.WeighTickets (Id, TicketNumber, TicketDate, VehicleId, CustomerId, ProductId, GrossWeight, TareWeight, PricePerTonne, Subtotal, Gst, Total, Status, Notes)
  SELECT 21, 'WB100021', '2025-07-18', 9, v.CustomerId, 5, 16100, 7350, 47.50, 415.62, 41.56, 457.18, 'Completed', '' FROM dbo.Vehicles v WHERE v.Id = 9;
INSERT INTO dbo.WeighTickets (Id, TicketNumber, TicketDate, VehicleId, CustomerId, ProductId, GrossWeight, TareWeight, PricePerTonne, Subtotal, Gst, Total, Status, Notes)
  SELECT 22, 'WB100022', '2025-07-21', 12, v.CustomerId, 3, 67500, 28100, 28.90, 1138.66, 113.87, 1252.53, 'Completed', '' FROM dbo.Vehicles v WHERE v.Id = 12;
INSERT INTO dbo.WeighTickets (Id, TicketNumber, TicketDate, VehicleId, CustomerId, ProductId, GrossWeight, TareWeight, PricePerTonne, Subtotal, Gst, Total, Status, Notes)
  SELECT 23, 'WB100023', '2025-07-22', 15, v.CustomerId, 8, 49200, 22950, 12.00, 315.00, 0.00, 315.00, 'Completed', 'Clean fill received' FROM dbo.Vehicles v WHERE v.Id = 15;
INSERT INTO dbo.WeighTickets (Id, TicketNumber, TicketDate, VehicleId, CustomerId, ProductId, GrossWeight, TareWeight, PricePerTonne, Subtotal, Gst, Total, Status, Notes)
  SELECT 24, 'WB100024', '2025-07-23', 3, v.CustomerId, 1, 23950, 10100, 42.50, 588.62, 58.86, 647.48, 'Completed', '' FROM dbo.Vehicles v WHERE v.Id = 3;
INSERT INTO dbo.WeighTickets (Id, TicketNumber, TicketDate, VehicleId, CustomerId, ProductId, GrossWeight, TareWeight, PricePerTonne, Subtotal, Gst, Total, Status, Notes)
  SELECT 25, 'WB100025', '2025-07-24', 17, v.CustomerId, 4, 25100, 12300, 33.00, 422.40, 42.24, 464.64, 'Void', 'Customer cancelled at weighbridge' FROM dbo.Vehicles v WHERE v.Id = 17;
INSERT INTO dbo.WeighTickets (Id, TicketNumber, TicketDate, VehicleId, CustomerId, ProductId, GrossWeight, TareWeight, PricePerTonne, Subtotal, Gst, Total, Status, Notes)
  SELECT 26, 'WB100026', '2025-07-25', 5, v.CustomerId, 2, 50300, 23100, 39.75, 1081.20, 108.12, 1189.32, 'Completed', 'Gross over limit - see supervisor' FROM dbo.Vehicles v WHERE v.Id = 5;
INSERT INTO dbo.WeighTickets (Id, TicketNumber, TicketDate, VehicleId, CustomerId, ProductId, GrossWeight, TareWeight, PricePerTonne, Subtotal, Gst, Total, Status, Notes)
  SELECT 27, 'WB100027', '2025-07-28', 13, v.CustomerId, 6, 78600, 31500, 18.25, 859.58, 85.96, 945.54, 'Completed', '' FROM dbo.Vehicles v WHERE v.Id = 13;
INSERT INTO dbo.WeighTickets (Id, TicketNumber, TicketDate, VehicleId, CustomerId, ProductId, GrossWeight, TareWeight, PricePerTonne, Subtotal, Gst, Total, Status, Notes)
  SELECT 28, 'WB100028', '2025-07-29', 2, v.CustomerId, 7, 20100, 9400, 96.00, 1027.20, 102.72, 1129.92, 'Completed', '' FROM dbo.Vehicles v WHERE v.Id = 2;
INSERT INTO dbo.WeighTickets (Id, TicketNumber, TicketDate, VehicleId, CustomerId, ProductId, GrossWeight, TareWeight, PricePerTonne, Subtotal, Gst, Total, Status, Notes)
  SELECT 29, 'WB100029', '2025-07-30', 20, v.CustomerId, 3, 48750, 23400, 28.90, 732.62, 73.26, 805.88, 'Open', 'Awaiting second weigh' FROM dbo.Vehicles v WHERE v.Id = 20;
INSERT INTO dbo.WeighTickets (Id, TicketNumber, TicketDate, VehicleId, CustomerId, ProductId, GrossWeight, TareWeight, PricePerTonne, Subtotal, Gst, Total, Status, Notes)
  SELECT 30, 'WB100030', '2025-07-31', 16, v.CustomerId, 5, 16050, 8950, 47.50, 337.25, 33.72, 370.97, 'Open', 'Tare estimated - confirm on return' FROM dbo.Vehicles v WHERE v.Id = 16;
SET IDENTITY_INSERT dbo.WeighTickets OFF;
GO

INSERT INTO dbo.Counters (CounterName, NextValue) VALUES ('TICKET', 100031);
GO
