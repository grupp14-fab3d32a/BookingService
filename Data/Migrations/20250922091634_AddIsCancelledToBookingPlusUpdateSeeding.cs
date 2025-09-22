using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Data.Migrations
{
    /// <inheritdoc />
    public partial class AddIsCancelledToBookingPlusUpdateSeeding : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsCancelled",
                table: "Bookings",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "Id",
                keyValue: new Guid("11111111-1111-1111-1111-111111111111"),
                column: "IsCancelled",
                value: false);

            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "Id",
                keyValue: new Guid("44444444-4444-4444-4444-444444444444"),
                column: "IsCancelled",
                value: false);

            migrationBuilder.InsertData(
                table: "Bookings",
                columns: new[] { "Id", "CreatedAt", "IsCancelled", "MemberId", "WorkoutId" },
                values: new object[] { new Guid("44444444-4444-4444-8888-444444444444"), new DateTime(2025, 9, 1, 20, 0, 0, 0, DateTimeKind.Utc), true, new Guid("55555555-5555-8888-5555-555555555555"), new Guid("66666666-6666-8888-6666-666666666666") });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Bookings",
                keyColumn: "Id",
                keyValue: new Guid("44444444-4444-4444-8888-444444444444"));

            migrationBuilder.DropColumn(
                name: "IsCancelled",
                table: "Bookings");
        }
    }
}
