using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EBook.Web.Data.Migrations
{
    public partial class SeedDataUpdated : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedDateTime", "DisplayOrder" },
                values: new object[] { new DateTime(2022, 2, 4, 12, 22, 56, 311, DateTimeKind.Local).AddTicks(9514), 5 });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedDateTime", "DisplayOrder" },
                values: new object[] { new DateTime(2022, 2, 3, 20, 23, 41, 487, DateTimeKind.Local).AddTicks(2343), 0 });
        }
    }
}
