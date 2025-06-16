-- Script para verificar problemas de datos NULL en la base de datos
-- Ejecutar este script en SQL Server Management Studio o similar

-- 1. Verificar mascotas con referencias inválidas
PRINT '=== VERIFICACIÓN DE MASCOTAS ==='

-- Mascotas con nombres NULL o vacíos
SELECT 'Mascotas con nombres NULL o vacíos:' as Problema, COUNT(*) as Cantidad
FROM Mascotas 
WHERE Nombre IS NULL OR LTRIM(RTRIM(Nombre)) = ''

-- Mascotas con IdEspecie que no existe en tabla Especies
SELECT 'Mascotas con IdEspecie inválido:' as Problema, COUNT(*) as Cantidad
FROM Mascotas m
LEFT JOIN Especies e ON m.IdEspecie = e.IdEspecie
WHERE e.IdEspecie IS NULL

-- Mascotas con IdRaza que no existe en tabla Razas
SELECT 'Mascotas con IdRaza inválido:' as Problema, COUNT(*) as Cantidad
FROM Mascotas m
LEFT JOIN Razas r ON m.IdRaza = r.IdRaza
WHERE r.IdRaza IS NULL

-- Mascotas con IdDueno que no existe en tabla Duenos
SELECT 'Mascotas con IdDueno inválido:' as Problema, COUNT(*) as Cantidad
FROM Mascotas m
LEFT JOIN Duenos d ON m.IdDueno = d.IdDueno
WHERE d.IdDueno IS NULL

PRINT ''
PRINT '=== VERIFICACIÓN DE DUEÑOS ==='

-- Dueños con nombres NULL o vacíos
SELECT 'Dueños con nombres NULL o vacíos:' as Problema, COUNT(*) as Cantidad
FROM Duenos 
WHERE Nombre IS NULL OR LTRIM(RTRIM(Nombre)) = ''

PRINT ''
PRINT '=== VERIFICACIÓN DE ESPECIES ==='

-- Especies con nombres NULL o vacíos
SELECT 'Especies con nombres NULL o vacíos:' as Problema, COUNT(*) as Cantidad
FROM Especies 
WHERE Nombre IS NULL OR LTRIM(RTRIM(Nombre)) = ''

PRINT ''
PRINT '=== VERIFICACIÓN DE RAZAS ==='

-- Razas con nombres NULL o vacíos
SELECT 'Razas con nombres NULL o vacíos:' as Problema, COUNT(*) as Cantidad
FROM Razas 
WHERE Nombre IS NULL OR LTRIM(RTRIM(Nombre)) = ''

PRINT ''
PRINT '=== DETALLES DE REGISTROS PROBLEMÁTICOS ==='

-- Mostrar mascotas específicas con problemas
SELECT 'MASCOTAS CON PROBLEMAS:' as Tipo
SELECT 
    m.IdMascota,
    m.Nombre as NombreMascota,
    m.IdEspecie,
    e.Nombre as NombreEspecie,
    m.IdRaza,
    r.Nombre as NombreRaza,
    m.IdDueno,
    d.Nombre as NombreDueno
FROM Mascotas m
LEFT JOIN Especies e ON m.IdEspecie = e.IdEspecie
LEFT JOIN Razas r ON m.IdRaza = r.IdRaza
LEFT JOIN Duenos d ON m.IdDueno = d.IdDueno
WHERE 
    m.Nombre IS NULL OR LTRIM(RTRIM(m.Nombre)) = ''
    OR e.IdEspecie IS NULL
    OR r.IdRaza IS NULL
    OR d.IdDueno IS NULL
ORDER BY m.IdMascota

PRINT ''
PRINT '=== VERIFICACIÓN DE COLUMNA ACTIVO ==='

-- Verificar si existe la columna Activo en las tablas
IF COL_LENGTH('Mascotas', 'Activo') IS NOT NULL
    SELECT 'Columna Activo existe en Mascotas' as Estado
ELSE
    SELECT 'Columna Activo NO existe en Mascotas' as Estado

IF COL_LENGTH('Duenos', 'Activo') IS NOT NULL
    SELECT 'Columna Activo existe en Duenos' as Estado
ELSE
    SELECT 'Columna Activo NO existe en Duenos' as Estado

IF COL_LENGTH('Especies', 'Activo') IS NOT NULL
    SELECT 'Columna Activo existe en Especies' as Estado
ELSE
    SELECT 'Columna Activo NO existe en Especies' as Estado

IF COL_LENGTH('Razas', 'Activo') IS NOT NULL
    SELECT 'Columna Activo existe en Razas' as Estado
ELSE
    SELECT 'Columna Activo NO existe en Razas' as Estado
