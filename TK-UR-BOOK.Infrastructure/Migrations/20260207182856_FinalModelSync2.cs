using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TK_UR_BOOK.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class FinalModelSync2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("4ca9451d-945d-4965-8a41-6716e90f790c"),
                column: "CreatedAt",
                value: new DateTime(2026, 2, 7, 0, 0, 0, 0, DateTimeKind.Utc));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("4ca9451d-945d-4965-8a41-6716e90f790c"),
                column: "CreatedAt",
                value: new DateTime(2026, 2, 7, 18, 16, 20, 826, DateTimeKind.Utc).AddTicks(198));
        }
    }
}
