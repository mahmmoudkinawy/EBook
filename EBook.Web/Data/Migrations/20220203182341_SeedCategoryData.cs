using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EBook.Web.Data.Migrations
{
    public partial class SeedCategoryData : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Id", "CreatedDateTime", "DisplayOrder", "Name" },
                values: new object[] { 1, new DateTime(2022, 2, 3, 20, 23, 41, 487, DateTimeKind.Local).AddTicks(2343), 0, "Test 1" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 1);
        }
    }
}
