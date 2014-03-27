namespace BundlerMarkdown
{
    using System.IO;
    using System;
    using System.Runtime.Remoting.Messaging;

    using BundlerMiddleware;
    using Microsoft.Owin;
    using MarkdownSharp;
    using Owin;

    public class BundlerMarkdown : BundlerMiddlewareBase
    {
        private static readonly Markdown markdown = new Markdown { EmptyElementSuffix = ">" };

        private readonly IFileResolver fileResolver;

        private readonly IBundlerResolver bundlerResolver;
        private Replacer replacer;
        private readonly string TemplatePath;

        public BundlerMarkdown(OwinMiddleware next, IFileResolver fileResolver, IBundlerResolver bundleResolver, string templatePath, BundlerRouteTable routes)
            : this(next, fileResolver, routes)
        {
            this.TemplatePath = templatePath;
            this.bundlerResolver = bundleResolver;
        }

        public BundlerMarkdown(OwinMiddleware next, IFileResolver fileResolver, BundlerRouteTable routes)
            : base(next, routes)
        {
            this.fileResolver = fileResolver;
        }

        public override async System.Threading.Tasks.Task<string> GetContent(IOwinContext context, BundlerRoute route)
        {
            this.replacer = new Replacer(this.bundlerResolver);
            var path = this.fileResolver.GetFilePath(context, route);

            using (var stream = File.OpenText(path))
            {
                var contents = await stream.ReadToEndAsync();

                if (string.IsNullOrEmpty(this.TemplatePath))
                {
                    return markdown.Transform(contents);
                }

                this.replacer.AddMatcher(Replacer.ContentMatcher, match => markdown.Transform(contents));
                return
                    await
                    this.replacer.MatchReplacer(
                        fileResolver.GetFilePath(context, new BundlerRoute("template", this.TemplatePath)));
            }
        }
    }
}

namespace BundlerMiddleware
{
    using System;

    public static class BundlerMarkdownExtensions
    {
        public static void UseBundlerMarkdown(this IAppBuilder app, BundlerRouteTable routes)
        {
            app.Use(typeof(BundlerMarkdown.BundlerMarkdown), new DefaultFileResolver(), routes);
        }

        public static void UseBundlerMarkdownWithTemplate(
            this IAppBuilder app,
            string templatePath,
            BundlerRouteTable routes)
        {
            app.Use(
                typeof(BundlerMarkdown.BundlerMarkdown),
                new DefaultFileResolver(),
                new DefaultBundleResolver(),
                templatePath,
                routes);
        }
    }


}
