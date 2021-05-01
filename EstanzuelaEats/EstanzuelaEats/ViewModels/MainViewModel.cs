
namespace EstanzuelaEats.ViewModels
{
    using System;
    using System.Collections.ObjectModel;
    using System.Windows.Input;
    using EstanzuelaEats.Common.Modelos;
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
        public RegisterViewModel Register { get; set; }
        public MyUserASP UserASP { get; set; }
        public ObservableCollection<MenuItemViewModel> Menu { get; set; } 

        public string UserFullName
        {
            get
            {
                if (this.UserASP != null && this.UserASP.Claims != null && this.UserASP.Claims.Count > 1)
                {
                    return $"{this.UserASP.Claims[0].ClaimValue} {this.UserASP.Claims[1].ClaimValue}";
                }

                return "avatarlogin.png";
            }
        }

        public string UserImageFullPath
        {
            get
            {
                if (this.UserASP != null && this.UserASP.Claims != null && this.UserASP.Claims.Count > 3)
                {
                    return $"https://estanzuelaeatsapi20212503.azurewebsites.net{this.UserASP.Claims[3].ClaimValue.Substring(1)}";
                }
                return null;
            }
        }

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
