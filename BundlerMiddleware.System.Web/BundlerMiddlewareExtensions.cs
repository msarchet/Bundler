namespace BundlerMiddleware
{
	using Owin;

    public static class BundlerMiddlewareIISExtensions
    {
		/// <summary>
		/// Includes the Bundler Middleware that runs using System.Web
		/// </summary>
		/// <param name="app"></param>
		/// <returns></returns>
        public static IAppBuilder UseBundlerMiddlewareForIIS(this IAppBuilder app)
        {
            return app.Use(typeof(BundlerMiddleware), new DefaultFileResolver(), new DefaultBundleResolver());
        }
    }
}
