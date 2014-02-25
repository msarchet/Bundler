namespace BundlerMiddleware
{
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.IO;
    using System.Net;
    using System.Text.RegularExpressions;
    using System.Threading.Tasks;

    using Microsoft.Owin;

	/// <summary>
	/// Used to resolve the bunlde tokens into html
	/// </summary>
    public class BundlerMiddleware : BundlerMiddlewareBase
    {    
        private static readonly Regex matcher = new Regex(@"\!\!(scripts|styles):([^\}]+?)\!\!", RegexOptions.Compiled); 
        private static readonly IDictionary<string ,string> contentCache = new ConcurrentDictionary<string, string>();

        private readonly IFileResolver fileResolver;
        private readonly IBundlerResolver bundleResolver;

        public BundlerMiddleware(OwinMiddleware next, IFileResolver fileResolver, IBundlerResolver bundleResolver, BundlerRouteTable routes) : base(next, routes)
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
        public override async Task<string> GetContent(IOwinContext context, BundlerRoute route)
        {
            if (contentCache.ContainsKey(route.Route))
            {
                return contentCache[route.Route];
            }

            return await MatchReplacer(fileResolver.GetFilePath(context, route));
        }
    }
}