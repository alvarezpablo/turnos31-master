-- Script para corregir problemas de datos NULL en la base de datos
-- IMPORTANTE: Hacer backup de la base de datos antes de ejecutar este script

BEGIN TRANSACTION

BEGIN TRY
    PRINT '=== INICIANDO CORRECCIÓN DE DATOS ==='
    
    -- 1. Agregar columna Activo si no existe
    PRINT 'Verificando y agregando columna Activo...'
    
    IF COL_LENGTH('Mascotas', 'Activo') IS NULL
    BEGIN
        ALTER TABLE Mascotas ADD Activo INT NOT NULL DEFAULT 1
        PRINT 'Columna Activo agregada a Mascotas'
    END
    
    IF COL_LENGTH('Duenos', 'Activo') IS NULL
    BEGIN
        ALTER TABLE Duenos ADD Activo INT NOT NULL DEFAULT 1
        PRINT 'Columna Activo agregada a Duenos'
    END
    
    IF COL_LENGTH('Especies', 'Activo') IS NULL
    BEGIN
        ALTER TABLE Especies ADD Activo INT NOT NULL DEFAULT 1
        PRINT 'Columna Activo agregada a Especies'
    END
    
    IF COL_LENGTH('Razas', 'Activo') IS NULL
    BEGIN
        ALTER TABLE Razas ADD Activo INT NOT NULL DEFAULT 1
        PRINT 'Columna Activo agregada a Razas'
    END
    
    -- 2. Crear registros por defecto para referencias faltantes
    PRINT 'Creando registros por defecto...'
    
    -- Especie por defecto
    IF NOT EXISTS (SELECT 1 FROM Especies WHERE IdEspecie = 0)
    BEGIN
        SET IDENTITY_INSERT Especies ON
        INSERT INTO Especies (IdEspecie, Nombre, Activo) VALUES (0, 'Sin Especie', 1)
        SET IDENTITY_INSERT Especies OFF
        PRINT 'Especie por defecto creada'
    END
    
    -- Raza por defecto
    IF NOT EXISTS (SELECT 1 FROM Razas WHERE IdRaza = 0)
    BEGIN
        SET IDENTITY_INSERT Razas ON
        INSERT INTO Razas (IdRaza, Nombre, IdEspecie, Activo) VALUES (0, 'Sin Raza', 0, 1)
        SET IDENTITY_INSERT Razas OFF
        PRINT 'Raza por defecto creada'
    END
    
    -- Dueño por defecto
    IF NOT EXISTS (SELECT 1 FROM Duenos WHERE IdDueno = 0)
    BEGIN
        SET IDENTITY_INSERT Duenos ON
        INSERT INTO Duenos (IdDueno, Nombre, Apellido, Telefono, Email, Activo) 
        VALUES (0, 'Sin Dueño', 'Asignado', '000-000-0000', 'sin.dueno@veterinaria.com', 1)
        SET IDENTITY_INSERT Duenos OFF
        PRINT 'Dueño por defecto creado'
    END
    
    -- 3. Corregir nombres NULL o vacíos en Dueños
    UPDATE Duenos 
    SET Nombre = 'Nombre No Especificado'
    WHERE Nombre IS NULL OR LTRIM(RTRIM(Nombre)) = ''
    
    PRINT 'Nombres de dueños corregidos: ' + CAST(@@ROWCOUNT AS VARCHAR(10))
    
    -- 4. Corregir nombres NULL o vacíos en Especies
    UPDATE Especies 
    SET Nombre = 'Especie No Especificada'
    WHERE Nombre IS NULL OR LTRIM(RTRIM(Nombre)) = ''
    
    PRINT 'Nombres de especies corregidos: ' + CAST(@@ROWCOUNT AS VARCHAR(10))
    
    -- 5. Corregir nombres NULL o vacíos en Razas
    UPDATE Razas 
    SET Nombre = 'Raza No Especificada'
    WHERE Nombre IS NULL OR LTRIM(RTRIM(Nombre)) = ''
    
    PRINT 'Nombres de razas corregidos: ' + CAST(@@ROWCOUNT AS VARCHAR(10))
    
    -- 6. Corregir mascotas con nombres NULL o vacíos
    UPDATE Mascotas 
    SET Nombre = 'Mascota Sin Nombre'
    WHERE Nombre IS NULL OR LTRIM(RTRIM(Nombre)) = ''
    
    PRINT 'Nombres de mascotas corregidos: ' + CAST(@@ROWCOUNT AS VARCHAR(10))
    
    -- 7. Corregir referencias inválidas en Mascotas
    -- IdEspecie inválido
    UPDATE m
    SET IdEspecie = 0
    FROM Mascotas m
    LEFT JOIN Especies e ON m.IdEspecie = e.IdEspecie
    WHERE e.IdEspecie IS NULL
    
    PRINT 'Referencias de especies corregidas: ' + CAST(@@ROWCOUNT AS VARCHAR(10))
    
    -- IdRaza inválido
    UPDATE m
    SET IdRaza = 0
    FROM Mascotas m
    LEFT JOIN Razas r ON m.IdRaza = r.IdRaza
    WHERE r.IdRaza IS NULL
    
    PRINT 'Referencias de razas corregidas: ' + CAST(@@ROWCOUNT AS VARCHAR(10))
    
    -- IdDueno inválido
    UPDATE m
    SET IdDueno = 0
    FROM Mascotas m
    LEFT JOIN Duenos d ON m.IdDueno = d.IdDueno
    WHERE d.IdDueno IS NULL
    
    PRINT 'Referencias de dueños corregidas: ' + CAST(@@ROWCOUNT AS VARCHAR(10))
    
    -- 8. Asegurar que todas las mascotas tengan Activo = 1
    UPDATE Mascotas SET Activo = 1 WHERE Activo IS NULL OR Activo = 0
    PRINT 'Estado Activo de mascotas corregido: ' + CAST(@@ROWCOUNT AS VARCHAR(10))
    
    COMMIT TRANSACTION
    PRINT '=== CORRECCIÓN COMPLETADA EXITOSAMENTE ==='
    
END TRY
BEGIN CATCH
    ROLLBACK TRANSACTION
    PRINT '=== ERROR EN LA CORRECCIÓN ==='
    PRINT 'Error: ' + ERROR_MESSAGE()
    PRINT 'Línea: ' + CAST(ERROR_LINE() AS VARCHAR(10))
END CATCH
