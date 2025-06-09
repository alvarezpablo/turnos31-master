# Sistema de Gestión de Turnos - Clínica Veterinaria

Este es un sistema de gestión de turnos para una clínica veterinaria desarrollado con ASP.NET Core 8.0.

## Requisitos Previos

- .NET SDK 8.0 o superior
- SQL Server
- Visual Studio 2022 o Visual Studio Code

## Configuración del Entorno de Desarrollo

1. Clonar el repositorio:
```bash
git clone [URL_DEL_REPOSITORIO]
cd Turnos31
```

2. Restaurar las dependencias:
```bash
dotnet restore
```

3. Configurar la base de datos:
   - Actualizar la cadena de conexión en `appsettings.json`
   - Ejecutar las migraciones:
```bash
dotnet ef database update
```

4. Ejecutar el proyecto:
```bash
dotnet run
```

## Estructura del Proyecto

- `Controllers/`: Controladores de la aplicación
- `Models/`: Modelos de datos y ViewModels
- `Views/`: Vistas de la aplicación
- `wwwroot/`: Archivos estáticos (CSS, JavaScript, imágenes)
- `Migrations/`: Migraciones de la base de datos

## Características Principales

- Gestión de turnos médicos
- Administración de veterinarios y especialidades
- Gestión de mascotas y sus historiales clínicos
- Sistema de autenticación y autorización
- Interfaz responsive con Materialize CSS

## Convenciones de Código

- Usar nombres descriptivos en español para clases y métodos
- Seguir las convenciones de C# y .NET
- Documentar cambios importantes en el código
- Realizar pruebas antes de hacer commit

## Flujo de Trabajo Git

1. Crear una rama para cada nueva característica:
```bash
git checkout -b feature/nombre-caracteristica
```

2. Realizar commits frecuentes y descriptivos:
```bash
git commit -m "Descripción clara del cambio"
```

3. Hacer push de los cambios:
```bash
git push origin feature/nombre-caracteristica
```

4. Crear un Pull Request para revisión

## Contacto

Para más información o dudas, contactar a:
[TU_CORREO_AQUÍ] 