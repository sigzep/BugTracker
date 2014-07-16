using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Cars3.Startup))]
namespace Cars3
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
