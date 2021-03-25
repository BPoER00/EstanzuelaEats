

namespace EstanzuelaEats.Interfaces
{

    using System.Globalization;

    interface ILocalize
    {
        CultureInfo GetCurrentCultureInfo();
        void SetLocale(CultureInfo ci);
    }
}
