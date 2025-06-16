-- Script para verificar específicamente la mascota "eliot" y sus problemas

PRINT '=== VERIFICACIÓN ESPECÍFICA DE MASCOTA ELIOT ==='

-- 1. Verificar que la mascota existe
SELECT 'Datos de la mascota eliot:' as Verificacion
SELECT * FROM Mascotas WHERE Nombre = 'eliot'

-- 2. Verificar las referencias de la mascota eliot
PRINT ''
PRINT 'Verificando referencias de la mascota eliot:'

SELECT 
    m.IdMascota,
    m.Nombre as NombreMascota,
    m.IdEspecie,
    e.Nombre as NombreEspecie,
    m.IdRaza,
    r.Nombre as NombreRaza,
    m.IdDueno,
    d.Nombre as NombreDueno,
    d.Apellido as ApellidoDueno,
    m.Activo,
    m.Color,
    m.Sexo
FROM Mascotas m
LEFT JOIN Especies e ON m.IdEspecie = e.IdEspecie
LEFT JOIN Razas r ON m.IdRaza = r.IdRaza
LEFT JOIN Duenos d ON m.IdDueno = d.IdDueno
WHERE m.Nombre = 'eliot'

-- 3. Verificar si existen las entidades referenciadas
PRINT ''
PRINT 'Verificando existencia de entidades referenciadas:'

DECLARE @IdEspecie INT, @IdRaza INT, @IdDueno INT

SELECT @IdEspecie = IdEspecie, @IdRaza = IdRaza, @IdDueno = IdDueno 
FROM Mascotas WHERE Nombre = 'eliot'

-- Verificar Especie
IF EXISTS (SELECT 1 FROM Especies WHERE IdEspecie = @IdEspecie)
    SELECT 'Especie existe' as Estado, @IdEspecie as IdEspecie
ELSE
    SELECT 'Especie NO existe' as Estado, @IdEspecie as IdEspecie

-- Verificar Raza
IF EXISTS (SELECT 1 FROM Razas WHERE IdRaza = @IdRaza)
    SELECT 'Raza existe' as Estado, @IdRaza as IdRaza
ELSE
    SELECT 'Raza NO existe' as Estado, @IdRaza as IdRaza

-- Verificar Dueño
IF EXISTS (SELECT 1 FROM Duenos WHERE IdDueno = @IdDueno)
    SELECT 'Dueño existe' as Estado, @IdDueno as IdDueno
ELSE
    SELECT 'Dueño NO existe' as Estado, @IdDueno as IdDueno

-- 4. Verificar datos de las entidades referenciadas
PRINT ''
PRINT 'Datos de las entidades referenciadas:'

SELECT 'Datos de la Especie:' as Tipo
SELECT * FROM Especies WHERE IdEspecie = @IdEspecie

SELECT 'Datos de la Raza:' as Tipo
SELECT * FROM Razas WHERE IdRaza = @IdRaza

SELECT 'Datos del Dueño:' as Tipo
SELECT * FROM Duenos WHERE IdDueno = @IdDueno

-- 5. Verificar si hay problemas con campos NULL
PRINT ''
PRINT 'Verificando campos NULL en mascota eliot:'

SELECT 
    CASE WHEN Nombre IS NULL THEN 'Nombre es NULL' ELSE 'Nombre OK' END as EstadoNombre,
    CASE WHEN Color IS NULL THEN 'Color es NULL' ELSE 'Color OK' END as EstadoColor,
    CASE WHEN Sexo IS NULL THEN 'Sexo es NULL' ELSE 'Sexo OK' END as EstadoSexo,
    CASE WHEN Activo IS NULL THEN 'Activo es NULL' ELSE 'Activo OK' END as EstadoActivo
FROM Mascotas WHERE Nombre = 'eliot'

-- 6. Contar todas las mascotas para comparar
PRINT ''
PRINT 'Conteo general de mascotas:'
SELECT 'Total de mascotas:' as Descripcion, COUNT(*) as Cantidad FROM Mascotas
SELECT 'Mascotas con nombre no NULL:' as Descripcion, COUNT(*) as Cantidad FROM Mascotas WHERE Nombre IS NOT NULL
SELECT 'Mascotas con nombre no vacío:' as Descripcion, COUNT(*) as Cantidad FROM Mascotas WHERE Nombre IS NOT NULL AND LTRIM(RTRIM(Nombre)) != ''
