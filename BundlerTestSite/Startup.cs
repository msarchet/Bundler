using System;
using System.Threading.Tasks;
using Microsoft.Owin;
using Owin;
using BundlerMiddleware;

[assembly: OwinStartup(typeof(BundlerTestSite.Startup))]

namespace BundlerTestSite
{
    public class Startup
    {
        public static BundlerRouteTable MarkdownRoutes = new BundlerRouteTable();
        public void Configuration(IAppBuilder app)
        {
			app.UseBundlerMiddlewareForIIS();
			app.UseBundlerMarkdown(MarkdownRoutes);
        }
    }
}
