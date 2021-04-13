
namespace EstanzuelaEats.ViewModels
{
    using EstanzuelaEats.Common.Modelos;
    using EstanzuelaEats.Helpers;
    using EstanzuelaEats.Services;
    using GalaSoft.MvvmLight.Command;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Windows.Input;
    using Xamarin.Forms;

    public class AddProductViewModel : BaseViewModel
    {
        #region Atributo
        private ApiService api;
        private bool isRunning;
        private bool isEnable;
        private ImageSource imageSource;
        #endregion

        #region Propiedades

        public string Name { get; set; }
        public string Price { get; set; }
        public string Description { get; set; }
        public bool IsRunning 
        {
            get { return this.isRunning; }
            set { this.SetValue(ref this.isRunning, value); }
        }

        public bool IsEnable 
        { 
            get { return this.isEnable; }
            set { this.SetValue(ref this.isEnable, value); }
        }

        public ImageSource ImageSource
        {
            get { return this.imageSource; }
            set { this.SetValue(ref this.imageSource, value); }
        }
        #endregion

        #region Constructores
        public AddProductViewModel()
        {
            this.api = new ApiService();
            this.IsEnable = true;
            this.ImageSource = "NoProducto.png";
        }
        #endregion

        #region Comandos

        public ICommand SaveCommand {
            get
            {
                return new RelayCommand(Save);
            }
        }

        private async void Save()
        {
            if (string.IsNullOrEmpty(this.Name))
            {
                await Application.Current.MainPage.DisplayAlert(
                    Languages.Error,
                    Languages.NameError,
                    Languages.Accept);
                return;
            }

            if (string.IsNullOrEmpty(this.Price))
            {
                await Application.Current.MainPage.DisplayAlert(
                    Languages.Error,
                    Languages.PriceError,
                    Languages.Accept);
                return;
            }

            var price = decimal.Parse(this.Price);

            if (price < 0)
            {
                await Application.Current.MainPage.DisplayAlert(
                    Languages.Error,
                    Languages.PriceError,
                    Languages.Accept);
                return;
            }

            if (string.IsNullOrEmpty(this.Description))
            {
                await Application.Current.MainPage.DisplayAlert(
                    Languages.Error,
                    Languages.DescriptionError,
                    Languages.Accept);
                return;
            }

            this.IsRunning = true;
            this.IsEnable = false;

            var Connection = await this.api.CheckConnection();
            if (!Connection.Logrado)
            {
                this.IsRunning = false;
                this.IsEnable = true;
                await Application.Current.MainPage.DisplayAlert(Languages.Error, Connection.Mensaje, Languages.Accept);
                return;
            }

            var producto = new Productos
            {
                NombreProducto = this.Name,
                PrecioProducto = price,
                DescripcionProducto = this.Description,
            };

            //conectamos con el diccionario de recursos para traer la url de nuestro backend de datos
            var url = Application.Current.Resources["UrlAPI"].ToString();
            var urlPrefix = Application.Current.Resources["UrlPrefix"].ToString();
            var urlProductsController = Application.Current.Resources["UrlProductsController"].ToString();

            //hacemos nuestra conexion al backend para traer una lista de datos de la clase products que esta en el Common
            var response = await this.api.Post<Productos>(url, urlPrefix, urlProductsController, producto);

            if (!response.Logrado)
            {
                this.IsRunning = false;
                this.IsEnable = true;
                await Application.Current.MainPage.DisplayAlert(
                    Languages.Error,
                    response.Mensaje,
                    Languages.Accept);
                return;
            }

            var newProduct = (Productos)response.Resultado;
            var MisProductos = ProductsViewModel.GetInstance();
            MisProductos.Productos.Add(newProduct);

            this.isRunning = false;
            this.isEnable = true;

            await Application.Current.MainPage.Navigation.PopAsync();

        }

        #endregion
    }
}
