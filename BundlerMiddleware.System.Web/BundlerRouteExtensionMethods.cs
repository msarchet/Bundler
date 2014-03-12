namespace BundlerMiddleware
{
    public static class BundlerRouteExtensionMethods
    {
		/// <summary>
		/// Helper method for resolving Virtual File paths
		/// </summary>
		/// <param name="table">The route table that the route will be added to</param>
		/// <param name="route">The route to resolve on</param>
		/// <param name="virutalPath">The location of the file to resolve</param>
		/// <returns>A <c href="BundlerRoute">BundlerRoute</c></returns>
        public static void FromVirtualPath(this BundlerRouteTable table, string route, string virutalPath)
        {
            table.Add(new BundlerRoute(route, System.Web.VirtualPathUtility.ToAbsolute(virutalPath)));
        }
    }
}
