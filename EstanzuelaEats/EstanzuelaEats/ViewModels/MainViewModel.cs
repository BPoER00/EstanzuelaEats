
namespace EstanzuelaEats.ViewModels
{
    using System;
    using System.Collections.ObjectModel;
    using System.Windows.Input;
    using EstanzuelaEats.Helpers;
    using GalaSoft.MvvmLight.Command;
    using Views;
    using Xamarin.Forms;

    public class MainViewModel
    {
        #region Propiedades

        public LoginViewModel Login { get; set; }
        public ProductsViewModel Productos { get; set; }
        public AddProductViewModel AddProduct { get; set; }
        public EditProductViewModel EditProduct { get; set; }
        public ObservableCollection<MenuItemViewModel> Menu { get; set; } 


        #endregion

        #region Constructores

        public MainViewModel()
        {
            instance = this;
            this.LoadMenu();
        }
 
        #endregion

        #region Comandos
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
            await App.Navigator.PushAsync(new AddProductPage());
        }
        #endregion

        #region Singleton

        private static MainViewModel instance;
        public static MainViewModel GetInstance()
        {
            if (instance == null)
            {
                return new MainViewModel();
            }

            return instance;
        }

        #endregion

        #region Metodos
        private void LoadMenu()
        {
            this.Menu = new ObservableCollection<MenuItemViewModel>();

            this.Menu.Add(new MenuItemViewModel
            {
                Icon = "Informacion.png",
                PageName = "AboutPage",
                Title = Languages.About,
            });

            this.Menu.Add(new MenuItemViewModel
            {
                Icon = "Ajustes.png",
                PageName = "SetupPage",
                Title = Languages.Setup,
            });

            this.Menu.Add(new MenuItemViewModel
            {
                Icon = "CerrarSesion.png",
                PageName = "LoginPage",
                Title = Languages.Exit,
            });
        }
        #endregion

    }
}
