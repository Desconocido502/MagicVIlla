using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace MagicVilla_API.Migrations
{
    /// <inheritdoc />
    public partial class AlimentarTablaVilla : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Villas",
                columns: new[] { "Id", "Amenidad", "Detalle", "FechaCreacion", "ImagenUrl", "MetrosCudrados", "Name", "Ocupantes", "Tarifa", "fechaActualizacion" },
                values: new object[,]
                {
                    { 1, "", "Detalle de la Villa...", new DateTime(2023, 11, 29, 22, 56, 35, 763, DateTimeKind.Local).AddTicks(764), "", 50, "Villa Real", 5, 200.0, new DateTime(2023, 11, 29, 22, 56, 35, 763, DateTimeKind.Local).AddTicks(789) },
                    { 2, "", "Detalle de la Villa...", new DateTime(2023, 11, 29, 22, 56, 35, 763, DateTimeKind.Local).AddTicks(792), "", 40, "Villa Vista a la Piscina", 4, 150.0, new DateTime(2023, 11, 29, 22, 56, 35, 763, DateTimeKind.Local).AddTicks(793) }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Villas",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Villas",
                keyColumn: "Id",
                keyValue: 2);
        }
    }
}
