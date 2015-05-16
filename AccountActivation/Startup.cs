using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(AccountActivation.Startup))]
namespace AccountActivation
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
