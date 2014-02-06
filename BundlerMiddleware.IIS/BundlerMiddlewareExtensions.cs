using Owin;
namespace BundlerMiddleware
{
    public static class BundlerMiddlewareIISExtensions
    {
        public static IAppBuilder UseBundlerMiddlewareForIIS(this IAppBuilder app)
        {
            return app.Use(typeof(BundlerMiddleware), new DefaultFileResolver(), new DefaultBundleResolver());
        }
    }
}
