using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VillaApi.Migrations
{
    /// <inheritdoc />
    public partial class RenameSpeacialDetailsColumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "SpeacialDetails",
                table: "VillaNumbers",
                newName: "SpecialDetails");

            migrationBuilder.RenameColumn(
                name: "CreadteDate",
                table: "VillaNumbers",
                newName: "CreatedDate");

            migrationBuilder.UpdateData(
                table: "Villas",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedDate",
                value: new DateTime(2025, 5, 7, 1, 34, 48, 968, DateTimeKind.Local).AddTicks(4654));

            migrationBuilder.UpdateData(
                table: "Villas",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedDate",
                value: new DateTime(2025, 5, 7, 1, 34, 48, 968, DateTimeKind.Local).AddTicks(4674));

            migrationBuilder.UpdateData(
                table: "Villas",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedDate",
                value: new DateTime(2025, 5, 7, 1, 34, 48, 968, DateTimeKind.Local).AddTicks(4677));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "SpecialDetails",
                table: "VillaNumbers",
                newName: "SpeacialDetails");

            migrationBuilder.RenameColumn(
                name: "CreatedDate",
                table: "VillaNumbers",
                newName: "CreadteDate");

            migrationBuilder.UpdateData(
                table: "Villas",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedDate",
                value: new DateTime(2025, 5, 5, 23, 21, 33, 892, DateTimeKind.Local).AddTicks(2102));

            migrationBuilder.UpdateData(
                table: "Villas",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedDate",
                value: new DateTime(2025, 5, 5, 23, 21, 33, 892, DateTimeKind.Local).AddTicks(2150));

            migrationBuilder.UpdateData(
                table: "Villas",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedDate",
                value: new DateTime(2025, 5, 5, 23, 21, 33, 892, DateTimeKind.Local).AddTicks(2725));
        }
    }
}
