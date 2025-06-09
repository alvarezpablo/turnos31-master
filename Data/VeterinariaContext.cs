using Microsoft.EntityFrameworkCore;
using Turnos31.Models;

namespace Turnos31.Data
{
    public class VeterinariaContext : DbContext
    {
        public VeterinariaContext(DbContextOptions<VeterinariaContext> options)
            : base(options)
        {
            Especies = Set<Especie>();
            Razas = Set<Raza>();
            Duenos = Set<Dueno>();
            Mascotas = Set<Mascota>();
            Veterinarios = Set<Veterinario>();
            Especialidades = Set<Especialidad>();
            Agendas = Set<Agenda>();
            Consultas = Set<Consulta>();
            Usuarios = Set<Usuario>();
            VeterinariosEspecialidades = Set<VeterinarioEspecialidad>();
            Examenes = Set<Examen>();
            ResultadosExamenes = Set<ResultadoExamen>();
            Diagnosticos = Set<Diagnostico>();
            Tratamientos = Set<Tratamiento>();
            Productos = Set<Producto>();
            ProductosTratamientos = Set<ProductoTratamiento>();
            Categorias = Set<Categoria>();
            FichasIngreso = Set<FichaIngreso>();
            MotivoVisitas = Set<MotivoVisita>();
            Login = Set<Login>();
            NivelUrgencias = Set<NivelUrgencia>();
            TipoServicios = Set<TipoServicio>();
            Turnos = Set<Turno>();
            Roles = Set<Rol>();
            Menus = Set<Menu>();
            MenuRoles = Set<MenuRol>();
            Usuarios = Set<Usuario>();
        }

        public DbSet<Especie> Especies { get; set; }
        public DbSet<Raza> Razas { get; set; }
        public DbSet<Dueno> Duenos { get; set; }
        public DbSet<Mascota> Mascotas { get; set; }
        public DbSet<Veterinario> Veterinarios { get; set; }
        public DbSet<Especialidad> Especialidades { get; set; }
        public DbSet<Agenda> Agendas { get; set; }
        public DbSet<Consulta> Consultas { get; set; }
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<VeterinarioEspecialidad> VeterinariosEspecialidades { get; set; }
        public DbSet<Examen> Examenes { get; set; }
        public DbSet<ResultadoExamen> ResultadosExamenes { get; set; }
        public DbSet<Diagnostico> Diagnosticos { get; set; }
        public DbSet<Tratamiento> Tratamientos { get; set; }
        public DbSet<Producto> Productos { get; set; }
        public DbSet<ProductoTratamiento> ProductosTratamientos { get; set; }
        public DbSet<Categoria> Categorias { get; set; }
        public DbSet<Login> Login { get; set; }
        public DbSet<FichaIngreso> FichasIngreso { get; set; }
        public DbSet<NivelUrgencia> NivelUrgencias { get; set; }
        public DbSet<MotivoVisita> MotivoVisitas { get; set; }
        public DbSet<TipoServicio> TipoServicios { get; set; }
        public DbSet<Turno> Turnos { get; set; }
        public DbSet<Rol> Roles { get; set; }
        public DbSet<Menu> Menus { get; set; }
        public DbSet<MenuRol> MenuRoles { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configurar relaciones de Usuario
            modelBuilder.Entity<Usuario>()
                .HasIndex(u => u.Email)
                .IsUnique();

            // Configurar relaciones de Menu
            modelBuilder.Entity<Menu>()
                .HasOne(m => m.MenuPadre)
                .WithMany()
                .HasForeignKey(m => m.MenuPadreId)
                .OnDelete(DeleteBehavior.Restrict);

            // Configurar relaciones de MenuRol
            modelBuilder.Entity<MenuRol>()
                .HasOne(mr => mr.Menu)
                .WithMany(m => m.MenuRoles)
                .HasForeignKey(mr => mr.MenuId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<MenuRol>()
                .HasOne(mr => mr.Rol)
                .WithMany(r => r.MenuRoles)
                .HasForeignKey(mr => mr.RolId)
                .OnDelete(DeleteBehavior.Cascade);

            // Configure relationships
            modelBuilder.Entity<Mascota>()
                .HasOne(m => m.Especie)
                .WithMany(e => e.Mascotas)
                .HasForeignKey(m => m.IdEspecie)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Mascota>()
                .HasOne(m => m.Raza)
                .WithMany(r => r.Mascotas)
                .HasForeignKey(m => m.IdRaza)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Mascota>()
                .HasOne(m => m.Dueno)
                .WithMany(d => d.Mascotas)
                .HasForeignKey(m => m.IdDueno)
                .OnDelete(DeleteBehavior.NoAction);

            // Configurar relaciones de Consulta
            modelBuilder.Entity<Consulta>()
                .HasOne(c => c.Agenda)
                .WithMany(a => a.Consultas)
                .HasForeignKey(c => c.IdAgenda)
                .OnDelete(DeleteBehavior.NoAction);

            // Configurar relaciones de Examen
            modelBuilder.Entity<Examen>()
                .HasOne(e => e.Mascota)
                .WithMany(m => m.Examenes)
                .HasForeignKey(e => e.IdMascota)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Examen>()
                .HasOne(e => e.Veterinario)
                .WithMany(v => v.Examenes)
                .HasForeignKey(e => e.IdVeterinario)
                .OnDelete(DeleteBehavior.NoAction);

            // Configurar relaciones de ResultadoExamen
            modelBuilder.Entity<ResultadoExamen>()
                .HasOne(r => r.Examen)
                .WithMany(e => e.Resultados)
                .HasForeignKey(r => r.IdExamen)
                .OnDelete(DeleteBehavior.NoAction);

            // Configurar relaciones de Diagnostico
            modelBuilder.Entity<Diagnostico>()
                .HasOne(d => d.Consulta)
                .WithMany(c => c.Diagnosticos)
                .HasForeignKey(d => d.IdConsulta)
                .OnDelete(DeleteBehavior.NoAction);

            // Configurar relaciones de Tratamiento
            modelBuilder.Entity<Tratamiento>()
                .HasOne(t => t.Consulta)
                .WithMany(c => c.Tratamientos)
                .HasForeignKey(t => t.IdConsulta)
                .OnDelete(DeleteBehavior.NoAction);

            // Configurar relaciones de ProductoTratamiento
            modelBuilder.Entity<ProductoTratamiento>()
                .HasOne(pt => pt.Producto)
                .WithMany(p => p.ProductosTratamientos)
                .HasForeignKey(pt => pt.IdProducto)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<ProductoTratamiento>()
                .HasOne(pt => pt.Tratamiento)
                .WithMany(t => t.ProductosTratamientos)
                .HasForeignKey(pt => pt.IdTratamiento)
                .OnDelete(DeleteBehavior.NoAction);

            // Configurar relaciones de Producto
            modelBuilder.Entity<Producto>()
                .HasOne(p => p.Categoria)
                .WithMany(c => c.Productos)
                .HasForeignKey(p => p.CategoriaId)
                .OnDelete(DeleteBehavior.NoAction);

            // Configurar relaciones de VeterinarioEspecialidad
            modelBuilder.Entity<VeterinarioEspecialidad>()
                .HasOne(ve => ve.Veterinario)
                .WithMany(v => v.Especialidades)
                .HasForeignKey(ve => ve.IdVeterinario)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<VeterinarioEspecialidad>()
                .HasOne(ve => ve.Especialidad)
                .WithMany(e => e.Veterinarios)
                .HasForeignKey(ve => ve.IdEspecialidad)
                .OnDelete(DeleteBehavior.NoAction);

            // Configurar relaciones de Turno
            modelBuilder.Entity<Turno>()
                .HasOne(t => t.Veterinario)
                .WithMany(v => v.Turnos)
                .HasForeignKey(t => t.IdVeterinario)
                .OnDelete(DeleteBehavior.NoAction);

            // Configurar precisiones para campos decimales
            modelBuilder.Entity<Consulta>()
                .Property(c => c.FrecuenciaCardiaca)
                .HasPrecision(5, 2);

            modelBuilder.Entity<Consulta>()
                .Property(c => c.FrecuenciaRespiratoria)
                .HasPrecision(5, 2);

            modelBuilder.Entity<Consulta>()
                .Property(c => c.Peso)
                .HasPrecision(5, 2);

            modelBuilder.Entity<Consulta>()
                .Property(c => c.Temperatura)
                .HasPrecision(5, 2);

            // Configurar la relación Usuario-Rol
            modelBuilder.Entity<Usuario>()
                .HasOne(u => u.Rol)
                .WithMany(r => r.Usuarios)
                .HasForeignKey(u => u.IdRol)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}