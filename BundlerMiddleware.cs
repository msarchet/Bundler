namespace BundlerMiddleware
{
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.IO;
    using System.Net;
    using System.Text.RegularExpressions;
    using System.Threading.Tasks;

    using Microsoft.Owin;

    public class BundlerMiddleware : OwinMiddleware
    {    
        private static readonly Regex matcher = new Regex(@"\!\!(scripts|styles):([^\}]+)\!\!", RegexOptions.Compiled); 
        private static readonly IDictionary<string ,string> contentCache = new ConcurrentDictionary<string, string>();

        private readonly IFileResolver fileResolver;
        private readonly IBundleResolver bundleResolver;

        public BundlerMiddleware(OwinMiddleware next) : base(next)
        {
           this.fileResolver = new DefaultFileResolver();
           this.bundleResolver = new DefaultBundleResolver();
        }

        public BundlerMiddleware(OwinMiddleware next, IFileResolver fileResolver, IBundleResolver bundleResolver) : base(next)
        {
            this.fileResolver = fileResolver;
            this.bundleResolver = bundleResolver;
        }

        private async Task<string> MatchReplacer(string path)
        {
            using (var stream = File.OpenText(path))
            {
                var file = await stream.ReadToEndAsync();
                return matcher.Replace(file, MatchReplace);
            }
        }

        private string MatchReplace(Match match)
        {
            return match.Groups[1].Value == "scripts"
                         ? bundleResolver.GetScriptTags(match.Groups[2].Value)
                         : bundleResolver.GetStyleTags(match.Groups[2].Value);
        }

        private async Task<string> GetContent(IOwinContext context, BundlerRoute route)
        {
            if (contentCache.ContainsKey(route.Route))
            {
                return contentCache[route.Route];
            }

            return await MatchReplacer(fileResolver.GetFilePath(context, route));
        }


        public override async Task Invoke(IOwinContext context)
        {
            var path = context.Request.Path.ToString(); // / /home

            if (BundlerRoutes.Routes.Exists(path))
            {
                var route = BundlerRoutes.Routes.Get(path);

                var content = await GetContent(context, route);
                context.Response.StatusCode = (int)HttpStatusCode.OK;
                context.Response.ContentType = "text/html";
                await context.Response.WriteAsync(content);
            }
            else
            {
                await Next.Invoke(context);
            }
        }
    }
}