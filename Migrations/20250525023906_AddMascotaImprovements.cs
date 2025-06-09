using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Turnos31.Migrations
{
    /// <inheritdoc />
    public partial class AddMascotaImprovements : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Color",
                table: "Mascotas",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(MAX)",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NumeroMicrochip",
                table: "Mascotas",
                type: "nvarchar(25)",
                maxLength: 25,
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "Peso",
                table: "Mascotas",
                type: "decimal(5,2)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Tamaño",
                table: "Mascotas",
                type: "varchar(20)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NumeroMicrochip",
                table: "Mascotas");

            migrationBuilder.DropColumn(
                name: "Peso",
                table: "Mascotas");

            migrationBuilder.DropColumn(
                name: "Tamaño",
                table: "Mascotas");

            migrationBuilder.AlterColumn<string>(
                name: "Color",
                table: "Mascotas",
                type: "varchar(MAX)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);
        }
    }
}
