namespace BundlerMiddleware
{
    using System.Web.Optimization;

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