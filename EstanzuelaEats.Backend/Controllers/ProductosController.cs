
namespace EstanzuelaEats.Backend.Controllers
{
    using Backend.Models;
    using Common.Modelos;
    using EstanzuelaEats.Backend.Helpers;
    using System.Data.Entity;
    using System.Linq;
    using System.Net;
    using System.Threading.Tasks;
    using System.Web.Mvc;

    [Authorize]
    public class ProductosController : Controller
    {
        private LocalDataContext db = new LocalDataContext();

        // GET: Productos
        public async Task<ActionResult> Index()
        {
            return View(await this.db.Productos.OrderBy(p => p.DescripcionProducto).ToListAsync());
        }

        // GET: Productos/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var productos = await db.Productos.FindAsync(id);
            if (productos == null)
            {
                return HttpNotFound();
            }
            return View(productos);
        }

        // GET: Productos/Create
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(ProductView productos)
        {
            if (ModelState.IsValid)
            {

                var pic = string.Empty;
                var folder = "~/Content/Productos";

                if (productos.ImageFile != null)
                {
                    pic = FilesHelper.UploadPhoto(productos.ImageFile, folder);
                    pic = $"{folder}/{pic}";
                }

                var product = this.ToProduct(productos, pic);


                db.Productos.Add(product);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(productos);
        }

        private Productos ToProduct(ProductView productos, string pic)
        {
            return new Productos
            {
                ProductoId = productos.ProductoId,
                NombreProducto = productos.NombreProducto,
                DescripcionProducto = productos.DescripcionProducto,
                PrecioProducto = productos.PrecioProducto,
                ImagePath = pic,
                PublicacionProducto = productos.PublicacionProducto,
                Existencias = productos.Existencias

            };
        }

        // GET: Productos/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var productos = await db.Productos.FindAsync(id);
            if (productos == null)
            {
                return HttpNotFound();
            }

            var productovista = this.ToView(productos);
            return View(productovista);
        }

        private ProductView ToView(Productos productos)
        {
            return new ProductView
            {
                ProductoId = productos.ProductoId,
                NombreProducto = productos.NombreProducto,
                DescripcionProducto = productos.DescripcionProducto,
                PrecioProducto = productos.PrecioProducto,
                ImagePath = productos.ImagePath,
                PublicacionProducto = productos.PublicacionProducto,
                Existencias = productos.Existencias
            };
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(ProductView productos)
        {
            if (ModelState.IsValid)
            {
                var pic = string.Empty;
                var folder = "~/Content/Productos";

                if (productos.ImageFile != null)
                {
                    pic = FilesHelper.UploadPhoto(productos.ImageFile, folder);
                    pic = $"{folder}/{pic}";
                }

                var product = this.ToProduct(productos, pic);

                this.db.Entry(product).State = EntityState.Modified;
                await this.db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(productos);
        }

        // GET: Productos/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var productos = await db.Productos.FindAsync(id);
            if (productos == null)
            {
                return HttpNotFound();
            }
            return View(productos);
        }

        // POST: Productos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            var productos = await db.Productos.FindAsync(id);
            db.Productos.Remove(productos);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
