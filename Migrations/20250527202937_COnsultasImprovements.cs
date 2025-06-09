using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Turnos31.Migrations
{
    /// <inheritdoc />
    public partial class COnsultasImprovements : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Consultas_Turnos_IdTurno",
                table: "Consultas");

            migrationBuilder.DropForeignKey(
                name: "FK_Turnos_Mascotas_IdMascota",
                table: "Turnos");

            migrationBuilder.DropForeignKey(
                name: "FK_Turnos_Veterinarios_IdVeterinario",
                table: "Turnos");

            migrationBuilder.DropIndex(
                name: "IX_Turnos_IdMascota",
                table: "Turnos");

            migrationBuilder.DropColumn(
                name: "Descripcion",
                table: "Turnos");

            migrationBuilder.DropColumn(
                name: "FechaHoraFin",
                table: "Turnos");

            migrationBuilder.DropColumn(
                name: "FechaHoraInicio",
                table: "Turnos");

            migrationBuilder.DropColumn(
                name: "FechaReserva",
                table: "Turnos");

            migrationBuilder.RenameColumn(
                name: "IdMascota",
                table: "Turnos",
                newName: "DuracionConsulta");

            migrationBuilder.RenameColumn(
                name: "Estado",
                table: "Turnos",
                newName: "DiaSemana");

            migrationBuilder.RenameColumn(
                name: "IdTurno",
                table: "Consultas",
                newName: "IdAgenda");

            migrationBuilder.RenameIndex(
                name: "IX_Consultas_IdTurno",
                table: "Consultas",
                newName: "IX_Consultas_IdAgenda");

            migrationBuilder.AlterColumn<string>(
                name: "Observaciones",
                table: "Turnos",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(500)",
                oldMaxLength: 500,
                oldNullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "Activo",
                table: "Turnos",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<TimeSpan>(
                name: "HoraFin",
                table: "Turnos",
                type: "time",
                nullable: false,
                defaultValue: new TimeSpan(0, 0, 0, 0, 0));

            migrationBuilder.AddColumn<TimeSpan>(
                name: "HoraInicio",
                table: "Turnos",
                type: "time",
                nullable: false,
                defaultValue: new TimeSpan(0, 0, 0, 0, 0));

            migrationBuilder.AlterColumn<string>(
                name: "Tratamiento",
                table: "Consultas",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(1000)",
                oldMaxLength: 1000,
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "Agendas",
                columns: table => new
                {
                    IdAgenda = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdMascota = table.Column<int>(type: "int", nullable: false),
                    IdVeterinario = table.Column<int>(type: "int", nullable: false),
                    FechaHoraInicio = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FechaHoraFin = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Estado = table.Column<int>(type: "int", nullable: false),
                    Descripcion = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    Observaciones = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    FechaReserva = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TipoConsulta = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MotivoVisita = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    EsUrgente = table.Column<bool>(type: "bit", nullable: false),
                    MascotaIdMascota = table.Column<int>(type: "int", nullable: false),
                    VeterinarioIdVeterinario = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Agendas", x => x.IdAgenda);
                    table.ForeignKey(
                        name: "FK_Agendas_Mascotas_MascotaIdMascota",
                        column: x => x.MascotaIdMascota,
                        principalTable: "Mascotas",
                        principalColumn: "IdMascota",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Agendas_Veterinarios_VeterinarioIdVeterinario",
                        column: x => x.VeterinarioIdVeterinario,
                        principalTable: "Veterinarios",
                        principalColumn: "IdVeterinario",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Agendas_MascotaIdMascota",
                table: "Agendas",
                column: "MascotaIdMascota");

            migrationBuilder.CreateIndex(
                name: "IX_Agendas_VeterinarioIdVeterinario",
                table: "Agendas",
                column: "VeterinarioIdVeterinario");

            migrationBuilder.AddForeignKey(
                name: "FK_Consultas_Agendas_IdAgenda",
                table: "Consultas",
                column: "IdAgenda",
                principalTable: "Agendas",
                principalColumn: "IdAgenda");

            migrationBuilder.AddForeignKey(
                name: "FK_Turnos_Veterinarios_IdVeterinario",
                table: "Turnos",
                column: "IdVeterinario",
                principalTable: "Veterinarios",
                principalColumn: "IdVeterinario");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Consultas_Agendas_IdAgenda",
                table: "Consultas");

            migrationBuilder.DropForeignKey(
                name: "FK_Turnos_Veterinarios_IdVeterinario",
                table: "Turnos");

            migrationBuilder.DropTable(
                name: "Agendas");

            migrationBuilder.DropColumn(
                name: "Activo",
                table: "Turnos");

            migrationBuilder.DropColumn(
                name: "HoraFin",
                table: "Turnos");

            migrationBuilder.DropColumn(
                name: "HoraInicio",
                table: "Turnos");

            migrationBuilder.RenameColumn(
                name: "DuracionConsulta",
                table: "Turnos",
                newName: "IdMascota");

            migrationBuilder.RenameColumn(
                name: "DiaSemana",
                table: "Turnos",
                newName: "Estado");

            migrationBuilder.RenameColumn(
                name: "IdAgenda",
                table: "Consultas",
                newName: "IdTurno");

            migrationBuilder.RenameIndex(
                name: "IX_Consultas_IdAgenda",
                table: "Consultas",
                newName: "IX_Consultas_IdTurno");

            migrationBuilder.AlterColumn<string>(
                name: "Observaciones",
                table: "Turnos",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(500)",
                oldMaxLength: 500);

            migrationBuilder.AddColumn<string>(
                name: "Descripcion",
                table: "Turnos",
                type: "varchar(MAX)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "FechaHoraFin",
                table: "Turnos",
                type: "datetime",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "FechaHoraInicio",
                table: "Turnos",
                type: "datetime",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "FechaReserva",
                table: "Turnos",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AlterColumn<string>(
                name: "Tratamiento",
                table: "Consultas",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(1000)",
                oldMaxLength: 1000);

            migrationBuilder.CreateIndex(
                name: "IX_Turnos_IdMascota",
                table: "Turnos",
                column: "IdMascota");

            migrationBuilder.AddForeignKey(
                name: "FK_Consultas_Turnos_IdTurno",
                table: "Consultas",
                column: "IdTurno",
                principalTable: "Turnos",
                principalColumn: "IdTurno");

            migrationBuilder.AddForeignKey(
                name: "FK_Turnos_Mascotas_IdMascota",
                table: "Turnos",
                column: "IdMascota",
                principalTable: "Mascotas",
                principalColumn: "IdMascota",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Turnos_Veterinarios_IdVeterinario",
                table: "Turnos",
                column: "IdVeterinario",
                principalTable: "Veterinarios",
                principalColumn: "IdVeterinario",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
