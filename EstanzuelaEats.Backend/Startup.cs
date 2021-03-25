using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(EstanzuelaEats.Backend.Startup))]
namespace EstanzuelaEats.Backend
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
