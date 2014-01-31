using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BundlerMiddleware
{
    public static class BundlerRouteExtensionMethods
    {
        public static BundlerRoute BundlerRouteFromVirtualPath(this BundlerRouteTable table, string route, string virutalPath)
        {
            return new BundlerRoute(route, System.Web.VirtualPathUtility.ToAbsolute(virutalPath));
        }
    }
}
