using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Turnos31.Migrations
{
    /// <inheritdoc />
    public partial class AddActivoToFichaIngreso : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Nombre",
                table: "Razas",
                type: "varchar(200)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(100)");

            migrationBuilder.AlterColumn<bool>(
                name: "Activo",
                table: "Razas",
                type: "INTEGER",
                nullable: false,
                defaultValue: true,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.AddColumn<string>(
                name: "Descripcion",
                table: "Razas",
                type: "nvarchar(1000)",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Color",
                table: "Mascotas",
                type: "TEXT",
                nullable: true,
                defaultValue: "No especificado",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldNullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "Activo",
                table: "Mascotas",
                type: "INTEGER",
                nullable: false,
                defaultValue: true);

            migrationBuilder.AddColumn<int>(
                name: "IdCliente",
                table: "Mascotas",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "Activo",
                table: "FichasIngreso",
                type: "INTEGER",
                nullable: false,
                defaultValue: true);

            migrationBuilder.AddColumn<bool>(
                name: "Activo",
                table: "Especies",
                type: "INTEGER",
                nullable: false,
                defaultValue: true);

            migrationBuilder.AddColumn<string>(
                name: "Descripcion",
                table: "Especies",
                type: "nvarchar(1000)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "Activo",
                table: "Duenos",
                type: "INTEGER",
                nullable: false,
                defaultValue: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Descripcion",
                table: "Razas");

            migrationBuilder.DropColumn(
                name: "Activo",
                table: "Mascotas");

            migrationBuilder.DropColumn(
                name: "IdCliente",
                table: "Mascotas");

            migrationBuilder.DropColumn(
                name: "Activo",
                table: "FichasIngreso");

            migrationBuilder.DropColumn(
                name: "Activo",
                table: "Especies");

            migrationBuilder.DropColumn(
                name: "Descripcion",
                table: "Especies");

            migrationBuilder.DropColumn(
                name: "Activo",
                table: "Duenos");

            migrationBuilder.AlterColumn<string>(
                name: "Nombre",
                table: "Razas",
                type: "varchar(100)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(200)");

            migrationBuilder.AlterColumn<int>(
                name: "Activo",
                table: "Razas",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "INTEGER",
                oldDefaultValue: true);

            migrationBuilder.AlterColumn<string>(
                name: "Color",
                table: "Mascotas",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldNullable: true,
                oldDefaultValue: "No especificado");
        }
    }
}
