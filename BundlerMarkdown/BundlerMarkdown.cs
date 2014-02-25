using BundlerMiddleware;
using MarkdownSharp;
using Microsoft.Owin;
using Owin;
using System.IO;

namespace BundlerMarkdown
{
    public class BundlerMarkdown : BundlerMiddlewareBase
    {
        private static readonly Markdown markdown = new Markdown { EmptyElementSuffix = ">" };
        private readonly IFileResolver fileResolver;

        public BundlerMarkdown(OwinMiddleware next, IFileResolver fileResolver, BundlerRouteTable routes)
            : base(next, routes)
        {
            this.fileResolver = fileResolver;
        }


        public async override System.Threading.Tasks.Task<string> GetContent(Microsoft.Owin.IOwinContext context, BundlerRoute route)
        {
            var path = this.fileResolver.GetFilePath(context, route);

            using (var stream = File.OpenText(path))
            {
                var contents = await stream.ReadToEndAsync();
                return markdown.Transform(contents);
            }
        }
    }
}

namespace BundlerMiddleware
{
    public static class BundlerMarkdownExtensions
    {
        public static void UseBundlerMarkdown(this IAppBuilder app, BundlerRouteTable routes)
        {
            app.Use(typeof(BundlerMarkdown.BundlerMarkdown), new DefaultFileResolver(), routes);
        }
    }
}
