
namespace EstanzuelaEats.ViewModels
{
    public class MainViewModel
    {
        public ProductsViewModel Productos { get; set; }

        public MainViewModel()
        {
            this.Productos = new ProductsViewModel();
        }
    }
}
