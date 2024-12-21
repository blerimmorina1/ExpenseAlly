using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ExpenseAlly.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateTransactionCategoryTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Type",
                table: "TransactionCategories",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Type",
                table: "TransactionCategories");
        }
    }
}
