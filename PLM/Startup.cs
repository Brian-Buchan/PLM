using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(PLM.Startup))]
namespace PLM
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
