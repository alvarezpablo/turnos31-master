-- Script para agregar Especies y Razas al menú de Administración
-- Este script puede ejecutarse independientemente si el menú principal ya existe

-- Verificar si ya existen las entradas
IF NOT EXISTS (SELECT 1 FROM Menus WHERE Nombre = 'Especies' AND Url = '/Especie/Index')
BEGIN
    -- Obtener el ID del menú de Administración
    DECLARE @AdminId INT = (SELECT Id FROM Menus WHERE Nombre = 'Administración');
    
    IF @AdminId IS NOT NULL
    BEGIN
        -- Insertar menú de Especies
        INSERT INTO Menus (Nombre, Url, Icono, MenuPadreId) 
        VALUES ('Especies', '/Especie/Index', 'bi-tags', @AdminId);
        
        PRINT 'Menú de Especies agregado correctamente';
    END
    ELSE
    BEGIN
        PRINT 'Error: No se encontró el menú de Administración';
    END
END
ELSE
BEGIN
    PRINT 'El menú de Especies ya existe';
END

IF NOT EXISTS (SELECT 1 FROM Menus WHERE Nombre = 'Razas' AND Url = '/Raza/Index')
BEGIN
    -- Obtener el ID del menú de Administración
    DECLARE @AdminId2 INT = (SELECT Id FROM Menus WHERE Nombre = 'Administración');
    
    IF @AdminId2 IS NOT NULL
    BEGIN
        -- Insertar menú de Razas
        INSERT INTO Menus (Nombre, Url, Icono, MenuPadreId) 
        VALUES ('Razas', '/Raza/Index', 'bi-list-check', @AdminId2);
        
        PRINT 'Menú de Razas agregado correctamente';
    END
    ELSE
    BEGIN
        PRINT 'Error: No se encontró el menú de Administración';
    END
END
ELSE
BEGIN
    PRINT 'El menú de Razas ya existe';
END

-- Asignar los nuevos menús al rol Administrador
DECLARE @AdminRolId INT = (SELECT IdRol FROM Roles WHERE NombreRol = 'Administrador');

IF @AdminRolId IS NOT NULL
BEGIN
    -- Asignar menú de Especies al rol Administrador
    IF NOT EXISTS (SELECT 1 FROM MenuRoles mr 
                   INNER JOIN Menus m ON mr.MenuId = m.Id 
                   WHERE m.Nombre = 'Especies' AND mr.RolId = @AdminRolId)
    BEGIN
        DECLARE @EspeciesMenuId INT = (SELECT Id FROM Menus WHERE Nombre = 'Especies' AND Url = '/Especie/Index');
        INSERT INTO MenuRoles (MenuId, RolId) VALUES (@EspeciesMenuId, @AdminRolId);
        PRINT 'Menú de Especies asignado al rol Administrador';
    END
    ELSE
    BEGIN
        PRINT 'El menú de Especies ya está asignado al rol Administrador';
    END
    
    -- Asignar menú de Razas al rol Administrador
    IF NOT EXISTS (SELECT 1 FROM MenuRoles mr 
                   INNER JOIN Menus m ON mr.MenuId = m.Id 
                   WHERE m.Nombre = 'Razas' AND mr.RolId = @AdminRolId)
    BEGIN
        DECLARE @RazasMenuId INT = (SELECT Id FROM Menus WHERE Nombre = 'Razas' AND Url = '/Raza/Index');
        INSERT INTO MenuRoles (MenuId, RolId) VALUES (@RazasMenuId, @AdminRolId);
        PRINT 'Menú de Razas asignado al rol Administrador';
    END
    ELSE
    BEGIN
        PRINT 'El menú de Razas ya está asignado al rol Administrador';
    END
END
ELSE
BEGIN
    PRINT 'Error: No se encontró el rol Administrador';
END

PRINT 'Script completado';
