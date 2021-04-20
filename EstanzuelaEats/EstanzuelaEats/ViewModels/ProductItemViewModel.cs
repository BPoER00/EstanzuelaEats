
namespace EstanzuelaEats.ViewModels
{
    using System.Linq;
    using System.Windows.Input;
    using Common.Modelos;
    using Services; 
    using GalaSoft.MvvmLight.Command;
    using Xamarin.Forms;
    using Helpers;
    using EstanzuelaEats.Views;

    public class ProductItemViewModel : Productos
    {
        #region Atributos

        private ApiService apiService;

        #endregion

        #region Constructor

        public ProductItemViewModel()
        {
            this.apiService = new ApiService();
        }

        #endregion

        #region Comandos

        public ICommand EditProductCommand
        {
            get
            {
                return new RelayCommand(EditProduct);
            }
        }

        private async void EditProduct()
        {
            MainViewModel.GetInstance().EditProduct = new EditProductViewModel(this);
            await Application.Current.MainPage.Navigation.PushAsync(new EditProductPage());
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

            if(!answer)
            {
                return;
            }

            var Connection = await this.apiService.CheckConnection();
            if (!Connection.Logrado)
            {
                await Application.Current.MainPage.DisplayAlert(Languages.Error, Connection.Mensaje, Languages.Accept);
                return;
            }

            //conectamos con el diccionario de recursos para traer la url de nuestro backend de datos
            var url = Application.Current.Resources["UrlAPI"].ToString();
            var urlPrefix = Application.Current.Resources["UrlPrefix"].ToString();
            var urlProductsController = Application.Current.Resources["UrlProductsController"].ToString();

            //hacemos nuestra conexion al backend para traer una lista de datos de la clase products que esta en el Common
            var response = await this.apiService.Delete(url, urlPrefix, urlProductsController, this.ProductoId, Settings.TokenType, Settings.AccessToken);

            if (!response.Logrado)
            {
                //si no lo hizo mostrara error en unos mensajes que se tienen en el Common y lo saca
                await Application.Current.MainPage.DisplayAlert(Languages.Error, response.Mensaje, Languages.Accept);
                return;
            }

            var productsViewModel = ProductsViewModel.GetInstance();
            var DeleteProduct = productsViewModel.MyProducts.Where(p => p.ProductoId == this.ProductoId).FirstOrDefault();

            if (DeleteProduct != null)
            {
                productsViewModel.MyProducts.Remove(DeleteProduct);

            }

            productsViewModel.RefreshList();
        }

        #endregion
    }
}
