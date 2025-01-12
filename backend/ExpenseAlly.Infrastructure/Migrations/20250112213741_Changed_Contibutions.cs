using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ExpenseAlly.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Changed_Contibutions : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "CreatedBy",
                table: "Contributions",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedOn",
                table: "Contributions",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<Guid>(
                name: "LastModifiedBy",
                table: "Contributions",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastModifiedOn",
                table: "Contributions",
                type: "datetime2",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "Contributions");

            migrationBuilder.DropColumn(
                name: "CreatedOn",
                table: "Contributions");

            migrationBuilder.DropColumn(
                name: "LastModifiedBy",
                table: "Contributions");

            migrationBuilder.DropColumn(
                name: "LastModifiedOn",
                table: "Contributions");
        }
    }
}
