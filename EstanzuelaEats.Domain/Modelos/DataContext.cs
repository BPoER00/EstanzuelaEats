

namespace EstanzuelaEats.Domain.Modelos
{
    using System.Data.Entity;
    using Common.Modelos;
    
    public class DataContext : DbContext
    {
        public DataContext()
            :base("DefaultConnection")
        {

        }

        public DbSet<Productos> Productos { get; set; }
    }
}
