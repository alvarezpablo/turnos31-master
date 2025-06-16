using Turnos31.Models;

namespace Turnos31.Data
{
    public static class DataSeeder
    {
        public static void SeedData(VeterinariaContext context)
        {
            // Asegurar que la base de datos esté creada
            context.Database.EnsureCreated();

            // Verificar si ya hay datos
            if (context.Especies.Any())
            {
                return; // Ya hay datos
            }

            // Crear especies
            var especies = new List<Especie>
            {
                new Especie { Nombre = "Canina" },
                new Especie { Nombre = "Felina" },
                new Especie { Nombre = "Equino" },
                new Especie { Nombre = "Aves" },
                new Especie { Nombre = "Reptiles" }
            };

            context.Especies.AddRange(especies);
            context.SaveChanges();

            // Crear razas
            var razas = new List<Raza>
            {
                // Razas caninas
                new Raza { Nombre = "Labrador", IdEspecie = especies[0].IdEspecie, Activo = true },
                new Raza { Nombre = "Golden Retriever", IdEspecie = especies[0].IdEspecie, Activo = true },
                new Raza { Nombre = "Pastor Alemán", IdEspecie = especies[0].IdEspecie, Activo = true },
                new Raza { Nombre = "Bulldog", IdEspecie = especies[0].IdEspecie, Activo = true },
                new Raza { Nombre = "Chihuahua", IdEspecie = especies[0].IdEspecie, Activo = true },

                // Razas felinas
                new Raza { Nombre = "Persa", IdEspecie = especies[1].IdEspecie, Activo = true },
                new Raza { Nombre = "Siamés", IdEspecie = especies[1].IdEspecie, Activo = true },
                new Raza { Nombre = "Maine Coon", IdEspecie = especies[1].IdEspecie, Activo = true },
                new Raza { Nombre = "Británico de Pelo Corto", IdEspecie = especies[1].IdEspecie, Activo = true },

                // Razas equinas
                new Raza { Nombre = "Pura Sangre", IdEspecie = especies[2].IdEspecie, Activo = true },
                new Raza { Nombre = "Cuarto de Milla", IdEspecie = especies[2].IdEspecie, Activo = true },

                // Aves
                new Raza { Nombre = "Canario", IdEspecie = especies[3].IdEspecie, Activo = true },
                new Raza { Nombre = "Loro", IdEspecie = especies[3].IdEspecie, Activo = true },

                // Reptiles
                new Raza { Nombre = "Iguana", IdEspecie = especies[4].IdEspecie, Activo = true },
                new Raza { Nombre = "Gecko", IdEspecie = especies[4].IdEspecie, Activo = true }
            };

            context.Razas.AddRange(razas);
            context.SaveChanges();

            // Crear dueños
            var duenos = new List<Dueno>
            {
                new Dueno
                {
                    Nombre = "Juan",
                    Apellido = "Pérez",
                    Direccion = "Calle 123",
                    Rut = "12345678-9",
                    Telefono = "123456789",
                    Email = "juan@email.com"
                },
                new Dueno
                {
                    Nombre = "María",
                    Apellido = "García",
                    Direccion = "Avenida 456",
                    Rut = "98765432-1",
                    Telefono = "987654321",
                    Email = "maria@email.com"
                }
            };

            context.Duenos.AddRange(duenos);
            context.SaveChanges();

            // Crear veterinarios
            var veterinarios = new List<Veterinario>
            {
                new Veterinario 
                { 
                    Nombre = "Dr. Carlos", 
                    Apellido = "Rodríguez", 
                    Direccion = "Clínica Central", 
                    Telefono = "555-0001", 
                    Email = "carlos@veterinaria.com",
                    HorarioAtencionDesde = new DateTime(1900, 1, 1, 8, 0, 0),
                    HorarioAtencionHasta = new DateTime(1900, 1, 1, 18, 0, 0)
                },
                new Veterinario 
                { 
                    Nombre = "Dra. Ana", 
                    Apellido = "López", 
                    Direccion = "Clínica Norte", 
                    Telefono = "555-0002", 
                    Email = "ana@veterinaria.com",
                    HorarioAtencionDesde = new DateTime(1900, 1, 1, 9, 0, 0),
                    HorarioAtencionHasta = new DateTime(1900, 1, 1, 17, 0, 0)
                }
            };

            context.Veterinarios.AddRange(veterinarios);
            context.SaveChanges();

            // Crear especialidades
            var especialidades = new List<Especialidad>
            {
                new Especialidad { Descripcion = "Medicina General" },
                new Especialidad { Descripcion = "Cirugía" },
                new Especialidad { Descripcion = "Dermatología" },
                new Especialidad { Descripcion = "Cardiología" },
                new Especialidad { Descripcion = "Oftalmología" }
            };

            context.Especialidades.AddRange(especialidades);
            context.SaveChanges();

            // Crear roles
            var roles = new List<Rol>
            {
                new Rol { NombreRol = "Administrador", Descripcion = "Acceso completo al sistema" },
                new Rol { NombreRol = "Veterinario", Descripcion = "Acceso a consultas y tratamientos" },
                new Rol { NombreRol = "Recepcionista", Descripcion = "Acceso a citas y pacientes" }
            };

            context.Roles.AddRange(roles);
            context.SaveChanges();

            // Crear usuarios
            var usuarios = new List<Usuario>
            {
                new Usuario
                {
                    Nombre = "Admin",
                    Apellido = "Sistema",
                    Password = "admin123",
                    Email = "admin@veterinaria.com",
                    IdRol = roles[0].IdRol,
                    Activo = true
                },
                new Usuario
                {
                    Nombre = "Dr. Veterinario",
                    Apellido = "Principal",
                    Password = "vet123",
                    Email = "vet1@veterinaria.com",
                    IdRol = roles[1].IdRol,
                    Activo = true
                }
            };

            context.Usuarios.AddRange(usuarios);
            context.SaveChanges();
        }
    }
}
