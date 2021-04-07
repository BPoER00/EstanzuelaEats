
namespace EstanzuelaEats.Backend.Models
{
    using Common.Modelos;
    using System.Web;

    public class ProductView : Productos
    {
        public HttpPostedFileBase ImageFile { get; set; }
    }
}