-- Script para corregir específicamente los problemas de la mascota "eliot"
-- IMPORTANTE: Hacer backup antes de ejecutar

BEGIN TRANSACTION

BEGIN TRY
    PRINT '=== CORRECCIÓN ESPECÍFICA DE MASCOTA ELIOT ==='
    
    -- 1. Verificar que la mascota existe
    IF NOT EXISTS (SELECT 1 FROM Mascotas WHERE Nombre = 'eliot')
    BEGIN
        PRINT 'ERROR: No se encontró la mascota eliot'
        ROLLBACK TRANSACTION
        RETURN
    END
    
    PRINT 'Mascota eliot encontrada, procediendo con correcciones...'
    
    -- 2. Obtener los IDs de referencia actuales
    DECLARE @IdMascota INT, @IdEspecie INT, @IdRaza INT, @IdDueno INT
    
    SELECT 
        @IdMascota = IdMascota,
        @IdEspecie = IdEspecie, 
        @IdRaza = IdRaza, 
        @IdDueno = IdDueno 
    FROM Mascotas WHERE Nombre = 'eliot'
    
    PRINT 'IDs actuales - Mascota: ' + CAST(@IdMascota AS VARCHAR(10)) + 
          ', Especie: ' + CAST(@IdEspecie AS VARCHAR(10)) + 
          ', Raza: ' + CAST(@IdRaza AS VARCHAR(10)) + 
          ', Dueño: ' + CAST(@IdDueno AS VARCHAR(10))
    
    -- 3. Crear entidades por defecto si no existen
    
    -- Crear especie por defecto si no existe
    IF NOT EXISTS (SELECT 1 FROM Especies WHERE IdEspecie = @IdEspecie)
    BEGIN
        -- Verificar si existe especie con ID 1 (común para perros)
        IF NOT EXISTS (SELECT 1 FROM Especies WHERE IdEspecie = 1)
        BEGIN
            SET IDENTITY_INSERT Especies ON
            INSERT INTO Especies (IdEspecie, Nombre) VALUES (1, 'Perro')
            SET IDENTITY_INSERT Especies OFF
            PRINT 'Especie por defecto (Perro) creada con ID 1'
        END
        
        -- Actualizar la mascota para usar la especie por defecto
        UPDATE Mascotas SET IdEspecie = 1 WHERE IdMascota = @IdMascota
        SET @IdEspecie = 1
        PRINT 'Mascota eliot actualizada para usar especie ID 1'
    END
    
    -- Crear raza por defecto si no existe
    IF NOT EXISTS (SELECT 1 FROM Razas WHERE IdRaza = @IdRaza)
    BEGIN
        -- Verificar si existe raza con ID 1
        IF NOT EXISTS (SELECT 1 FROM Razas WHERE IdRaza = 1)
        BEGIN
            SET IDENTITY_INSERT Razas ON
            INSERT INTO Razas (IdRaza, Nombre, IdEspecie) VALUES (1, 'Mestizo', @IdEspecie)
            SET IDENTITY_INSERT Razas OFF
            PRINT 'Raza por defecto (Mestizo) creada con ID 1'
        END
        
        -- Actualizar la mascota para usar la raza por defecto
        UPDATE Mascotas SET IdRaza = 1 WHERE IdMascota = @IdMascota
        SET @IdRaza = 1
        PRINT 'Mascota eliot actualizada para usar raza ID 1'
    END
    
    -- Crear dueño por defecto si no existe
    IF NOT EXISTS (SELECT 1 FROM Duenos WHERE IdDueno = @IdDueno)
    BEGIN
        -- Verificar si existe dueño con ID 1
        IF NOT EXISTS (SELECT 1 FROM Duenos WHERE IdDueno = 1)
        BEGIN
            SET IDENTITY_INSERT Duenos ON
            INSERT INTO Duenos (IdDueno, Nombre, Apellido, Telefono, Email) 
            VALUES (1, 'Propietario', 'Temporal', '000-000-0000', 'temporal@veterinaria.com')
            SET IDENTITY_INSERT Duenos OFF
            PRINT 'Dueño por defecto creado con ID 1'
        END
        
        -- Actualizar la mascota para usar el dueño por defecto
        UPDATE Mascotas SET IdDueno = 1 WHERE IdMascota = @IdMascota
        SET @IdDueno = 1
        PRINT 'Mascota eliot actualizada para usar dueño ID 1'
    END
    
    -- 4. Corregir campos NULL en la mascota eliot
    UPDATE Mascotas 
    SET 
        Color = ISNULL(Color, 'No especificado'),
        Sexo = ISNULL(Sexo, 'M'),
        Activo = ISNULL(Activo, 1)
    WHERE IdMascota = @IdMascota
    
    PRINT 'Campos NULL de mascota eliot corregidos'
    
    -- 5. Agregar columna Activo si no existe
    IF COL_LENGTH('Mascotas', 'Activo') IS NULL
    BEGIN
        ALTER TABLE Mascotas ADD Activo INT NOT NULL DEFAULT 1
        PRINT 'Columna Activo agregada a tabla Mascotas'
    END
    
    IF COL_LENGTH('Duenos', 'Activo') IS NULL
    BEGIN
        ALTER TABLE Duenos ADD Activo INT NOT NULL DEFAULT 1
        PRINT 'Columna Activo agregada a tabla Duenos'
    END
    
    IF COL_LENGTH('Especies', 'Activo') IS NULL
    BEGIN
        ALTER TABLE Especies ADD Activo INT NOT NULL DEFAULT 1
        PRINT 'Columna Activo agregada a tabla Especies'
    END
    
    IF COL_LENGTH('Razas', 'Activo') IS NULL
    BEGIN
        ALTER TABLE Razas ADD Activo INT NOT NULL DEFAULT 1
        PRINT 'Columna Activo agregada a tabla Razas'
    END
    
    -- 6. Verificar el resultado final
    PRINT ''
    PRINT 'Estado final de la mascota eliot:'
    SELECT 
        m.IdMascota,
        m.Nombre,
        m.Color,
        m.Sexo,
        m.Activo,
        e.Nombre as Especie,
        r.Nombre as Raza,
        d.Nombre + ' ' + d.Apellido as Dueno
    FROM Mascotas m
    LEFT JOIN Especies e ON m.IdEspecie = e.IdEspecie
    LEFT JOIN Razas r ON m.IdRaza = r.IdRaza
    LEFT JOIN Duenos d ON m.IdDueno = d.IdDueno
    WHERE m.IdMascota = @IdMascota
    
    COMMIT TRANSACTION
    PRINT '=== CORRECCIÓN COMPLETADA EXITOSAMENTE ==='
    
END TRY
BEGIN CATCH
    ROLLBACK TRANSACTION
    PRINT '=== ERROR EN LA CORRECCIÓN ==='
    PRINT 'Error: ' + ERROR_MESSAGE()
    PRINT 'Línea: ' + CAST(ERROR_LINE() AS VARCHAR(10))
END CATCH
