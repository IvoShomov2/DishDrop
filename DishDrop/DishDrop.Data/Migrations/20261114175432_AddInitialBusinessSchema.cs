using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DishDrop.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddInitialBusinessSchema : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Stop before schema changes if a legacy database contains duplicate nonempty emails.
            migrationBuilder.Sql("""
                IF EXISTS (
                    SELECT [NormalizedEmail] FROM [AspNetUsers]
                    WHERE [NormalizedEmail] IS NOT NULL AND [NormalizedEmail] <> N''
                    GROUP BY [NormalizedEmail] HAVING COUNT(*) > 1
                )
                    THROW 51000, 'Duplicate nonempty NormalizedEmail values in AspNetUsers. Resolve explicitly before migration; no users were merged or deleted.', 1;
                """);

            migrationBuilder.DropIndex(
                name: "EmailIndex",
                table: "AspNetUsers");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAtUtc",
                table: "AspNetUsers",
                type: "datetime2",
                nullable: false,
                defaultValueSql: "SYSUTCDATETIME()");

            migrationBuilder.AddColumn<string>(
                name: "FirstName",
                table: "AspNetUsers",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "AspNetUsers",
                type: "bit",
                nullable: false,
                defaultValue: true);

            migrationBuilder.AddColumn<string>(
                name: "LastName",
                table: "AspNetUsers",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "Allergen",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Code = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Allergen", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AuditLog",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ActorUserId = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: true),
                    Action = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    EntityType = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    EntityId = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: false),
                    CreatedAtUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Reason = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    CorrelationId = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AuditLog", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AuditLog_AspNetUsers_ActorUserId",
                        column: x => x.ActorUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "City",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    NormalizedName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_City", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CourierProfile",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: false),
                    ApprovalStatus = table.Column<int>(type: "int", nullable: false),
                    VehicleType = table.Column<int>(type: "int", nullable: false),
                    IsAvailable = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAtUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ApprovedAtUtc = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CourierProfile", x => x.UserId);
                    table.CheckConstraint("CK_CourierProfile_ApprovalStatus", "[ApprovalStatus] BETWEEN 0 AND 2");
                    table.CheckConstraint("CK_CourierProfile_VehicleType", "[VehicleType] BETWEEN 0 AND 2");
                    table.ForeignKey(
                        name: "FK_CourierProfile_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "CuisineType",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    NormalizedName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CuisineType", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DeliveryArea",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CityId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    NormalizedName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DeliveryArea", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DeliveryArea_City_CityId",
                        column: x => x.CityId,
                        principalTable: "City",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Restaurant",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CityId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    Slug = table.Column<string>(type: "nvarchar(180)", maxLength: 180, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: false),
                    Address = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    ImagePath = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    ApprovalStatus = table.Column<int>(type: "int", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsAcceptingOrders = table.Column<bool>(type: "bit", nullable: false),
                    MinimumOrderAmount = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    EstimatedPreparationMinutes = table.Column<int>(type: "int", nullable: false),
                    CreatedAtUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Restaurant", x => x.Id);
                    table.CheckConstraint("CK_Restaurant_ApprovalStatus", "[ApprovalStatus] BETWEEN 0 AND 2");
                    table.CheckConstraint("CK_Restaurant_MinimumOrder", "[MinimumOrderAmount] >= 0");
                    table.CheckConstraint("CK_Restaurant_Preparation", "[EstimatedPreparationMinutes] > 0");
                    table.ForeignKey(
                        name: "FK_Restaurant_City_CityId",
                        column: x => x.CityId,
                        principalTable: "City",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "CourierApprovalHistory",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CourierId = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: false),
                    FromStatus = table.Column<int>(type: "int", nullable: true),
                    ToStatus = table.Column<int>(type: "int", nullable: false),
                    ChangedByUserId = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: false),
                    ChangedAtUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Reason = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CourierApprovalHistory", x => x.Id);
                    table.CheckConstraint("CK_CourierApprovalHistory_FromStatus", "[FromStatus] IS NULL OR [FromStatus] BETWEEN 0 AND 2");
                    table.CheckConstraint("CK_CourierApprovalHistory_ToStatus", "[ToStatus] BETWEEN 0 AND 2");
                    table.ForeignKey(
                        name: "FK_CourierApprovalHistory_AspNetUsers_ChangedByUserId",
                        column: x => x.ChangedByUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_CourierApprovalHistory_CourierProfile_CourierId",
                        column: x => x.CourierId,
                        principalTable: "CourierProfile",
                        principalColumn: "UserId");
                });

            migrationBuilder.CreateTable(
                name: "CourierDeliveryArea",
                columns: table => new
                {
                    CourierId = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: false),
                    DeliveryAreaId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CourierDeliveryArea", x => new { x.CourierId, x.DeliveryAreaId }).Annotation("SqlServer:Clustered", false);
                    table.ForeignKey(
                        name: "FK_CourierDeliveryArea_CourierProfile_CourierId",
                        column: x => x.CourierId,
                        principalTable: "CourierProfile",
                        principalColumn: "UserId");
                    table.ForeignKey(
                        name: "FK_CourierDeliveryArea_DeliveryArea_DeliveryAreaId",
                        column: x => x.DeliveryAreaId,
                        principalTable: "DeliveryArea",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "CustomerAddress",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: false),
                    DeliveryAreaId = table.Column<int>(type: "int", nullable: false),
                    Label = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    StreetAddress = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: false),
                    Building = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    Entrance = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    Floor = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    Apartment = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    DeliveryInstructions = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    IsDefault = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomerAddress", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CustomerAddress_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_CustomerAddress_DeliveryArea_DeliveryAreaId",
                        column: x => x.DeliveryAreaId,
                        principalTable: "DeliveryArea",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Cart",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: false),
                    RestaurantId = table.Column<int>(type: "int", nullable: true),
                    UpdatedAtUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cart", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Cart_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Cart_Restaurant_RestaurantId",
                        column: x => x.RestaurantId,
                        principalTable: "Restaurant",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Menu",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RestaurantId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    DisplayOrder = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Menu", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Menu_Restaurant_RestaurantId",
                        column: x => x.RestaurantId,
                        principalTable: "Restaurant",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Order",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OrderNumber = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: false),
                    CustomerId = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: false),
                    RestaurantId = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    Subtotal = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    DeliveryFee = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    TotalAmount = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    CurrencyCode = table.Column<string>(type: "nvarchar(3)", maxLength: 3, nullable: false),
                    CustomerNote = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    CreatedAtUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EstimatedReadyAtUtc = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CheckoutToken = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CheckoutRequestHash = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                    RestaurantNameSnapshot = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    PickupCitySnapshot = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    PickupAddressSnapshot = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: false),
                    PickupPhoneSnapshot = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Order", x => x.Id);
                    table.CheckConstraint("CK_Order_Amounts", "[Subtotal] >= 0 AND [DeliveryFee] >= 0 AND [TotalAmount] = [Subtotal] + [DeliveryFee]");
                    table.CheckConstraint("CK_Order_Currency", "LEN([CurrencyCode]) = 3 AND [CurrencyCode] COLLATE Latin1_General_100_BIN2 NOT LIKE '%[^A-Z]%'");
                    table.CheckConstraint("CK_Order_Hash", "LEN([CheckoutRequestHash]) = 64 AND [CheckoutRequestHash] COLLATE Latin1_General_100_BIN2 NOT LIKE '%[^0-9A-Fa-f]%'");
                    table.CheckConstraint("CK_Order_Status", "[Status] BETWEEN 0 AND 8");
                    table.ForeignKey(
                        name: "FK_Order_AspNetUsers_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Order_Restaurant_RestaurantId",
                        column: x => x.RestaurantId,
                        principalTable: "Restaurant",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "RestaurantApprovalHistory",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RestaurantId = table.Column<int>(type: "int", nullable: false),
                    FromStatus = table.Column<int>(type: "int", nullable: true),
                    ToStatus = table.Column<int>(type: "int", nullable: false),
                    ChangedByUserId = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: false),
                    ChangedAtUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Reason = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RestaurantApprovalHistory", x => x.Id);
                    table.CheckConstraint("CK_RestaurantApprovalHistory_FromStatus", "[FromStatus] IS NULL OR [FromStatus] BETWEEN 0 AND 2");
                    table.CheckConstraint("CK_RestaurantApprovalHistory_ToStatus", "[ToStatus] BETWEEN 0 AND 2");
                    table.ForeignKey(
                        name: "FK_RestaurantApprovalHistory_AspNetUsers_ChangedByUserId",
                        column: x => x.ChangedByUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_RestaurantApprovalHistory_Restaurant_RestaurantId",
                        column: x => x.RestaurantId,
                        principalTable: "Restaurant",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "RestaurantCuisineType",
                columns: table => new
                {
                    RestaurantId = table.Column<int>(type: "int", nullable: false),
                    CuisineTypeId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RestaurantCuisineType", x => new { x.RestaurantId, x.CuisineTypeId });
                    table.ForeignKey(
                        name: "FK_RestaurantCuisineType_CuisineType_CuisineTypeId",
                        column: x => x.CuisineTypeId,
                        principalTable: "CuisineType",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_RestaurantCuisineType_Restaurant_RestaurantId",
                        column: x => x.RestaurantId,
                        principalTable: "Restaurant",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "RestaurantDeliveryArea",
                columns: table => new
                {
                    RestaurantId = table.Column<int>(type: "int", nullable: false),
                    DeliveryAreaId = table.Column<int>(type: "int", nullable: false),
                    DeliveryFee = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RestaurantDeliveryArea", x => new { x.RestaurantId, x.DeliveryAreaId });
                    table.CheckConstraint("CK_RestaurantDeliveryArea_Fee", "[DeliveryFee] >= 0");
                    table.ForeignKey(
                        name: "FK_RestaurantDeliveryArea_DeliveryArea_DeliveryAreaId",
                        column: x => x.DeliveryAreaId,
                        principalTable: "DeliveryArea",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_RestaurantDeliveryArea_Restaurant_RestaurantId",
                        column: x => x.RestaurantId,
                        principalTable: "Restaurant",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "RestaurantScheduleException",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RestaurantId = table.Column<int>(type: "int", nullable: false),
                    Date = table.Column<DateOnly>(type: "date", nullable: false),
                    IsClosed = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RestaurantScheduleException", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RestaurantScheduleException_Restaurant_RestaurantId",
                        column: x => x.RestaurantId,
                        principalTable: "Restaurant",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "RestaurantStaff",
                columns: table => new
                {
                    RestaurantId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: false),
                    StaffRole = table.Column<int>(type: "int", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAtUtc = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RestaurantStaff", x => new { x.RestaurantId, x.UserId }).Annotation("SqlServer:Clustered", false);
                    table.CheckConstraint("CK_RestaurantStaff_StaffRole", "[StaffRole] BETWEEN 0 AND 1");
                    table.ForeignKey(
                        name: "FK_RestaurantStaff_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_RestaurantStaff_Restaurant_RestaurantId",
                        column: x => x.RestaurantId,
                        principalTable: "Restaurant",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "RestaurantWorkingPeriod",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RestaurantId = table.Column<int>(type: "int", nullable: false),
                    DayOfWeek = table.Column<int>(type: "int", nullable: false),
                    OpensAt = table.Column<TimeOnly>(type: "time", nullable: false),
                    ClosesAt = table.Column<TimeOnly>(type: "time", nullable: false),
                    ClosesNextDay = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RestaurantWorkingPeriod", x => x.Id);
                    table.CheckConstraint("CK_RestaurantWorkingPeriod_DayOfWeek", "[DayOfWeek] BETWEEN 0 AND 6");
                    table.CheckConstraint("CK_RestaurantWorkingPeriod_Period", "([ClosesNextDay] = 0 AND [ClosesAt] > [OpensAt]) OR ([ClosesNextDay] = 1 AND [ClosesAt] < [OpensAt])");
                    table.ForeignKey(
                        name: "FK_RestaurantWorkingPeriod_Restaurant_RestaurantId",
                        column: x => x.RestaurantId,
                        principalTable: "Restaurant",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "MenuCategory",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MenuId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    NormalizedName = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    DisplayOrder = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MenuCategory", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MenuCategory_Menu_MenuId",
                        column: x => x.MenuId,
                        principalTable: "Menu",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Delivery",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OrderId = table.Column<int>(type: "int", nullable: false),
                    CourierId = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: true),
                    DeliveryAreaId = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    CreatedAtUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    AssignedAtUtc = table.Column<DateTime>(type: "datetime2", nullable: true),
                    PickedUpAtUtc = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeliveredAtUtc = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Delivery", x => x.Id);
                    table.CheckConstraint("CK_Delivery_Courier", "([Status] = 0 AND [CourierId] IS NULL) OR ([Status] IN (1, 2, 3, 4) AND [CourierId] IS NOT NULL) OR [Status] = 5");
                    table.CheckConstraint("CK_Delivery_Status", "[Status] BETWEEN 0 AND 5");
                    table.ForeignKey(
                        name: "FK_Delivery_CourierProfile_CourierId",
                        column: x => x.CourierId,
                        principalTable: "CourierProfile",
                        principalColumn: "UserId");
                    table.ForeignKey(
                        name: "FK_Delivery_DeliveryArea_DeliveryAreaId",
                        column: x => x.DeliveryAreaId,
                        principalTable: "DeliveryArea",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Delivery_Order_OrderId",
                        column: x => x.OrderId,
                        principalTable: "Order",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "OrderDeliveryAddress",
                columns: table => new
                {
                    OrderId = table.Column<int>(type: "int", nullable: false),
                    DeliveryAreaId = table.Column<int>(type: "int", nullable: false),
                    RecipientName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    CityName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    AreaName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    StreetAddress = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: false),
                    Building = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    Entrance = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    Floor = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    Apartment = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    DeliveryInstructions = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderDeliveryAddress", x => x.OrderId);
                    table.ForeignKey(
                        name: "FK_OrderDeliveryAddress_DeliveryArea_DeliveryAreaId",
                        column: x => x.DeliveryAreaId,
                        principalTable: "DeliveryArea",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_OrderDeliveryAddress_Order_OrderId",
                        column: x => x.OrderId,
                        principalTable: "Order",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "OrderStatusHistory",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OrderId = table.Column<int>(type: "int", nullable: false),
                    FromStatus = table.Column<int>(type: "int", nullable: true),
                    ToStatus = table.Column<int>(type: "int", nullable: false),
                    ChangedByUserId = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: true),
                    ChangedAtUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Reason = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderStatusHistory", x => x.Id);
                    table.CheckConstraint("CK_OrderStatusHistory_FromStatus", "[FromStatus] IS NULL OR [FromStatus] BETWEEN 0 AND 8");
                    table.CheckConstraint("CK_OrderStatusHistory_ToStatus", "[ToStatus] BETWEEN 0 AND 8");
                    table.ForeignKey(
                        name: "FK_OrderStatusHistory_AspNetUsers_ChangedByUserId",
                        column: x => x.ChangedByUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_OrderStatusHistory_Order_OrderId",
                        column: x => x.OrderId,
                        principalTable: "Order",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Payment",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OrderId = table.Column<int>(type: "int", nullable: false),
                    Method = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    CurrencyCode = table.Column<string>(type: "nvarchar(3)", maxLength: 3, nullable: false),
                    CollectedAtUtc = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CollectedByUserId = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: true),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Payment", x => x.Id);
                    table.CheckConstraint("CK_Payment_Amount", "[Amount] >= 0");
                    table.CheckConstraint("CK_Payment_Currency", "LEN([CurrencyCode]) = 3 AND [CurrencyCode] COLLATE Latin1_General_100_BIN2 NOT LIKE '%[^A-Z]%'");
                    table.CheckConstraint("CK_Payment_Method", "[Method] BETWEEN 0 AND 0");
                    table.CheckConstraint("CK_Payment_Status", "[Status] BETWEEN 0 AND 3");
                    table.ForeignKey(
                        name: "FK_Payment_AspNetUsers_CollectedByUserId",
                        column: x => x.CollectedByUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Payment_Order_OrderId",
                        column: x => x.OrderId,
                        principalTable: "Order",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Review",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OrderId = table.Column<int>(type: "int", nullable: false),
                    RestaurantRating = table.Column<int>(type: "int", nullable: false),
                    DeliveryRating = table.Column<int>(type: "int", nullable: true),
                    Comment = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    CreatedAtUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAtUtc = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModerationStatus = table.Column<int>(type: "int", nullable: false),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Review", x => x.Id);
                    table.CheckConstraint("CK_Review_ModerationStatus", "[ModerationStatus] BETWEEN 0 AND 1");
                    table.CheckConstraint("CK_Review_Ratings", "[RestaurantRating] BETWEEN 1 AND 5 AND ([DeliveryRating] IS NULL OR [DeliveryRating] BETWEEN 1 AND 5)");
                    table.ForeignKey(
                        name: "FK_Review_Order_OrderId",
                        column: x => x.OrderId,
                        principalTable: "Order",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "SupportTicket",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedByUserId = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: false),
                    OrderId = table.Column<int>(type: "int", nullable: true),
                    Category = table.Column<int>(type: "int", nullable: false),
                    Subject = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    AssignedAdminId = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: true),
                    CreatedAtUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAtUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SupportTicket", x => x.Id);
                    table.CheckConstraint("CK_SupportTicket_Category", "[Category] BETWEEN 0 AND 3");
                    table.CheckConstraint("CK_SupportTicket_Status", "[Status] BETWEEN 0 AND 3");
                    table.ForeignKey(
                        name: "FK_SupportTicket_AspNetUsers_AssignedAdminId",
                        column: x => x.AssignedAdminId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_SupportTicket_AspNetUsers_CreatedByUserId",
                        column: x => x.CreatedByUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_SupportTicket_Order_OrderId",
                        column: x => x.OrderId,
                        principalTable: "Order",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "RestaurantScheduleExceptionPeriod",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RestaurantScheduleExceptionId = table.Column<int>(type: "int", nullable: false),
                    OpensAt = table.Column<TimeOnly>(type: "time", nullable: false),
                    ClosesAt = table.Column<TimeOnly>(type: "time", nullable: false),
                    ClosesNextDay = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RestaurantScheduleExceptionPeriod", x => x.Id);
                    table.CheckConstraint("CK_RestaurantScheduleExceptionPeriod_Period", "([ClosesNextDay] = 0 AND [ClosesAt] > [OpensAt]) OR ([ClosesNextDay] = 1 AND [ClosesAt] < [OpensAt])");
                    table.ForeignKey(
                        name: "FK_RestaurantScheduleExceptionPeriod_RestaurantScheduleException_RestaurantScheduleExceptionId",
                        column: x => x.RestaurantScheduleExceptionId,
                        principalTable: "RestaurantScheduleException",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Dish",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MenuCategoryId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: false),
                    ImagePath = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsAvailable = table.Column<bool>(type: "bit", nullable: false),
                    DisplayOrder = table.Column<int>(type: "int", nullable: false),
                    CreatedAtUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Dish", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Dish_MenuCategory_MenuCategoryId",
                        column: x => x.MenuCategoryId,
                        principalTable: "MenuCategory",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "DeliveryAssignmentHistory",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DeliveryId = table.Column<int>(type: "int", nullable: false),
                    CourierId = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: false),
                    AssignedAtUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ReleasedAtUtc = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ReleaseReason = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DeliveryAssignmentHistory", x => x.Id);
                    table.CheckConstraint("CK_DeliveryAssignmentHistory_Release", "[ReleasedAtUtc] IS NULL OR [ReleasedAtUtc] >= [AssignedAtUtc]");
                    table.ForeignKey(
                        name: "FK_DeliveryAssignmentHistory_CourierProfile_CourierId",
                        column: x => x.CourierId,
                        principalTable: "CourierProfile",
                        principalColumn: "UserId");
                    table.ForeignKey(
                        name: "FK_DeliveryAssignmentHistory_Delivery_DeliveryId",
                        column: x => x.DeliveryId,
                        principalTable: "Delivery",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "DeliveryStatusHistory",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DeliveryId = table.Column<int>(type: "int", nullable: false),
                    FromStatus = table.Column<int>(type: "int", nullable: true),
                    ToStatus = table.Column<int>(type: "int", nullable: false),
                    ChangedByUserId = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: true),
                    ChangedAtUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Reason = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DeliveryStatusHistory", x => x.Id);
                    table.CheckConstraint("CK_DeliveryStatusHistory_FromStatus", "[FromStatus] IS NULL OR [FromStatus] BETWEEN 0 AND 5");
                    table.CheckConstraint("CK_DeliveryStatusHistory_ToStatus", "[ToStatus] BETWEEN 0 AND 5");
                    table.ForeignKey(
                        name: "FK_DeliveryStatusHistory_AspNetUsers_ChangedByUserId",
                        column: x => x.ChangedByUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_DeliveryStatusHistory_Delivery_DeliveryId",
                        column: x => x.DeliveryId,
                        principalTable: "Delivery",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ReviewReply",
                columns: table => new
                {
                    ReviewId = table.Column<int>(type: "int", nullable: false),
                    AuthorUserId = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: false),
                    Text = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: false),
                    CreatedAtUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAtUtc = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModerationStatus = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReviewReply", x => x.ReviewId);
                    table.CheckConstraint("CK_ReviewReply_ModerationStatus", "[ModerationStatus] BETWEEN 0 AND 1");
                    table.ForeignKey(
                        name: "FK_ReviewReply_AspNetUsers_AuthorUserId",
                        column: x => x.AuthorUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ReviewReply_Review_ReviewId",
                        column: x => x.ReviewId,
                        principalTable: "Review",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Notification",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RecipientUserId = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: false),
                    Type = table.Column<int>(type: "int", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Message = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: false),
                    OrderId = table.Column<int>(type: "int", nullable: true),
                    SupportTicketId = table.Column<int>(type: "int", nullable: true),
                    CreatedAtUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ReadAtUtc = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Notification", x => x.Id);
                    table.CheckConstraint("CK_Notification_Type", "[Type] BETWEEN 0 AND 4");
                    table.ForeignKey(
                        name: "FK_Notification_AspNetUsers_RecipientUserId",
                        column: x => x.RecipientUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Notification_Order_OrderId",
                        column: x => x.OrderId,
                        principalTable: "Order",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Notification_SupportTicket_SupportTicketId",
                        column: x => x.SupportTicketId,
                        principalTable: "SupportTicket",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "SupportTicketMessage",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SupportTicketId = table.Column<int>(type: "int", nullable: false),
                    AuthorUserId = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: false),
                    Text = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: false),
                    CreatedAtUtc = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SupportTicketMessage", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SupportTicketMessage_AspNetUsers_AuthorUserId",
                        column: x => x.AuthorUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_SupportTicketMessage_SupportTicket_SupportTicketId",
                        column: x => x.SupportTicketId,
                        principalTable: "SupportTicket",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "DishAllergen",
                columns: table => new
                {
                    DishId = table.Column<int>(type: "int", nullable: false),
                    AllergenId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DishAllergen", x => new { x.DishId, x.AllergenId });
                    table.ForeignKey(
                        name: "FK_DishAllergen_Allergen_AllergenId",
                        column: x => x.AllergenId,
                        principalTable: "Allergen",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_DishAllergen_Dish_DishId",
                        column: x => x.DishId,
                        principalTable: "Dish",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "DishOptionGroup",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DishId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    MinSelections = table.Column<int>(type: "int", nullable: false),
                    MaxSelections = table.Column<int>(type: "int", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    DisplayOrder = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DishOptionGroup", x => x.Id);
                    table.CheckConstraint("CK_DishOptionGroup_Selections", "[MinSelections] >= 0 AND [MinSelections] <= [MaxSelections] AND [MaxSelections] > 0");
                    table.ForeignKey(
                        name: "FK_DishOptionGroup_Dish_DishId",
                        column: x => x.DishId,
                        principalTable: "Dish",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "DishVariant",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DishId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    NormalizedName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    PortionQuantity = table.Column<decimal>(type: "decimal(18,3)", precision: 18, scale: 3, nullable: true),
                    PortionUnit = table.Column<int>(type: "int", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsAvailable = table.Column<bool>(type: "bit", nullable: false),
                    DisplayOrder = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DishVariant", x => x.Id);
                    table.CheckConstraint("CK_DishVariant_Portion", "([PortionQuantity] IS NULL AND [PortionUnit] IS NULL) OR ([PortionQuantity] IS NOT NULL AND [PortionUnit] IS NOT NULL AND [PortionQuantity] > 0)");
                    table.CheckConstraint("CK_DishVariant_PortionUnit", "[PortionUnit] IS NULL OR [PortionUnit] BETWEEN 0 AND 2");
                    table.CheckConstraint("CK_DishVariant_Price", "[Price] > 0");
                    table.ForeignKey(
                        name: "FK_DishVariant_Dish_DishId",
                        column: x => x.DishId,
                        principalTable: "Dish",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "DishOption",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DishOptionGroupId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    AdditionalPrice = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsAvailable = table.Column<bool>(type: "bit", nullable: false),
                    DisplayOrder = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DishOption", x => x.Id);
                    table.CheckConstraint("CK_DishOption_Price", "[AdditionalPrice] >= 0");
                    table.ForeignKey(
                        name: "FK_DishOption_DishOptionGroup_DishOptionGroupId",
                        column: x => x.DishOptionGroupId,
                        principalTable: "DishOptionGroup",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "CartItem",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CartId = table.Column<int>(type: "int", nullable: false),
                    DishVariantId = table.Column<int>(type: "int", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    SpecialInstructions = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CartItem", x => x.Id);
                    table.CheckConstraint("CK_CartItem_Quantity", "[Quantity] BETWEEN 1 AND 99");
                    table.ForeignKey(
                        name: "FK_CartItem_Cart_CartId",
                        column: x => x.CartId,
                        principalTable: "Cart",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_CartItem_DishVariant_DishVariantId",
                        column: x => x.DishVariantId,
                        principalTable: "DishVariant",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "OrderItem",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OrderId = table.Column<int>(type: "int", nullable: false),
                    DishVariantId = table.Column<int>(type: "int", nullable: false),
                    DishName = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    VariantName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    BaseUnitPrice = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    OptionsUnitPrice = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    LineTotal = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    SpecialInstructions = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderItem", x => x.Id);
                    table.CheckConstraint("CK_OrderItem_Amounts", "[BaseUnitPrice] > 0 AND [OptionsUnitPrice] >= 0 AND [LineTotal] = ([BaseUnitPrice] + [OptionsUnitPrice]) * [Quantity]");
                    table.CheckConstraint("CK_OrderItem_Quantity", "[Quantity] BETWEEN 1 AND 99");
                    table.ForeignKey(
                        name: "FK_OrderItem_DishVariant_DishVariantId",
                        column: x => x.DishVariantId,
                        principalTable: "DishVariant",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_OrderItem_Order_OrderId",
                        column: x => x.OrderId,
                        principalTable: "Order",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "DishOptionAllergen",
                columns: table => new
                {
                    DishOptionId = table.Column<int>(type: "int", nullable: false),
                    AllergenId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DishOptionAllergen", x => new { x.DishOptionId, x.AllergenId });
                    table.ForeignKey(
                        name: "FK_DishOptionAllergen_Allergen_AllergenId",
                        column: x => x.AllergenId,
                        principalTable: "Allergen",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_DishOptionAllergen_DishOption_DishOptionId",
                        column: x => x.DishOptionId,
                        principalTable: "DishOption",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "CartItemOption",
                columns: table => new
                {
                    CartItemId = table.Column<int>(type: "int", nullable: false),
                    DishOptionId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CartItemOption", x => new { x.CartItemId, x.DishOptionId });
                    table.ForeignKey(
                        name: "FK_CartItemOption_CartItem_CartItemId",
                        column: x => x.CartItemId,
                        principalTable: "CartItem",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_CartItemOption_DishOption_DishOptionId",
                        column: x => x.DishOptionId,
                        principalTable: "DishOption",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "OrderItemOption",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OrderItemId = table.Column<int>(type: "int", nullable: false),
                    DishOptionId = table.Column<int>(type: "int", nullable: false),
                    NameSnapshot = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    AdditionalPrice = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderItemOption", x => x.Id);
                    table.CheckConstraint("CK_OrderItemOption_Price", "[AdditionalPrice] >= 0");
                    table.ForeignKey(
                        name: "FK_OrderItemOption_DishOption_DishOptionId",
                        column: x => x.DishOptionId,
                        principalTable: "DishOption",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_OrderItemOption_OrderItem_OrderItemId",
                        column: x => x.OrderItemId,
                        principalTable: "OrderItem",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "NormalizedEmail",
                unique: true,
                filter: "[NormalizedEmail] IS NOT NULL AND [NormalizedEmail] <> N''");

            migrationBuilder.CreateIndex(
                name: "IX_Allergen_Code",
                table: "Allergen",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AuditLog_ActorUserId_CreatedAtUtc",
                table: "AuditLog",
                columns: new[] { "ActorUserId", "CreatedAtUtc" });

            migrationBuilder.CreateIndex(
                name: "IX_AuditLog_EntityType_EntityId_CreatedAtUtc",
                table: "AuditLog",
                columns: new[] { "EntityType", "EntityId", "CreatedAtUtc" });

            migrationBuilder.CreateIndex(
                name: "IX_Cart_RestaurantId",
                table: "Cart",
                column: "RestaurantId");

            migrationBuilder.CreateIndex(
                name: "IX_Cart_UserId",
                table: "Cart",
                column: "UserId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CartItem_CartId",
                table: "CartItem",
                column: "CartId");

            migrationBuilder.CreateIndex(
                name: "IX_CartItem_DishVariantId",
                table: "CartItem",
                column: "DishVariantId");

            migrationBuilder.CreateIndex(
                name: "IX_CartItemOption_DishOptionId",
                table: "CartItemOption",
                column: "DishOptionId");

            migrationBuilder.CreateIndex(
                name: "IX_City_NormalizedName",
                table: "City",
                column: "NormalizedName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CourierApprovalHistory_ChangedByUserId",
                table: "CourierApprovalHistory",
                column: "ChangedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_CourierApprovalHistory_CourierId_ChangedAtUtc",
                table: "CourierApprovalHistory",
                columns: new[] { "CourierId", "ChangedAtUtc" });

            migrationBuilder.CreateIndex(
                name: "IX_CourierDeliveryArea_DeliveryAreaId",
                table: "CourierDeliveryArea",
                column: "DeliveryAreaId");

            migrationBuilder.CreateIndex(
                name: "IX_CuisineType_NormalizedName",
                table: "CuisineType",
                column: "NormalizedName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CustomerAddress_DeliveryAreaId",
                table: "CustomerAddress",
                column: "DeliveryAreaId");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerAddress_UserId",
                table: "CustomerAddress",
                column: "UserId",
                unique: true,
                filter: "[IsDefault] = 1");

            migrationBuilder.CreateIndex(
                name: "IX_Delivery_CourierId",
                table: "Delivery",
                column: "CourierId",
                unique: true,
                filter: "[Status] IN (1, 2)");

            migrationBuilder.CreateIndex(
                name: "IX_Delivery_DeliveryAreaId",
                table: "Delivery",
                column: "DeliveryAreaId");

            migrationBuilder.CreateIndex(
                name: "IX_Delivery_OrderId",
                table: "Delivery",
                column: "OrderId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_DeliveryArea_CityId_NormalizedName",
                table: "DeliveryArea",
                columns: new[] { "CityId", "NormalizedName" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_DeliveryAssignmentHistory_CourierId",
                table: "DeliveryAssignmentHistory",
                column: "CourierId");

            migrationBuilder.CreateIndex(
                name: "IX_DeliveryAssignmentHistory_DeliveryId",
                table: "DeliveryAssignmentHistory",
                column: "DeliveryId",
                unique: true,
                filter: "[ReleasedAtUtc] IS NULL");

            migrationBuilder.CreateIndex(
                name: "IX_DeliveryStatusHistory_ChangedByUserId",
                table: "DeliveryStatusHistory",
                column: "ChangedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_DeliveryStatusHistory_DeliveryId_ChangedAtUtc",
                table: "DeliveryStatusHistory",
                columns: new[] { "DeliveryId", "ChangedAtUtc" });

            migrationBuilder.CreateIndex(
                name: "IX_Dish_MenuCategoryId_DisplayOrder",
                table: "Dish",
                columns: new[] { "MenuCategoryId", "DisplayOrder" });

            migrationBuilder.CreateIndex(
                name: "IX_DishAllergen_AllergenId",
                table: "DishAllergen",
                column: "AllergenId");

            migrationBuilder.CreateIndex(
                name: "IX_DishOption_DishOptionGroupId",
                table: "DishOption",
                column: "DishOptionGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_DishOptionAllergen_AllergenId",
                table: "DishOptionAllergen",
                column: "AllergenId");

            migrationBuilder.CreateIndex(
                name: "IX_DishOptionGroup_DishId",
                table: "DishOptionGroup",
                column: "DishId");

            migrationBuilder.CreateIndex(
                name: "IX_DishVariant_DishId_NormalizedName",
                table: "DishVariant",
                columns: new[] { "DishId", "NormalizedName" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Menu_RestaurantId_DisplayOrder",
                table: "Menu",
                columns: new[] { "RestaurantId", "DisplayOrder" });

            migrationBuilder.CreateIndex(
                name: "IX_MenuCategory_MenuId_NormalizedName",
                table: "MenuCategory",
                columns: new[] { "MenuId", "NormalizedName" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Notification_OrderId",
                table: "Notification",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_Notification_RecipientUserId_CreatedAtUtc",
                table: "Notification",
                columns: new[] { "RecipientUserId", "CreatedAtUtc" });

            migrationBuilder.CreateIndex(
                name: "IX_Notification_SupportTicketId",
                table: "Notification",
                column: "SupportTicketId");

            migrationBuilder.CreateIndex(
                name: "IX_Notification_Unread",
                table: "Notification",
                columns: new[] { "RecipientUserId", "CreatedAtUtc" },
                filter: "[ReadAtUtc] IS NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Order_CheckoutToken",
                table: "Order",
                column: "CheckoutToken",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Order_CustomerId_CreatedAtUtc",
                table: "Order",
                columns: new[] { "CustomerId", "CreatedAtUtc" });

            migrationBuilder.CreateIndex(
                name: "IX_Order_OrderNumber",
                table: "Order",
                column: "OrderNumber",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Order_RestaurantId_Status_CreatedAtUtc",
                table: "Order",
                columns: new[] { "RestaurantId", "Status", "CreatedAtUtc" });

            migrationBuilder.CreateIndex(
                name: "IX_OrderDeliveryAddress_DeliveryAreaId",
                table: "OrderDeliveryAddress",
                column: "DeliveryAreaId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderItem_DishVariantId",
                table: "OrderItem",
                column: "DishVariantId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderItem_OrderId",
                table: "OrderItem",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderItemOption_DishOptionId",
                table: "OrderItemOption",
                column: "DishOptionId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderItemOption_OrderItemId_DishOptionId",
                table: "OrderItemOption",
                columns: new[] { "OrderItemId", "DishOptionId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_OrderStatusHistory_ChangedByUserId",
                table: "OrderStatusHistory",
                column: "ChangedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderStatusHistory_OrderId_ChangedAtUtc",
                table: "OrderStatusHistory",
                columns: new[] { "OrderId", "ChangedAtUtc" });

            migrationBuilder.CreateIndex(
                name: "IX_Payment_CollectedByUserId",
                table: "Payment",
                column: "CollectedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Payment_OrderId",
                table: "Payment",
                column: "OrderId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Restaurant_CityId",
                table: "Restaurant",
                column: "CityId");

            migrationBuilder.CreateIndex(
                name: "IX_Restaurant_Slug",
                table: "Restaurant",
                column: "Slug",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_RestaurantApprovalHistory_ChangedByUserId",
                table: "RestaurantApprovalHistory",
                column: "ChangedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_RestaurantApprovalHistory_RestaurantId_ChangedAtUtc",
                table: "RestaurantApprovalHistory",
                columns: new[] { "RestaurantId", "ChangedAtUtc" });

            migrationBuilder.CreateIndex(
                name: "IX_RestaurantCuisineType_CuisineTypeId",
                table: "RestaurantCuisineType",
                column: "CuisineTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_RestaurantDeliveryArea_DeliveryAreaId",
                table: "RestaurantDeliveryArea",
                column: "DeliveryAreaId");

            migrationBuilder.CreateIndex(
                name: "IX_RestaurantScheduleException_RestaurantId_Date",
                table: "RestaurantScheduleException",
                columns: new[] { "RestaurantId", "Date" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_RestaurantScheduleExceptionPeriod_RestaurantScheduleExceptionId",
                table: "RestaurantScheduleExceptionPeriod",
                column: "RestaurantScheduleExceptionId");

            migrationBuilder.CreateIndex(
                name: "IX_RestaurantStaff_UserId",
                table: "RestaurantStaff",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_RestaurantWorkingPeriod_RestaurantId_DayOfWeek",
                table: "RestaurantWorkingPeriod",
                columns: new[] { "RestaurantId", "DayOfWeek" });

            migrationBuilder.CreateIndex(
                name: "IX_Review_OrderId",
                table: "Review",
                column: "OrderId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ReviewReply_AuthorUserId",
                table: "ReviewReply",
                column: "AuthorUserId");

            migrationBuilder.CreateIndex(
                name: "IX_SupportTicket_AssignedAdminId",
                table: "SupportTicket",
                column: "AssignedAdminId");

            migrationBuilder.CreateIndex(
                name: "IX_SupportTicket_CreatedByUserId_CreatedAtUtc",
                table: "SupportTicket",
                columns: new[] { "CreatedByUserId", "CreatedAtUtc" });

            migrationBuilder.CreateIndex(
                name: "IX_SupportTicket_OrderId",
                table: "SupportTicket",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_SupportTicket_Status_UpdatedAtUtc",
                table: "SupportTicket",
                columns: new[] { "Status", "UpdatedAtUtc" });

            migrationBuilder.CreateIndex(
                name: "IX_SupportTicketMessage_AuthorUserId",
                table: "SupportTicketMessage",
                column: "AuthorUserId");

            migrationBuilder.CreateIndex(
                name: "IX_SupportTicketMessage_SupportTicketId_CreatedAtUtc",
                table: "SupportTicketMessage",
                columns: new[] { "SupportTicketId", "CreatedAtUtc" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AuditLog");

            migrationBuilder.DropTable(
                name: "CartItemOption");

            migrationBuilder.DropTable(
                name: "CourierApprovalHistory");

            migrationBuilder.DropTable(
                name: "CourierDeliveryArea");

            migrationBuilder.DropTable(
                name: "CustomerAddress");

            migrationBuilder.DropTable(
                name: "DeliveryAssignmentHistory");

            migrationBuilder.DropTable(
                name: "DeliveryStatusHistory");

            migrationBuilder.DropTable(
                name: "DishAllergen");

            migrationBuilder.DropTable(
                name: "DishOptionAllergen");

            migrationBuilder.DropTable(
                name: "Notification");

            migrationBuilder.DropTable(
                name: "OrderDeliveryAddress");

            migrationBuilder.DropTable(
                name: "OrderItemOption");

            migrationBuilder.DropTable(
                name: "OrderStatusHistory");

            migrationBuilder.DropTable(
                name: "Payment");

            migrationBuilder.DropTable(
                name: "RestaurantApprovalHistory");

            migrationBuilder.DropTable(
                name: "RestaurantCuisineType");

            migrationBuilder.DropTable(
                name: "RestaurantDeliveryArea");

            migrationBuilder.DropTable(
                name: "RestaurantScheduleExceptionPeriod");

            migrationBuilder.DropTable(
                name: "RestaurantStaff");

            migrationBuilder.DropTable(
                name: "RestaurantWorkingPeriod");

            migrationBuilder.DropTable(
                name: "ReviewReply");

            migrationBuilder.DropTable(
                name: "SupportTicketMessage");

            migrationBuilder.DropTable(
                name: "CartItem");

            migrationBuilder.DropTable(
                name: "Delivery");

            migrationBuilder.DropTable(
                name: "Allergen");

            migrationBuilder.DropTable(
                name: "DishOption");

            migrationBuilder.DropTable(
                name: "OrderItem");

            migrationBuilder.DropTable(
                name: "CuisineType");

            migrationBuilder.DropTable(
                name: "RestaurantScheduleException");

            migrationBuilder.DropTable(
                name: "Review");

            migrationBuilder.DropTable(
                name: "SupportTicket");

            migrationBuilder.DropTable(
                name: "Cart");

            migrationBuilder.DropTable(
                name: "CourierProfile");

            migrationBuilder.DropTable(
                name: "DeliveryArea");

            migrationBuilder.DropTable(
                name: "DishOptionGroup");

            migrationBuilder.DropTable(
                name: "DishVariant");

            migrationBuilder.DropTable(
                name: "Order");

            migrationBuilder.DropTable(
                name: "Dish");

            migrationBuilder.DropTable(
                name: "MenuCategory");

            migrationBuilder.DropTable(
                name: "Menu");

            migrationBuilder.DropTable(
                name: "Restaurant");

            migrationBuilder.DropTable(
                name: "City");

            migrationBuilder.DropIndex(
                name: "EmailIndex",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "CreatedAtUtc",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "FirstName",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "LastName",
                table: "AspNetUsers");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "NormalizedEmail");
        }
    }
}
