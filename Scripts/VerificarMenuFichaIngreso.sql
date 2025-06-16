-- Script para verificar que el menú Fichas de Ingreso esté configurado correctamente

-- 1. Verificar que existe el menú Pacientes
SELECT 'Menú Pacientes' as Verificacion, COUNT(*) as Existe
FROM Menus 
WHERE Nombre = 'Pacientes';

-- 2. Verificar submenús de Pacientes
SELECT 
    'Submenús de Pacientes' as Verificacion,
    m.Nombre as Submenu,
    m.Url,
    m.Icono
FROM Menus m
INNER JOIN Menus mp ON m.MenuPadreId = mp.Id
WHERE mp.Nombre = 'Pacientes'
ORDER BY m.Nombre;

-- 3. Verificar que Fichas de Ingreso tenga permisos asignados
SELECT 
    'Permisos Fichas de Ingreso' as Verificacion,
    r.NombreRol as Rol
FROM Menus m
INNER JOIN MenuRoles mr ON m.Id = mr.MenuId
INNER JOIN Roles r ON mr.RolId = r.IdRol
WHERE m.Nombre = 'Fichas de Ingreso';

-- 4. Verificar que existan las tablas necesarias
SELECT 'Tabla NivelUrgencias' as Verificacion, COUNT(*) as Registros FROM NivelUrgencias
UNION ALL
SELECT 'Tabla MotivoVisitas' as Verificacion, COUNT(*) as Registros FROM MotivoVisitas
UNION ALL
SELECT 'Tabla TipoServicios' as Verificacion, COUNT(*) as Registros FROM TipoServicios
UNION ALL
SELECT 'Tabla FichasIngreso' as Verificacion, COUNT(*) as Registros FROM FichasIngreso;

-- 5. Verificar estructura de la tabla FichasIngreso
SELECT 
    'Columnas FichasIngreso' as Verificacion,
    COLUMN_NAME as Columna,
    DATA_TYPE as Tipo,
    IS_NULLABLE as PermiteNull
FROM INFORMATION_SCHEMA.COLUMNS 
WHERE TABLE_NAME = 'FichasIngreso'
ORDER BY ORDINAL_POSITION;
