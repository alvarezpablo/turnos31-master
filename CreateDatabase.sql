-- Create database
IF NOT EXISTS (SELECT * FROM sys.databases WHERE name = 'VeterinariaDb')
BEGIN
    CREATE DATABASE VeterinariaDb;
END
GO

USE VeterinariaDb;
GO

-- Create tables
CREATE TABLE Duenos (
    IdDueno INT IDENTITY(1,1) PRIMARY KEY,
    Nombre NVARCHAR(100) NOT NULL,
    Apellido NVARCHAR(100) NOT NULL,
    Direccion VARCHAR(MAX),
    Telefono VARCHAR(20),
    Email VARCHAR(100),
    Status INT DEFAULT 1
);

CREATE TABLE Especies (
    IdEspecie INT IDENTITY(1,1) PRIMARY KEY,
    Nombre VARCHAR(100) NOT NULL,
    Status INT DEFAULT 1
);

CREATE TABLE Razas (
    IdRaza INT IDENTITY(1,1) PRIMARY KEY,
    Nombre VARCHAR(100) NOT NULL,
    IdEspecie INT NOT NULL,
    Status INT DEFAULT 1,
    CONSTRAINT FK_Razas_Especies FOREIGN KEY (IdEspecie) REFERENCES Especies(IdEspecie) ON DELETE NO ACTION
);

CREATE TABLE Mascotas (
    IdMascota INT IDENTITY(1,1) PRIMARY KEY,
    Nombre NVARCHAR(100) NOT NULL,
    IdEspecie INT NOT NULL,
    IdRaza INT NOT NULL,
    FechaNacimiento DATETIME2 NULL,
    Sexo NVARCHAR(1) NOT NULL,
    Color VARCHAR(MAX) NULL,
    Pelaje VARCHAR(MAX) NULL,
    Alergia VARCHAR(MAX) NULL,
    Observaciones VARCHAR(MAX) NULL,
    IdDueno INT NOT NULL,
    Status INT DEFAULT 1,
    CONSTRAINT FK_Mascotas_Especies FOREIGN KEY (IdEspecie) REFERENCES Especies(IdEspecie) ON DELETE NO ACTION,
    CONSTRAINT FK_Mascotas_Razas FOREIGN KEY (IdRaza) REFERENCES Razas(IdRaza) ON DELETE NO ACTION,
    CONSTRAINT FK_Mascotas_Duenos FOREIGN KEY (IdDueno) REFERENCES Duenos(IdDueno) ON DELETE NO ACTION
);

CREATE TABLE Veterinarios (
    IdVeterinario INT IDENTITY(1,1) PRIMARY KEY,
    Nombre NVARCHAR(100) NOT NULL,
    Apellido NVARCHAR(100) NOT NULL,
    Direccion VARCHAR(MAX),
    Telefono VARCHAR(20),
    Email VARCHAR(100),
    HorarioAtencionDesde TIME,
    HorarioAtencionHasta TIME,
    Status INT DEFAULT 1
);

CREATE TABLE Especialidades (
    IdEspecialidad INT IDENTITY(1,1) PRIMARY KEY,
    Nombre VARCHAR(100) NOT NULL,
    Status INT DEFAULT 1
);

CREATE TABLE VeterinariosEspecialidades (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    IdVeterinario INT NOT NULL,
    IdEspecialidad INT NOT NULL,
    Status INT DEFAULT 1,
    CONSTRAINT FK_VetEsp_Veterinarios FOREIGN KEY (IdVeterinario) REFERENCES Veterinarios(IdVeterinario) ON DELETE NO ACTION,
    CONSTRAINT FK_VetEsp_Especialidades FOREIGN KEY (IdEspecialidad) REFERENCES Especialidades(IdEspecialidad) ON DELETE NO ACTION
);

CREATE TABLE Turnos (
    IdTurno INT IDENTITY(1,1) PRIMARY KEY,
    IdMascota INT NOT NULL,
    IdVeterinario INT NOT NULL,
    FechaHora DATETIME NOT NULL,
    Descripcion VARCHAR(MAX),
    Status INT DEFAULT 1,
    CONSTRAINT FK_Turnos_Mascotas FOREIGN KEY (IdMascota) REFERENCES Mascotas(IdMascota) ON DELETE NO ACTION,
    CONSTRAINT FK_Turnos_Veterinarios FOREIGN KEY (IdVeterinario) REFERENCES Veterinarios(IdVeterinario) ON DELETE NO ACTION
);

CREATE TABLE Consultas (
    IdConsulta INT IDENTITY(1,1) PRIMARY KEY,
    IdMascota INT NOT NULL,
    IdVeterinario INT NOT NULL,
    FechaHora DATETIME NOT NULL,
    Motivo VARCHAR(MAX),
    Diagnostico VARCHAR(MAX),
    Tratamiento VARCHAR(MAX),
    Observaciones VARCHAR(MAX),
    Status INT DEFAULT 1,
    CONSTRAINT FK_Consultas_Mascotas FOREIGN KEY (IdMascota) REFERENCES Mascotas(IdMascota) ON DELETE NO ACTION,
    CONSTRAINT FK_Consultas_Veterinarios FOREIGN KEY (IdVeterinario) REFERENCES Veterinarios(IdVeterinario) ON DELETE NO ACTION
);

CREATE TABLE Examenes (
    IdExamen INT IDENTITY(1,1) PRIMARY KEY,
    Nombre VARCHAR(100) NOT NULL,
    Descripcion VARCHAR(MAX),
    Status INT DEFAULT 1
);

CREATE TABLE ResultadosExamenes (
    IdResultadoExamen INT IDENTITY(1,1) PRIMARY KEY,
    IdConsulta INT NOT NULL,
    IdExamen INT NOT NULL,
    Resultado VARCHAR(MAX),
    FechaHora DATETIME NOT NULL,
    Status INT DEFAULT 1,
    CONSTRAINT FK_ResultadosExamenes_Consultas FOREIGN KEY (IdConsulta) REFERENCES Consultas(IdConsulta) ON DELETE NO ACTION,
    CONSTRAINT FK_ResultadosExamenes_Examenes FOREIGN KEY (IdExamen) REFERENCES Examenes(IdExamen) ON DELETE NO ACTION
);

CREATE TABLE Productos (
    IdProducto INT IDENTITY(1,1) PRIMARY KEY,
    Nombre VARCHAR(100) NOT NULL,
    Descripcion VARCHAR(MAX),
    Status INT DEFAULT 1
);

CREATE TABLE Tratamientos (
    IdTratamiento INT IDENTITY(1,1) PRIMARY KEY,
    Nombre VARCHAR(100) NOT NULL,
    Descripcion VARCHAR(MAX),
    Status INT DEFAULT 1
);

CREATE TABLE ProductosTratamientos (
    IdProductoTratamiento INT IDENTITY(1,1) PRIMARY KEY,
    IdProducto INT NOT NULL,
    IdTratamiento INT NOT NULL,
    Status INT DEFAULT 1,
    CONSTRAINT FK_ProdTrat_Productos FOREIGN KEY (IdProducto) REFERENCES Productos(IdProducto) ON DELETE NO ACTION,
    CONSTRAINT FK_ProdTrat_Tratamientos FOREIGN KEY (IdTratamiento) REFERENCES Tratamientos(IdTratamiento) ON DELETE NO ACTION
);

CREATE TABLE LoginUsuarios (
    IdLoginUsuario INT IDENTITY(1,1) PRIMARY KEY,
    Usuario VARCHAR(50) NOT NULL,
    Password VARCHAR(100) NOT NULL,
    Status INT DEFAULT 1
); 