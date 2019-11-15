using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(zubwebback.Startup))]

namespace zubwebback
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureMobileApp(app);
        }
    }
}