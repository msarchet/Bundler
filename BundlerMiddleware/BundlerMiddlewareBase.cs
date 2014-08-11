using Microsoft.Owin;
using System.Net;
using System.Threading.Tasks;
namespace BundlerMiddleware
{
    public abstract class BundlerMiddlewareBase : OwinMiddleware
    {
        private readonly BundlerRouteTable routes;
        public BundlerMiddlewareBase(OwinMiddleware next, BundlerRouteTable routes) : base(next) 
        {
            this.routes = routes; 
        }
        public abstract Task<string> GetContent(IOwinContext context, IBundlerRoute route);
    
        public override async Task Invoke(IOwinContext context)
        {
            var path = context.Request.Path.ToString();

            if (routes.Exists(path))
            {
                var route = routes.Get(path);

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
