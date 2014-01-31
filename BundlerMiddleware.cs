
namespace BundlerMiddleware
{
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.IO;
    using System.Net;
    using System.Text.RegularExpressions;
    using System.Threading.Tasks;
    using System.Web.Optimization;

    using Microsoft.Owin;

    public class BundlerMiddleware : OwinMiddleware
    {    
        private static readonly Regex matcher = new Regex(@"\!\!(scripts|styles):([^\}]+)\!\!", RegexOptions.Compiled); 
        private static readonly IDictionary<string ,string> contentCache = new ConcurrentDictionary<string, string>();
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

            var baseContext = context.Environment["System.Web.HttpContextBase"] as System.Web.HttpContextBase;
            if (baseContext != null)
            {
                var fullPath = baseContext.Server.MapPath(route.FilePath);
                return await MatchReplacer(fullPath);
            }

            return null;
        }

        public override async Task Invoke(IOwinContext context)
        {
            var path = context.Request.Path.ToString();
            if (BundlerRoutes.Routes.Exists(path))
            {
                var route = BundlerRoutes.Routes.Get(path);
                context.Response.StatusCode = (int)HttpStatusCode.OK;
                context.Response.ContentType = "text/html";
                var content = await GetContent(context, route);

                if (content != null)
                {
                    await context.Response.WriteAsync(content);
                }  
                else
                {
                    await Next.Invoke(context);
                }
            }
            else
            {
                await Next.Invoke(context);
            }
        }
    }
}