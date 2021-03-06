USE [master]
GO
/****** Object:  Database [BlackSmith]    Script Date: 9/9/2019 12:16:35 AM ******/
CREATE DATABASE [BlackSmith]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'BlackSmith', FILENAME = N'D:\ProgramFiles\Microsoft\SQL Server\MSSQL14.SQLEXPRESS\MSSQL\DATA\BlackSmith.mdf' , SIZE = 5120KB , MAXSIZE = UNLIMITED, FILEGROWTH = 1024KB )
 LOG ON 
( NAME = N'BlackSmith_log', FILENAME = N'D:\ProgramFiles\Microsoft\SQL Server\MSSQL14.SQLEXPRESS\MSSQL\DATA\BlackSmith_log.ldf' , SIZE = 2560KB , MAXSIZE = 2048GB , FILEGROWTH = 10%)
GO
ALTER DATABASE [BlackSmith] SET COMPATIBILITY_LEVEL = 110
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [BlackSmith].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [BlackSmith] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [BlackSmith] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [BlackSmith] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [BlackSmith] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [BlackSmith] SET ARITHABORT OFF 
GO
ALTER DATABASE [BlackSmith] SET AUTO_CLOSE ON 
GO
ALTER DATABASE [BlackSmith] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [BlackSmith] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [BlackSmith] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [BlackSmith] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [BlackSmith] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [BlackSmith] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [BlackSmith] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [BlackSmith] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [BlackSmith] SET  DISABLE_BROKER 
GO
ALTER DATABASE [BlackSmith] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [BlackSmith] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [BlackSmith] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [BlackSmith] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [BlackSmith] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [BlackSmith] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [BlackSmith] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [BlackSmith] SET RECOVERY SIMPLE 
GO
ALTER DATABASE [BlackSmith] SET  MULTI_USER 
GO
ALTER DATABASE [BlackSmith] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [BlackSmith] SET DB_CHAINING OFF 
GO
ALTER DATABASE [BlackSmith] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [BlackSmith] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [BlackSmith] SET DELAYED_DURABILITY = DISABLED 
GO
ALTER DATABASE [BlackSmith] SET QUERY_STORE = OFF
GO
USE [BlackSmith]
GO
/****** Object:  User [IIS APPPOOL\NoManagedCodeAppPool]    Script Date: 9/9/2019 12:16:35 AM ******/
CREATE USER [IIS APPPOOL\NoManagedCodeAppPool] FOR LOGIN [IIS APPPOOL\NoManagedCodeAppPool] WITH DEFAULT_SCHEMA=[dbo]
GO
ALTER ROLE [db_datareader] ADD MEMBER [IIS APPPOOL\NoManagedCodeAppPool]
GO
ALTER ROLE [db_datawriter] ADD MEMBER [IIS APPPOOL\NoManagedCodeAppPool]
GO
/****** Object:  Table [dbo].[tCustomer]    Script Date: 9/9/2019 12:16:36 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tCustomer](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](500) NOT NULL,
	[Address] [nvarchar](200) NULL,
	[ContactNo] [nvarchar](200) NULL,
	[EmailId] [nvarchar](500) NULL,
	[CreatedOn] [datetime] NOT NULL,
	[CreatedBy] [bigint] NOT NULL,
	[ModifiedOn] [datetime] NULL,
	[ModifiedBy] [bigint] NULL,
	[IsDeleted] [bit] NOT NULL,
	[GSTIN] [nvarchar](500) NULL,
	[PAN] [nvarchar](500) NULL,
 CONSTRAINT [PK_tCustomer_1] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tInventoryItem]    Script Date: 9/9/2019 12:16:36 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tInventoryItem](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](500) NOT NULL,
	[Description] [nvarchar](2000) NULL,
	[CreatedOn] [datetime] NOT NULL,
	[CreatedBy] [bigint] NOT NULL,
	[ModifiedOn] [datetime] NULL,
	[ModifiedBy] [bigint] NULL,
	[IsDeleted] [bit] NOT NULL,
	[Availibility] [numeric](18, 3) NOT NULL,
	[SSN] [nvarchar](50) NULL,
 CONSTRAINT [PK_tInventoryItem_1] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tProduct]    Script Date: 9/9/2019 12:16:36 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tProduct](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](500) NOT NULL,
	[Description] [nvarchar](2000) NULL,
	[CreatedOn] [datetime] NOT NULL,
	[CreatedBy] [bigint] NOT NULL,
	[ModifiedOn] [datetime] NULL,
	[ModifiedBy] [bigint] NULL,
	[IsDeleted] [bit] NOT NULL,
	[Image] [nvarchar](50) NULL,
	[Price] [numeric](18, 2) NULL,
	[Availibility] [numeric](18, 3) NOT NULL,
	[SSN] [nvarchar](50) NULL,
 CONSTRAINT [PK_tProduct_1] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tProduction]    Script Date: 9/9/2019 12:16:36 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tProduction](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[Date] [datetime] NULL,
	[CreatedOn] [datetime] NOT NULL,
	[CreatedBy] [bigint] NOT NULL,
	[ModifiedBy] [bigint] NULL,
	[ModifiedOn] [datetime] NULL,
	[IsDeleted] [bit] NOT NULL,
	[Note] [nvarchar](max) NULL,
 CONSTRAINT [PK_tProduction] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tProductionInventoryItem]    Script Date: 9/9/2019 12:16:36 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tProductionInventoryItem](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[FK_ProductionId] [bigint] NOT NULL,
	[FK_InventoryItemId] [bigint] NOT NULL,
	[Quantity] [numeric](18, 3) NOT NULL,
	[CreatedBy] [bigint] NOT NULL,
	[CreatedOn] [datetime] NOT NULL,
	[ModifiedBy] [bigint] NULL,
	[ModifiedOn] [datetime] NULL,
	[IsDeleted] [bit] NULL,
 CONSTRAINT [PK_tProductionInventory] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tProductionProduct]    Script Date: 9/9/2019 12:16:36 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tProductionProduct](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[FK_ProductionId] [bigint] NOT NULL,
	[FK_ProductId] [bigint] NOT NULL,
	[Quantity] [numeric](18, 3) NOT NULL,
	[CreatedOn] [datetime] NOT NULL,
	[CreatedBy] [bigint] NOT NULL,
	[ModifiedOn] [datetime] NULL,
	[ModifiedBy] [bigint] NULL,
	[IsDeleted] [bit] NOT NULL,
 CONSTRAINT [PK_tProductionProduct] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tPurchase]    Script Date: 9/9/2019 12:16:36 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tPurchase](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[CreatedOn] [datetime] NOT NULL,
	[CreatedBy] [bigint] NOT NULL,
	[ModifiedOn] [datetime] NULL,
	[ModifiedBy] [bigint] NULL,
	[IsDeleted] [bit] NOT NULL,
	[Discount] [numeric](18, 2) NULL,
	[CGSTRate] [numeric](18, 2) NULL,
	[CGSTTax] [numeric](18, 2) NULL,
	[Note] [nvarchar](2000) NULL,
	[PurchaseId] [nvarchar](2000) NULL,
	[PurchaseDate] [datetime] NOT NULL,
	[SGSTRate] [numeric](18, 2) NULL,
	[SGSTTax] [numeric](18, 2) NULL,
	[Total] [numeric](18, 2) NULL,
	[PaymentTerm] [nvarchar](50) NULL,
	[DispatchThru] [nvarchar](50) NULL,
	[FinalTotal] [numeric](18, 2) NULL,
	[FK_SuplierId] [bigint] NOT NULL,
	[RoundOffTotal] [numeric](18, 2) NULL,
 CONSTRAINT [PK_tPurchase] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tPurchaseDetail]    Script Date: 9/9/2019 12:16:36 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tPurchaseDetail](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[Fk_InventoryItemId] [bigint] NOT NULL,
	[Fk_PurchaseId] [bigint] NOT NULL,
	[Price] [numeric](18, 2) NOT NULL,
	[Quantity] [numeric](18, 3) NULL,
	[CreatedBy] [bigint] NOT NULL,
	[CreatedOn] [datetime] NOT NULL,
	[ModifiedBy] [bigint] NULL,
	[ModifiedOn] [datetime] NULL,
	[IsDeleted] [bit] NOT NULL,
 CONSTRAINT [PK_tPurchaseDetail] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tPurchasePayment]    Script Date: 9/9/2019 12:16:36 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tPurchasePayment](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[Fk_PurchaseId] [bigint] NOT NULL,
	[Amount] [numeric](18, 2) NOT NULL,
	[Note] [nvarchar](2000) NULL,
	[CreatedBy] [bigint] NOT NULL,
	[CreatedOn] [datetime] NOT NULL,
	[ModifiedBy] [bigint] NULL,
	[ModifiedOn] [datetime] NULL,
	[IsDeleted] [bit] NOT NULL,
	[PaymentDate] [datetime] NULL,
 CONSTRAINT [PK_tPurchasePayment] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tSale]    Script Date: 9/9/2019 12:16:36 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tSale](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[Fk_CustomerId] [bigint] NOT NULL,
	[CreatedOn] [datetime] NOT NULL,
	[CreatedBy] [bigint] NOT NULL,
	[ModifiedOn] [datetime] NULL,
	[ModifiedBy] [bigint] NULL,
	[IsDeleted] [bit] NOT NULL,
	[BillId] [nvarchar](max) NOT NULL,
	[CGSTRate] [numeric](18, 2) NOT NULL,
	[SGSTRate] [numeric](18, 2) NOT NULL,
	[CGSTTax] [numeric](18, 2) NOT NULL,
	[Discount] [numeric](18, 2) NULL,
	[Note] [nvarchar](2000) NULL,
	[BillDate] [datetime] NOT NULL,
	[Total] [numeric](18, 2) NOT NULL,
	[FinalTotal] [numeric](18, 2) NOT NULL,
	[SGSTTax] [numeric](18, 2) NOT NULL,
	[PaymentTerm] [nvarchar](500) NULL,
	[DispatchThru] [nvarchar](500) NULL,
	[RoundOffTotal] [numeric](18, 2) NULL,
 CONSTRAINT [PK_tSale] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tSaleDetail]    Script Date: 9/9/2019 12:16:36 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tSaleDetail](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[Fk_SaleId] [bigint] NOT NULL,
	[Fk_ProductId] [bigint] NOT NULL,
	[Price] [numeric](18, 2) NULL,
	[Quantity] [numeric](18, 3) NULL,
	[CreatedBy] [bigint] NOT NULL,
	[CreatedOn] [datetime] NOT NULL,
	[ModifiedBy] [bigint] NULL,
	[ModifiedOn] [datetime] NULL,
	[IsDeleted] [bit] NOT NULL,
 CONSTRAINT [PK_tSaleDetail] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tSalePayment]    Script Date: 9/9/2019 12:16:36 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tSalePayment](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[Fk_SaleId] [bigint] NOT NULL,
	[Amount] [numeric](18, 2) NOT NULL,
	[Note] [nvarchar](2000) NULL,
	[CreatedBy] [bigint] NOT NULL,
	[CreatedOn] [datetime] NOT NULL,
	[ModifiedBy] [bigint] NULL,
	[ModifiedOn] [datetime] NULL,
	[IsDeleted] [bit] NOT NULL,
	[PaymentDate] [datetime] NOT NULL,
 CONSTRAINT [PK_tSalePayment] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tSuplier]    Script Date: 9/9/2019 12:16:36 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tSuplier](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](500) NOT NULL,
	[Address] [nvarchar](2000) NULL,
	[ContactNo] [nvarchar](500) NULL,
	[EmailId] [nvarchar](500) NULL,
	[CreatedOn] [datetime] NOT NULL,
	[CreatedBy] [bigint] NOT NULL,
	[ModifiedOn] [datetime] NULL,
	[ModifiedBy] [bigint] NULL,
	[IsDeleted] [bit] NOT NULL,
	[GSTIN] [nvarchar](500) NULL,
	[PAN] [nvarchar](500) NULL,
 CONSTRAINT [PK_tSuplier] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[tCustomer] ADD  CONSTRAINT [DF_tCustomer_IsDeleted]  DEFAULT ((0)) FOR [IsDeleted]
GO
ALTER TABLE [dbo].[tInventoryItem] ADD  CONSTRAINT [DF_tInventoryItem_IsDeleted]  DEFAULT ((0)) FOR [IsDeleted]
GO
ALTER TABLE [dbo].[tInventoryItem] ADD  CONSTRAINT [DF_tInventoryItem_Availibility]  DEFAULT ((0)) FOR [Availibility]
GO
ALTER TABLE [dbo].[tProduct] ADD  CONSTRAINT [DF_tProduct_IsDeleted]  DEFAULT ((0)) FOR [IsDeleted]
GO
ALTER TABLE [dbo].[tProduct] ADD  CONSTRAINT [DF_tProduct_Price]  DEFAULT ((0)) FOR [Price]
GO
ALTER TABLE [dbo].[tProduct] ADD  CONSTRAINT [DF_tProduct_Availibility]  DEFAULT ((0)) FOR [Availibility]
GO
ALTER TABLE [dbo].[tProduction] ADD  CONSTRAINT [DF_tProduction_IsDeleted]  DEFAULT ((0)) FOR [IsDeleted]
GO
ALTER TABLE [dbo].[tProductionInventoryItem] ADD  CONSTRAINT [DF_tProductionInventory_Quantity]  DEFAULT ((0)) FOR [Quantity]
GO
ALTER TABLE [dbo].[tProductionInventoryItem] ADD  CONSTRAINT [DF_tProductionInventory_IsDeleted]  DEFAULT ((0)) FOR [IsDeleted]
GO
ALTER TABLE [dbo].[tProductionProduct] ADD  CONSTRAINT [DF_tProductionProduct_Quantity]  DEFAULT ((0)) FOR [Quantity]
GO
ALTER TABLE [dbo].[tProductionProduct] ADD  CONSTRAINT [DF_tProductionProduct_IsDeleted]  DEFAULT ((0)) FOR [IsDeleted]
GO
ALTER TABLE [dbo].[tPurchase] ADD  CONSTRAINT [DF_tPurchase_IsDeleted]  DEFAULT ((0)) FOR [IsDeleted]
GO
ALTER TABLE [dbo].[tPurchase] ADD  CONSTRAINT [DF_tPurchase_Discount]  DEFAULT ((0)) FOR [Discount]
GO
ALTER TABLE [dbo].[tPurchase] ADD  CONSTRAINT [DF_tPurchase_GSTRate]  DEFAULT ((0)) FOR [CGSTRate]
GO
ALTER TABLE [dbo].[tPurchase] ADD  CONSTRAINT [DF_tPurchase_GSTAmount]  DEFAULT ((0)) FOR [CGSTTax]
GO
ALTER TABLE [dbo].[tPurchase] ADD  CONSTRAINT [DF_tPurchase_FK_SuplierId]  DEFAULT ((0)) FOR [FK_SuplierId]
GO
ALTER TABLE [dbo].[tPurchaseDetail] ADD  CONSTRAINT [DF_tPurchaseDetail_IsDeleted]  DEFAULT ((0)) FOR [IsDeleted]
GO
ALTER TABLE [dbo].[tPurchasePayment] ADD  CONSTRAINT [DF_tPurchasePayment_IsDeleted]  DEFAULT ((0)) FOR [IsDeleted]
GO
ALTER TABLE [dbo].[tSale] ADD  CONSTRAINT [DF_tSale_IsDeleted]  DEFAULT ((0)) FOR [IsDeleted]
GO
ALTER TABLE [dbo].[tSale] ADD  CONSTRAINT [DF_tSale_GSTRate]  DEFAULT ((0)) FOR [CGSTRate]
GO
ALTER TABLE [dbo].[tSale] ADD  CONSTRAINT [DF_tSale_GSTAmount]  DEFAULT ((0)) FOR [CGSTTax]
GO
ALTER TABLE [dbo].[tSale] ADD  CONSTRAINT [DF_tSale_Discount]  DEFAULT ((0)) FOR [Discount]
GO
ALTER TABLE [dbo].[tSaleDetail] ADD  CONSTRAINT [DF_tSaleDetail_IsDeleted]  DEFAULT ((0)) FOR [IsDeleted]
GO
ALTER TABLE [dbo].[tSalePayment] ADD  CONSTRAINT [DF_tSalePayment_IsDeleted]  DEFAULT ((0)) FOR [IsDeleted]
GO
ALTER TABLE [dbo].[tSuplier] ADD  CONSTRAINT [DF_tSuplier_IsDeleted]  DEFAULT ((0)) FOR [IsDeleted]
GO
ALTER TABLE [dbo].[tProductionInventoryItem]  WITH CHECK ADD  CONSTRAINT [FK_tProductionInventoryItem_tInventoryItem] FOREIGN KEY([FK_InventoryItemId])
REFERENCES [dbo].[tInventoryItem] ([Id])
GO
ALTER TABLE [dbo].[tProductionInventoryItem] CHECK CONSTRAINT [FK_tProductionInventoryItem_tInventoryItem]
GO
ALTER TABLE [dbo].[tProductionInventoryItem]  WITH CHECK ADD  CONSTRAINT [FK_tProductionInventoryItem_tProduction] FOREIGN KEY([FK_ProductionId])
REFERENCES [dbo].[tProduction] ([Id])
GO
ALTER TABLE [dbo].[tProductionInventoryItem] CHECK CONSTRAINT [FK_tProductionInventoryItem_tProduction]
GO
ALTER TABLE [dbo].[tProductionProduct]  WITH CHECK ADD  CONSTRAINT [FK_tProductionProduct_tProduct] FOREIGN KEY([FK_ProductId])
REFERENCES [dbo].[tProduct] ([Id])
GO
ALTER TABLE [dbo].[tProductionProduct] CHECK CONSTRAINT [FK_tProductionProduct_tProduct]
GO
ALTER TABLE [dbo].[tProductionProduct]  WITH CHECK ADD  CONSTRAINT [FK_tProductionProduct_tProduction] FOREIGN KEY([FK_ProductionId])
REFERENCES [dbo].[tProduction] ([Id])
GO
ALTER TABLE [dbo].[tProductionProduct] CHECK CONSTRAINT [FK_tProductionProduct_tProduction]
GO
ALTER TABLE [dbo].[tPurchaseDetail]  WITH CHECK ADD  CONSTRAINT [FK_tPurchaseDetail_tInventoryItem] FOREIGN KEY([Fk_InventoryItemId])
REFERENCES [dbo].[tInventoryItem] ([Id])
GO
ALTER TABLE [dbo].[tPurchaseDetail] CHECK CONSTRAINT [FK_tPurchaseDetail_tInventoryItem]
GO
ALTER TABLE [dbo].[tPurchaseDetail]  WITH CHECK ADD  CONSTRAINT [FK_tPurchaseDetail_tPurchase] FOREIGN KEY([Fk_PurchaseId])
REFERENCES [dbo].[tPurchase] ([Id])
GO
ALTER TABLE [dbo].[tPurchaseDetail] CHECK CONSTRAINT [FK_tPurchaseDetail_tPurchase]
GO
ALTER TABLE [dbo].[tPurchasePayment]  WITH CHECK ADD  CONSTRAINT [FK_tPurchasePayment_tPurchase] FOREIGN KEY([Fk_PurchaseId])
REFERENCES [dbo].[tPurchase] ([Id])
GO
ALTER TABLE [dbo].[tPurchasePayment] CHECK CONSTRAINT [FK_tPurchasePayment_tPurchase]
GO
ALTER TABLE [dbo].[tSale]  WITH CHECK ADD  CONSTRAINT [FK_tSale_tCustomer] FOREIGN KEY([Fk_CustomerId])
REFERENCES [dbo].[tCustomer] ([Id])
GO
ALTER TABLE [dbo].[tSale] CHECK CONSTRAINT [FK_tSale_tCustomer]
GO
ALTER TABLE [dbo].[tSaleDetail]  WITH CHECK ADD  CONSTRAINT [FK_tSaleDetail_tProduct] FOREIGN KEY([Fk_ProductId])
REFERENCES [dbo].[tProduct] ([Id])
GO
ALTER TABLE [dbo].[tSaleDetail] CHECK CONSTRAINT [FK_tSaleDetail_tProduct]
GO
ALTER TABLE [dbo].[tSaleDetail]  WITH CHECK ADD  CONSTRAINT [FK_tSaleDetail_tSale] FOREIGN KEY([Fk_SaleId])
REFERENCES [dbo].[tSale] ([Id])
GO
ALTER TABLE [dbo].[tSaleDetail] CHECK CONSTRAINT [FK_tSaleDetail_tSale]
GO
ALTER TABLE [dbo].[tSaleDetail]  WITH CHECK ADD  CONSTRAINT [FK_tSaleDetail_tSaleDetail] FOREIGN KEY([Id])
REFERENCES [dbo].[tSaleDetail] ([Id])
GO
ALTER TABLE [dbo].[tSaleDetail] CHECK CONSTRAINT [FK_tSaleDetail_tSaleDetail]
GO
ALTER TABLE [dbo].[tSalePayment]  WITH CHECK ADD  CONSTRAINT [FK_tSalePayment_tSale] FOREIGN KEY([Fk_SaleId])
REFERENCES [dbo].[tSale] ([Id])
GO
ALTER TABLE [dbo].[tSalePayment] CHECK CONSTRAINT [FK_tSalePayment_tSale]
GO
USE [master]
GO
ALTER DATABASE [BlackSmith] SET  READ_WRITE 
GO
