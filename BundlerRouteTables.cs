using System.Collections.Generic;
using System.Web.Management;
using System.Web.Routing;

namespace BundlerMiddleware
{
    public class BundlerRoutes
    {
        public static BundlerRouteTable Routes = new BundlerRouteTable();
    }

    public class BundlerRouteTable : ICollection<BundlerRoute>
    {
        private ICollection<BundlerRoute> Routes = new List<BundlerRoute>();

        public void Add(BundlerRoute item)
        {
            Routes.Add(item);
        }

        public void Clear()
        {
            Routes.Clear();
        }

        public bool Contains(BundlerRoute item)
        {
            return Routes.Contains(item);
        }

        public void CopyTo(BundlerRoute[] array, int arrayIndex)
        {
            Routes.CopyTo(array, arrayIndex);
        }

        public int Count
        {
            get { return Routes.Count; }
        }

        public bool IsReadOnly
        {
            get { return Routes.IsReadOnly;  }
        }

        public bool Remove(BundlerRoute item)
        {
            return Routes.Remove(item);
        }

        public IEnumerator<BundlerRoute> GetEnumerator()
        {
            return Routes.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return Routes.GetEnumerator();
        }
    }

    public class BundlerRoute
    {
        public string Route { get; set; }
        public string FilePath { get; set; }

        public BundlerRoute(string route, string filePath)
        {
            this.Route = route;
            this.FilePath = filePath;
        }

        public static BundlerRoute BundlerRouteFromVirutalPath(string route, string virutalPath)
        {
            return new BundlerRoute(route, System.Web.VirtualPathUtility.ToAbsolute(virutalPath));
        }
    }
}
