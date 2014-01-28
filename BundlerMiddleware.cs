using System.Linq;
using BundlerMiddleware;

namespace mist.Middleware
{
    using System.IO;
    using System.Net;
    using System.Text.RegularExpressions;
    using System.Threading.Tasks;
    using System.Web.Optimization;

    using Microsoft.Owin;

    public class BundlerMiddleware : OwinMiddleware
    {    
        public BundlerMiddleware(OwinMiddleware next) : base(next)
        {
        }

        private async static Task<string> MatchReplacer(string path)
        {
            using (var stream = File.OpenText(path))
            {
                var file = await stream.ReadToEndAsync();
                var matcher = new Regex(@"\!\!(scripts|styles):([^\}]+)\!\!", RegexOptions.Compiled);
                return matcher.Replace(file, MatchReplace);
            }
        }

        private static string MatchReplace(Match match)
        {
            return (match.Groups[1].Value == "scripts"
                         ? Scripts.Render(match.Groups[2].Value)
                         : Styles.Render(match.Groups[2].Value)).ToString();
        }

        public override async Task Invoke(IOwinContext context)
        {
            var route = BundlerRoutes.Routes.FirstOrDefault(r => r.Route.Equals(context.Request.Path.ToString()));
            if (route != null)
            {
                context.Response.StatusCode = (int)HttpStatusCode.OK;
                context.Response.ContentType = "text/html";
                var baseContext = context.Environment["System.Web.HttpContextBase"] as System.Web.HttpContextBase;
                if (baseContext != null)
                {
                    var fullPath = baseContext.Server.MapPath(route.FilePath);
                    await context.Response.WriteAsync(await MatchReplacer(fullPath));
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