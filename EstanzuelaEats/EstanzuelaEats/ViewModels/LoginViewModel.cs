using EstanzuelaEats.Helpers;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace EstanzuelaEats.ViewModels
{
    public class LoginViewModel : BaseViewModel
    {
        #region Atributos

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



        }

        #endregion
    }
}
