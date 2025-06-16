-- Script para corregir la asignación de menús de Especies y Razas
-- Este script asegura que los menús estén correctamente asignados al rol Administrador

PRINT '=== CORRECCIÓN DE MENÚS ESPECIES Y RAZAS ==='
PRINT ''

-- Obtener IDs necesarios
DECLARE @AdminRolId INT = (SELECT IdRol FROM Roles WHERE NombreRol = 'Administrador');
DECLARE @AdminMenuId INT = (SELECT Id FROM Menus WHERE Nombre = 'Administración');

-- Verificar que existan los elementos necesarios
IF @AdminRolId IS NULL
BEGIN
    PRINT 'ERROR: No se encontró el rol Administrador'
    RETURN
END

IF @AdminMenuId IS NULL
BEGIN
    PRINT 'ERROR: No se encontró el menú Administración'
    RETURN
END

PRINT 'Rol Administrador ID: ' + CAST(@AdminRolId AS VARCHAR(10))
PRINT 'Menú Administración ID: ' + CAST(@AdminMenuId AS VARCHAR(10))
PRINT ''

-- 1. Insertar menú de Especies si no existe
IF NOT EXISTS (SELECT 1 FROM Menus WHERE Nombre = 'Especies' AND Url = '/Especie/Index')
BEGIN
    INSERT INTO Menus (Nombre, Url, Icono, MenuPadreId) 
    VALUES ('Especies', '/Especie/Index', 'bi-tags', @AdminMenuId);
    PRINT 'Menú de Especies creado'
END
ELSE
BEGIN
    PRINT 'Menú de Especies ya existe'
END

-- 2. Insertar menú de Razas si no existe
IF NOT EXISTS (SELECT 1 FROM Menus WHERE Nombre = 'Razas' AND Url = '/Raza/Index')
BEGIN
    INSERT INTO Menus (Nombre, Url, Icono, MenuPadreId) 
    VALUES ('Razas', '/Raza/Index', 'bi-list-check', @AdminMenuId);
    PRINT 'Menú de Razas creado'
END
ELSE
BEGIN
    PRINT 'Menú de Razas ya existe'
END

-- Obtener IDs de los menús de Especies y Razas
DECLARE @EspeciesMenuId INT = (SELECT Id FROM Menus WHERE Nombre = 'Especies' AND Url = '/Especie/Index');
DECLARE @RazasMenuId INT = (SELECT Id FROM Menus WHERE Nombre = 'Razas' AND Url = '/Raza/Index');

PRINT 'Menú Especies ID: ' + CAST(ISNULL(@EspeciesMenuId, 0) AS VARCHAR(10))
PRINT 'Menú Razas ID: ' + CAST(ISNULL(@RazasMenuId, 0) AS VARCHAR(10))
PRINT ''

-- 3. Asignar menú de Especies al rol Administrador
IF @EspeciesMenuId IS NOT NULL
BEGIN
    IF NOT EXISTS (SELECT 1 FROM MenuRoles WHERE MenuId = @EspeciesMenuId AND RolId = @AdminRolId)
    BEGIN
        INSERT INTO MenuRoles (MenuId, RolId) VALUES (@EspeciesMenuId, @AdminRolId);
        PRINT 'Menú de Especies asignado al rol Administrador'
    END
    ELSE
    BEGIN
        PRINT 'Menú de Especies ya está asignado al rol Administrador'
    END
END

-- 4. Asignar menú de Razas al rol Administrador
IF @RazasMenuId IS NOT NULL
BEGIN
    IF NOT EXISTS (SELECT 1 FROM MenuRoles WHERE MenuId = @RazasMenuId AND RolId = @AdminRolId)
    BEGIN
        INSERT INTO MenuRoles (MenuId, RolId) VALUES (@RazasMenuId, @AdminRolId);
        PRINT 'Menú de Razas asignado al rol Administrador'
    END
    ELSE
    BEGIN
        PRINT 'Menú de Razas ya está asignado al rol Administrador'
    END
END

PRINT ''
PRINT '=== VERIFICACIÓN FINAL ==='

-- Verificar que todo esté correcto
SELECT 
    m.Nombre as MenuNombre,
    m.Url,
    m.Icono,
    mp.Nombre as MenuPadre,
    r.NombreRol
FROM MenuRoles mr
INNER JOIN Menus m ON mr.MenuId = m.Id
INNER JOIN Roles r ON mr.RolId = r.IdRol
LEFT JOIN Menus mp ON m.MenuPadreId = mp.Id
WHERE r.NombreRol = 'Administrador' 
  AND m.Nombre IN ('Especies', 'Razas')
ORDER BY m.Nombre

PRINT ''
PRINT '=== CORRECCIÓN COMPLETADA ==='
