USE VeterinariaDb;
GO

IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Usuarios')
BEGIN
    CREATE TABLE Usuarios (
        IdUsuario INT IDENTITY(1,1) PRIMARY KEY,
        Nombre VARCHAR(100) NOT NULL,
        Apellido VARCHAR(100) NOT NULL,
        Email VARCHAR(100) NOT NULL,
        Password VARCHAR(100) NOT NULL,
        Rol VARCHAR(20) NOT NULL DEFAULT 'Usuario',
        Activo BIT NOT NULL DEFAULT 1
    );
END
GO 