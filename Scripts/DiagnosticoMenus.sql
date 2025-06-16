-- Script de diagnóstico para verificar el estado de los menús
-- Ejecutar este script para ver qué está pasando con los menús

PRINT '=== DIAGNÓSTICO DE MENÚS ==='
PRINT ''

-- 1. Verificar roles existentes
PRINT '1. ROLES EXISTENTES:'
SELECT IdRol, NombreRol, Descripcion FROM Roles
PRINT ''

-- 2. Verificar menús principales
PRINT '2. MENÚS PRINCIPALES (sin padre):'
SELECT Id, Nombre, Url, Icono FROM Menus WHERE MenuPadreId IS NULL ORDER BY Nombre
PRINT ''

-- 3. Verificar submenús de Administración
PRINT '3. SUBMENÚS DE ADMINISTRACIÓN:'
SELECT 
    m.Id, 
    m.Nombre, 
    m.Url, 
    m.Icono,
    mp.Nombre as MenuPadre
FROM Menus m
LEFT JOIN Menus mp ON m.MenuPadreId = mp.Id
WHERE mp.Nombre = 'Administración' OR m.Nombre = 'Administración'
ORDER BY m.Nombre
PRINT ''

-- 4. Verificar si existen Especies y Razas en Menus
PRINT '4. VERIFICAR ESPECIES Y RAZAS EN MENUS:'
SELECT Id, Nombre, Url, Icono, MenuPadreId 
FROM Menus 
WHERE Nombre IN ('Especies', 'Razas')
PRINT ''

-- 5. Verificar asignaciones de MenuRoles para Administrador
PRINT '5. MENÚS ASIGNADOS AL ROL ADMINISTRADOR:'
SELECT 
    mr.Id as MenuRolId,
    m.Id as MenuId,
    m.Nombre as MenuNombre,
    m.Url,
    r.NombreRol
FROM MenuRoles mr
INNER JOIN Menus m ON mr.MenuId = m.Id
INNER JOIN Roles r ON mr.RolId = r.IdRol
WHERE r.NombreRol = 'Administrador'
ORDER BY m.Nombre
PRINT ''

-- 6. Verificar si Especies y Razas están asignadas al rol Administrador
PRINT '6. ESPECIES Y RAZAS ASIGNADAS AL ADMINISTRADOR:'
SELECT 
    mr.Id as MenuRolId,
    m.Nombre as MenuNombre,
    r.NombreRol
FROM MenuRoles mr
INNER JOIN Menus m ON mr.MenuId = m.Id
INNER JOIN Roles r ON mr.RolId = r.IdRol
WHERE r.NombreRol = 'Administrador' 
  AND m.Nombre IN ('Especies', 'Razas')
PRINT ''

-- 7. Contar total de menús vs menús asignados al Administrador
PRINT '7. RESUMEN DE CONTEOS:'
SELECT 
    (SELECT COUNT(*) FROM Menus) as TotalMenus,
    (SELECT COUNT(*) FROM MenuRoles mr INNER JOIN Roles r ON mr.RolId = r.IdRol WHERE r.NombreRol = 'Administrador') as MenusAsignadosAdmin
PRINT ''

PRINT '=== FIN DEL DIAGNÓSTICO ==='
