
namespace EstanzuelaEats.ViewModels
{
    using System.Windows.Input;
    using Helpers;
    using Services;
    using GalaSoft.MvvmLight.Command;
    using Xamarin.Forms;
    using EstanzuelaEats.Views;

    public class LoginViewModel : BaseViewModel
    {
        #region Atributos

        private ApiService apiService;
        private bool isRunning;
        private bool isEnable;

        #endregion

        #region Propiedades

        public string Email { get; set; }
        public string Password { get; set; }
        public bool IsRemembered { get; set; }
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

        #endregion

        #region Constructores

        public LoginViewModel()
        {
            this.IsEnable = true;
            this.IsRemembered = true;
            this.apiService = new ApiService();
        }

        #endregion

        #region Comandos

        public ICommand LoginCommand 
        {
            get
            {
                return new RelayCommand(Login);
            }
        }

        private async void Login()
        {
            if (string.IsNullOrEmpty(this.Email))
            {
                await Application.Current.MainPage.DisplayAlert
                    (
                    Languages.Error,
                    Languages.EmailValidation,
                    Languages.Accept
                    );

                return;
            }

            if (string.IsNullOrEmpty(this.Password))
            {
                await Application.Current.MainPage.DisplayAlert
                    (
                    Languages.Error,
                    Languages.PasswordValidation,
                    Languages.Accept
                    );

                return;
            }

            //activamos el refresh
            this.IsRunning = true;
            this.isEnable = false;

            //comprobamos si el usuario tiene conexion a internet
            var Connection = await this.apiService.CheckConnection();
            if (!Connection.Logrado)
            {
                this.IsRunning = false;
                this.IsEnable = true;
                await Application.Current.MainPage.DisplayAlert(Languages.Error, Connection.Mensaje, Languages.Accept);
                return;
            }

            //conectamos con el diccionario de recursos para traer la url de nuestro backend de datos
            var url = Application.Current.Resources["UrlAPI"].ToString();
            var token = await this.apiService.GetToken(url, this.Email, this.Password);

            if (token == null || string.IsNullOrEmpty(token.AccessToken))
            {
                await Application.Current.MainPage.DisplayAlert
                    (Languages.Error,
                    Languages.SomethingWrong,
                    Languages.Accept
                    );
                return;
            }

            Settings.TokenType = token.TokenType;
            Settings.AccessToken = token.AccessToken;
            Settings.IsRemembered = this.IsRemembered;


            this.IsRunning = false;
            this.IsEnable = true;

            MainViewModel.GetInstance().Productos = new ProductsViewModel();
            Application.Current.MainPage = new ProductsPage();
        }

        #endregion
    }
}
