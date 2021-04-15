namespace EstanzuelaEats.ViewModels
{
    using Common.Modelos;
    using GalaSoft.MvvmLight.Command;
    using Helpers;
    using Services;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Windows.Input;
    using Xamarin.Forms;

    public class ProductsViewModel : BaseViewModel
    {

        #region Atributos

        //objetos Privados

        //variable privada llamada Products que es la que actualiza cada uno de sus datos
        private ObservableCollection<ProductItemViewModel> productos;

        //variable privada que nos ayuda a refrescar
        private bool isRefreshing;

        //variable privada que nos ayuda a instanciar la clase ApiService que es donde tendremos cada uno de nuestros servicios
        private ApiService apiService;


        #endregion


        #region Propiedades
        public ObservableCollection<ProductItemViewModel> Productos
        {
            get { return this.productos; }
            set { this.SetValue(ref this.productos, value); }
        }

        //valida el regresco de la lista esta llama al metodo privado para poder refrescar
        public bool IsRefreshing
        {
            get { return this.isRefreshing; }
            set { this.SetValue(ref this.isRefreshing, value); }
        }
        #endregion


        #region Constructores
        public ProductsViewModel()
        {
            instance = this;
            this.apiService = new ApiService();
            this.LoadProducts();
        }
        #endregion


        #region Singleton

        private static ProductsViewModel instance;
        public static ProductsViewModel GetInstance()
        {
            if(instance == null)
            {
                return new ProductsViewModel();
            }

            return instance;
        }

        #endregion


        #region Metodos
        private async void LoadProducts()
        {
            //activamos el refresh
            this.IsRefreshing = true;

            //comprobamos si el usuario tiene conexion a internet
            var Connection = await this.apiService.CheckConnection();
            if (!Connection.Logrado)
            {
                this.IsRefreshing = false;
                await Application.Current.MainPage.DisplayAlert(Languages.Error, Connection.Mensaje, Languages.Accept);
                return;
            }

            //conectamos con el diccionario de recursos para traer la url de nuestro backend de datos
            var url = Application.Current.Resources["UrlAPI"].ToString();
            var urlPrefix = Application.Current.Resources["UrlPrefix"].ToString();
            var urlProductsController = Application.Current.Resources["UrlProductsController"].ToString();

            //hacemos nuestra conexion al backend para traer una lista de datos de la clase products que esta en el Common
            var response = await this.apiService.GetList<Productos>(url, urlPrefix, urlProductsController);

            //revisamos si realizo correctamenta la accion
            if (!response.Logrado)
            {
                //si no lo hizo mostrara error en unos mensajes que se tienen en el Common y lo saca
                this.IsRefreshing = false;
                await Application.Current.MainPage.DisplayAlert(Languages.Error, response.Mensaje, Languages.Accept);
                return;
            }

            //si lo hizo devuelve la lista y la muestra en pantalla
            var lista = (List<Productos>)response.Resultado;
            var myList = lista.Select(p => new ProductItemViewModel { 
                ProductoId = p.ProductoId,
                NombreProducto = p.NombreProducto,
                PrecioProducto = p.PrecioProducto,
                DescripcionProducto = p.DescripcionProducto,
                Existencias = p.Existencias,
                PublicacionProducto = p.PublicacionProducto,
                ImagePath = p.ImagePath,
                ImageArray = p.ImageArray
            });
            this.Productos = new ObservableCollection<ProductItemViewModel>(myList);
            this.IsRefreshing = false;
        }
        #endregion


        #region Comandos
        //nos ayuda a refrescar la pagina en la que estamos en la lista
        public ICommand RefreshCommand
        {
            get
            {
                return new RelayCommand(LoadProducts);
            }
        }

        #endregion


    }
}
