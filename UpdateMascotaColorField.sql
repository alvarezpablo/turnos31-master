-- Script para verificar y corregir la tabla Mascotas en hosting
-- PROBLEMA IDENTIFICADO: La tabla tiene IdCliente en lugar de IdDueno
USE [db_ab9aed_veterinaria];
GO

-- 1. Verificar la estructura actual de la tabla Mascotas
PRINT '=== ESTRUCTURA DE LA TABLA MASCOTAS ==='
SELECT
    ORDINAL_POSITION as Posicion,
    COLUMN_NAME as Columna,
    DATA_TYPE as Tipo,
    IS_NULLABLE as PermiteNulos,
    COLUMN_DEFAULT as ValorPorDefecto,
    CHARACTER_MAXIMUM_LENGTH as LongitudMaxima
FROM INFORMATION_SCHEMA.COLUMNS
WHERE TABLE_NAME = 'Mascotas'
ORDER BY ORDINAL_POSITION;

-- 2. Verificar si existe la columna IdCliente (problema identificado)
PRINT '=== VERIFICANDO COLUMNA IDCLIENTE ==='
IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'Mascotas' AND COLUMN_NAME = 'IdCliente')
    PRINT 'CONFIRMADO: La tabla tiene columna IdCliente'
ELSE
    PRINT 'La tabla NO tiene columna IdCliente'

-- 3. Verificar si existe la columna IdDueno
IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'Mascotas' AND COLUMN_NAME = 'IdDueno')
    PRINT 'La tabla tiene columna IdDueno'
ELSE
    PRINT 'La tabla NO tiene columna IdDueno'

-- 4. Verificar registros con problemas (adaptado para IdCliente)
PRINT '=== REGISTROS CON PROBLEMAS ==='
SELECT
    IdMascota,
    Nombre,
    Color,
    Sexo,
    IdEspecie,
    IdRaza,
    CASE
        WHEN EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'Mascotas' AND COLUMN_NAME = 'IdCliente')
        THEN 'Verificar IdCliente en consulta separada'
        ELSE 'IdCliente no existe'
    END as EstadoIdCliente
FROM Mascotas
WHERE Color IS NULL
   OR Sexo IS NULL
   OR Nombre IS NULL
   OR IdEspecie IS NULL
   OR IdRaza IS NULL;

-- 3. Actualizar registros con Color NULL o vacío
UPDATE Mascotas
SET Color = 'No especificado'
WHERE Color IS NULL OR Color = '' OR LEN(LTRIM(RTRIM(Color))) = 0;

-- 4. Verificar que no hay valores NULL en campos críticos
SELECT
    COUNT(*) as TotalRegistros,
    SUM(CASE WHEN Color IS NULL THEN 1 ELSE 0 END) as ColorNulos,
    SUM(CASE WHEN Sexo IS NULL THEN 1 ELSE 0 END) as SexoNulos,
    SUM(CASE WHEN Nombre IS NULL THEN 1 ELSE 0 END) as NombreNulos
FROM Mascotas;

-- 5. Verificar integridad referencial
SELECT
    m.IdMascota,
    m.Nombre,
    m.IdEspecie,
    e.Nombre as EspecieNombre,
    m.IdRaza,
    r.Nombre as RazaNombre,
    m.IdDueno,
    d.Nombre as DuenoNombre
FROM Mascotas m
LEFT JOIN Especies e ON m.IdEspecie = e.IdEspecie
LEFT JOIN Razas r ON m.IdRaza = r.IdRaza
LEFT JOIN Duenos d ON m.IdDueno = d.IdDueno
WHERE e.IdEspecie IS NULL
   OR r.IdRaza IS NULL
   OR d.IdDueno IS NULL;

-- 6. Mostrar algunos registros de ejemplo después de la actualización
SELECT TOP 5
    IdMascota,
    Nombre,
    Color,
    Sexo,
    IdEspecie,
    IdRaza,
    IdDueno
FROM Mascotas
ORDER BY IdMascota DESC;
