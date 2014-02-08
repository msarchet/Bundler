namespace BundlerMiddleware
{
	/// <summary>
	/// Used for creating BundlerResolvers for injecting tags into the response
	/// </summary>
    public interface IBundlerResolver
    {
		/// <summary>
		/// Get the HTML to be inserted into response
		/// </summary>
		/// <param name="bundleName">The name of the bundle</param>
		/// <returns>An HTML string containing the Style Tags</returns>
        string GetStyleTags(string bundleName);

		/// <summary>
		/// Get the HTML to be inserted into response
		/// </summary>
		/// <param name="bundleName">The name of the bundle</param>
		/// <returns>An HTML string containing the Script Tags</returns>
        string GetScriptTags(string bundleName);
    }
}