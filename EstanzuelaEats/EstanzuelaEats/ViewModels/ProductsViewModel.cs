namespace EstanzuelaEats.ViewModels
{
    using Common.Modelos;
    using GalaSoft.MvvmLight.Command;
    using Helpers;
    using Services;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Windows.Input;
    using Xamarin.Forms;

    public class ProductsViewModel : BaseViewModel
    {

        #region Atributos

        private string filter;

        //variable privada llamada Products que es la que actualiza cada uno de sus datos
        private ObservableCollection<ProductItemViewModel> productos;

        //variable privada que nos ayuda a refrescar
        private bool isRefreshing;

        //variable privada que nos ayuda a instanciar la clase ApiService que es donde tendremos cada uno de nuestros servicios
        private ApiService apiService;

        private DataService dataService;


        #endregion


        #region Propiedades

        public string Filter
        {
            get { return this.filter; }

            set 
            { 
                this.filter = value;
                this.RefreshList();
            }
        }
        public List<Productos> MyProducts { get; set; }

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

        public Category Category
        {
            get;
            set;
        }
        #endregion


        #region Constructores
        public ProductsViewModel(Category category)
        {
            instance = this;
            this.Category = category;
            this.apiService = new ApiService();
            this.dataService = new DataService();
            this.LoadProducts();
        }
        #endregion


        #region Singleton

        private static ProductsViewModel instance;

        public static ProductsViewModel GetInstance()
        {
            return instance;
        }

        #endregion


        #region Metodos
        private async void LoadProducts()
        {
            this.IsRefreshing = true;

            var connection = await this.apiService.CheckConnection();
            if (!connection.Logrado)
            {
                this.IsRefreshing = false;
                await Application.Current.MainPage.DisplayAlert
                    (Languages.Error, 
                     connection.Mensaje, 
                     Languages.Accept
                     );
                return;
            }

            var answer = await this.LoadProductsFromApi();
            if (answer)
            {
                this.RefreshList();
            }

            this.IsRefreshing = false;
            this.IsRefreshing = false;
        }

        private async Task LoadProductsFromDB()
        {
            this.MyProducts = await this.dataService.GetAllProducts();
        }

        private async Task SaveProductsDB()
        {
            await this.dataService.DeleteAllProducts();
            dataService.Insert(this.MyProducts);
        }

        private async Task<bool> LoadProductsFromApi()
        {
            var url = Application.Current.Resources["UrlAPI"].ToString();
            var prefix = Application.Current.Resources["UrlPrefix"].ToString();
            var controller = Application.Current.Resources["UrlProductsController"].ToString();
            var response = await this.apiService.GetList<Productos>(url, prefix, controller, this.Category.CategoriaId, Settings.TokenType, Settings.AccessToken);
            if (!response.Logrado)
            {
                return false;
            }

            this.MyProducts = (List<Productos>)response.Resultado;
            return true;
        }

        public void RefreshList()
        {
            if (string.IsNullOrEmpty(Filter))
            {

                var myListProductItemViewModel = this.MyProducts.Select(p => new ProductItemViewModel
                {
                    ProductoId = p.ProductoId,
                    NombreProducto = p.NombreProducto,
                    PrecioProducto = p.PrecioProducto,
                    DescripcionProducto = p.DescripcionProducto,
                    Existencias = p.Existencias,
                    PublicacionProducto = p.PublicacionProducto,
                    ImagePath = p.ImagePath,
                    ImageArray = p.ImageArray
                });
                this.Productos = new ObservableCollection<ProductItemViewModel>(myListProductItemViewModel.OrderBy(p => p.NombreProducto));
            }
            else
            {
                var myListProductItemViewModel = this.MyProducts.Select(p => new ProductItemViewModel
                {
                    ProductoId = p.ProductoId,
                    NombreProducto = p.NombreProducto,
                    PrecioProducto = p.PrecioProducto,
                    DescripcionProducto = p.DescripcionProducto,
                    Existencias = p.Existencias,
                    PublicacionProducto = p.PublicacionProducto,
                    ImagePath = p.ImagePath,
                    ImageArray = p.ImageArray
                }).Where(p => p.NombreProducto.ToLower().Contains(this.Filter.ToLower())).ToList();
                this.Productos = new ObservableCollection<ProductItemViewModel>(myListProductItemViewModel.OrderBy(p => p.NombreProducto));
            }
        }
        #endregion


        #region Comandos
        public ICommand SearchCommand 
        {
            get
            {
                return new RelayCommand(RefreshList);
            }
        }

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
