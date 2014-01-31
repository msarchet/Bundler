namespace BundlerMiddleware
{
    public interface IBundleResolver
    {
        string GetStyleTags(string bundleName);
        string GetScriptTags(string bundleName);
    }
}