
namespace EstanzuelaEats.Backend.Models
{
    using System.Web;
    using Common.Modelos;

    public class ProductView : Productos
    {
        public HttpPostedFileBase ImageFile { get; set; }
    }
}