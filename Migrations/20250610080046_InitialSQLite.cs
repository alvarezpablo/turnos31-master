using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Turnos31.Migrations
{
    /// <inheritdoc />
    public partial class InitialSQLite : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Categorias",
                columns: table => new
                {
                    IdCategoria = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Nombre = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categorias", x => x.IdCategoria);
                });

            migrationBuilder.CreateTable(
                name: "Duenos",
                columns: table => new
                {
                    IdDueno = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Nombre = table.Column<string>(type: "varchar(100)", nullable: false),
                    Apellido = table.Column<string>(type: "varchar(100)", nullable: false),
                    Direccion = table.Column<string>(type: "varchar(200)", nullable: false),
                    Rut = table.Column<string>(type: "varchar(20)", nullable: false),
                    Telefono = table.Column<string>(type: "varchar(20)", nullable: false),
                    Email = table.Column<string>(type: "varchar(100)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Duenos", x => x.IdDueno);
                });

            migrationBuilder.CreateTable(
                name: "Especialidades",
                columns: table => new
                {
                    IdEspecialidad = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Descripcion = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Especialidades", x => x.IdEspecialidad);
                });

            migrationBuilder.CreateTable(
                name: "Especies",
                columns: table => new
                {
                    IdEspecie = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Nombre = table.Column<string>(type: "varchar(100)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Especies", x => x.IdEspecie);
                });

            migrationBuilder.CreateTable(
                name: "Login",
                columns: table => new
                {
                    LoginId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Usuario = table.Column<string>(type: "TEXT", nullable: false),
                    Password = table.Column<string>(type: "TEXT", nullable: false),
                    Rol = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Login", x => x.LoginId);
                });

            migrationBuilder.CreateTable(
                name: "Menus",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Nombre = table.Column<string>(type: "TEXT", nullable: true),
                    Url = table.Column<string>(type: "TEXT", nullable: true),
                    Icono = table.Column<string>(type: "TEXT", nullable: true),
                    MenuPadreId = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Menus", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Menus_Menus_MenuPadreId",
                        column: x => x.MenuPadreId,
                        principalTable: "Menus",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "MotivoVisitas",
                columns: table => new
                {
                    IdMotivoVisita = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Nombre = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MotivoVisitas", x => x.IdMotivoVisita);
                });

            migrationBuilder.CreateTable(
                name: "NivelUrgencias",
                columns: table => new
                {
                    IdNivelUrgencia = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Nombre = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NivelUrgencias", x => x.IdNivelUrgencia);
                });

            migrationBuilder.CreateTable(
                name: "Roles",
                columns: table => new
                {
                    IdRol = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    NombreRol = table.Column<string>(type: "TEXT", nullable: true),
                    Descripcion = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.IdRol);
                });

            migrationBuilder.CreateTable(
                name: "TipoServicios",
                columns: table => new
                {
                    IdTipoServicio = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Nombre = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TipoServicios", x => x.IdTipoServicio);
                });

            migrationBuilder.CreateTable(
                name: "Veterinarios",
                columns: table => new
                {
                    IdVeterinario = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Nombre = table.Column<string>(type: "varchar(100)", nullable: false),
                    Apellido = table.Column<string>(type: "varchar(100)", nullable: false),
                    Direccion = table.Column<string>(type: "varchar(200)", nullable: false),
                    Telefono = table.Column<string>(type: "varchar(20)", nullable: false),
                    Email = table.Column<string>(type: "varchar(100)", nullable: false),
                    HorarioAtencionDesde = table.Column<DateTime>(type: "TEXT", nullable: false),
                    HorarioAtencionHasta = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Veterinarios", x => x.IdVeterinario);
                });

            migrationBuilder.CreateTable(
                name: "Productos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Nombre = table.Column<string>(type: "varchar(MAX)", nullable: false),
                    Costo = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Precio = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Stock = table.Column<int>(type: "INTEGER", nullable: false),
                    Proveedor = table.Column<string>(type: "varchar(MAX)", nullable: true),
                    EstadoProducto = table.Column<string>(type: "varchar(MAX)", nullable: true),
                    CategoriaId = table.Column<int>(type: "INTEGER", nullable: false),
                    Foto = table.Column<string>(type: "varchar(MAX)", nullable: true),
                    Status = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Productos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Productos_Categorias_CategoriaId",
                        column: x => x.CategoriaId,
                        principalTable: "Categorias",
                        principalColumn: "IdCategoria");
                });

            migrationBuilder.CreateTable(
                name: "Razas",
                columns: table => new
                {
                    IdRaza = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Nombre = table.Column<string>(type: "varchar(100)", nullable: false),
                    IdEspecie = table.Column<int>(type: "INTEGER", nullable: false),
                    Activo = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Razas", x => x.IdRaza);
                    table.ForeignKey(
                        name: "FK_Razas_Especies_IdEspecie",
                        column: x => x.IdEspecie,
                        principalTable: "Especies",
                        principalColumn: "IdEspecie",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MenuRoles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    MenuId = table.Column<int>(type: "INTEGER", nullable: false),
                    RolId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MenuRoles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MenuRoles_Menus_MenuId",
                        column: x => x.MenuId,
                        principalTable: "Menus",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MenuRoles_Roles_RolId",
                        column: x => x.RolId,
                        principalTable: "Roles",
                        principalColumn: "IdRol",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Usuarios",
                columns: table => new
                {
                    IdUsuario = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Nombre = table.Column<string>(type: "varchar(100)", nullable: false),
                    Apellido = table.Column<string>(type: "varchar(100)", nullable: false),
                    Email = table.Column<string>(type: "varchar(100)", nullable: false),
                    Password = table.Column<string>(type: "varchar(100)", nullable: false),
                    IdRol = table.Column<int>(type: "INTEGER", nullable: false),
                    Activo = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Usuarios", x => x.IdUsuario);
                    table.ForeignKey(
                        name: "FK_Usuarios_Roles_IdRol",
                        column: x => x.IdRol,
                        principalTable: "Roles",
                        principalColumn: "IdRol",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Turnos",
                columns: table => new
                {
                    IdTurno = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    IdVeterinario = table.Column<int>(type: "INTEGER", nullable: false),
                    DiaSemana = table.Column<int>(type: "INTEGER", nullable: false),
                    HoraInicio = table.Column<TimeSpan>(type: "TEXT", nullable: false),
                    HoraFin = table.Column<TimeSpan>(type: "TEXT", nullable: false),
                    DuracionConsulta = table.Column<int>(type: "INTEGER", nullable: false),
                    Activo = table.Column<bool>(type: "INTEGER", nullable: false),
                    Observaciones = table.Column<string>(type: "TEXT", maxLength: 500, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Turnos", x => x.IdTurno);
                    table.ForeignKey(
                        name: "FK_Turnos_Veterinarios_IdVeterinario",
                        column: x => x.IdVeterinario,
                        principalTable: "Veterinarios",
                        principalColumn: "IdVeterinario");
                });

            migrationBuilder.CreateTable(
                name: "VeterinariosEspecialidades",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    IdVeterinario = table.Column<int>(type: "INTEGER", nullable: false),
                    IdEspecialidad = table.Column<int>(type: "INTEGER", nullable: false),
                    Status = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VeterinariosEspecialidades", x => x.Id);
                    table.ForeignKey(
                        name: "FK_VeterinariosEspecialidades_Especialidades_IdEspecialidad",
                        column: x => x.IdEspecialidad,
                        principalTable: "Especialidades",
                        principalColumn: "IdEspecialidad");
                    table.ForeignKey(
                        name: "FK_VeterinariosEspecialidades_Veterinarios_IdVeterinario",
                        column: x => x.IdVeterinario,
                        principalTable: "Veterinarios",
                        principalColumn: "IdVeterinario");
                });

            migrationBuilder.CreateTable(
                name: "Mascotas",
                columns: table => new
                {
                    IdMascota = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Nombre = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    IdEspecie = table.Column<int>(type: "INTEGER", nullable: false),
                    IdRaza = table.Column<int>(type: "INTEGER", nullable: false),
                    FechaNacimiento = table.Column<DateTime>(type: "TEXT", nullable: true),
                    Sexo = table.Column<string>(type: "nvarchar(1)", nullable: false),
                    Color = table.Column<string>(type: "TEXT", nullable: true),
                    NumeroMicrochip = table.Column<string>(type: "TEXT", maxLength: 25, nullable: true),
                    Peso = table.Column<decimal>(type: "decimal(5,2)", nullable: true),
                    Tamaño = table.Column<string>(type: "varchar(20)", nullable: true),
                    Pelaje = table.Column<string>(type: "varchar(MAX)", nullable: true),
                    Alergia = table.Column<string>(type: "varchar(MAX)", nullable: true),
                    Observaciones = table.Column<string>(type: "varchar(MAX)", nullable: true),
                    IdDueno = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Mascotas", x => x.IdMascota);
                    table.ForeignKey(
                        name: "FK_Mascotas_Duenos_IdDueno",
                        column: x => x.IdDueno,
                        principalTable: "Duenos",
                        principalColumn: "IdDueno");
                    table.ForeignKey(
                        name: "FK_Mascotas_Especies_IdEspecie",
                        column: x => x.IdEspecie,
                        principalTable: "Especies",
                        principalColumn: "IdEspecie");
                    table.ForeignKey(
                        name: "FK_Mascotas_Razas_IdRaza",
                        column: x => x.IdRaza,
                        principalTable: "Razas",
                        principalColumn: "IdRaza");
                });

            migrationBuilder.CreateTable(
                name: "Agendas",
                columns: table => new
                {
                    IdAgenda = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    IdMascota = table.Column<int>(type: "INTEGER", nullable: false),
                    IdVeterinario = table.Column<int>(type: "INTEGER", nullable: false),
                    FechaHoraInicio = table.Column<DateTime>(type: "TEXT", nullable: false),
                    FechaHoraFin = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Estado = table.Column<int>(type: "INTEGER", nullable: false),
                    Descripcion = table.Column<string>(type: "TEXT", maxLength: 500, nullable: false),
                    Observaciones = table.Column<string>(type: "TEXT", maxLength: 500, nullable: false),
                    FechaReserva = table.Column<DateTime>(type: "TEXT", nullable: false),
                    TipoConsulta = table.Column<string>(type: "TEXT", nullable: false),
                    MotivoVisita = table.Column<string>(type: "TEXT", maxLength: 200, nullable: false),
                    EsUrgente = table.Column<bool>(type: "INTEGER", nullable: false),
                    MascotaIdMascota = table.Column<int>(type: "INTEGER", nullable: false),
                    VeterinarioIdVeterinario = table.Column<int>(type: "INTEGER", nullable: false)
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

            migrationBuilder.CreateTable(
                name: "FichasIngreso",
                columns: table => new
                {
                    IdFichaIngreso = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    IdDueno = table.Column<int>(type: "INTEGER", nullable: false),
                    IdMascota = table.Column<int>(type: "INTEGER", nullable: false),
                    FechaHoraIngreso = table.Column<DateTime>(type: "TEXT", nullable: false),
                    IdNivelUrgencia = table.Column<int>(type: "INTEGER", nullable: false),
                    IdMotivoVisita = table.Column<int>(type: "INTEGER", nullable: false),
                    IdTipoServicio = table.Column<int>(type: "INTEGER", nullable: false),
                    Observaciones = table.Column<string>(type: "TEXT", maxLength: 500, nullable: true),
                    Estado = table.Column<int>(type: "INTEGER", nullable: false),
                    FechaActualizacion = table.Column<DateTime>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FichasIngreso", x => x.IdFichaIngreso);
                    table.ForeignKey(
                        name: "FK_FichasIngreso_Duenos_IdDueno",
                        column: x => x.IdDueno,
                        principalTable: "Duenos",
                        principalColumn: "IdDueno",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FichasIngreso_Mascotas_IdMascota",
                        column: x => x.IdMascota,
                        principalTable: "Mascotas",
                        principalColumn: "IdMascota",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FichasIngreso_MotivoVisitas_IdMotivoVisita",
                        column: x => x.IdMotivoVisita,
                        principalTable: "MotivoVisitas",
                        principalColumn: "IdMotivoVisita",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FichasIngreso_NivelUrgencias_IdNivelUrgencia",
                        column: x => x.IdNivelUrgencia,
                        principalTable: "NivelUrgencias",
                        principalColumn: "IdNivelUrgencia",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FichasIngreso_TipoServicios_IdTipoServicio",
                        column: x => x.IdTipoServicio,
                        principalTable: "TipoServicios",
                        principalColumn: "IdTipoServicio",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Consultas",
                columns: table => new
                {
                    IdConsulta = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    IdAgenda = table.Column<int>(type: "INTEGER", nullable: false),
                    Motivo = table.Column<string>(type: "TEXT", maxLength: 500, nullable: false),
                    Diagnostico = table.Column<string>(type: "TEXT", maxLength: 1000, nullable: false),
                    Tratamiento = table.Column<string>(type: "TEXT", maxLength: 1000, nullable: true),
                    Observaciones = table.Column<string>(type: "TEXT", maxLength: 1000, nullable: true),
                    FechaHora = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Peso = table.Column<decimal>(type: "decimal(5,2)", precision: 5, scale: 2, nullable: true),
                    Temperatura = table.Column<decimal>(type: "decimal(5,2)", precision: 5, scale: 2, nullable: true),
                    FrecuenciaCardiaca = table.Column<decimal>(type: "decimal(5,2)", precision: 5, scale: 2, nullable: true),
                    FrecuenciaRespiratoria = table.Column<decimal>(type: "decimal(5,2)", precision: 5, scale: 2, nullable: true),
                    MascotaIdMascota = table.Column<int>(type: "INTEGER", nullable: true),
                    VeterinarioIdVeterinario = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Consultas", x => x.IdConsulta);
                    table.ForeignKey(
                        name: "FK_Consultas_Agendas_IdAgenda",
                        column: x => x.IdAgenda,
                        principalTable: "Agendas",
                        principalColumn: "IdAgenda");
                    table.ForeignKey(
                        name: "FK_Consultas_Mascotas_MascotaIdMascota",
                        column: x => x.MascotaIdMascota,
                        principalTable: "Mascotas",
                        principalColumn: "IdMascota");
                    table.ForeignKey(
                        name: "FK_Consultas_Veterinarios_VeterinarioIdVeterinario",
                        column: x => x.VeterinarioIdVeterinario,
                        principalTable: "Veterinarios",
                        principalColumn: "IdVeterinario");
                });

            migrationBuilder.CreateTable(
                name: "Diagnosticos",
                columns: table => new
                {
                    IdDiagnostico = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Detalle = table.Column<string>(type: "varchar(MAX)", nullable: true),
                    FechaDiagnostico = table.Column<DateTime>(type: "date", nullable: true),
                    IdConsulta = table.Column<int>(type: "INTEGER", nullable: false),
                    Status = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Diagnosticos", x => x.IdDiagnostico);
                    table.ForeignKey(
                        name: "FK_Diagnosticos_Consultas_IdConsulta",
                        column: x => x.IdConsulta,
                        principalTable: "Consultas",
                        principalColumn: "IdConsulta");
                });

            migrationBuilder.CreateTable(
                name: "Examenes",
                columns: table => new
                {
                    IdExamen = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Mucosa = table.Column<string>(type: "varchar(MAX)", nullable: true),
                    Piel = table.Column<string>(type: "varchar(MAX)", nullable: true),
                    Conjuntival = table.Column<string>(type: "varchar(MAX)", nullable: true),
                    Oral = table.Column<string>(type: "varchar(MAX)", nullable: true),
                    SistemaReproductor = table.Column<string>(type: "varchar(MAX)", nullable: true),
                    Rectal = table.Column<string>(type: "varchar(MAX)", nullable: true),
                    Ojos = table.Column<string>(type: "varchar(MAX)", nullable: true),
                    NodulosLinfaticos = table.Column<string>(type: "varchar(MAX)", nullable: true),
                    Locomocion = table.Column<string>(type: "varchar(MAX)", nullable: true),
                    SistemaCardiovascular = table.Column<string>(type: "varchar(MAX)", nullable: true),
                    SistemaRespiratorio = table.Column<string>(type: "varchar(MAX)", nullable: true),
                    SistemaDigestivo = table.Column<string>(type: "varchar(MAX)", nullable: true),
                    SistemaUrinario = table.Column<string>(type: "varchar(MAX)", nullable: true),
                    IdConsulta = table.Column<int>(type: "INTEGER", nullable: false),
                    IdMascota = table.Column<int>(type: "INTEGER", nullable: false),
                    IdVeterinario = table.Column<int>(type: "INTEGER", nullable: false),
                    Status = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Examenes", x => x.IdExamen);
                    table.ForeignKey(
                        name: "FK_Examenes_Consultas_IdConsulta",
                        column: x => x.IdConsulta,
                        principalTable: "Consultas",
                        principalColumn: "IdConsulta",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Examenes_Mascotas_IdMascota",
                        column: x => x.IdMascota,
                        principalTable: "Mascotas",
                        principalColumn: "IdMascota");
                    table.ForeignKey(
                        name: "FK_Examenes_Veterinarios_IdVeterinario",
                        column: x => x.IdVeterinario,
                        principalTable: "Veterinarios",
                        principalColumn: "IdVeterinario");
                });

            migrationBuilder.CreateTable(
                name: "Tratamientos",
                columns: table => new
                {
                    IdTratamiento = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Detalle = table.Column<string>(type: "varchar(MAX)", nullable: true),
                    IdConsulta = table.Column<int>(type: "INTEGER", nullable: false),
                    Status = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tratamientos", x => x.IdTratamiento);
                    table.ForeignKey(
                        name: "FK_Tratamientos_Consultas_IdConsulta",
                        column: x => x.IdConsulta,
                        principalTable: "Consultas",
                        principalColumn: "IdConsulta");
                });

            migrationBuilder.CreateTable(
                name: "ResultadosExamenes",
                columns: table => new
                {
                    IdResultado = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    IdConsulta = table.Column<int>(type: "INTEGER", nullable: false),
                    IdExamen = table.Column<int>(type: "INTEGER", nullable: false),
                    Resultado = table.Column<string>(type: "varchar(MAX)", nullable: false),
                    FechaRealizacion = table.Column<DateTime>(type: "datetime", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ResultadosExamenes", x => x.IdResultado);
                    table.ForeignKey(
                        name: "FK_ResultadosExamenes_Consultas_IdConsulta",
                        column: x => x.IdConsulta,
                        principalTable: "Consultas",
                        principalColumn: "IdConsulta",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ResultadosExamenes_Examenes_IdExamen",
                        column: x => x.IdExamen,
                        principalTable: "Examenes",
                        principalColumn: "IdExamen");
                });

            migrationBuilder.CreateTable(
                name: "ProductosTratamientos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    IdTratamiento = table.Column<int>(type: "INTEGER", nullable: false),
                    IdProducto = table.Column<int>(type: "INTEGER", nullable: false),
                    Status = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductosTratamientos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductosTratamientos_Productos_IdProducto",
                        column: x => x.IdProducto,
                        principalTable: "Productos",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ProductosTratamientos_Tratamientos_IdTratamiento",
                        column: x => x.IdTratamiento,
                        principalTable: "Tratamientos",
                        principalColumn: "IdTratamiento");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Agendas_MascotaIdMascota",
                table: "Agendas",
                column: "MascotaIdMascota");

            migrationBuilder.CreateIndex(
                name: "IX_Agendas_VeterinarioIdVeterinario",
                table: "Agendas",
                column: "VeterinarioIdVeterinario");

            migrationBuilder.CreateIndex(
                name: "IX_Consultas_IdAgenda",
                table: "Consultas",
                column: "IdAgenda");

            migrationBuilder.CreateIndex(
                name: "IX_Consultas_MascotaIdMascota",
                table: "Consultas",
                column: "MascotaIdMascota");

            migrationBuilder.CreateIndex(
                name: "IX_Consultas_VeterinarioIdVeterinario",
                table: "Consultas",
                column: "VeterinarioIdVeterinario");

            migrationBuilder.CreateIndex(
                name: "IX_Diagnosticos_IdConsulta",
                table: "Diagnosticos",
                column: "IdConsulta");

            migrationBuilder.CreateIndex(
                name: "IX_Examenes_IdConsulta",
                table: "Examenes",
                column: "IdConsulta");

            migrationBuilder.CreateIndex(
                name: "IX_Examenes_IdMascota",
                table: "Examenes",
                column: "IdMascota");

            migrationBuilder.CreateIndex(
                name: "IX_Examenes_IdVeterinario",
                table: "Examenes",
                column: "IdVeterinario");

            migrationBuilder.CreateIndex(
                name: "IX_FichasIngreso_IdDueno",
                table: "FichasIngreso",
                column: "IdDueno");

            migrationBuilder.CreateIndex(
                name: "IX_FichasIngreso_IdMascota",
                table: "FichasIngreso",
                column: "IdMascota");

            migrationBuilder.CreateIndex(
                name: "IX_FichasIngreso_IdMotivoVisita",
                table: "FichasIngreso",
                column: "IdMotivoVisita");

            migrationBuilder.CreateIndex(
                name: "IX_FichasIngreso_IdNivelUrgencia",
                table: "FichasIngreso",
                column: "IdNivelUrgencia");

            migrationBuilder.CreateIndex(
                name: "IX_FichasIngreso_IdTipoServicio",
                table: "FichasIngreso",
                column: "IdTipoServicio");

            migrationBuilder.CreateIndex(
                name: "IX_Mascotas_IdDueno",
                table: "Mascotas",
                column: "IdDueno");

            migrationBuilder.CreateIndex(
                name: "IX_Mascotas_IdEspecie",
                table: "Mascotas",
                column: "IdEspecie");

            migrationBuilder.CreateIndex(
                name: "IX_Mascotas_IdRaza",
                table: "Mascotas",
                column: "IdRaza");

            migrationBuilder.CreateIndex(
                name: "IX_MenuRoles_MenuId",
                table: "MenuRoles",
                column: "MenuId");

            migrationBuilder.CreateIndex(
                name: "IX_MenuRoles_RolId",
                table: "MenuRoles",
                column: "RolId");

            migrationBuilder.CreateIndex(
                name: "IX_Menus_MenuPadreId",
                table: "Menus",
                column: "MenuPadreId");

            migrationBuilder.CreateIndex(
                name: "IX_Productos_CategoriaId",
                table: "Productos",
                column: "CategoriaId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductosTratamientos_IdProducto",
                table: "ProductosTratamientos",
                column: "IdProducto");

            migrationBuilder.CreateIndex(
                name: "IX_ProductosTratamientos_IdTratamiento",
                table: "ProductosTratamientos",
                column: "IdTratamiento");

            migrationBuilder.CreateIndex(
                name: "IX_Razas_IdEspecie",
                table: "Razas",
                column: "IdEspecie");

            migrationBuilder.CreateIndex(
                name: "IX_ResultadosExamenes_IdConsulta",
                table: "ResultadosExamenes",
                column: "IdConsulta");

            migrationBuilder.CreateIndex(
                name: "IX_ResultadosExamenes_IdExamen",
                table: "ResultadosExamenes",
                column: "IdExamen");

            migrationBuilder.CreateIndex(
                name: "IX_Tratamientos_IdConsulta",
                table: "Tratamientos",
                column: "IdConsulta");

            migrationBuilder.CreateIndex(
                name: "IX_Turnos_IdVeterinario",
                table: "Turnos",
                column: "IdVeterinario");

            migrationBuilder.CreateIndex(
                name: "IX_Usuarios_Email",
                table: "Usuarios",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Usuarios_IdRol",
                table: "Usuarios",
                column: "IdRol");

            migrationBuilder.CreateIndex(
                name: "IX_VeterinariosEspecialidades_IdEspecialidad",
                table: "VeterinariosEspecialidades",
                column: "IdEspecialidad");

            migrationBuilder.CreateIndex(
                name: "IX_VeterinariosEspecialidades_IdVeterinario",
                table: "VeterinariosEspecialidades",
                column: "IdVeterinario");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Diagnosticos");

            migrationBuilder.DropTable(
                name: "FichasIngreso");

            migrationBuilder.DropTable(
                name: "Login");

            migrationBuilder.DropTable(
                name: "MenuRoles");

            migrationBuilder.DropTable(
                name: "ProductosTratamientos");

            migrationBuilder.DropTable(
                name: "ResultadosExamenes");

            migrationBuilder.DropTable(
                name: "Turnos");

            migrationBuilder.DropTable(
                name: "Usuarios");

            migrationBuilder.DropTable(
                name: "VeterinariosEspecialidades");

            migrationBuilder.DropTable(
                name: "MotivoVisitas");

            migrationBuilder.DropTable(
                name: "NivelUrgencias");

            migrationBuilder.DropTable(
                name: "TipoServicios");

            migrationBuilder.DropTable(
                name: "Menus");

            migrationBuilder.DropTable(
                name: "Productos");

            migrationBuilder.DropTable(
                name: "Tratamientos");

            migrationBuilder.DropTable(
                name: "Examenes");

            migrationBuilder.DropTable(
                name: "Roles");

            migrationBuilder.DropTable(
                name: "Especialidades");

            migrationBuilder.DropTable(
                name: "Categorias");

            migrationBuilder.DropTable(
                name: "Consultas");

            migrationBuilder.DropTable(
                name: "Agendas");

            migrationBuilder.DropTable(
                name: "Mascotas");

            migrationBuilder.DropTable(
                name: "Veterinarios");

            migrationBuilder.DropTable(
                name: "Duenos");

            migrationBuilder.DropTable(
                name: "Razas");

            migrationBuilder.DropTable(
                name: "Especies");
        }
    }
}
