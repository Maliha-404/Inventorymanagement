USE [InventoryDB]
GO
ALTER TABLE [dbo].[StockMovements] DROP CONSTRAINT [FK_StockMovements_Users_UserID]
GO
ALTER TABLE [dbo].[StockMovements] DROP CONSTRAINT [FK_StockMovements_Items_ItemID]
GO
ALTER TABLE [dbo].[Orders] DROP CONSTRAINT [FK_Orders_Suppliers_SupplierID]
GO
ALTER TABLE [dbo].[Orders] DROP CONSTRAINT [FK_Orders_Items_ItemID]
GO
ALTER TABLE [dbo].[Items] DROP CONSTRAINT [FK_Items_Suppliers_SupplierID]
GO
ALTER TABLE [dbo].[Alerts] DROP CONSTRAINT [FK_Alerts_Items_ItemID]
GO
ALTER TABLE [dbo].[StockMovements] DROP CONSTRAINT [DF__StockMove__Actio__47DBAE45]
GO
/****** Object:  Index [IX_StockMovements_UserID]    Script Date: 5/17/2025 1:12:04 PM ******/
DROP INDEX [IX_StockMovements_UserID] ON [dbo].[StockMovements]
GO
/****** Object:  Index [IX_StockMovements_ItemID]    Script Date: 5/17/2025 1:12:04 PM ******/
DROP INDEX [IX_StockMovements_ItemID] ON [dbo].[StockMovements]
GO
/****** Object:  Index [IX_Orders_SupplierID]    Script Date: 5/17/2025 1:12:04 PM ******/
DROP INDEX [IX_Orders_SupplierID] ON [dbo].[Orders]
GO
/****** Object:  Index [IX_Orders_ItemID]    Script Date: 5/17/2025 1:12:04 PM ******/
DROP INDEX [IX_Orders_ItemID] ON [dbo].[Orders]
GO
/****** Object:  Index [IX_Items_SupplierID]    Script Date: 5/17/2025 1:12:04 PM ******/
DROP INDEX [IX_Items_SupplierID] ON [dbo].[Items]
GO
/****** Object:  Index [IX_Alerts_ItemID]    Script Date: 5/17/2025 1:12:04 PM ******/
DROP INDEX [IX_Alerts_ItemID] ON [dbo].[Alerts]
GO
/****** Object:  Table [dbo].[Users]    Script Date: 5/17/2025 1:12:04 PM ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Users]') AND type in (N'U'))
DROP TABLE [dbo].[Users]
GO
/****** Object:  Table [dbo].[Suppliers]    Script Date: 5/17/2025 1:12:04 PM ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Suppliers]') AND type in (N'U'))
DROP TABLE [dbo].[Suppliers]
GO
/****** Object:  Table [dbo].[StockMovements]    Script Date: 5/17/2025 1:12:04 PM ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[StockMovements]') AND type in (N'U'))
DROP TABLE [dbo].[StockMovements]
GO
/****** Object:  Table [dbo].[Orders]    Script Date: 5/17/2025 1:12:04 PM ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Orders]') AND type in (N'U'))
DROP TABLE [dbo].[Orders]
GO
/****** Object:  Table [dbo].[Items]    Script Date: 5/17/2025 1:12:04 PM ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Items]') AND type in (N'U'))
DROP TABLE [dbo].[Items]
GO
/****** Object:  Table [dbo].[Alerts]    Script Date: 5/17/2025 1:12:04 PM ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Alerts]') AND type in (N'U'))
DROP TABLE [dbo].[Alerts]
GO
/****** Object:  Table [dbo].[__EFMigrationsHistory]    Script Date: 5/17/2025 1:12:04 PM ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[__EFMigrationsHistory]') AND type in (N'U'))
DROP TABLE [dbo].[__EFMigrationsHistory]
GO
USE [master]
GO
/****** Object:  Database [InventoryDB]    Script Date: 5/17/2025 1:12:04 PM ******/
DROP DATABASE [InventoryDB]
GO
/****** Object:  Database [InventoryDB]    Script Date: 5/17/2025 1:12:04 PM ******/
CREATE DATABASE [InventoryDB]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'InventoryDB', FILENAME = N'C:\Users\Dell\InventoryDB.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'InventoryDB_log', FILENAME = N'C:\Users\Dell\InventoryDB_log.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
 WITH CATALOG_COLLATION = DATABASE_DEFAULT
GO
ALTER DATABASE [InventoryDB] SET COMPATIBILITY_LEVEL = 150
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [InventoryDB].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [InventoryDB] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [InventoryDB] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [InventoryDB] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [InventoryDB] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [InventoryDB] SET ARITHABORT OFF 
GO
ALTER DATABASE [InventoryDB] SET AUTO_CLOSE ON 
GO
ALTER DATABASE [InventoryDB] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [InventoryDB] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [InventoryDB] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [InventoryDB] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [InventoryDB] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [InventoryDB] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [InventoryDB] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [InventoryDB] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [InventoryDB] SET  ENABLE_BROKER 
GO
ALTER DATABASE [InventoryDB] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [InventoryDB] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [InventoryDB] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [InventoryDB] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [InventoryDB] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [InventoryDB] SET READ_COMMITTED_SNAPSHOT ON 
GO
ALTER DATABASE [InventoryDB] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [InventoryDB] SET RECOVERY SIMPLE 
GO
ALTER DATABASE [InventoryDB] SET  MULTI_USER 
GO
ALTER DATABASE [InventoryDB] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [InventoryDB] SET DB_CHAINING OFF 
GO
ALTER DATABASE [InventoryDB] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [InventoryDB] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [InventoryDB] SET DELAYED_DURABILITY = DISABLED 
GO
ALTER DATABASE [InventoryDB] SET ACCELERATED_DATABASE_RECOVERY = OFF  
GO
ALTER DATABASE [InventoryDB] SET QUERY_STORE = OFF
GO
USE [InventoryDB]
GO
/****** Object:  Table [dbo].[__EFMigrationsHistory]    Script Date: 5/17/2025 1:12:05 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[__EFMigrationsHistory](
	[MigrationId] [nvarchar](150) NOT NULL,
	[ProductVersion] [nvarchar](32) NOT NULL,
 CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY CLUSTERED 
(
	[MigrationId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Alerts]    Script Date: 5/17/2025 1:12:05 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Alerts](
	[AlertID] [int] IDENTITY(1,1) NOT NULL,
	[ItemID] [int] NOT NULL,
	[IsActive] [bit] NOT NULL,
	[Message] [nvarchar](255) NOT NULL,
	[CreatedDate] [datetime2](7) NOT NULL,
 CONSTRAINT [PK_Alerts] PRIMARY KEY CLUSTERED 
(
	[AlertID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Items]    Script Date: 5/17/2025 1:12:05 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Items](
	[ItemID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](100) NOT NULL,
	[Quantity] [int] NOT NULL,
	[Price] [decimal](18, 2) NOT NULL,
	[LowStockThreshold] [int] NOT NULL,
	[SupplierID] [int] NOT NULL,
 CONSTRAINT [PK_Items] PRIMARY KEY CLUSTERED 
(
	[ItemID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Orders]    Script Date: 5/17/2025 1:12:05 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Orders](
	[OrderID] [int] IDENTITY(1,1) NOT NULL,
	[ItemID] [int] NOT NULL,
	[SupplierID] [int] NOT NULL,
	[Quantity] [int] NOT NULL,
	[OrderDate] [datetime2](7) NOT NULL,
	[IsDelivered] [bit] NOT NULL,
 CONSTRAINT [PK_Orders] PRIMARY KEY CLUSTERED 
(
	[OrderID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[StockMovements]    Script Date: 5/17/2025 1:12:05 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[StockMovements](
	[MovementID] [int] IDENTITY(1,1) NOT NULL,
	[ItemID] [int] NOT NULL,
	[UserID] [int] NOT NULL,
	[OldQuantity] [int] NOT NULL,
	[NewQuantity] [int] NOT NULL,
	[MovementDate] [datetime2](7) NOT NULL,
	[Action] [nvarchar](max) NOT NULL,
 CONSTRAINT [PK_StockMovements] PRIMARY KEY CLUSTERED 
(
	[MovementID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Suppliers]    Script Date: 5/17/2025 1:12:05 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Suppliers](
	[SupplierID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](100) NOT NULL,
	[Email] [nvarchar](max) NOT NULL,
	[ContactPhone] [nvarchar](max) NOT NULL,
 CONSTRAINT [PK_Suppliers] PRIMARY KEY CLUSTERED 
(
	[SupplierID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Users]    Script Date: 5/17/2025 1:12:05 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Users](
	[UserID] [int] IDENTITY(1,1) NOT NULL,
	[Username] [nvarchar](100) NOT NULL,
	[Password] [nvarchar](max) NOT NULL,
	[Role] [int] NOT NULL,
 CONSTRAINT [PK_Users] PRIMARY KEY CLUSTERED 
(
	[UserID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
INSERT [dbo].[__EFMigrationsHistory] ([MigrationId], [ProductVersion]) VALUES (N'20250513185848_InitialCreate', N'9.0.4')
INSERT [dbo].[__EFMigrationsHistory] ([MigrationId], [ProductVersion]) VALUES (N'20250516150921_AddActionToStockMovement', N'9.0.4')
INSERT [dbo].[__EFMigrationsHistory] ([MigrationId], [ProductVersion]) VALUES (N'20250516174849_UpdateItemsandOrders', N'9.0.4')
INSERT [dbo].[__EFMigrationsHistory] ([MigrationId], [ProductVersion]) VALUES (N'20250517062018_updateItems', N'9.0.4')
INSERT [dbo].[__EFMigrationsHistory] ([MigrationId], [ProductVersion]) VALUES (N'20250517062855_UpdateAlltables', N'9.0.4')
INSERT [dbo].[__EFMigrationsHistory] ([MigrationId], [ProductVersion]) VALUES (N'20250517063028_UpdateAlertsOrdersStockMovements', N'9.0.4')
GO
SET IDENTITY_INSERT [dbo].[Items] ON 

INSERT [dbo].[Items] ([ItemID], [Name], [Quantity], [Price], [LowStockThreshold], [SupplierID]) VALUES (3, N'Laptop', 50, CAST(75000.00 AS Decimal(18, 2)), 10, 6)
INSERT [dbo].[Items] ([ItemID], [Name], [Quantity], [Price], [LowStockThreshold], [SupplierID]) VALUES (4, N'Smartphone', 100, CAST(50000.00 AS Decimal(18, 2)), 10, 7)
INSERT [dbo].[Items] ([ItemID], [Name], [Quantity], [Price], [LowStockThreshold], [SupplierID]) VALUES (5, N'Wireless Mouse', 150, CAST(2000.00 AS Decimal(18, 2)), 10, 8)
INSERT [dbo].[Items] ([ItemID], [Name], [Quantity], [Price], [LowStockThreshold], [SupplierID]) VALUES (6, N'Keyboard', 120, CAST(3500.00 AS Decimal(18, 2)), 10, 9)
INSERT [dbo].[Items] ([ItemID], [Name], [Quantity], [Price], [LowStockThreshold], [SupplierID]) VALUES (7, N'Headphones', 80, CAST(7500.00 AS Decimal(18, 2)), 10, 10)
INSERT [dbo].[Items] ([ItemID], [Name], [Quantity], [Price], [LowStockThreshold], [SupplierID]) VALUES (8, N'Monitor', 56, CAST(10000.00 AS Decimal(18, 2)), 10, 8)
INSERT [dbo].[Items] ([ItemID], [Name], [Quantity], [Price], [LowStockThreshold], [SupplierID]) VALUES (10, N'Speaker', 60, CAST(7000.00 AS Decimal(18, 2)), 10, 10)
SET IDENTITY_INSERT [dbo].[Items] OFF
GO
SET IDENTITY_INSERT [dbo].[Suppliers] ON 

INSERT [dbo].[Suppliers] ([SupplierID], [Name], [Email], [ContactPhone]) VALUES (6, N'Tech Supplies Ltd.', N'tech@supplies.com', N'013-456-7890')
INSERT [dbo].[Suppliers] ([SupplierID], [Name], [Email], [ContactPhone]) VALUES (7, N'MobileTech Distributors', N'mobile@tech.com', N'0154-567-8901')
INSERT [dbo].[Suppliers] ([SupplierID], [Name], [Email], [ContactPhone]) VALUES (8, N'	Gadget Hub', N'gadgets@hub.com', N'	01945-678-9012')
INSERT [dbo].[Suppliers] ([SupplierID], [Name], [Email], [ContactPhone]) VALUES (9, N'	OfficeTech Supplies', N'office@techsupplies.com', N'	0156-789-0123')
INSERT [dbo].[Suppliers] ([SupplierID], [Name], [Email], [ContactPhone]) VALUES (10, N'Flash Media Supplies', N'flash@media.com', N'	0167-890-1234')
SET IDENTITY_INSERT [dbo].[Suppliers] OFF
GO
SET IDENTITY_INSERT [dbo].[Users] ON 

INSERT [dbo].[Users] ([UserID], [Username], [Password], [Role]) VALUES (12, N'admin', N'admin123', 0)
INSERT [dbo].[Users] ([UserID], [Username], [Password], [Role]) VALUES (13, N'Maliha', N'123456', 1)
INSERT [dbo].[Users] ([UserID], [Username], [Password], [Role]) VALUES (14, N'OfficeTech', N'12356', 2)
SET IDENTITY_INSERT [dbo].[Users] OFF
GO
/****** Object:  Index [IX_Alerts_ItemID]    Script Date: 5/17/2025 1:12:05 PM ******/
CREATE NONCLUSTERED INDEX [IX_Alerts_ItemID] ON [dbo].[Alerts]
(
	[ItemID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_Items_SupplierID]    Script Date: 5/17/2025 1:12:05 PM ******/
CREATE NONCLUSTERED INDEX [IX_Items_SupplierID] ON [dbo].[Items]
(
	[SupplierID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_Orders_ItemID]    Script Date: 5/17/2025 1:12:05 PM ******/
CREATE NONCLUSTERED INDEX [IX_Orders_ItemID] ON [dbo].[Orders]
(
	[ItemID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_Orders_SupplierID]    Script Date: 5/17/2025 1:12:05 PM ******/
CREATE NONCLUSTERED INDEX [IX_Orders_SupplierID] ON [dbo].[Orders]
(
	[SupplierID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_StockMovements_ItemID]    Script Date: 5/17/2025 1:12:05 PM ******/
CREATE NONCLUSTERED INDEX [IX_StockMovements_ItemID] ON [dbo].[StockMovements]
(
	[ItemID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_StockMovements_UserID]    Script Date: 5/17/2025 1:12:05 PM ******/
CREATE NONCLUSTERED INDEX [IX_StockMovements_UserID] ON [dbo].[StockMovements]
(
	[UserID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
ALTER TABLE [dbo].[StockMovements] ADD  DEFAULT (N'') FOR [Action]
GO
ALTER TABLE [dbo].[Alerts]  WITH CHECK ADD  CONSTRAINT [FK_Alerts_Items_ItemID] FOREIGN KEY([ItemID])
REFERENCES [dbo].[Items] ([ItemID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Alerts] CHECK CONSTRAINT [FK_Alerts_Items_ItemID]
GO
ALTER TABLE [dbo].[Items]  WITH CHECK ADD  CONSTRAINT [FK_Items_Suppliers_SupplierID] FOREIGN KEY([SupplierID])
REFERENCES [dbo].[Suppliers] ([SupplierID])
GO
ALTER TABLE [dbo].[Items] CHECK CONSTRAINT [FK_Items_Suppliers_SupplierID]
GO
ALTER TABLE [dbo].[Orders]  WITH CHECK ADD  CONSTRAINT [FK_Orders_Items_ItemID] FOREIGN KEY([ItemID])
REFERENCES [dbo].[Items] ([ItemID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Orders] CHECK CONSTRAINT [FK_Orders_Items_ItemID]
GO
ALTER TABLE [dbo].[Orders]  WITH CHECK ADD  CONSTRAINT [FK_Orders_Suppliers_SupplierID] FOREIGN KEY([SupplierID])
REFERENCES [dbo].[Suppliers] ([SupplierID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Orders] CHECK CONSTRAINT [FK_Orders_Suppliers_SupplierID]
GO
ALTER TABLE [dbo].[StockMovements]  WITH CHECK ADD  CONSTRAINT [FK_StockMovements_Items_ItemID] FOREIGN KEY([ItemID])
REFERENCES [dbo].[Items] ([ItemID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[StockMovements] CHECK CONSTRAINT [FK_StockMovements_Items_ItemID]
GO
ALTER TABLE [dbo].[StockMovements]  WITH CHECK ADD  CONSTRAINT [FK_StockMovements_Users_UserID] FOREIGN KEY([UserID])
REFERENCES [dbo].[Users] ([UserID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[StockMovements] CHECK CONSTRAINT [FK_StockMovements_Users_UserID]
GO
USE [master]
GO
ALTER DATABASE [InventoryDB] SET  READ_WRITE 
GO
