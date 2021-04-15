
namespace EstanzuelaEats.Api.Controllers
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using System.IO;
    using System.Linq;
    using System.Net;
    using System.Threading.Tasks;
    using System.Web.Http;
    using System.Web.Http.Description;
    using Common.Modelos;
    using Domain.Modelos;
    using EstanzuelaEats.Api.Helpers;

    public class ProductosController : ApiController
    {
        private DataContext db = new DataContext();

        // GET: api/Productos
        public IQueryable<Productos> GetProductos()
        {
            return db.Productos.OrderBy(p => p.NombreProducto);
        }

        // GET: api/Productos/5
        [ResponseType(typeof(Productos))]
        public async Task<IHttpActionResult> GetProductos(int id)
        {
            var productos = await db.Productos.FindAsync(id);

            if (productos == null)
            {
                return NotFound();
            }

            return Ok(productos);
        }

        // PUT: api/Productos/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutProductos(int id, Productos productos)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != productos.ProductoId)
            {
                return BadRequest();
            }

            db.Entry(productos).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductosExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/Productos
        [ResponseType(typeof(Productos))]
        public async Task<IHttpActionResult> PostProductos(Productos productos)
        {
            productos.Existencias = true;
            productos.PublicacionProducto = DateTime.Now.ToUniversalTime();

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (productos.ImageArray != null && productos.ImageArray.Length > 0)
            {
                var stream = new MemoryStream(productos.ImageArray);
                var guid = Guid.NewGuid().ToString();
                var file = $"{guid}.jpg";
                var folder = "~/Content/Productos";
                var fullPath = $"{folder}/{file}";
                var response = FilesHelper.UploadPhoto(stream, folder, file);

                if (response)
                {
                    productos.ImagePath = fullPath;
                }
            }

            db.Productos.Add(productos);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = productos.ProductoId }, productos);
        }

        // DELETE: api/Productos/5
        [ResponseType(typeof(Productos))]
        public async Task<IHttpActionResult> DeleteProductos(int id)
        {
            Productos productos = await db.Productos.FindAsync(id);
            if (productos == null)
            {
                return NotFound();
            }

            db.Productos.Remove(productos);
            await db.SaveChangesAsync();

            return Ok(productos);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ProductosExists(int id)
        {
            return db.Productos.Count(e => e.ProductoId == id) > 0;
        }
    }
}