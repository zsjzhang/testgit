using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Vcyber.BLMS.FrontWeb.Startup))]
namespace Vcyber.BLMS.FrontWeb
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
