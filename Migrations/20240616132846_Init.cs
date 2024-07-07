using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace RecipeProject.Migrations
{
    /// <inheritdoc />
    public partial class Init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Categoryname = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CategoryImagePath = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Userroles",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Userroles", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Persons",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Firstname = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Lastname = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Gender = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Phone = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ImagePath = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RoleId = table.Column<int>(type: "int", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Persons", x => x.id);
                    table.ForeignKey(
                        name: "FK_Persons_Userroles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Userroles",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "Admins",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PersonId = table.Column<int>(type: "int", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Admins", x => x.id);
                    table.ForeignKey(
                        name: "FK_Admins_Persons_PersonId",
                        column: x => x.PersonId,
                        principalTable: "Persons",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "Chefs",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PersonId = table.Column<int>(type: "int", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Chefs", x => x.id);
                    table.ForeignKey(
                        name: "FK_Chefs_Persons_PersonId",
                        column: x => x.PersonId,
                        principalTable: "Persons",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "Systemusers",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PersonId = table.Column<int>(type: "int", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Systemusers", x => x.id);
                    table.ForeignKey(
                        name: "FK_Systemusers_Persons_PersonId",
                        column: x => x.PersonId,
                        principalTable: "Persons",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "Websitedetails",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Websitetext = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Texttype = table.Column<int>(type: "int", nullable: false),
                    AdminId = table.Column<int>(type: "int", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Websitedetails", x => x.id);
                    table.ForeignKey(
                        name: "FK_Websitedetails_Admins_AdminId",
                        column: x => x.AdminId,
                        principalTable: "Admins",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "Recipes",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Receipename = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Price = table.Column<double>(type: "float", nullable: false),
                    Ingredients = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Details = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ImagePath = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Isapproved = table.Column<bool>(type: "bit", nullable: false),
                    ChefId = table.Column<int>(type: "int", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Recipes", x => x.id);
                    table.ForeignKey(
                        name: "FK_Recipes_Chefs_ChefId",
                        column: x => x.ChefId,
                        principalTable: "Chefs",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "Testimonials",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Usercomment = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsShown = table.Column<bool>(type: "bit", nullable: false),
                    Rating = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Testimonials", x => x.id);
                    table.ForeignKey(
                        name: "FK_Testimonials_Systemusers_UserId",
                        column: x => x.UserId,
                        principalTable: "Systemusers",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "Visacards",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Firstname = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Lastname = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Cardnumber = table.Column<long>(type: "bigint", nullable: false),
                    Pin = table.Column<byte>(type: "tinyint", nullable: false),
                    Expirydate = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Cvv = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Visacards", x => x.id);
                    table.ForeignKey(
                        name: "FK_Visacards_Systemusers_UserId",
                        column: x => x.UserId,
                        principalTable: "Systemusers",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "Recipecategories",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RecipeId = table.Column<int>(type: "int", nullable: true),
                    CategoryId = table.Column<int>(type: "int", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Recipecategories", x => x.id);
                    table.ForeignKey(
                        name: "FK_Recipecategories_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_Recipecategories_Recipes_RecipeId",
                        column: x => x.RecipeId,
                        principalTable: "Recipes",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "Recipepayments",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RecipeId = table.Column<int>(type: "int", nullable: true),
                    UserId = table.Column<int>(type: "int", nullable: true),
                    Totalamount = table.Column<double>(type: "float", nullable: true),
                    Paymentfilepath = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Recipepayments", x => x.id);
                    table.ForeignKey(
                        name: "FK_Recipepayments_Recipes_RecipeId",
                        column: x => x.RecipeId,
                        principalTable: "Recipes",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_Recipepayments_Systemusers_UserId",
                        column: x => x.UserId,
                        principalTable: "Systemusers",
                        principalColumn: "id");
                });

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "id", "CategoryImagePath", "Categoryname", "CreatedBy", "CreatedDate", "IsDeleted", "ModifiedBy", "ModifiedDate" },
                values: new object[,]
                {
                    { 1, "Dingo/img/food_item/food_item_1.png", "Breakfast", null, new DateTime(2024, 6, 16, 16, 28, 46, 671, DateTimeKind.Local).AddTicks(9107), false, null, null },
                    { 2, "Dingo/img/food_item/food_item_1.png", "Lunch", null, new DateTime(2024, 6, 16, 16, 28, 46, 671, DateTimeKind.Local).AddTicks(9108), false, null, null },
                    { 3, "Dingo/img/food_item/food_item_1.png", "Dinner", null, new DateTime(2024, 6, 16, 16, 28, 46, 671, DateTimeKind.Local).AddTicks(9109), false, null, null },
                    { 4, "Dingo/img/food_item/food_item_1.png", "Sweets", null, new DateTime(2024, 6, 16, 16, 28, 46, 671, DateTimeKind.Local).AddTicks(9144), false, null, null },
                    { 5, "Dingo/img/food_item/food_item_1.png", "Juices", null, new DateTime(2024, 6, 16, 16, 28, 46, 671, DateTimeKind.Local).AddTicks(9146), false, null, null },
                    { 6, "Dingo/img/food_item/food_item_1.png", "Vegeterian", null, new DateTime(2024, 6, 16, 16, 28, 46, 671, DateTimeKind.Local).AddTicks(9147), false, null, null }
                });

            migrationBuilder.InsertData(
                table: "Userroles",
                columns: new[] { "id", "CreatedBy", "CreatedDate", "IsDeleted", "ModifiedBy", "ModifiedDate", "RoleName" },
                values: new object[,]
                {
                    { 1, null, new DateTime(2024, 6, 16, 16, 28, 46, 671, DateTimeKind.Local).AddTicks(9014), false, null, null, "Admin" },
                    { 2, null, new DateTime(2024, 6, 16, 16, 28, 46, 671, DateTimeKind.Local).AddTicks(9026), false, null, null, "Chef" },
                    { 3, null, new DateTime(2024, 6, 16, 16, 28, 46, 671, DateTimeKind.Local).AddTicks(9027), false, null, null, "User" },
                    { 4, null, new DateTime(2024, 6, 16, 16, 28, 46, 671, DateTimeKind.Local).AddTicks(9028), false, null, null, "Guest User" }
                });

            migrationBuilder.InsertData(
                table: "Persons",
                columns: new[] { "id", "CreatedBy", "CreatedDate", "Email", "Firstname", "Gender", "ImagePath", "IsDeleted", "Lastname", "ModifiedBy", "ModifiedDate", "Password", "Phone", "RoleId" },
                values: new object[,]
                {
                    { 1, null, new DateTime(2024, 6, 16, 16, 28, 46, 671, DateTimeKind.Local).AddTicks(9062), "Admin@Admin.com", "Admin", "Male", null, false, "Admin", null, null, "Test@12345", "0771", 1 },
                    { 2, null, new DateTime(2024, 6, 16, 16, 28, 46, 671, DateTimeKind.Local).AddTicks(9064), "Chef@Chef.com", "Chef", "Male", null, false, "Chef", null, null, "Test@12345", "0771", 2 },
                    { 3, null, new DateTime(2024, 6, 16, 16, 28, 46, 671, DateTimeKind.Local).AddTicks(9065), "User@User.com", "User", "Male", null, false, "User", null, null, "Test@12345", "0771", 3 }
                });

            migrationBuilder.InsertData(
                table: "Admins",
                columns: new[] { "id", "CreatedBy", "CreatedDate", "IsDeleted", "ModifiedBy", "ModifiedDate", "PersonId" },
                values: new object[] { 1, null, null, false, null, null, 1 });

            migrationBuilder.InsertData(
                table: "Chefs",
                columns: new[] { "id", "CreatedBy", "CreatedDate", "IsDeleted", "ModifiedBy", "ModifiedDate", "PersonId" },
                values: new object[] { 1, null, null, false, null, null, 2 });

            migrationBuilder.InsertData(
                table: "Systemusers",
                columns: new[] { "id", "CreatedBy", "CreatedDate", "IsDeleted", "ModifiedBy", "ModifiedDate", "PersonId" },
                values: new object[] { 1, null, null, false, null, null, 3 });

            migrationBuilder.InsertData(
                table: "Recipes",
                columns: new[] { "id", "ChefId", "CreatedBy", "CreatedDate", "Details", "ImagePath", "Ingredients", "IsDeleted", "Isapproved", "ModifiedBy", "ModifiedDate", "Price", "Receipename" },
                values: new object[,]
                {
                    { 1, 1, null, new DateTime(2024, 6, 16, 16, 28, 46, 671, DateTimeKind.Local).AddTicks(9162), null, "food_item_2.png", "meat, salt", false, true, null, null, 23.5, "Meat with Salt Recepie" },
                    { 2, 1, null, new DateTime(2024, 6, 16, 16, 28, 46, 671, DateTimeKind.Local).AddTicks(9164), null, "food_item_2.png", "chicken, salt", false, true, null, null, 13.5, "Chicken with Salt Recepie" }
                });

            migrationBuilder.InsertData(
                table: "Testimonials",
                columns: new[] { "id", "CreatedBy", "CreatedDate", "IsDeleted", "IsShown", "ModifiedBy", "ModifiedDate", "Rating", "UserId", "Usercomment" },
                values: new object[] { 1, null, new DateTime(2024, 6, 16, 16, 28, 46, 671, DateTimeKind.Local).AddTicks(9208), false, true, null, null, 5, 1, "Amazing service , delecious recipes" });

            migrationBuilder.InsertData(
                table: "Visacards",
                columns: new[] { "id", "Cardnumber", "CreatedBy", "CreatedDate", "Cvv", "Expirydate", "Firstname", "IsDeleted", "Lastname", "ModifiedBy", "ModifiedDate", "Pin", "UserId" },
                values: new object[] { 1, 1234131L, null, new DateTime(2024, 6, 16, 16, 28, 46, 671, DateTimeKind.Local).AddTicks(9200), "123", "01/27", "User", false, "User", null, null, (byte)101, 1 });

            migrationBuilder.InsertData(
                table: "Websitedetails",
                columns: new[] { "id", "AdminId", "CreatedBy", "CreatedDate", "IsDeleted", "ModifiedBy", "ModifiedDate", "Texttype", "Websitetext" },
                values: new object[,]
                {
                    { 1, 1, null, new DateTime(2024, 6, 16, 16, 28, 46, 671, DateTimeKind.Local).AddTicks(9215), false, null, null, 1, "Welcome to Recipe Blog\r\nDelivering great food for more than 18 years!" },
                    { 2, 1, null, new DateTime(2024, 6, 16, 16, 28, 46, 671, DateTimeKind.Local).AddTicks(9216), false, null, null, 3, "Contact Us" },
                    { 3, 1, null, new DateTime(2024, 6, 16, 16, 28, 46, 671, DateTimeKind.Local).AddTicks(9217), false, null, null, 2, "A108 Adam Street, New York, NY 535022" }
                });

            migrationBuilder.InsertData(
                table: "Recipecategories",
                columns: new[] { "id", "CategoryId", "CreatedBy", "CreatedDate", "IsDeleted", "ModifiedBy", "ModifiedDate", "RecipeId" },
                values: new object[,]
                {
                    { 1, 1, null, new DateTime(2024, 6, 16, 16, 28, 46, 671, DateTimeKind.Local).AddTicks(9172), false, null, null, 1 },
                    { 2, 4, null, new DateTime(2024, 6, 16, 16, 28, 46, 671, DateTimeKind.Local).AddTicks(9173), false, null, null, 1 },
                    { 3, 3, null, new DateTime(2024, 6, 16, 16, 28, 46, 671, DateTimeKind.Local).AddTicks(9174), false, null, null, 2 },
                    { 4, 2, null, new DateTime(2024, 6, 16, 16, 28, 46, 671, DateTimeKind.Local).AddTicks(9174), false, null, null, 2 }
                });

            migrationBuilder.InsertData(
                table: "Recipepayments",
                columns: new[] { "id", "CreatedBy", "CreatedDate", "IsDeleted", "ModifiedBy", "ModifiedDate", "Paymentfilepath", "RecipeId", "Totalamount", "UserId" },
                values: new object[,]
                {
                    { 1, null, new DateTime(2024, 6, 16, 16, 28, 46, 671, DateTimeKind.Local).AddTicks(9187), false, null, null, "file path", 1, 23.5, 1 },
                    { 2, null, new DateTime(2024, 6, 16, 16, 28, 46, 671, DateTimeKind.Local).AddTicks(9188), false, null, null, "file path2", 2, 13.5, 1 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Admins_IsDeleted",
                table: "Admins",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_Admins_PersonId",
                table: "Admins",
                column: "PersonId");

            migrationBuilder.CreateIndex(
                name: "IX_Categories_IsDeleted",
                table: "Categories",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_Chefs_IsDeleted",
                table: "Chefs",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_Chefs_PersonId",
                table: "Chefs",
                column: "PersonId");

            migrationBuilder.CreateIndex(
                name: "IX_Persons_IsDeleted",
                table: "Persons",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_Persons_RoleId",
                table: "Persons",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_Recipecategories_CategoryId",
                table: "Recipecategories",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Recipecategories_IsDeleted",
                table: "Recipecategories",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_Recipecategories_RecipeId",
                table: "Recipecategories",
                column: "RecipeId");

            migrationBuilder.CreateIndex(
                name: "IX_Recipepayments_IsDeleted",
                table: "Recipepayments",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_Recipepayments_RecipeId",
                table: "Recipepayments",
                column: "RecipeId");

            migrationBuilder.CreateIndex(
                name: "IX_Recipepayments_UserId",
                table: "Recipepayments",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Recipes_ChefId",
                table: "Recipes",
                column: "ChefId");

            migrationBuilder.CreateIndex(
                name: "IX_Recipes_IsDeleted",
                table: "Recipes",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_Systemusers_IsDeleted",
                table: "Systemusers",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_Systemusers_PersonId",
                table: "Systemusers",
                column: "PersonId");

            migrationBuilder.CreateIndex(
                name: "IX_Testimonials_IsDeleted",
                table: "Testimonials",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_Testimonials_UserId",
                table: "Testimonials",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Userroles_IsDeleted",
                table: "Userroles",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_Visacards_IsDeleted",
                table: "Visacards",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_Visacards_UserId",
                table: "Visacards",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Websitedetails_AdminId",
                table: "Websitedetails",
                column: "AdminId");

            migrationBuilder.CreateIndex(
                name: "IX_Websitedetails_IsDeleted",
                table: "Websitedetails",
                column: "IsDeleted");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Recipecategories");

            migrationBuilder.DropTable(
                name: "Recipepayments");

            migrationBuilder.DropTable(
                name: "Testimonials");

            migrationBuilder.DropTable(
                name: "Visacards");

            migrationBuilder.DropTable(
                name: "Websitedetails");

            migrationBuilder.DropTable(
                name: "Categories");

            migrationBuilder.DropTable(
                name: "Recipes");

            migrationBuilder.DropTable(
                name: "Systemusers");

            migrationBuilder.DropTable(
                name: "Admins");

            migrationBuilder.DropTable(
                name: "Chefs");

            migrationBuilder.DropTable(
                name: "Persons");

            migrationBuilder.DropTable(
                name: "Userroles");
        }
    }
}
