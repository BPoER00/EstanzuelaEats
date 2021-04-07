using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(EstanzuelaEats.Api.Startup))]

namespace EstanzuelaEats.Api
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
