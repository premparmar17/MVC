using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(ClaimBasedAuthentication.Startup))]
namespace ClaimBasedAuthentication
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
