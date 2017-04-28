using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Vcyber.BLMS.ManageWeb.Startup))]
namespace Vcyber.BLMS.ManageWeb
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
