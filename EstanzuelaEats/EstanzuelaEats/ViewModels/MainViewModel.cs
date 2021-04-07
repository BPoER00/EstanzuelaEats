
namespace EstanzuelaEats.ViewModels
{
    using System;
    using System.Windows.Input;
    using GalaSoft.MvvmLight.Command;
    using Views;
    using Xamarin.Forms;

    public class MainViewModel
    {
        public ProductsViewModel Productos { get; set; }
        public AddProductViewModel AddProduct { get; set; }

        public MainViewModel()
        {
            this.Productos = new ProductsViewModel();

        }

        public ICommand AddProductCommand
        {
            get
            {
                return new RelayCommand(GoToAddProduct);
            }
        }

        private async void GoToAddProduct()
        {
            this.AddProduct = new AddProductViewModel();
            await Application.Current.MainPage.Navigation.PushAsync(new AddProductPage());
        }
    }
}
