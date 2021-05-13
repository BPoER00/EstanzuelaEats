
namespace EstanzuelaEats.ViewModels
{
    using GalaSoft.MvvmLight.Command;
    using System;
    using System.Windows.Input;
    using Xamarin.Forms;

    public class SetupViewModel
    {
        #region Propiedades

        public string Foto { get; set; }
        public string Titulo { get; set; }
        public string PageName { get; set; }

        #endregion

        #region Comandos

        public ICommand GotoCommand
        {
            get
            {
                return new RelayCommand(Goto);
            }

        }

        private void Goto()
        {
            if (this.PageName == "SetupRepository")
            {
                Device.OpenUri(new Uri("https://github.com/BPoER00/EstanzuelaEats"));
            }
            else if(this.PageName == "SetupHttp")
            {
                Device.OpenUri(new Uri("https://estanzuelaeatsbackend2021.azurewebsites.net/"));
            }
        }

        #endregion
    }
}
