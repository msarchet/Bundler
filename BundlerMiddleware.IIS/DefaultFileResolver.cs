namespace BundlerMiddleware
{
    using System;

    using Microsoft.Owin;

    public class DefaultFileResolver : IFileResolver
    {
        public string GetFilePath(IOwinContext context, BundlerRoute route)
        {
            var baseContext = context.Environment["System.Web.HttpContextBase"] as System.Web.HttpContextBase;

            if (baseContext == null)
            {
                throw new Exception("Unable to resolve file path becuase you're not on IIS");
            }
            
            return baseContext.Server.MapPath(route.FilePath);
        }
    }
}