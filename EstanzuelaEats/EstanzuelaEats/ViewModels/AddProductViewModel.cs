
namespace EstanzuelaEats.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Common.Modelos;
    using Helpers;
    using Services;
    using GalaSoft.MvvmLight.Command;
    using Plugin.Media.Abstractions;
    using System.Windows.Input;
    using Xamarin.Forms;
    using Plugin.Media;
    using System.Collections.ObjectModel;
    using System.Threading.Tasks;

    public class AddProductViewModel : BaseViewModel
    {
        #region Atributo
        private MediaFile file;
        private ApiService api;
        private bool isRunning;
        private bool isEnable;
        private ImageSource imageSource;
        private ObservableCollection<Category> categories;
        private Category category;
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

        public List<Category> MyCategories { get; set; }

        public Category Category
        {
            get { return this.category; }
            set { this.SetValue(ref this.category, value); }
        }

        public ObservableCollection<Category> Categories
        {
            get { return this.categories; }
            set { this.SetValue(ref this.categories, value); }
        }
        #endregion

        #region Constructores
        public AddProductViewModel()
        {
            this.api = new ApiService();
            this.IsEnable = true;
            this.ImageSource = "NoProducto.png";
            this.LoadCategories();
        }
        #endregion

        #region Metodos
        private async void LoadCategories()
        {
            this.IsRunning = true;
            this.IsEnable = false;

            var connection = await this.api.CheckConnection();
            if (!connection.Logrado)
            {
                this.IsRunning = false;
                this.IsEnable = true;
                await Application.Current.MainPage.DisplayAlert
                    (Languages.Error, 
                     connection.Mensaje, 
                     Languages.Accept
                     );
                return;
            }

            var answer = await this.LoadCategoriesFromAPI();
            if (answer)
            {
                this.RefreshList();
            }

            this.IsRunning = false;
            this.IsEnable = true;
        }

        private void RefreshList()
        {
            this.Categories = new ObservableCollection<Category>(this.MyCategories.OrderBy(c => c.Description));
        }

        private async Task<bool> LoadCategoriesFromAPI()
        {
            var url = Application.Current.Resources["UrlAPI"].ToString();
            var prefix = Application.Current.Resources["UrlPrefix"].ToString();
            var controller = Application.Current.Resources["UrlCategoriesController"].ToString();
            var response = await this.api.GetList<Category>(url, prefix, controller, Settings.TokenType, Settings.AccessToken);
            if (!response.Logrado)
            {
                return false;
            }

            this.MyCategories = (List<Category>)response.Resultado;
            return true;
        }
        #endregion

        #region Comandos

        public ICommand ChangeImageCommand 
        {
            get
            {
                return new RelayCommand(ChangeImage);
            }
        }

        private async void ChangeImage()
        {
            await CrossMedia.Current.Initialize();

            var source = await Application.Current.MainPage.DisplayActionSheet(
                Languages.ImageSource,
                Languages.Cancel,
                null,
                Languages.FromGallery,
                Languages.NewPicture);

            if (source == Languages.Cancel)
            {
                this.file = null;
                return;
            }

            if (source == Languages.NewPicture)
            {
                this.file = await CrossMedia.Current.TakePhotoAsync(
                    new StoreCameraMediaOptions
                    {
                        Directory = "Sample",
                        Name = "test.jpg",
                        PhotoSize = PhotoSize.Small,
                    }
                );
            }
            else
            {
                this.file = await CrossMedia.Current.PickPhotoAsync();
            }

            if (this.file != null)
            {
                this.ImageSource = ImageSource.FromStream(() =>
                {
                    var stream = this.file.GetStream();
                    return stream;
                });
            }
        }

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

            if (this.Category == null)
            {
                await Application.Current.MainPage.DisplayAlert(
                    Languages.Error,
                    Languages.CategoryError,
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

            byte[] imageArray = null;
            if (this.file != null)
            {
                imageArray = FilesHelper.ReadFully(this.file.GetStream());
            }

            var producto = new Productos
            {
                NombreProducto = this.Name,
                PrecioProducto = price,
                DescripcionProducto = this.Description,
                ImageArray = imageArray,
                IdCategoria = this.Category.CategoriaId,
                UserId = MainViewModel.GetInstance().UserASP.Id,
            };

            //conectamos con el diccionario de recursos para traer la url de nuestro backend de datos
            var url = Application.Current.Resources["UrlAPI"].ToString();
            var urlPrefix = Application.Current.Resources["UrlPrefix"].ToString();
            var urlProductsController = Application.Current.Resources["UrlProductsController"].ToString();

            //hacemos nuestra conexion al backend para traer una lista de datos de la clase products que esta en el Common
            var response = await this.api.Post<Productos>(url, urlPrefix, urlProductsController, producto, Settings.TokenType, Settings.AccessToken);

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
            var productsViewModel = ProductsViewModel.GetInstance();
            productsViewModel.MyProducts.Add(newProduct);
            productsViewModel.RefreshList();

            this.isRunning = false;
            this.isEnable = true;

            await App.Navigator.PopAsync();

        }

        #endregion
    }
}
