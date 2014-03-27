namespace BundlerMarkdown
{
    using System.IO;
    using System;
    using System.Runtime.Remoting.Messaging;

    using BundlerMiddleware;
    using MarkdownSharp;
    using Microsoft.Owin;
    using Owin;

    public class BundlerMarkdown : BundlerMiddlewareBase
    {
        private static readonly Markdown markdown = new Markdown { EmptyElementSuffix = ">" };
        private readonly IFileResolver fileResolver;
        private readonly IBundlerResolver bundlerResolver;
        private readonly string TemplatePath;
        private readonly BundleMatcher bundleMatcher;	

        private Replacer replacer;

		/// <summary>
		/// Creates a BundlerMarkdown Middleware with a template and bundling
		/// </summary>
		/// <param name="next"></param>
		/// <param name="fileResolver"></param>
		/// <param name="bundleResolver"></param>
		/// <param name="templatePath"></param>
		/// <param name="routes"></param>
        public BundlerMarkdown(OwinMiddleware next, IFileResolver fileResolver, IBundlerResolver bundleResolver, string templatePath, BundlerRouteTable routes)
            : this(next, fileResolver, routes)
        {
            this.TemplatePath = templatePath;
            this.bundlerResolver = bundleResolver;
            this.bundleMatcher = new BundleMatcher(this.bundlerResolver);
        }

        public BundlerMarkdown(OwinMiddleware next, IFileResolver fileResolver, BundlerRouteTable routes)
            : base(next, routes)
        {
            this.fileResolver = fileResolver;
        }

        public override async System.Threading.Tasks.Task<string> GetContent(IOwinContext context, BundlerRoute route)
        {
            this.replacer = new Replacer();
            var path = this.fileResolver.GetFilePath(context, route);

            using (var stream = File.OpenText(path))
            {
                var contents = await stream.ReadToEndAsync();

                if (string.IsNullOrEmpty(this.TemplatePath))
                {
                    return markdown.Transform(contents);
                }

                this.replacer.AddMatcher(this.bundleMatcher.Matcher, this.bundleMatcher.BundleMatchReplace);
                this.replacer.AddMatcher(Matchers.ContentMatcher, match => markdown.Transform(contents));

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
    using Owin;

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