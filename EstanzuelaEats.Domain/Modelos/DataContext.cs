

namespace EstanzuelaEats.Domain.Modelos
{
    using Common.Modelos;
    using System.Data.Entity;

    public class DataContext : DbContext
    {
        public DataContext()
            : base("DefaultConnection")
        {

        }

        public DbSet<Productos> Productos { get; set; }
        public DbSet<Category> Categories { get; set; }
    }
}
