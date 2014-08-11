namespace BundlerMiddleware
{
    using System.Web.Optimization;

    /// <summary>
    /// Standard Bundle Resolver
    /// </summary>
    public class DefaultBundleResolver : IBundlerResolver
    {

        public string GetStyleTags(string bundleName)
        {
            return Styles.Render(bundleName).ToString();
        }

        public string GetScriptTags(string bundleName)
        {
            return Scripts.Render(bundleName).ToString();
        }
    }
}