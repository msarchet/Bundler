namespace BundlerMiddleware
{
    public interface IBundlerResolver
    {
        string GetStyleTags(string bundleName);
        string GetScriptTags(string bundleName);
    }
}