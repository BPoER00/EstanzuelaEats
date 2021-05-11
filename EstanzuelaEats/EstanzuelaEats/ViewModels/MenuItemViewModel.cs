
namespace EstanzuelaEats.ViewModels
{
    using EstanzuelaEats.Helpers;
    using EstanzuelaEats.Views;
    using GalaSoft.MvvmLight.Command;
    using System;
    using System.Windows.Input;
    using Xamarin.Forms;

    public class MenuItemViewModel
    {
        #region Propiedades

        public string Icon { get; set; }

        public string Title { get; set; }

        public string PageName { get; set; }

        #endregion



        #region Comandos

        public ICommand GotoCommand 
        { get 
            {
                return new RelayCommand(Goto);
            }
                
        }

        private async void Goto()
        {
            if (this.PageName == "LoginPage")
            {
                Settings.AccessToken = string.Empty;
                Settings.TokenType = string.Empty;
                Settings.IsRemembered = false;

                MainViewModel.GetInstance().Login = new LoginViewModel();
                Application.Current.MainPage = new NavigationPage(new LoginPage());
            }
            else if (this.PageName == "AboutPage")
            {
                App.Master.IsPresented = false;
                await App.Navigator.PushAsync(new MapPage());
            }
        }

        #endregion
    }
}
