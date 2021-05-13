
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
        public UsuariosASP UserASP { get; set; }
        public ObservableCollection<MenuItemViewModel> Menu { get; set; } 
        public CategoriesViewModel Categories { get; set; }
        public ObservableCollection<InformationViewModel> Info { get; set; } 
        public ObservableCollection<SetupViewModel> Setup { get; set; }

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
                foreach (var claim in this.UserASP.Claims)
                {
                    if (claim.ClaimType == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/uri")
                    {
                        if (claim.ClaimValue.StartsWith("~"))
                        {
                            return $"https://estanzuelaeats.azurewebsites.net{claim.ClaimValue.Substring(1)}";
                        }

                        return claim.ClaimValue;
                    }
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
            this.LoadInfo();
            this.LoadSetup();
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

        private void LoadInfo()
        {
            this.Info = new ObservableCollection<InformationViewModel>();

            this.Info.Add(new InformationViewModel
            {
                Nombre = "Nombre: Bryan Emanuel Paz Ramirez"
            });

            this.Info.Add(new InformationViewModel
            {
                Nombre = "Carnet: 1190-19-3929"
            });

            this.Info.Add(new InformationViewModel
            {
                Nombre = "Tarea: Proyecto Final"
            });

            this.Info.Add(new InformationViewModel
            {
                Nombre = "Curso: Programacion III"
            });

            this.Info.Add(new InformationViewModel
            {
                Nombre = "Catedratico: Ing Jose Vinicio Peña Roman"
            });
        }
        
        private void LoadSetup()
        {
            this.Setup = new ObservableCollection<SetupViewModel>();

            this.Setup.Add(new SetupViewModel
            {
                Foto = "imagehttp.png",
                Titulo = "Visitanos nuestra Pagina",
                PageName = "SetupHttp"
            });

            this.Setup.Add(new SetupViewModel
            {
                Foto = "imagerepository.png",
                Titulo = "Nuestro Repositorio",
                PageName = "SetupRepository"
            });


        }
        #endregion

    }
}
