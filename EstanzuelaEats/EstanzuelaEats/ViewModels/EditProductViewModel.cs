
namespace EstanzuelaEats.ViewModels
{
    using Common.Modelos;
    using EstanzuelaEats.Helpers;
    using EstanzuelaEats.Services;
    using GalaSoft.MvvmLight.Command;
    using Plugin.Media;
    using Plugin.Media.Abstractions;
    using System.Linq;
    using System.Windows.Input;
    using Xamarin.Forms;

    public class EditProductViewModel : BaseViewModel
    {
        #region Atributos

        private Productos product;
        private MediaFile file;
        private ApiService api;
        private bool isRunning;
        private bool isEnable;
        private ImageSource imageSource;

        #endregion

        #region Propiedades

        public Productos Product
        {
                get { return this.product; }
                set { this.SetValue(ref this.product, value); }   
        }

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

        public EditProductViewModel(Productos product)
        {
            this.product = product;
            this.api = new ApiService();
            this.IsEnable = true;
            this.ImageSource = product.ImageFullPath;
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

        public ICommand SaveCommand
        {
            get
            {
                return new RelayCommand(Save);
            }
        }

        private async void Save()
        {
            if (string.IsNullOrEmpty(this.Product.NombreProducto))
            {
                await Application.Current.MainPage.DisplayAlert(
                    Languages.Error,
                    Languages.NameError,
                    Languages.Accept);
                return;
            }

            if (this.Product.PrecioProducto < 0)
            {
                await Application.Current.MainPage.DisplayAlert(
                    Languages.Error,
                    Languages.PriceError,
                    Languages.Accept);
                return;
            }

            if (string.IsNullOrEmpty(this.Product.DescripcionProducto))
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

            byte[] imageArray = null;
            if (this.file != null)
            {
                imageArray = FilesHelper.ReadFully(this.file.GetStream());
                this.Product.ImageArray = imageArray;
            }

            //conectamos con el diccionario de recursos para traer la url de nuestro backend de datos
            var url = Application.Current.Resources["UrlAPI"].ToString();
            var urlPrefix = Application.Current.Resources["UrlPrefix"].ToString();
            var urlProductsController = Application.Current.Resources["UrlProductsController"].ToString();

            //hacemos nuestra conexion al backend para traer una lista de datos de la clase products que esta en el Common
            var response = await this.api.Put<Productos>(url, urlPrefix, urlProductsController, this.Product, this.Product.ProductoId, Settings.TokenType, Settings.AccessToken);

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

            var oldProduct = productsViewModel.MyProducts.Where(p => p.ProductoId == this.Product.ProductoId).FirstOrDefault();
            if(oldProduct != null)
            {
                productsViewModel.MyProducts.Remove(oldProduct);
            }
            
            productsViewModel.MyProducts.Add(newProduct);
            productsViewModel.RefreshList();

            this.isRunning = false;
            this.isEnable = true;

            await Application.Current.MainPage.Navigation.PopAsync();

        }

        public ICommand DeleteProductCommand
        {
            get
            {
                return new RelayCommand(DeleteProduct);
            }
        }

        private async void DeleteProduct()
        {
            var answer = await Application.Current.MainPage.DisplayAlert(
                Languages.Confirm,
                Languages.DeleteConfirmation,
                Languages.Yes,
                Languages.No
                );

            if (!answer)
            {
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

            //conectamos con el diccionario de recursos para traer la url de nuestro backend de datos
            var url = Application.Current.Resources["UrlAPI"].ToString();
            var urlPrefix = Application.Current.Resources["UrlPrefix"].ToString();
            var urlProductsController = Application.Current.Resources["UrlProductsController"].ToString();

            //hacemos nuestra conexion al backend para traer una lista de datos de la clase products que esta en el Common
            var response = await this.api.Delete(url, urlPrefix, urlProductsController, this.Product.ProductoId, Settings.TokenType, Settings.AccessToken);

            if (!response.Logrado)
            {
                //si no lo hizo mostrara error en unos mensajes que se tienen en el Common y lo saca
                this.IsRunning = false;
                this.IsEnable = true;
                await Application.Current.MainPage.DisplayAlert(Languages.Error, response.Mensaje, Languages.Accept);
                return;
            }

            var productsViewModel = ProductsViewModel.GetInstance();
            var DeleteProduct = productsViewModel.MyProducts.Where(p => p.ProductoId == this.Product.ProductoId).FirstOrDefault();

            if (DeleteProduct != null)
            {
                productsViewModel.MyProducts.Remove(DeleteProduct);

            }

            productsViewModel.RefreshList();

            this.IsRunning = false;
            this.IsEnable = true;

            await Application.Current.MainPage.Navigation.PopAsync();
        }

        #endregion
    }
}
