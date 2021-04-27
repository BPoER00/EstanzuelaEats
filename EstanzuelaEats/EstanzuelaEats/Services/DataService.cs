
namespace EstanzuelaEats.Services
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Common.Modelos;
    using Interfaces;
    using SQLite;
    using Xamarin.Forms;

    public class DataService
    {
        private SQLiteAsyncConnection connection;

        public DataService()
        {
            this.OpenOrCreateDB();
        }

        private async Task OpenOrCreateDB()
        {
            var databasePath = DependencyService.Get<IPathService>().GetDatabasePath();
            this.connection = new SQLiteAsyncConnection(databasePath);
            await connection.CreateTableAsync<Productos>().ConfigureAwait(false);
        }

        public async Task Insert<T>(T model)
        {
            await this.connection.InsertAsync(model);
        }

        public async Task Insert<T>(List<T> models)
        {
            await this.connection.InsertAllAsync(models);
        }

        public async Task Update<T>(T model)
        {
            await this.connection.UpdateAsync(model);
        }

        public async Task Update<T>(List<T> models)
        {
            await this.connection.UpdateAllAsync(models);
        }

        public async Task Delete<T>(T model)
        {
            await this.connection.DeleteAsync(model);
        }

        public async Task<List<Productos>> GetAllProducts()
        {
            var query = await this.connection.QueryAsync<Productos>("select * from [Productos]");
            var array = query.ToArray();
            var list = array.Select(p => new Productos
            {
                ProductoId = p.ProductoId,
                NombreProducto = p.NombreProducto,
                PrecioProducto = p.PrecioProducto,
                DescripcionProducto = p.DescripcionProducto,
                ImagePath = p.ImagePath,
                PublicacionProducto = p.PublicacionProducto,
                Existencias = p.Existencias
            }).ToList();
            return list;
        }

        public async Task DeleteAllProducts()
        {
            var query = await this.connection.QueryAsync<Productos>("delete from [Productos]");
        }
    }
}
