using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(blogen.Startup))]
namespace blogen
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
