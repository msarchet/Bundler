namespace BundlerMiddleware
{
    using Microsoft.Owin;

    /// <summary>
    /// Used for resolving file paths to full paths
    /// </summary>
    public interface IFileResolver
    {
        /// <summary>
        /// Gets the full file path for the route
        /// </summary>
        /// <param name="context">Request Context</param>
        /// <param name="route">Route to resolve the path for</param>
        /// <returns>The full file path</returns>
        string GetFilePath(IOwinContext context, IBundlerRoute route);
    }
}
