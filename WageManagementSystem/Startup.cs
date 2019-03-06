using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(WageManagementSystem.Startup))]
namespace WageManagementSystem
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
