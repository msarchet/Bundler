namespace BundlerMiddleware
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.IO;
    using System.Net;
    using System.Runtime.Remoting.Messaging;
    using System.Text.RegularExpressions;
    using System.Threading.Tasks;
    using System.Web.Optimization;
    using System.Web.Routing;

    using Microsoft.Owin;

    public class Example
    {
        public void doSoemthing()
        {
            BundlerMiddleware.GetFilePathResolver = (context, route) => "/";
        }
    }

    public class BundlerMiddleware : OwinMiddleware
    {    
        private static readonly Regex matcher = new Regex(@"\!\!(scripts|styles):([^\}]+)\!\!", RegexOptions.Compiled); 
        private static readonly IDictionary<string ,string> contentCache = new ConcurrentDictionary<string, string>();

        public static Func<IOwinContext, BundlerRoute, string> GetFilePathResolver =
            (context, route) => GetFilePath(context, route);

        public BundlerMiddleware(OwinMiddleware next) : base(next)
        {
        }

        private async static Task<string> MatchReplacer(string path)
        {
            using (var stream = File.OpenText(path))
            {
                var file = await stream.ReadToEndAsync();
                return matcher.Replace(file, MatchReplace);
            }
        }

        private static string MatchReplace(Match match)
        {
            return (match.Groups[1].Value == "scripts"
                         ? Scripts.Render(match.Groups[2].Value)
                         : Styles.Render(match.Groups[2].Value)).ToString();
        }

        private async static Task<string> GetContent(IOwinContext context, BundlerRoute route)
        {
            if (contentCache.ContainsKey(route.Route))
            {
                return contentCache[route.Route];
            }

            return await MatchReplacer(GetFilePathResolver(context, route));
        }

        private static string GetFilePath(IOwinContext context, BundlerRoute route)
        {
            var baseContext = context.Environment["System.Web.HttpContextBase"] as System.Web.HttpContextBase;

            if (baseContext == null)
            {
                throw new Exception("Unable to resolve file path becuase you're not on IIS");
            }
            
            return baseContext.Server.MapPath(route.FilePath);
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