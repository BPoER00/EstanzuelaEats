
namespace EstanzuelaEats.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    public class MainViewModel
    {
        public ProductsViewModel Productos { get; set; }

        public MainViewModel()
        {
            this.Productos = new ProductsViewModel();
        }
    }
}
