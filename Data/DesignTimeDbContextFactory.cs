using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Turnos31.Data
{
    public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<VeterinariaContext>
    {
        public VeterinariaContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<VeterinariaContext>();
            optionsBuilder.UseSqlite("Data Source=veterinaria.db");

            return new VeterinariaContext(optionsBuilder.Options);
        }
    }
}
