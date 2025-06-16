-- Script para insertar datos de ejemplo para Ficha de Ingreso

-- Insertar Niveles de Urgencia
INSERT INTO NivelUrgencias (Nombre) VALUES 
('Control'),
('Urgencia'),
('Examen'),
('Consulta');

-- Insertar Motivos de Visita
INSERT INTO MotivoVisitas (Nombre) VALUES 
('Consulta General'),
('Procedimiento'),
('Examen');

-- Insertar Tipos de Servicio
INSERT INTO TipoServicios (Nombre) VALUES 
('Consulta Veterinaria'),
('Cirugía'),
('Vacunación'),
('Desparasitación'),
('Examen de Laboratorio'),
('Radiografía'),
('Ecografía'),
('Limpieza Dental'),
('Castración'),
('Esterilización'),
('Control de Salud'),
('Emergencia');

-- Verificar que se insertaron correctamente
SELECT 'NivelUrgencias' as Tabla, COUNT(*) as Registros FROM NivelUrgencias
UNION ALL
SELECT 'MotivoVisitas' as Tabla, COUNT(*) as Registros FROM MotivoVisitas
UNION ALL
SELECT 'TipoServicios' as Tabla, COUNT(*) as Registros FROM TipoServicios;
