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
        private static readonly IDictionary<string ,string> contentCache = new ConcurrentDictionary<string, string>();

	    private readonly Replacer replacer;
        
        private readonly IFileResolver fileResolver;
	    private readonly IBundlerResolver bundleResolver;

        public BundlerMiddleware(OwinMiddleware next, IFileResolver fileResolver, IBundlerResolver bundleResolver, BundlerRouteTable routes) : base(next, routes)
        {
            this.fileResolver = fileResolver;
            this.replacer = new Replacer(bundleResolver);
        }

        public override async Task<string> GetContent(IOwinContext context, BundlerRoute route)
        {

            return await this.replacer.MatchReplacer(this.fileResolver.GetFilePath(context, route));

        }
    }
}