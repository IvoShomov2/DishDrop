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
IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'00000000000000_CreateIdentitySchema'
)
BEGIN
    CREATE TABLE [AspNetRoles] (
        [Id] nvarchar(450) NOT NULL,
        [Name] nvarchar(256) NULL,
        [NormalizedName] nvarchar(256) NULL,
        [ConcurrencyStamp] nvarchar(max) NULL,
        CONSTRAINT [PK_AspNetRoles] PRIMARY KEY ([Id])
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'00000000000000_CreateIdentitySchema'
)
BEGIN
    CREATE TABLE [AspNetUsers] (
        [Id] nvarchar(450) NOT NULL,
        [UserName] nvarchar(256) NULL,
        [NormalizedUserName] nvarchar(256) NULL,
        [Email] nvarchar(256) NULL,
        [NormalizedEmail] nvarchar(256) NULL,
        [EmailConfirmed] bit NOT NULL,
        [PasswordHash] nvarchar(max) NULL,
        [SecurityStamp] nvarchar(max) NULL,
        [ConcurrencyStamp] nvarchar(max) NULL,
        [PhoneNumber] nvarchar(max) NULL,
        [PhoneNumberConfirmed] bit NOT NULL,
        [TwoFactorEnabled] bit NOT NULL,
        [LockoutEnd] datetimeoffset NULL,
        [LockoutEnabled] bit NOT NULL,
        [AccessFailedCount] int NOT NULL,
        CONSTRAINT [PK_AspNetUsers] PRIMARY KEY ([Id])
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'00000000000000_CreateIdentitySchema'
)
BEGIN
    CREATE TABLE [AspNetRoleClaims] (
        [Id] int NOT NULL IDENTITY,
        [RoleId] nvarchar(450) NOT NULL,
        [ClaimType] nvarchar(max) NULL,
        [ClaimValue] nvarchar(max) NULL,
        CONSTRAINT [PK_AspNetRoleClaims] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_AspNetRoleClaims_AspNetRoles_RoleId] FOREIGN KEY ([RoleId]) REFERENCES [AspNetRoles] ([Id]) ON DELETE CASCADE
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'00000000000000_CreateIdentitySchema'
)
BEGIN
    CREATE TABLE [AspNetUserClaims] (
        [Id] int NOT NULL IDENTITY,
        [UserId] nvarchar(450) NOT NULL,
        [ClaimType] nvarchar(max) NULL,
        [ClaimValue] nvarchar(max) NULL,
        CONSTRAINT [PK_AspNetUserClaims] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_AspNetUserClaims_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE CASCADE
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'00000000000000_CreateIdentitySchema'
)
BEGIN
    CREATE TABLE [AspNetUserLogins] (
        [LoginProvider] nvarchar(128) NOT NULL,
        [ProviderKey] nvarchar(128) NOT NULL,
        [ProviderDisplayName] nvarchar(max) NULL,
        [UserId] nvarchar(450) NOT NULL,
        CONSTRAINT [PK_AspNetUserLogins] PRIMARY KEY ([LoginProvider], [ProviderKey]),
        CONSTRAINT [FK_AspNetUserLogins_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE CASCADE
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'00000000000000_CreateIdentitySchema'
)
BEGIN
    CREATE TABLE [AspNetUserRoles] (
        [UserId] nvarchar(450) NOT NULL,
        [RoleId] nvarchar(450) NOT NULL,
        CONSTRAINT [PK_AspNetUserRoles] PRIMARY KEY ([UserId], [RoleId]),
        CONSTRAINT [FK_AspNetUserRoles_AspNetRoles_RoleId] FOREIGN KEY ([RoleId]) REFERENCES [AspNetRoles] ([Id]) ON DELETE CASCADE,
        CONSTRAINT [FK_AspNetUserRoles_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE CASCADE
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'00000000000000_CreateIdentitySchema'
)
BEGIN
    CREATE TABLE [AspNetUserTokens] (
        [UserId] nvarchar(450) NOT NULL,
        [LoginProvider] nvarchar(128) NOT NULL,
        [Name] nvarchar(128) NOT NULL,
        [Value] nvarchar(max) NULL,
        CONSTRAINT [PK_AspNetUserTokens] PRIMARY KEY ([UserId], [LoginProvider], [Name]),
        CONSTRAINT [FK_AspNetUserTokens_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE CASCADE
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'00000000000000_CreateIdentitySchema'
)
BEGIN
    CREATE INDEX [IX_AspNetRoleClaims_RoleId] ON [AspNetRoleClaims] ([RoleId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'00000000000000_CreateIdentitySchema'
)
BEGIN
    EXEC(N'CREATE UNIQUE INDEX [RoleNameIndex] ON [AspNetRoles] ([NormalizedName]) WHERE [NormalizedName] IS NOT NULL');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'00000000000000_CreateIdentitySchema'
)
BEGIN
    CREATE INDEX [IX_AspNetUserClaims_UserId] ON [AspNetUserClaims] ([UserId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'00000000000000_CreateIdentitySchema'
)
BEGIN
    CREATE INDEX [IX_AspNetUserLogins_UserId] ON [AspNetUserLogins] ([UserId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'00000000000000_CreateIdentitySchema'
)
BEGIN
    CREATE INDEX [IX_AspNetUserRoles_RoleId] ON [AspNetUserRoles] ([RoleId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'00000000000000_CreateIdentitySchema'
)
BEGIN
    CREATE INDEX [EmailIndex] ON [AspNetUsers] ([NormalizedEmail]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'00000000000000_CreateIdentitySchema'
)
BEGIN
    EXEC(N'CREATE UNIQUE INDEX [UserNameIndex] ON [AspNetUsers] ([NormalizedUserName]) WHERE [NormalizedUserName] IS NOT NULL');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'00000000000000_CreateIdentitySchema'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'00000000000000_CreateIdentitySchema', N'10.0.14');
END;

COMMIT;
GO

BEGIN TRANSACTION;
IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261114175432_AddInitialBusinessSchema'
)
BEGIN
    IF EXISTS (
        SELECT [NormalizedEmail] FROM [AspNetUsers]
        WHERE [NormalizedEmail] IS NOT NULL AND [NormalizedEmail] <> N''
        GROUP BY [NormalizedEmail] HAVING COUNT(*) > 1
    )
        THROW 51000, 'Duplicate nonempty NormalizedEmail values in AspNetUsers. Resolve explicitly before migration; no users were merged or deleted.', 1;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261114175432_AddInitialBusinessSchema'
)
BEGIN
    DROP INDEX [EmailIndex] ON [AspNetUsers];
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261114175432_AddInitialBusinessSchema'
)
BEGIN
    ALTER TABLE [AspNetUsers] ADD [CreatedAtUtc] datetime2 NOT NULL DEFAULT (SYSUTCDATETIME());
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261114175432_AddInitialBusinessSchema'
)
BEGIN
    ALTER TABLE [AspNetUsers] ADD [FirstName] nvarchar(100) NOT NULL DEFAULT N'';
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261114175432_AddInitialBusinessSchema'
)
BEGIN
    ALTER TABLE [AspNetUsers] ADD [IsActive] bit NOT NULL DEFAULT CAST(1 AS bit);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261114175432_AddInitialBusinessSchema'
)
BEGIN
    ALTER TABLE [AspNetUsers] ADD [LastName] nvarchar(100) NOT NULL DEFAULT N'';
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261114175432_AddInitialBusinessSchema'
)
BEGIN
    CREATE TABLE [Allergen] (
        [Id] int NOT NULL IDENTITY,
        [Code] nvarchar(30) NOT NULL,
        [Name] nvarchar(150) NOT NULL,
        CONSTRAINT [PK_Allergen] PRIMARY KEY ([Id])
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261114175432_AddInitialBusinessSchema'
)
BEGIN
    CREATE TABLE [AuditLog] (
        [Id] int NOT NULL IDENTITY,
        [ActorUserId] nvarchar(450) NULL,
        [Action] nvarchar(100) NOT NULL,
        [EntityType] nvarchar(100) NOT NULL,
        [EntityId] nvarchar(450) NOT NULL,
        [CreatedAtUtc] datetime2 NOT NULL,
        [Reason] nvarchar(1000) NULL,
        [CorrelationId] nvarchar(100) NULL,
        CONSTRAINT [PK_AuditLog] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_AuditLog_AspNetUsers_ActorUserId] FOREIGN KEY ([ActorUserId]) REFERENCES [AspNetUsers] ([Id])
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261114175432_AddInitialBusinessSchema'
)
BEGIN
    CREATE TABLE [City] (
        [Id] int NOT NULL IDENTITY,
        [Name] nvarchar(100) NOT NULL,
        [NormalizedName] nvarchar(100) NOT NULL,
        [IsActive] bit NOT NULL,
        CONSTRAINT [PK_City] PRIMARY KEY ([Id])
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261114175432_AddInitialBusinessSchema'
)
BEGIN
    CREATE TABLE [CourierProfile] (
        [UserId] nvarchar(450) NOT NULL,
        [ApprovalStatus] int NOT NULL,
        [VehicleType] int NOT NULL,
        [IsAvailable] bit NOT NULL,
        [CreatedAtUtc] datetime2 NOT NULL,
        [ApprovedAtUtc] datetime2 NULL,
        [RowVersion] rowversion NOT NULL,
        CONSTRAINT [PK_CourierProfile] PRIMARY KEY ([UserId]),
        CONSTRAINT [CK_CourierProfile_ApprovalStatus] CHECK ([ApprovalStatus] BETWEEN 0 AND 2),
        CONSTRAINT [CK_CourierProfile_VehicleType] CHECK ([VehicleType] BETWEEN 0 AND 2),
        CONSTRAINT [FK_CourierProfile_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [AspNetUsers] ([Id])
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261114175432_AddInitialBusinessSchema'
)
BEGIN
    CREATE TABLE [CuisineType] (
        [Id] int NOT NULL IDENTITY,
        [Name] nvarchar(100) NOT NULL,
        [NormalizedName] nvarchar(100) NOT NULL,
        [IsActive] bit NOT NULL,
        CONSTRAINT [PK_CuisineType] PRIMARY KEY ([Id])
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261114175432_AddInitialBusinessSchema'
)
BEGIN
    CREATE TABLE [DeliveryArea] (
        [Id] int NOT NULL IDENTITY,
        [CityId] int NOT NULL,
        [Name] nvarchar(100) NOT NULL,
        [NormalizedName] nvarchar(100) NOT NULL,
        [IsActive] bit NOT NULL,
        CONSTRAINT [PK_DeliveryArea] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_DeliveryArea_City_CityId] FOREIGN KEY ([CityId]) REFERENCES [City] ([Id])
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261114175432_AddInitialBusinessSchema'
)
BEGIN
    CREATE TABLE [Restaurant] (
        [Id] int NOT NULL IDENTITY,
        [CityId] int NOT NULL,
        [Name] nvarchar(150) NOT NULL,
        [Slug] nvarchar(180) NOT NULL,
        [Description] nvarchar(2000) NOT NULL,
        [Address] nvarchar(300) NOT NULL,
        [PhoneNumber] nvarchar(30) NOT NULL,
        [ImagePath] nvarchar(500) NULL,
        [ApprovalStatus] int NOT NULL,
        [IsActive] bit NOT NULL,
        [IsAcceptingOrders] bit NOT NULL,
        [MinimumOrderAmount] decimal(18,2) NOT NULL,
        [EstimatedPreparationMinutes] int NOT NULL,
        [CreatedAtUtc] datetime2 NOT NULL,
        [RowVersion] rowversion NOT NULL,
        CONSTRAINT [PK_Restaurant] PRIMARY KEY ([Id]),
        CONSTRAINT [CK_Restaurant_ApprovalStatus] CHECK ([ApprovalStatus] BETWEEN 0 AND 2),
        CONSTRAINT [CK_Restaurant_MinimumOrder] CHECK ([MinimumOrderAmount] >= 0),
        CONSTRAINT [CK_Restaurant_Preparation] CHECK ([EstimatedPreparationMinutes] > 0),
        CONSTRAINT [FK_Restaurant_City_CityId] FOREIGN KEY ([CityId]) REFERENCES [City] ([Id])
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261114175432_AddInitialBusinessSchema'
)
BEGIN
    CREATE TABLE [CourierApprovalHistory] (
        [Id] int NOT NULL IDENTITY,
        [CourierId] nvarchar(450) NOT NULL,
        [FromStatus] int NULL,
        [ToStatus] int NOT NULL,
        [ChangedByUserId] nvarchar(450) NOT NULL,
        [ChangedAtUtc] datetime2 NOT NULL,
        [Reason] nvarchar(1000) NOT NULL,
        CONSTRAINT [PK_CourierApprovalHistory] PRIMARY KEY ([Id]),
        CONSTRAINT [CK_CourierApprovalHistory_FromStatus] CHECK ([FromStatus] IS NULL OR [FromStatus] BETWEEN 0 AND 2),
        CONSTRAINT [CK_CourierApprovalHistory_ToStatus] CHECK ([ToStatus] BETWEEN 0 AND 2),
        CONSTRAINT [FK_CourierApprovalHistory_AspNetUsers_ChangedByUserId] FOREIGN KEY ([ChangedByUserId]) REFERENCES [AspNetUsers] ([Id]),
        CONSTRAINT [FK_CourierApprovalHistory_CourierProfile_CourierId] FOREIGN KEY ([CourierId]) REFERENCES [CourierProfile] ([UserId])
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261114175432_AddInitialBusinessSchema'
)
BEGIN
    CREATE TABLE [CourierDeliveryArea] (
        [CourierId] nvarchar(450) NOT NULL,
        [DeliveryAreaId] int NOT NULL,
        CONSTRAINT [PK_CourierDeliveryArea] PRIMARY KEY NONCLUSTERED ([CourierId], [DeliveryAreaId]),
        CONSTRAINT [FK_CourierDeliveryArea_CourierProfile_CourierId] FOREIGN KEY ([CourierId]) REFERENCES [CourierProfile] ([UserId]),
        CONSTRAINT [FK_CourierDeliveryArea_DeliveryArea_DeliveryAreaId] FOREIGN KEY ([DeliveryAreaId]) REFERENCES [DeliveryArea] ([Id])
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261114175432_AddInitialBusinessSchema'
)
BEGIN
    CREATE TABLE [CustomerAddress] (
        [Id] int NOT NULL IDENTITY,
        [UserId] nvarchar(450) NOT NULL,
        [DeliveryAreaId] int NOT NULL,
        [Label] nvarchar(100) NOT NULL,
        [StreetAddress] nvarchar(300) NOT NULL,
        [Building] nvarchar(30) NULL,
        [Entrance] nvarchar(30) NULL,
        [Floor] nvarchar(30) NULL,
        [Apartment] nvarchar(30) NULL,
        [DeliveryInstructions] nvarchar(1000) NULL,
        [IsDefault] bit NOT NULL,
        CONSTRAINT [PK_CustomerAddress] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_CustomerAddress_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [AspNetUsers] ([Id]),
        CONSTRAINT [FK_CustomerAddress_DeliveryArea_DeliveryAreaId] FOREIGN KEY ([DeliveryAreaId]) REFERENCES [DeliveryArea] ([Id])
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261114175432_AddInitialBusinessSchema'
)
BEGIN
    CREATE TABLE [Cart] (
        [Id] int NOT NULL IDENTITY,
        [UserId] nvarchar(450) NOT NULL,
        [RestaurantId] int NULL,
        [UpdatedAtUtc] datetime2 NOT NULL,
        [RowVersion] rowversion NOT NULL,
        CONSTRAINT [PK_Cart] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_Cart_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [AspNetUsers] ([Id]),
        CONSTRAINT [FK_Cart_Restaurant_RestaurantId] FOREIGN KEY ([RestaurantId]) REFERENCES [Restaurant] ([Id])
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261114175432_AddInitialBusinessSchema'
)
BEGIN
    CREATE TABLE [Menu] (
        [Id] int NOT NULL IDENTITY,
        [RestaurantId] int NOT NULL,
        [Name] nvarchar(150) NOT NULL,
        [Description] nvarchar(1000) NULL,
        [IsActive] bit NOT NULL,
        [DisplayOrder] int NOT NULL,
        CONSTRAINT [PK_Menu] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_Menu_Restaurant_RestaurantId] FOREIGN KEY ([RestaurantId]) REFERENCES [Restaurant] ([Id])
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261114175432_AddInitialBusinessSchema'
)
BEGIN
    CREATE TABLE [Order] (
        [Id] int NOT NULL IDENTITY,
        [OrderNumber] nvarchar(40) NOT NULL,
        [CustomerId] nvarchar(450) NOT NULL,
        [RestaurantId] int NOT NULL,
        [Status] int NOT NULL,
        [Subtotal] decimal(18,2) NOT NULL,
        [DeliveryFee] decimal(18,2) NOT NULL,
        [TotalAmount] decimal(18,2) NOT NULL,
        [CurrencyCode] nvarchar(3) NOT NULL,
        [CustomerNote] nvarchar(1000) NULL,
        [CreatedAtUtc] datetime2 NOT NULL,
        [EstimatedReadyAtUtc] datetime2 NULL,
        [CheckoutToken] uniqueidentifier NOT NULL,
        [CheckoutRequestHash] nvarchar(64) NOT NULL,
        [RestaurantNameSnapshot] nvarchar(150) NOT NULL,
        [PickupCitySnapshot] nvarchar(100) NOT NULL,
        [PickupAddressSnapshot] nvarchar(300) NOT NULL,
        [PickupPhoneSnapshot] nvarchar(30) NOT NULL,
        [RowVersion] rowversion NOT NULL,
        CONSTRAINT [PK_Order] PRIMARY KEY ([Id]),
        CONSTRAINT [CK_Order_Amounts] CHECK ([Subtotal] >= 0 AND [DeliveryFee] >= 0 AND [TotalAmount] = [Subtotal] + [DeliveryFee]),
        CONSTRAINT [CK_Order_Currency] CHECK (LEN([CurrencyCode]) = 3 AND [CurrencyCode] COLLATE Latin1_General_100_BIN2 NOT LIKE '%[^A-Z]%'),
        CONSTRAINT [CK_Order_Hash] CHECK (LEN([CheckoutRequestHash]) = 64 AND [CheckoutRequestHash] COLLATE Latin1_General_100_BIN2 NOT LIKE '%[^0-9A-Fa-f]%'),
        CONSTRAINT [CK_Order_Status] CHECK ([Status] BETWEEN 0 AND 8),
        CONSTRAINT [FK_Order_AspNetUsers_CustomerId] FOREIGN KEY ([CustomerId]) REFERENCES [AspNetUsers] ([Id]),
        CONSTRAINT [FK_Order_Restaurant_RestaurantId] FOREIGN KEY ([RestaurantId]) REFERENCES [Restaurant] ([Id])
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261114175432_AddInitialBusinessSchema'
)
BEGIN
    CREATE TABLE [RestaurantApprovalHistory] (
        [Id] int NOT NULL IDENTITY,
        [RestaurantId] int NOT NULL,
        [FromStatus] int NULL,
        [ToStatus] int NOT NULL,
        [ChangedByUserId] nvarchar(450) NOT NULL,
        [ChangedAtUtc] datetime2 NOT NULL,
        [Reason] nvarchar(1000) NOT NULL,
        CONSTRAINT [PK_RestaurantApprovalHistory] PRIMARY KEY ([Id]),
        CONSTRAINT [CK_RestaurantApprovalHistory_FromStatus] CHECK ([FromStatus] IS NULL OR [FromStatus] BETWEEN 0 AND 2),
        CONSTRAINT [CK_RestaurantApprovalHistory_ToStatus] CHECK ([ToStatus] BETWEEN 0 AND 2),
        CONSTRAINT [FK_RestaurantApprovalHistory_AspNetUsers_ChangedByUserId] FOREIGN KEY ([ChangedByUserId]) REFERENCES [AspNetUsers] ([Id]),
        CONSTRAINT [FK_RestaurantApprovalHistory_Restaurant_RestaurantId] FOREIGN KEY ([RestaurantId]) REFERENCES [Restaurant] ([Id])
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261114175432_AddInitialBusinessSchema'
)
BEGIN
    CREATE TABLE [RestaurantCuisineType] (
        [RestaurantId] int NOT NULL,
        [CuisineTypeId] int NOT NULL,
        CONSTRAINT [PK_RestaurantCuisineType] PRIMARY KEY ([RestaurantId], [CuisineTypeId]),
        CONSTRAINT [FK_RestaurantCuisineType_CuisineType_CuisineTypeId] FOREIGN KEY ([CuisineTypeId]) REFERENCES [CuisineType] ([Id]),
        CONSTRAINT [FK_RestaurantCuisineType_Restaurant_RestaurantId] FOREIGN KEY ([RestaurantId]) REFERENCES [Restaurant] ([Id])
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261114175432_AddInitialBusinessSchema'
)
BEGIN
    CREATE TABLE [RestaurantDeliveryArea] (
        [RestaurantId] int NOT NULL,
        [DeliveryAreaId] int NOT NULL,
        [DeliveryFee] decimal(18,2) NOT NULL,
        [IsActive] bit NOT NULL,
        CONSTRAINT [PK_RestaurantDeliveryArea] PRIMARY KEY ([RestaurantId], [DeliveryAreaId]),
        CONSTRAINT [CK_RestaurantDeliveryArea_Fee] CHECK ([DeliveryFee] >= 0),
        CONSTRAINT [FK_RestaurantDeliveryArea_DeliveryArea_DeliveryAreaId] FOREIGN KEY ([DeliveryAreaId]) REFERENCES [DeliveryArea] ([Id]),
        CONSTRAINT [FK_RestaurantDeliveryArea_Restaurant_RestaurantId] FOREIGN KEY ([RestaurantId]) REFERENCES [Restaurant] ([Id])
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261114175432_AddInitialBusinessSchema'
)
BEGIN
    CREATE TABLE [RestaurantScheduleException] (
        [Id] int NOT NULL IDENTITY,
        [RestaurantId] int NOT NULL,
        [Date] date NOT NULL,
        [IsClosed] bit NOT NULL,
        CONSTRAINT [PK_RestaurantScheduleException] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_RestaurantScheduleException_Restaurant_RestaurantId] FOREIGN KEY ([RestaurantId]) REFERENCES [Restaurant] ([Id])
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261114175432_AddInitialBusinessSchema'
)
BEGIN
    CREATE TABLE [RestaurantStaff] (
        [RestaurantId] int NOT NULL,
        [UserId] nvarchar(450) NOT NULL,
        [StaffRole] int NOT NULL,
        [IsActive] bit NOT NULL,
        [CreatedAtUtc] datetime2 NOT NULL,
        CONSTRAINT [PK_RestaurantStaff] PRIMARY KEY NONCLUSTERED ([RestaurantId], [UserId]),
        CONSTRAINT [CK_RestaurantStaff_StaffRole] CHECK ([StaffRole] BETWEEN 0 AND 1),
        CONSTRAINT [FK_RestaurantStaff_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [AspNetUsers] ([Id]),
        CONSTRAINT [FK_RestaurantStaff_Restaurant_RestaurantId] FOREIGN KEY ([RestaurantId]) REFERENCES [Restaurant] ([Id])
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261114175432_AddInitialBusinessSchema'
)
BEGIN
    CREATE TABLE [RestaurantWorkingPeriod] (
        [Id] int NOT NULL IDENTITY,
        [RestaurantId] int NOT NULL,
        [DayOfWeek] int NOT NULL,
        [OpensAt] time NOT NULL,
        [ClosesAt] time NOT NULL,
        [ClosesNextDay] bit NOT NULL,
        CONSTRAINT [PK_RestaurantWorkingPeriod] PRIMARY KEY ([Id]),
        CONSTRAINT [CK_RestaurantWorkingPeriod_DayOfWeek] CHECK ([DayOfWeek] BETWEEN 0 AND 6),
        CONSTRAINT [CK_RestaurantWorkingPeriod_Period] CHECK (([ClosesNextDay] = 0 AND [ClosesAt] > [OpensAt]) OR ([ClosesNextDay] = 1 AND [ClosesAt] < [OpensAt])),
        CONSTRAINT [FK_RestaurantWorkingPeriod_Restaurant_RestaurantId] FOREIGN KEY ([RestaurantId]) REFERENCES [Restaurant] ([Id])
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261114175432_AddInitialBusinessSchema'
)
BEGIN
    CREATE TABLE [MenuCategory] (
        [Id] int NOT NULL IDENTITY,
        [MenuId] int NOT NULL,
        [Name] nvarchar(150) NOT NULL,
        [NormalizedName] nvarchar(150) NOT NULL,
        [IsActive] bit NOT NULL,
        [DisplayOrder] int NOT NULL,
        CONSTRAINT [PK_MenuCategory] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_MenuCategory_Menu_MenuId] FOREIGN KEY ([MenuId]) REFERENCES [Menu] ([Id])
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261114175432_AddInitialBusinessSchema'
)
BEGIN
    CREATE TABLE [Delivery] (
        [Id] int NOT NULL IDENTITY,
        [OrderId] int NOT NULL,
        [CourierId] nvarchar(450) NULL,
        [DeliveryAreaId] int NOT NULL,
        [Status] int NOT NULL,
        [CreatedAtUtc] datetime2 NOT NULL,
        [AssignedAtUtc] datetime2 NULL,
        [PickedUpAtUtc] datetime2 NULL,
        [DeliveredAtUtc] datetime2 NULL,
        [RowVersion] rowversion NOT NULL,
        CONSTRAINT [PK_Delivery] PRIMARY KEY ([Id]),
        CONSTRAINT [CK_Delivery_Courier] CHECK (([Status] = 0 AND [CourierId] IS NULL) OR ([Status] IN (1, 2, 3, 4) AND [CourierId] IS NOT NULL) OR [Status] = 5),
        CONSTRAINT [CK_Delivery_Status] CHECK ([Status] BETWEEN 0 AND 5),
        CONSTRAINT [FK_Delivery_CourierProfile_CourierId] FOREIGN KEY ([CourierId]) REFERENCES [CourierProfile] ([UserId]),
        CONSTRAINT [FK_Delivery_DeliveryArea_DeliveryAreaId] FOREIGN KEY ([DeliveryAreaId]) REFERENCES [DeliveryArea] ([Id]),
        CONSTRAINT [FK_Delivery_Order_OrderId] FOREIGN KEY ([OrderId]) REFERENCES [Order] ([Id])
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261114175432_AddInitialBusinessSchema'
)
BEGIN
    CREATE TABLE [OrderDeliveryAddress] (
        [OrderId] int NOT NULL,
        [DeliveryAreaId] int NOT NULL,
        [RecipientName] nvarchar(200) NOT NULL,
        [PhoneNumber] nvarchar(30) NOT NULL,
        [CityName] nvarchar(100) NOT NULL,
        [AreaName] nvarchar(100) NOT NULL,
        [StreetAddress] nvarchar(300) NOT NULL,
        [Building] nvarchar(30) NULL,
        [Entrance] nvarchar(30) NULL,
        [Floor] nvarchar(30) NULL,
        [Apartment] nvarchar(30) NULL,
        [DeliveryInstructions] nvarchar(1000) NULL,
        CONSTRAINT [PK_OrderDeliveryAddress] PRIMARY KEY ([OrderId]),
        CONSTRAINT [FK_OrderDeliveryAddress_DeliveryArea_DeliveryAreaId] FOREIGN KEY ([DeliveryAreaId]) REFERENCES [DeliveryArea] ([Id]),
        CONSTRAINT [FK_OrderDeliveryAddress_Order_OrderId] FOREIGN KEY ([OrderId]) REFERENCES [Order] ([Id])
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261114175432_AddInitialBusinessSchema'
)
BEGIN
    CREATE TABLE [OrderStatusHistory] (
        [Id] int NOT NULL IDENTITY,
        [OrderId] int NOT NULL,
        [FromStatus] int NULL,
        [ToStatus] int NOT NULL,
        [ChangedByUserId] nvarchar(450) NULL,
        [ChangedAtUtc] datetime2 NOT NULL,
        [Reason] nvarchar(1000) NOT NULL,
        CONSTRAINT [PK_OrderStatusHistory] PRIMARY KEY ([Id]),
        CONSTRAINT [CK_OrderStatusHistory_FromStatus] CHECK ([FromStatus] IS NULL OR [FromStatus] BETWEEN 0 AND 8),
        CONSTRAINT [CK_OrderStatusHistory_ToStatus] CHECK ([ToStatus] BETWEEN 0 AND 8),
        CONSTRAINT [FK_OrderStatusHistory_AspNetUsers_ChangedByUserId] FOREIGN KEY ([ChangedByUserId]) REFERENCES [AspNetUsers] ([Id]),
        CONSTRAINT [FK_OrderStatusHistory_Order_OrderId] FOREIGN KEY ([OrderId]) REFERENCES [Order] ([Id])
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261114175432_AddInitialBusinessSchema'
)
BEGIN
    CREATE TABLE [Payment] (
        [Id] int NOT NULL IDENTITY,
        [OrderId] int NOT NULL,
        [Method] int NOT NULL,
        [Status] int NOT NULL,
        [Amount] decimal(18,2) NOT NULL,
        [CurrencyCode] nvarchar(3) NOT NULL,
        [CollectedAtUtc] datetime2 NULL,
        [CollectedByUserId] nvarchar(450) NULL,
        [RowVersion] rowversion NOT NULL,
        CONSTRAINT [PK_Payment] PRIMARY KEY ([Id]),
        CONSTRAINT [CK_Payment_Amount] CHECK ([Amount] >= 0),
        CONSTRAINT [CK_Payment_Currency] CHECK (LEN([CurrencyCode]) = 3 AND [CurrencyCode] COLLATE Latin1_General_100_BIN2 NOT LIKE '%[^A-Z]%'),
        CONSTRAINT [CK_Payment_Method] CHECK ([Method] BETWEEN 0 AND 0),
        CONSTRAINT [CK_Payment_Status] CHECK ([Status] BETWEEN 0 AND 3),
        CONSTRAINT [FK_Payment_AspNetUsers_CollectedByUserId] FOREIGN KEY ([CollectedByUserId]) REFERENCES [AspNetUsers] ([Id]),
        CONSTRAINT [FK_Payment_Order_OrderId] FOREIGN KEY ([OrderId]) REFERENCES [Order] ([Id])
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261114175432_AddInitialBusinessSchema'
)
BEGIN
    CREATE TABLE [Review] (
        [Id] int NOT NULL IDENTITY,
        [OrderId] int NOT NULL,
        [RestaurantRating] int NOT NULL,
        [DeliveryRating] int NULL,
        [Comment] nvarchar(2000) NULL,
        [CreatedAtUtc] datetime2 NOT NULL,
        [UpdatedAtUtc] datetime2 NULL,
        [ModerationStatus] int NOT NULL,
        [RowVersion] rowversion NOT NULL,
        CONSTRAINT [PK_Review] PRIMARY KEY ([Id]),
        CONSTRAINT [CK_Review_ModerationStatus] CHECK ([ModerationStatus] BETWEEN 0 AND 1),
        CONSTRAINT [CK_Review_Ratings] CHECK ([RestaurantRating] BETWEEN 1 AND 5 AND ([DeliveryRating] IS NULL OR [DeliveryRating] BETWEEN 1 AND 5)),
        CONSTRAINT [FK_Review_Order_OrderId] FOREIGN KEY ([OrderId]) REFERENCES [Order] ([Id])
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261114175432_AddInitialBusinessSchema'
)
BEGIN
    CREATE TABLE [SupportTicket] (
        [Id] int NOT NULL IDENTITY,
        [CreatedByUserId] nvarchar(450) NOT NULL,
        [OrderId] int NULL,
        [Category] int NOT NULL,
        [Subject] nvarchar(200) NOT NULL,
        [Status] int NOT NULL,
        [AssignedAdminId] nvarchar(450) NULL,
        [CreatedAtUtc] datetime2 NOT NULL,
        [UpdatedAtUtc] datetime2 NOT NULL,
        [RowVersion] rowversion NOT NULL,
        CONSTRAINT [PK_SupportTicket] PRIMARY KEY ([Id]),
        CONSTRAINT [CK_SupportTicket_Category] CHECK ([Category] BETWEEN 0 AND 3),
        CONSTRAINT [CK_SupportTicket_Status] CHECK ([Status] BETWEEN 0 AND 3),
        CONSTRAINT [FK_SupportTicket_AspNetUsers_AssignedAdminId] FOREIGN KEY ([AssignedAdminId]) REFERENCES [AspNetUsers] ([Id]),
        CONSTRAINT [FK_SupportTicket_AspNetUsers_CreatedByUserId] FOREIGN KEY ([CreatedByUserId]) REFERENCES [AspNetUsers] ([Id]),
        CONSTRAINT [FK_SupportTicket_Order_OrderId] FOREIGN KEY ([OrderId]) REFERENCES [Order] ([Id])
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261114175432_AddInitialBusinessSchema'
)
BEGIN
    CREATE TABLE [RestaurantScheduleExceptionPeriod] (
        [Id] int NOT NULL IDENTITY,
        [RestaurantScheduleExceptionId] int NOT NULL,
        [OpensAt] time NOT NULL,
        [ClosesAt] time NOT NULL,
        [ClosesNextDay] bit NOT NULL,
        CONSTRAINT [PK_RestaurantScheduleExceptionPeriod] PRIMARY KEY ([Id]),
        CONSTRAINT [CK_RestaurantScheduleExceptionPeriod_Period] CHECK (([ClosesNextDay] = 0 AND [ClosesAt] > [OpensAt]) OR ([ClosesNextDay] = 1 AND [ClosesAt] < [OpensAt])),
        CONSTRAINT [FK_RestaurantScheduleExceptionPeriod_RestaurantScheduleException_RestaurantScheduleExceptionId] FOREIGN KEY ([RestaurantScheduleExceptionId]) REFERENCES [RestaurantScheduleException] ([Id])
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261114175432_AddInitialBusinessSchema'
)
BEGIN
    CREATE TABLE [Dish] (
        [Id] int NOT NULL IDENTITY,
        [MenuCategoryId] int NOT NULL,
        [Name] nvarchar(150) NOT NULL,
        [Description] nvarchar(2000) NOT NULL,
        [ImagePath] nvarchar(500) NULL,
        [IsActive] bit NOT NULL,
        [IsAvailable] bit NOT NULL,
        [DisplayOrder] int NOT NULL,
        [CreatedAtUtc] datetime2 NOT NULL,
        [RowVersion] rowversion NOT NULL,
        CONSTRAINT [PK_Dish] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_Dish_MenuCategory_MenuCategoryId] FOREIGN KEY ([MenuCategoryId]) REFERENCES [MenuCategory] ([Id])
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261114175432_AddInitialBusinessSchema'
)
BEGIN
    CREATE TABLE [DeliveryAssignmentHistory] (
        [Id] int NOT NULL IDENTITY,
        [DeliveryId] int NOT NULL,
        [CourierId] nvarchar(450) NOT NULL,
        [AssignedAtUtc] datetime2 NOT NULL,
        [ReleasedAtUtc] datetime2 NULL,
        [ReleaseReason] nvarchar(1000) NULL,
        CONSTRAINT [PK_DeliveryAssignmentHistory] PRIMARY KEY ([Id]),
        CONSTRAINT [CK_DeliveryAssignmentHistory_Release] CHECK ([ReleasedAtUtc] IS NULL OR [ReleasedAtUtc] >= [AssignedAtUtc]),
        CONSTRAINT [FK_DeliveryAssignmentHistory_CourierProfile_CourierId] FOREIGN KEY ([CourierId]) REFERENCES [CourierProfile] ([UserId]),
        CONSTRAINT [FK_DeliveryAssignmentHistory_Delivery_DeliveryId] FOREIGN KEY ([DeliveryId]) REFERENCES [Delivery] ([Id])
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261114175432_AddInitialBusinessSchema'
)
BEGIN
    CREATE TABLE [DeliveryStatusHistory] (
        [Id] int NOT NULL IDENTITY,
        [DeliveryId] int NOT NULL,
        [FromStatus] int NULL,
        [ToStatus] int NOT NULL,
        [ChangedByUserId] nvarchar(450) NULL,
        [ChangedAtUtc] datetime2 NOT NULL,
        [Reason] nvarchar(1000) NOT NULL,
        CONSTRAINT [PK_DeliveryStatusHistory] PRIMARY KEY ([Id]),
        CONSTRAINT [CK_DeliveryStatusHistory_FromStatus] CHECK ([FromStatus] IS NULL OR [FromStatus] BETWEEN 0 AND 5),
        CONSTRAINT [CK_DeliveryStatusHistory_ToStatus] CHECK ([ToStatus] BETWEEN 0 AND 5),
        CONSTRAINT [FK_DeliveryStatusHistory_AspNetUsers_ChangedByUserId] FOREIGN KEY ([ChangedByUserId]) REFERENCES [AspNetUsers] ([Id]),
        CONSTRAINT [FK_DeliveryStatusHistory_Delivery_DeliveryId] FOREIGN KEY ([DeliveryId]) REFERENCES [Delivery] ([Id])
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261114175432_AddInitialBusinessSchema'
)
BEGIN
    CREATE TABLE [ReviewReply] (
        [ReviewId] int NOT NULL,
        [AuthorUserId] nvarchar(450) NOT NULL,
        [Text] nvarchar(2000) NOT NULL,
        [CreatedAtUtc] datetime2 NOT NULL,
        [UpdatedAtUtc] datetime2 NULL,
        [ModerationStatus] int NOT NULL,
        CONSTRAINT [PK_ReviewReply] PRIMARY KEY ([ReviewId]),
        CONSTRAINT [CK_ReviewReply_ModerationStatus] CHECK ([ModerationStatus] BETWEEN 0 AND 1),
        CONSTRAINT [FK_ReviewReply_AspNetUsers_AuthorUserId] FOREIGN KEY ([AuthorUserId]) REFERENCES [AspNetUsers] ([Id]),
        CONSTRAINT [FK_ReviewReply_Review_ReviewId] FOREIGN KEY ([ReviewId]) REFERENCES [Review] ([Id])
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261114175432_AddInitialBusinessSchema'
)
BEGIN
    CREATE TABLE [Notification] (
        [Id] int NOT NULL IDENTITY,
        [RecipientUserId] nvarchar(450) NOT NULL,
        [Type] int NOT NULL,
        [Title] nvarchar(200) NOT NULL,
        [Message] nvarchar(2000) NOT NULL,
        [OrderId] int NULL,
        [SupportTicketId] int NULL,
        [CreatedAtUtc] datetime2 NOT NULL,
        [ReadAtUtc] datetime2 NULL,
        CONSTRAINT [PK_Notification] PRIMARY KEY ([Id]),
        CONSTRAINT [CK_Notification_Type] CHECK ([Type] BETWEEN 0 AND 4),
        CONSTRAINT [FK_Notification_AspNetUsers_RecipientUserId] FOREIGN KEY ([RecipientUserId]) REFERENCES [AspNetUsers] ([Id]),
        CONSTRAINT [FK_Notification_Order_OrderId] FOREIGN KEY ([OrderId]) REFERENCES [Order] ([Id]),
        CONSTRAINT [FK_Notification_SupportTicket_SupportTicketId] FOREIGN KEY ([SupportTicketId]) REFERENCES [SupportTicket] ([Id])
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261114175432_AddInitialBusinessSchema'
)
BEGIN
    CREATE TABLE [SupportTicketMessage] (
        [Id] int NOT NULL IDENTITY,
        [SupportTicketId] int NOT NULL,
        [AuthorUserId] nvarchar(450) NOT NULL,
        [Text] nvarchar(4000) NOT NULL,
        [CreatedAtUtc] datetime2 NOT NULL,
        CONSTRAINT [PK_SupportTicketMessage] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_SupportTicketMessage_AspNetUsers_AuthorUserId] FOREIGN KEY ([AuthorUserId]) REFERENCES [AspNetUsers] ([Id]),
        CONSTRAINT [FK_SupportTicketMessage_SupportTicket_SupportTicketId] FOREIGN KEY ([SupportTicketId]) REFERENCES [SupportTicket] ([Id])
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261114175432_AddInitialBusinessSchema'
)
BEGIN
    CREATE TABLE [DishAllergen] (
        [DishId] int NOT NULL,
        [AllergenId] int NOT NULL,
        CONSTRAINT [PK_DishAllergen] PRIMARY KEY ([DishId], [AllergenId]),
        CONSTRAINT [FK_DishAllergen_Allergen_AllergenId] FOREIGN KEY ([AllergenId]) REFERENCES [Allergen] ([Id]),
        CONSTRAINT [FK_DishAllergen_Dish_DishId] FOREIGN KEY ([DishId]) REFERENCES [Dish] ([Id])
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261114175432_AddInitialBusinessSchema'
)
BEGIN
    CREATE TABLE [DishOptionGroup] (
        [Id] int NOT NULL IDENTITY,
        [DishId] int NOT NULL,
        [Name] nvarchar(150) NOT NULL,
        [MinSelections] int NOT NULL,
        [MaxSelections] int NOT NULL,
        [IsActive] bit NOT NULL,
        [DisplayOrder] int NOT NULL,
        CONSTRAINT [PK_DishOptionGroup] PRIMARY KEY ([Id]),
        CONSTRAINT [CK_DishOptionGroup_Selections] CHECK ([MinSelections] >= 0 AND [MinSelections] <= [MaxSelections] AND [MaxSelections] > 0),
        CONSTRAINT [FK_DishOptionGroup_Dish_DishId] FOREIGN KEY ([DishId]) REFERENCES [Dish] ([Id])
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261114175432_AddInitialBusinessSchema'
)
BEGIN
    CREATE TABLE [DishVariant] (
        [Id] int NOT NULL IDENTITY,
        [DishId] int NOT NULL,
        [Name] nvarchar(100) NOT NULL,
        [NormalizedName] nvarchar(100) NOT NULL,
        [Price] decimal(18,2) NOT NULL,
        [PortionQuantity] decimal(18,3) NULL,
        [PortionUnit] int NULL,
        [IsActive] bit NOT NULL,
        [IsAvailable] bit NOT NULL,
        [DisplayOrder] int NOT NULL,
        CONSTRAINT [PK_DishVariant] PRIMARY KEY ([Id]),
        CONSTRAINT [CK_DishVariant_Portion] CHECK (([PortionQuantity] IS NULL AND [PortionUnit] IS NULL) OR ([PortionQuantity] IS NOT NULL AND [PortionUnit] IS NOT NULL AND [PortionQuantity] > 0)),
        CONSTRAINT [CK_DishVariant_PortionUnit] CHECK ([PortionUnit] IS NULL OR [PortionUnit] BETWEEN 0 AND 2),
        CONSTRAINT [CK_DishVariant_Price] CHECK ([Price] > 0),
        CONSTRAINT [FK_DishVariant_Dish_DishId] FOREIGN KEY ([DishId]) REFERENCES [Dish] ([Id])
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261114175432_AddInitialBusinessSchema'
)
BEGIN
    CREATE TABLE [DishOption] (
        [Id] int NOT NULL IDENTITY,
        [DishOptionGroupId] int NOT NULL,
        [Name] nvarchar(150) NOT NULL,
        [AdditionalPrice] decimal(18,2) NOT NULL,
        [IsActive] bit NOT NULL,
        [IsAvailable] bit NOT NULL,
        [DisplayOrder] int NOT NULL,
        CONSTRAINT [PK_DishOption] PRIMARY KEY ([Id]),
        CONSTRAINT [CK_DishOption_Price] CHECK ([AdditionalPrice] >= 0),
        CONSTRAINT [FK_DishOption_DishOptionGroup_DishOptionGroupId] FOREIGN KEY ([DishOptionGroupId]) REFERENCES [DishOptionGroup] ([Id])
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261114175432_AddInitialBusinessSchema'
)
BEGIN
    CREATE TABLE [CartItem] (
        [Id] int NOT NULL IDENTITY,
        [CartId] int NOT NULL,
        [DishVariantId] int NOT NULL,
        [Quantity] int NOT NULL,
        [SpecialInstructions] nvarchar(1000) NULL,
        CONSTRAINT [PK_CartItem] PRIMARY KEY ([Id]),
        CONSTRAINT [CK_CartItem_Quantity] CHECK ([Quantity] BETWEEN 1 AND 99),
        CONSTRAINT [FK_CartItem_Cart_CartId] FOREIGN KEY ([CartId]) REFERENCES [Cart] ([Id]),
        CONSTRAINT [FK_CartItem_DishVariant_DishVariantId] FOREIGN KEY ([DishVariantId]) REFERENCES [DishVariant] ([Id])
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261114175432_AddInitialBusinessSchema'
)
BEGIN
    CREATE TABLE [OrderItem] (
        [Id] int NOT NULL IDENTITY,
        [OrderId] int NOT NULL,
        [DishVariantId] int NOT NULL,
        [DishName] nvarchar(150) NOT NULL,
        [VariantName] nvarchar(100) NOT NULL,
        [BaseUnitPrice] decimal(18,2) NOT NULL,
        [OptionsUnitPrice] decimal(18,2) NOT NULL,
        [Quantity] int NOT NULL,
        [LineTotal] decimal(18,2) NOT NULL,
        [SpecialInstructions] nvarchar(1000) NULL,
        CONSTRAINT [PK_OrderItem] PRIMARY KEY ([Id]),
        CONSTRAINT [CK_OrderItem_Amounts] CHECK ([BaseUnitPrice] > 0 AND [OptionsUnitPrice] >= 0 AND [LineTotal] = ([BaseUnitPrice] + [OptionsUnitPrice]) * [Quantity]),
        CONSTRAINT [CK_OrderItem_Quantity] CHECK ([Quantity] BETWEEN 1 AND 99),
        CONSTRAINT [FK_OrderItem_DishVariant_DishVariantId] FOREIGN KEY ([DishVariantId]) REFERENCES [DishVariant] ([Id]),
        CONSTRAINT [FK_OrderItem_Order_OrderId] FOREIGN KEY ([OrderId]) REFERENCES [Order] ([Id])
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261114175432_AddInitialBusinessSchema'
)
BEGIN
    CREATE TABLE [DishOptionAllergen] (
        [DishOptionId] int NOT NULL,
        [AllergenId] int NOT NULL,
        CONSTRAINT [PK_DishOptionAllergen] PRIMARY KEY ([DishOptionId], [AllergenId]),
        CONSTRAINT [FK_DishOptionAllergen_Allergen_AllergenId] FOREIGN KEY ([AllergenId]) REFERENCES [Allergen] ([Id]),
        CONSTRAINT [FK_DishOptionAllergen_DishOption_DishOptionId] FOREIGN KEY ([DishOptionId]) REFERENCES [DishOption] ([Id])
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261114175432_AddInitialBusinessSchema'
)
BEGIN
    CREATE TABLE [CartItemOption] (
        [CartItemId] int NOT NULL,
        [DishOptionId] int NOT NULL,
        CONSTRAINT [PK_CartItemOption] PRIMARY KEY ([CartItemId], [DishOptionId]),
        CONSTRAINT [FK_CartItemOption_CartItem_CartItemId] FOREIGN KEY ([CartItemId]) REFERENCES [CartItem] ([Id]),
        CONSTRAINT [FK_CartItemOption_DishOption_DishOptionId] FOREIGN KEY ([DishOptionId]) REFERENCES [DishOption] ([Id])
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261114175432_AddInitialBusinessSchema'
)
BEGIN
    CREATE TABLE [OrderItemOption] (
        [Id] int NOT NULL IDENTITY,
        [OrderItemId] int NOT NULL,
        [DishOptionId] int NOT NULL,
        [NameSnapshot] nvarchar(150) NOT NULL,
        [AdditionalPrice] decimal(18,2) NOT NULL,
        CONSTRAINT [PK_OrderItemOption] PRIMARY KEY ([Id]),
        CONSTRAINT [CK_OrderItemOption_Price] CHECK ([AdditionalPrice] >= 0),
        CONSTRAINT [FK_OrderItemOption_DishOption_DishOptionId] FOREIGN KEY ([DishOptionId]) REFERENCES [DishOption] ([Id]),
        CONSTRAINT [FK_OrderItemOption_OrderItem_OrderItemId] FOREIGN KEY ([OrderItemId]) REFERENCES [OrderItem] ([Id])
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261114175432_AddInitialBusinessSchema'
)
BEGIN
    EXEC(N'CREATE UNIQUE INDEX [EmailIndex] ON [AspNetUsers] ([NormalizedEmail]) WHERE [NormalizedEmail] IS NOT NULL AND [NormalizedEmail] <> N''''');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261114175432_AddInitialBusinessSchema'
)
BEGIN
    CREATE UNIQUE INDEX [IX_Allergen_Code] ON [Allergen] ([Code]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261114175432_AddInitialBusinessSchema'
)
BEGIN
    CREATE INDEX [IX_AuditLog_ActorUserId_CreatedAtUtc] ON [AuditLog] ([ActorUserId], [CreatedAtUtc]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261114175432_AddInitialBusinessSchema'
)
BEGIN
    CREATE INDEX [IX_AuditLog_EntityType_EntityId_CreatedAtUtc] ON [AuditLog] ([EntityType], [EntityId], [CreatedAtUtc]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261114175432_AddInitialBusinessSchema'
)
BEGIN
    CREATE INDEX [IX_Cart_RestaurantId] ON [Cart] ([RestaurantId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261114175432_AddInitialBusinessSchema'
)
BEGIN
    CREATE UNIQUE INDEX [IX_Cart_UserId] ON [Cart] ([UserId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261114175432_AddInitialBusinessSchema'
)
BEGIN
    CREATE INDEX [IX_CartItem_CartId] ON [CartItem] ([CartId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261114175432_AddInitialBusinessSchema'
)
BEGIN
    CREATE INDEX [IX_CartItem_DishVariantId] ON [CartItem] ([DishVariantId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261114175432_AddInitialBusinessSchema'
)
BEGIN
    CREATE INDEX [IX_CartItemOption_DishOptionId] ON [CartItemOption] ([DishOptionId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261114175432_AddInitialBusinessSchema'
)
BEGIN
    CREATE UNIQUE INDEX [IX_City_NormalizedName] ON [City] ([NormalizedName]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261114175432_AddInitialBusinessSchema'
)
BEGIN
    CREATE INDEX [IX_CourierApprovalHistory_ChangedByUserId] ON [CourierApprovalHistory] ([ChangedByUserId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261114175432_AddInitialBusinessSchema'
)
BEGIN
    CREATE INDEX [IX_CourierApprovalHistory_CourierId_ChangedAtUtc] ON [CourierApprovalHistory] ([CourierId], [ChangedAtUtc]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261114175432_AddInitialBusinessSchema'
)
BEGIN
    CREATE INDEX [IX_CourierDeliveryArea_DeliveryAreaId] ON [CourierDeliveryArea] ([DeliveryAreaId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261114175432_AddInitialBusinessSchema'
)
BEGIN
    CREATE UNIQUE INDEX [IX_CuisineType_NormalizedName] ON [CuisineType] ([NormalizedName]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261114175432_AddInitialBusinessSchema'
)
BEGIN
    CREATE INDEX [IX_CustomerAddress_DeliveryAreaId] ON [CustomerAddress] ([DeliveryAreaId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261114175432_AddInitialBusinessSchema'
)
BEGIN
    EXEC(N'CREATE UNIQUE INDEX [IX_CustomerAddress_UserId] ON [CustomerAddress] ([UserId]) WHERE [IsDefault] = 1');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261114175432_AddInitialBusinessSchema'
)
BEGIN
    EXEC(N'CREATE UNIQUE INDEX [IX_Delivery_CourierId] ON [Delivery] ([CourierId]) WHERE [Status] IN (1, 2)');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261114175432_AddInitialBusinessSchema'
)
BEGIN
    CREATE INDEX [IX_Delivery_DeliveryAreaId] ON [Delivery] ([DeliveryAreaId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261114175432_AddInitialBusinessSchema'
)
BEGIN
    CREATE UNIQUE INDEX [IX_Delivery_OrderId] ON [Delivery] ([OrderId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261114175432_AddInitialBusinessSchema'
)
BEGIN
    CREATE UNIQUE INDEX [IX_DeliveryArea_CityId_NormalizedName] ON [DeliveryArea] ([CityId], [NormalizedName]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261114175432_AddInitialBusinessSchema'
)
BEGIN
    CREATE INDEX [IX_DeliveryAssignmentHistory_CourierId] ON [DeliveryAssignmentHistory] ([CourierId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261114175432_AddInitialBusinessSchema'
)
BEGIN
    EXEC(N'CREATE UNIQUE INDEX [IX_DeliveryAssignmentHistory_DeliveryId] ON [DeliveryAssignmentHistory] ([DeliveryId]) WHERE [ReleasedAtUtc] IS NULL');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261114175432_AddInitialBusinessSchema'
)
BEGIN
    CREATE INDEX [IX_DeliveryStatusHistory_ChangedByUserId] ON [DeliveryStatusHistory] ([ChangedByUserId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261114175432_AddInitialBusinessSchema'
)
BEGIN
    CREATE INDEX [IX_DeliveryStatusHistory_DeliveryId_ChangedAtUtc] ON [DeliveryStatusHistory] ([DeliveryId], [ChangedAtUtc]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261114175432_AddInitialBusinessSchema'
)
BEGIN
    CREATE INDEX [IX_Dish_MenuCategoryId_DisplayOrder] ON [Dish] ([MenuCategoryId], [DisplayOrder]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261114175432_AddInitialBusinessSchema'
)
BEGIN
    CREATE INDEX [IX_DishAllergen_AllergenId] ON [DishAllergen] ([AllergenId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261114175432_AddInitialBusinessSchema'
)
BEGIN
    CREATE INDEX [IX_DishOption_DishOptionGroupId] ON [DishOption] ([DishOptionGroupId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261114175432_AddInitialBusinessSchema'
)
BEGIN
    CREATE INDEX [IX_DishOptionAllergen_AllergenId] ON [DishOptionAllergen] ([AllergenId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261114175432_AddInitialBusinessSchema'
)
BEGIN
    CREATE INDEX [IX_DishOptionGroup_DishId] ON [DishOptionGroup] ([DishId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261114175432_AddInitialBusinessSchema'
)
BEGIN
    CREATE UNIQUE INDEX [IX_DishVariant_DishId_NormalizedName] ON [DishVariant] ([DishId], [NormalizedName]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261114175432_AddInitialBusinessSchema'
)
BEGIN
    CREATE INDEX [IX_Menu_RestaurantId_DisplayOrder] ON [Menu] ([RestaurantId], [DisplayOrder]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261114175432_AddInitialBusinessSchema'
)
BEGIN
    CREATE UNIQUE INDEX [IX_MenuCategory_MenuId_NormalizedName] ON [MenuCategory] ([MenuId], [NormalizedName]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261114175432_AddInitialBusinessSchema'
)
BEGIN
    CREATE INDEX [IX_Notification_OrderId] ON [Notification] ([OrderId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261114175432_AddInitialBusinessSchema'
)
BEGIN
    CREATE INDEX [IX_Notification_RecipientUserId_CreatedAtUtc] ON [Notification] ([RecipientUserId], [CreatedAtUtc]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261114175432_AddInitialBusinessSchema'
)
BEGIN
    CREATE INDEX [IX_Notification_SupportTicketId] ON [Notification] ([SupportTicketId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261114175432_AddInitialBusinessSchema'
)
BEGIN
    EXEC(N'CREATE INDEX [IX_Notification_Unread] ON [Notification] ([RecipientUserId], [CreatedAtUtc]) WHERE [ReadAtUtc] IS NULL');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261114175432_AddInitialBusinessSchema'
)
BEGIN
    CREATE UNIQUE INDEX [IX_Order_CheckoutToken] ON [Order] ([CheckoutToken]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261114175432_AddInitialBusinessSchema'
)
BEGIN
    CREATE INDEX [IX_Order_CustomerId_CreatedAtUtc] ON [Order] ([CustomerId], [CreatedAtUtc]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261114175432_AddInitialBusinessSchema'
)
BEGIN
    CREATE UNIQUE INDEX [IX_Order_OrderNumber] ON [Order] ([OrderNumber]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261114175432_AddInitialBusinessSchema'
)
BEGIN
    CREATE INDEX [IX_Order_RestaurantId_Status_CreatedAtUtc] ON [Order] ([RestaurantId], [Status], [CreatedAtUtc]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261114175432_AddInitialBusinessSchema'
)
BEGIN
    CREATE INDEX [IX_OrderDeliveryAddress_DeliveryAreaId] ON [OrderDeliveryAddress] ([DeliveryAreaId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261114175432_AddInitialBusinessSchema'
)
BEGIN
    CREATE INDEX [IX_OrderItem_DishVariantId] ON [OrderItem] ([DishVariantId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261114175432_AddInitialBusinessSchema'
)
BEGIN
    CREATE INDEX [IX_OrderItem_OrderId] ON [OrderItem] ([OrderId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261114175432_AddInitialBusinessSchema'
)
BEGIN
    CREATE INDEX [IX_OrderItemOption_DishOptionId] ON [OrderItemOption] ([DishOptionId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261114175432_AddInitialBusinessSchema'
)
BEGIN
    CREATE UNIQUE INDEX [IX_OrderItemOption_OrderItemId_DishOptionId] ON [OrderItemOption] ([OrderItemId], [DishOptionId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261114175432_AddInitialBusinessSchema'
)
BEGIN
    CREATE INDEX [IX_OrderStatusHistory_ChangedByUserId] ON [OrderStatusHistory] ([ChangedByUserId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261114175432_AddInitialBusinessSchema'
)
BEGIN
    CREATE INDEX [IX_OrderStatusHistory_OrderId_ChangedAtUtc] ON [OrderStatusHistory] ([OrderId], [ChangedAtUtc]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261114175432_AddInitialBusinessSchema'
)
BEGIN
    CREATE INDEX [IX_Payment_CollectedByUserId] ON [Payment] ([CollectedByUserId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261114175432_AddInitialBusinessSchema'
)
BEGIN
    CREATE UNIQUE INDEX [IX_Payment_OrderId] ON [Payment] ([OrderId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261114175432_AddInitialBusinessSchema'
)
BEGIN
    CREATE INDEX [IX_Restaurant_CityId] ON [Restaurant] ([CityId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261114175432_AddInitialBusinessSchema'
)
BEGIN
    CREATE UNIQUE INDEX [IX_Restaurant_Slug] ON [Restaurant] ([Slug]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261114175432_AddInitialBusinessSchema'
)
BEGIN
    CREATE INDEX [IX_RestaurantApprovalHistory_ChangedByUserId] ON [RestaurantApprovalHistory] ([ChangedByUserId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261114175432_AddInitialBusinessSchema'
)
BEGIN
    CREATE INDEX [IX_RestaurantApprovalHistory_RestaurantId_ChangedAtUtc] ON [RestaurantApprovalHistory] ([RestaurantId], [ChangedAtUtc]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261114175432_AddInitialBusinessSchema'
)
BEGIN
    CREATE INDEX [IX_RestaurantCuisineType_CuisineTypeId] ON [RestaurantCuisineType] ([CuisineTypeId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261114175432_AddInitialBusinessSchema'
)
BEGIN
    CREATE INDEX [IX_RestaurantDeliveryArea_DeliveryAreaId] ON [RestaurantDeliveryArea] ([DeliveryAreaId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261114175432_AddInitialBusinessSchema'
)
BEGIN
    CREATE UNIQUE INDEX [IX_RestaurantScheduleException_RestaurantId_Date] ON [RestaurantScheduleException] ([RestaurantId], [Date]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261114175432_AddInitialBusinessSchema'
)
BEGIN
    CREATE INDEX [IX_RestaurantScheduleExceptionPeriod_RestaurantScheduleExceptionId] ON [RestaurantScheduleExceptionPeriod] ([RestaurantScheduleExceptionId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261114175432_AddInitialBusinessSchema'
)
BEGIN
    CREATE INDEX [IX_RestaurantStaff_UserId] ON [RestaurantStaff] ([UserId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261114175432_AddInitialBusinessSchema'
)
BEGIN
    CREATE INDEX [IX_RestaurantWorkingPeriod_RestaurantId_DayOfWeek] ON [RestaurantWorkingPeriod] ([RestaurantId], [DayOfWeek]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261114175432_AddInitialBusinessSchema'
)
BEGIN
    CREATE UNIQUE INDEX [IX_Review_OrderId] ON [Review] ([OrderId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261114175432_AddInitialBusinessSchema'
)
BEGIN
    CREATE INDEX [IX_ReviewReply_AuthorUserId] ON [ReviewReply] ([AuthorUserId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261114175432_AddInitialBusinessSchema'
)
BEGIN
    CREATE INDEX [IX_SupportTicket_AssignedAdminId] ON [SupportTicket] ([AssignedAdminId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261114175432_AddInitialBusinessSchema'
)
BEGIN
    CREATE INDEX [IX_SupportTicket_CreatedByUserId_CreatedAtUtc] ON [SupportTicket] ([CreatedByUserId], [CreatedAtUtc]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261114175432_AddInitialBusinessSchema'
)
BEGIN
    CREATE INDEX [IX_SupportTicket_OrderId] ON [SupportTicket] ([OrderId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261114175432_AddInitialBusinessSchema'
)
BEGIN
    CREATE INDEX [IX_SupportTicket_Status_UpdatedAtUtc] ON [SupportTicket] ([Status], [UpdatedAtUtc]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261114175432_AddInitialBusinessSchema'
)
BEGIN
    CREATE INDEX [IX_SupportTicketMessage_AuthorUserId] ON [SupportTicketMessage] ([AuthorUserId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261114175432_AddInitialBusinessSchema'
)
BEGIN
    CREATE INDEX [IX_SupportTicketMessage_SupportTicketId_CreatedAtUtc] ON [SupportTicketMessage] ([SupportTicketId], [CreatedAtUtc]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261114175432_AddInitialBusinessSchema'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20261114175432_AddInitialBusinessSchema', N'10.0.14');
END;

COMMIT;
GO
