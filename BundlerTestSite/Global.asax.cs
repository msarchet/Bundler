using BundlerMiddleware;
using System;
using System.Web.Optimization;

namespace BundlerTestSite
{
    public class Global : System.Web.HttpApplication
    {
        protected void Application_Start(object sender, EventArgs e)
        {
            BundleTable.Bundles.Add(new ScriptBundle("~/bundles/script").IncludeDirectory("~/Scripts", "*.js", true));
			BundleTable.Bundles.Add(new StyleBundle("~/bundles/style").IncludeDirectory("~/Styles", "*.css", true));
            BundlerRoutes.Routes.FromVirtualPath("/test", "~/Normal/test.html");
            Startup.MarkdownRoutes.FromVirtualPath("/markdown", "~/Markdown/markdown.md");
            Startup.MarkdownRoutesWithTemplate.FromVirtualPath("/template", "~/Markdown/markdown.md");
        }
    }
}