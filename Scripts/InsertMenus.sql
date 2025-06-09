-- Primero, limpiar las tablas existentes
DELETE FROM MenuRoles;
DELETE FROM Menus;

-- Insertar menús principales
INSERT INTO Menus (Nombre, Url, Icono, MenuPadreId) VALUES 
('Inicio', '/Home/Index', 'bi-house-door', NULL),
('Administración', '#', 'bi-gear', NULL),
('Médicos', '#', 'bi-person-vcard', NULL),
('Pacientes', '#', 'bi-paw', NULL),
('Turnos', '/Turno/Index', 'bi-calendar-check', NULL),
('Consultas', '/Consulta/Index', 'bi-clipboard2-pulse', NULL);

-- Obtener IDs de menús principales
DECLARE @InicioId INT = (SELECT Id FROM Menus WHERE Nombre = 'Inicio');
DECLARE @AdminId INT = (SELECT Id FROM Menus WHERE Nombre = 'Administración');
DECLARE @MedicosId INT = (SELECT Id FROM Menus WHERE Nombre = 'Médicos');
DECLARE @PacientesId INT = (SELECT Id FROM Menus WHERE Nombre = 'Pacientes');

-- Insertar submenús de Administración
INSERT INTO Menus (Nombre, Url, Icono, MenuPadreId) VALUES
('Usuarios', '/Usuario/Index', 'bi-people', @AdminId),
('Roles', '/Rol/Index', 'bi-person-badge', @AdminId),
('Especialidades', '/Especialidad/Index', 'bi-clipboard2-pulse', @AdminId);

-- Insertar submenús de Médicos
INSERT INTO Menus (Nombre, Url, Icono, MenuPadreId) VALUES
('Lista de Médicos', '/Medico/Index', 'bi-person', @MedicosId),
('Agendas', '/Agenda/Index', 'bi-calendar3', @MedicosId);

-- Insertar submenús de Pacientes
INSERT INTO Menus (Nombre, Url, Icono, MenuPadreId) VALUES
('Mascotas', '/Mascota/Index', 'bi-paw', @PacientesId),
('Dueños', '/Dueno/Index', 'bi-person', @PacientesId);

-- Obtener IDs de roles
DECLARE @AdminRolId INT = (SELECT IdRol FROM Roles WHERE NombreRol = 'Administrador');
DECLARE @VetRolId INT = (SELECT IdRol FROM Roles WHERE NombreRol = 'Veterinario');

-- Asignar todos los menús al rol Administrador
INSERT INTO MenuRoles (MenuId, RolId)
SELECT m.Id, @AdminRolId
FROM Menus m;

-- Asignar menús específicos al rol Veterinario
INSERT INTO MenuRoles (MenuId, RolId)
SELECT m.Id, @VetRolId
FROM Menus m
WHERE m.Nombre IN ('Inicio', 'Médicos', 'Pacientes', 'Turnos', 'Consultas')
   OR m.Nombre IN ('Lista de Médicos', 'Agendas', 'Mascotas', 'Dueños'); 