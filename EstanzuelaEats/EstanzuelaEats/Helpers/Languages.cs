
namespace EstanzuelaEats.Helpers
{
    using Interfaces;
    using Resources;
    using Xamarin.Forms;

    public static class Languages
    {
        static Languages()
        {
            var ci = DependencyService.Get<ILocalize>().GetCurrentCultureInfo();
            Resource.Culture = ci;
            DependencyService.Get<ILocalize>().SetLocale(ci);
        }

        public static string Accept
        {
            get { return Resource.Accept; }
        }

        public static string Error
        {
            get { return Resource.Error; }
        }

        public static string NoInternet
        {
            get { return Resource.NoInternet; }
        }

        public static string Products
        {
            get { return Resource.Products; }
        }

        public static string TurnOnInternet
        {
            get { return Resource.TurnOnInternet; }
        }

        public static string IdProduct
        {
            get { return Resource.IdProduct; }
        }

        public static string Image
        {
            get { return Resource.Image; }
        }

        public static string IsAvailable
        {
            get { return Resource.IsAvailable; }
        }

        public static string Descripcion
        {
            get { return Resource.Descripcion; }
        }

        public static string Price
        {
            get { return Resource.Price; }
        }

        public static string PublishOn
        {
            get { return Resource.PublishOn; }
        }
    }
}
