using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Vcyber.BLMS.MobileWeb.Startup))]
namespace Vcyber.BLMS.MobileWeb
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
