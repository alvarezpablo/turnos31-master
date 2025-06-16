-- Script para agregar la opción "Fichas de Ingreso" al menú Pacientes
-- Ejecutar este script en el hosting de SQL Server

-- Obtener el ID del menú Pacientes
DECLARE @PacientesId INT = (SELECT Id FROM Menus WHERE Nombre = 'Pacientes');

-- Verificar si el menú Pacientes existe
IF @PacientesId IS NULL
BEGIN
    PRINT 'ERROR: No se encontró el menú Pacientes';
    RETURN;
END

-- Verificar si ya existe el menú "Fichas de Ingreso"
IF NOT EXISTS (SELECT 1 FROM Menus WHERE Nombre = 'Fichas de Ingreso' AND MenuPadreId = @PacientesId)
BEGIN
    -- Insertar el submenú "Fichas de Ingreso"
    INSERT INTO Menus (Nombre, Url, Icono, MenuPadreId) 
    VALUES ('Fichas de Ingreso', '/FichaIngreso/Index', 'bi-clipboard2-pulse', @PacientesId);
    
    PRINT 'Menú "Fichas de Ingreso" agregado exitosamente';
    
    -- Obtener el ID del menú recién creado
    DECLARE @FichaIngresoMenuId INT = SCOPE_IDENTITY();
    
    -- Obtener IDs de roles
    DECLARE @AdminRolId INT = (SELECT IdRol FROM Roles WHERE NombreRol = 'Administrador');
    DECLARE @VetRolId INT = (SELECT IdRol FROM Roles WHERE NombreRol = 'Veterinario');
    
    -- Asignar el menú al rol Administrador
    IF @AdminRolId IS NOT NULL AND NOT EXISTS (SELECT 1 FROM MenuRoles WHERE MenuId = @FichaIngresoMenuId AND RolId = @AdminRolId)
    BEGIN
        INSERT INTO MenuRoles (MenuId, RolId) VALUES (@FichaIngresoMenuId, @AdminRolId);
        PRINT 'Menú asignado al rol Administrador';
    END
    
    -- Asignar el menú al rol Veterinario
    IF @VetRolId IS NOT NULL AND NOT EXISTS (SELECT 1 FROM MenuRoles WHERE MenuId = @FichaIngresoMenuId AND RolId = @VetRolId)
    BEGIN
        INSERT INTO MenuRoles (MenuId, RolId) VALUES (@FichaIngresoMenuId, @VetRolId);
        PRINT 'Menú asignado al rol Veterinario';
    END
END
ELSE
BEGIN
    PRINT 'El menú "Fichas de Ingreso" ya existe';
END

-- Verificar el resultado
SELECT 
    m.Id,
    m.Nombre,
    m.Url,
    m.Icono,
    mp.Nombre as MenuPadre
FROM Menus m
LEFT JOIN Menus mp ON m.MenuPadreId = mp.Id
WHERE m.MenuPadreId = @PacientesId
ORDER BY m.Nombre;

-- Verificar los roles asignados
SELECT 
    m.Nombre as Menu,
    r.NombreRol as Rol
FROM Menus m
INNER JOIN MenuRoles mr ON m.Id = mr.MenuId
INNER JOIN Roles r ON mr.RolId = r.IdRol
WHERE m.Nombre = 'Fichas de Ingreso'
ORDER BY r.NombreRol;
