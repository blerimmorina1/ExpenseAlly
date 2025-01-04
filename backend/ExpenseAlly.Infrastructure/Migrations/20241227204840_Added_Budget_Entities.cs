using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ExpenseAlly.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Added_Budget_Entities : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Transactions_TransactionCategories_CategoryId",
                table: "Transactions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TransactionCategories",
                table: "TransactionCategories");

            migrationBuilder.AlterTable(
                name: "Transactions",
                comment: "This entity is used to store all the transactions of user added to the system.",
                oldComment: "This entity is used to store all the transactions of user added to the system .");

            migrationBuilder.AlterTable(
                name: "TransactionCategories",
                comment: "This entity is used to store all categories for transactions.");

            migrationBuilder.AlterColumn<int>(
                name: "Type",
                table: "Transactions",
                type: "int",
                nullable: false,
                comment: "The type of transaction, such as Income or Expense.",
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "Notes",
                table: "Transactions",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true,
                comment: "Additional notes or description for the transaction.",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "Date",
                table: "Transactions",
                type: "datetime2",
                nullable: false,
                comment: "The date when the transaction occurred.",
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<Guid>(
                name: "CategoryId",
                table: "Transactions",
                type: "uniqueidentifier",
                nullable: false,
                comment: "The ID of the category associated with this transaction.",
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AlterColumn<decimal>(
                name: "Amount",
                table: "Transactions",
                type: "decimal(18,2)",
                nullable: false,
                comment: "The amount of money involved in the transaction.",
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

            migrationBuilder.AlterColumn<int>(
                name: "Type",
                table: "TransactionCategories",
                type: "int",
                nullable: false,
                comment: "The type of transaction associated with this category, such as Income or Expense.",
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "TransactionCategories",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                comment: "The name of the transaction category.",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "TransactionCategories",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true,
                comment: "Optional description for the transaction category.",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_TransactionCategories_Id",
                table: "TransactionCategories",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "Budgets",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false, comment: "The name of the budget."),
                    TotalLimit = table.Column<decimal>(type: "decimal(18,2)", nullable: false, comment: "The total spending limit for the budget."),
                    TotalSpent = table.Column<decimal>(type: "decimal(18,2)", nullable: false, comment: "The total amount spent of the budget."),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false, comment: "The start date of the budget period."),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: false, comment: "The end date of the budget period."),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Budgets_Id", x => x.Id);
                },
                comment: "This entity is used to store user budgets.");

            migrationBuilder.CreateTable(
                name: "BudgetDetails",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    BudgetId = table.Column<Guid>(type: "uniqueidentifier", nullable: false, comment: "The ID of the associated budget."),
                    CategoryId = table.Column<Guid>(type: "uniqueidentifier", nullable: false, comment: "The ID of the associated transaction category."),
                    Limit = table.Column<decimal>(type: "decimal(18,2)", nullable: false, comment: "The spending limit for this category within the budget."),
                    Spent = table.Column<decimal>(type: "decimal(18,2)", nullable: false, comment: "The amount spent for this category within the budget."),
                    TransactionCategoryId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BudgetDetails_Id", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BudgetDetails_Budgets",
                        column: x => x.BudgetId,
                        principalTable: "Budgets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BudgetDetails_TransactionCategories",
                        column: x => x.CategoryId,
                        principalTable: "TransactionCategories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_BudgetDetails_TransactionCategories_TransactionCategoryId",
                        column: x => x.TransactionCategoryId,
                        principalTable: "TransactionCategories",
                        principalColumn: "Id");
                },
                comment: "This entity is used to store details of budgets, including category limits.");

            migrationBuilder.CreateIndex(
                name: "IX_BudgetDetails_BudgetId",
                table: "BudgetDetails",
                column: "BudgetId");

            migrationBuilder.CreateIndex(
                name: "IX_BudgetDetails_CategoryId",
                table: "BudgetDetails",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_BudgetDetails_TransactionCategoryId",
                table: "BudgetDetails",
                column: "TransactionCategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_Transactions_Category",
                table: "Transactions",
                column: "CategoryId",
                principalTable: "TransactionCategories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Transactions_Category",
                table: "Transactions");

            migrationBuilder.DropTable(
                name: "BudgetDetails");

            migrationBuilder.DropTable(
                name: "Budgets");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TransactionCategories_Id",
                table: "TransactionCategories");

            migrationBuilder.AlterTable(
                name: "Transactions",
                comment: "This entity is used to store all the transactions of user added to the system .",
                oldComment: "This entity is used to store all the transactions of user added to the system.");

            migrationBuilder.AlterTable(
                name: "TransactionCategories",
                oldComment: "This entity is used to store all categories for transactions.");

            migrationBuilder.AlterColumn<int>(
                name: "Type",
                table: "Transactions",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldComment: "The type of transaction, such as Income or Expense.");

            migrationBuilder.AlterColumn<string>(
                name: "Notes",
                table: "Transactions",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(500)",
                oldMaxLength: 500,
                oldNullable: true,
                oldComment: "Additional notes or description for the transaction.");

            migrationBuilder.AlterColumn<DateTime>(
                name: "Date",
                table: "Transactions",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldComment: "The date when the transaction occurred.");

            migrationBuilder.AlterColumn<Guid>(
                name: "CategoryId",
                table: "Transactions",
                type: "uniqueidentifier",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldComment: "The ID of the category associated with this transaction.");

            migrationBuilder.AlterColumn<decimal>(
                name: "Amount",
                table: "Transactions",
                type: "decimal(18,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)",
                oldComment: "The amount of money involved in the transaction.");

            migrationBuilder.AlterColumn<int>(
                name: "Type",
                table: "TransactionCategories",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldComment: "The type of transaction associated with this category, such as Income or Expense.");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "TransactionCategories",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100,
                oldComment: "The name of the transaction category.");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "TransactionCategories",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(500)",
                oldMaxLength: 500,
                oldNullable: true,
                oldComment: "Optional description for the transaction category.");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TransactionCategories",
                table: "TransactionCategories",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Transactions_TransactionCategories_CategoryId",
                table: "Transactions",
                column: "CategoryId",
                principalTable: "TransactionCategories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
