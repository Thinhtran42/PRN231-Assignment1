IF OBJECT_ID(N'[__EFMigrationsHistory]') IS NULL
BEGIN
    CREATE TABLE [__EFMigrationsHistory] (
        [MigrationId] nvarchar(150) NOT NULL,
        [ProductVersion] nvarchar(32) NOT NULL,
        CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY ([MigrationId])
    );
END;
GO

BEGIN TRANSACTION;
GO

CREATE TABLE [Category] (
    [CategoryId] int NOT NULL,
    [CategoryName] varchar(40) NOT NULL,
    CONSTRAINT [PK__Category__19093A0B51D6CA75] PRIMARY KEY ([CategoryId])
);
GO

CREATE TABLE [Member] (
    [MemberId] int NOT NULL IDENTITY,
    [Email] varchar(100) NOT NULL,
    [CompanyName] varchar(40) NOT NULL,
    [City] varchar(15) NOT NULL,
    [Country] varchar(15) NOT NULL,
    [Password] varchar(30) NOT NULL,
    [Role] nvarchar(max) NOT NULL,
    CONSTRAINT [PK__Member__0CF04B187FC38C31] PRIMARY KEY ([MemberId])
);
GO

CREATE TABLE [Product] (
    [ProductId] int NOT NULL,
    [CategoryId] int NULL,
    [ProductName] varchar(40) NOT NULL,
    [Weight] varchar(20) NOT NULL,
    [UnitPrice] money NOT NULL,
    [UnitsInStock] int NOT NULL,
    CONSTRAINT [PK__Product__B40CC6CDCD1CE058] PRIMARY KEY ([ProductId]),
    CONSTRAINT [FK__Product__Categor__3E52440B] FOREIGN KEY ([CategoryId]) REFERENCES [Category] ([CategoryId]) ON DELETE CASCADE
);
GO

CREATE TABLE [Order] (
    [OrderId] int NOT NULL,
    [MemberId] int NULL,
    [OrderDate] datetime NOT NULL,
    [RequiredDate] datetime NULL,
    [ShippedDate] datetime NULL,
    [Freight] money NULL,
    CONSTRAINT [PK__Order__C3905BCFE0640B54] PRIMARY KEY ([OrderId]),
    CONSTRAINT [FK__Order__MemberId__398D8EEE] FOREIGN KEY ([MemberId]) REFERENCES [Member] ([MemberId]) ON DELETE CASCADE
);
GO

CREATE TABLE [OrderDetail] (
    [OrderId] int NOT NULL,
    [ProductId] int NOT NULL,
    [UnitPrice] money NOT NULL,
    [Quantity] int NOT NULL,
    [Discount] float NOT NULL,
    CONSTRAINT [PK__OrderDet__08D097A351433680] PRIMARY KEY ([OrderId], [ProductId]),
    CONSTRAINT [FK__OrderDeta__Order__412EB0B6] FOREIGN KEY ([OrderId]) REFERENCES [Order] ([OrderId]) ON DELETE CASCADE,
    CONSTRAINT [FK__OrderDeta__Produ__4222D4EF] FOREIGN KEY ([ProductId]) REFERENCES [Product] ([ProductId]) ON DELETE CASCADE
);
GO

CREATE INDEX [IX_Order_MemberId] ON [Order] ([MemberId]);
GO

CREATE INDEX [IX_OrderDetail_ProductId] ON [OrderDetail] ([ProductId]);
GO

CREATE INDEX [IX_Product_CategoryId] ON [Product] ([CategoryId]);
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20240410100413_Initial', N'7.0.17');
GO

COMMIT;
GO

