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
        private readonly BundleMatcher bundleMatcher;
        private readonly IFileResolver fileResolver;
        private readonly Replacer replacer = new Replacer();

        public BundlerMiddleware(OwinMiddleware next, IFileResolver fileResolver, IBundlerResolver bundleResolver, BundlerRouteTable routes) : base(next, routes)
        {
            this.fileResolver = fileResolver;
			this.bundleMatcher = new BundleMatcher(bundleResolver);
            this.replacer.AddMatcher(bundleMatcher.Matcher, bundleMatcher.BundleMatchReplace);
        }

        public override async Task<string> GetContent(IOwinContext context, BundlerRoute route)
        {
            return await this.replacer.MatchReplacer(this.fileResolver.GetFilePath(context, route));
        }
    }
}