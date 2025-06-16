-- Script para agregar la columna Activo a la tabla FichasIngreso en SQL Server
-- Ejecutar este script en el hosting de SQL Server

-- Verificar si la columna Activo ya existe
IF NOT EXISTS (
    SELECT * FROM INFORMATION_SCHEMA.COLUMNS 
    WHERE TABLE_NAME = 'FichasIngreso' 
    AND COLUMN_NAME = 'Activo'
)
BEGIN
    -- Agregar la columna Activo con valor por defecto 1 (true)
    ALTER TABLE FichasIngreso 
    ADD Activo bit NOT NULL DEFAULT 1;
    
    PRINT 'Columna Activo agregada exitosamente a la tabla FichasIngreso';
END
ELSE
BEGIN
    PRINT 'La columna Activo ya existe en la tabla FichasIngreso';
END

-- Verificar que se agregó correctamente
SELECT 
    COLUMN_NAME,
    DATA_TYPE,
    IS_NULLABLE,
    COLUMN_DEFAULT
FROM INFORMATION_SCHEMA.COLUMNS 
WHERE TABLE_NAME = 'FichasIngreso' 
AND COLUMN_NAME = 'Activo';
