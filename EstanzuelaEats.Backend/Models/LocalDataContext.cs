

namespace EstanzuelaEats.Backend.Models
{

    using System.Data.Entity;
    using Domain.Modelos;
    

    public class LocalDataContext : DataContext
    {

        public new DbSet<EstanzuelaEats.Common.Modelos.Productos> Productos { get; set; }

    }
}