[assembly: Xamarin.Forms.Dependency(typeof(EstanzuelaEats.Droid.Implementations.PathService))]


namespace EstanzuelaEats.Droid.Implementations
{
    using Interfaces;
    using System;
    using System.IO;
    public class PathService : IPathService
    {
        public string GetDatabasePath()
        {
            string path = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            return Path.Combine(path, "EstanzuelaDB.db3");
        }
    }
}