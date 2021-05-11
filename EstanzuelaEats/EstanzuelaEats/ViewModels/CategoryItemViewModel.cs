using System;
using System.Collections.Generic;
using System.Text;

namespace EstanzuelaEats.ViewModels
{
    using System.Windows.Input;
    using Common.Modelos;
    using GalaSoft.MvvmLight.Command;
    using Views;

    public class CategoryItemViewModel : Category
    {
        #region Commands
        public ICommand GotoCategoryCommand
        {
            get
            {
                return new RelayCommand(GotoCategory);
            }
        }

        private async void GotoCategory()
        {
            MainViewModel.GetInstance().Productos = new ProductsViewModel(this);
            await App.Navigator.PushAsync(new ProductsPage());
        }
        #endregion
    }
}
