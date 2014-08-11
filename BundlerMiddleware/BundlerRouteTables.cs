using System.Collections.Generic;

namespace BundlerMiddleware
{
    using System.Collections.Concurrent;
    using System.Linq;
    using System.Text.RegularExpressions;

    public class BundlerRoutes
    {
        public static BundlerRouteTable Routes = new BundlerRouteTable();
    }

    public class BundlerRouteTable : ICollection<IBundlerRoute>
    {
        public string RoutePrefix { get; set; }

        private readonly List<IBundlerRoute> Routes = new List<IBundlerRoute>();


        public bool Exists(string route)
        {
            return Routes.Any(r => r.Matches(route));
        }

        public IBundlerRoute Get(string route)
        {
            return Routes.First(r => r.Matches(route));
        }

        public void Add(IBundlerRoute item)
        {
            Routes.Add(item);
        }

        public void Clear()
        {
            Routes.Clear();
        }

        public bool Contains(IBundlerRoute item)
        {
            throw new System.NotImplementedException();
        }

        public void CopyTo(IBundlerRoute[] array, int arrayIndex)
        {
            Routes.CopyTo(array, arrayIndex);
        }

        public bool Remove(IBundlerRoute item)
        {
            throw new System.NotImplementedException();
        }

        public int Count
        {
            get { return Routes.Count; }
        }

        public bool IsReadOnly
        {
            get
            {
                return false;
            }
        }

        public bool Remove(BundlerRoute item)
        {
            return Routes.Remove(item);
        }

        public IEnumerator<IBundlerRoute> GetEnumerator()
        {
            return Routes.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return Routes.GetEnumerator();
        }
    }

    public interface IBundlerRoute
    {
        bool Matches(string url);

        string FilePath { get; set; }
    }

    public class BundlerRoute : IBundlerRoute
    {
        /// <summary>
        /// Route to be resolved
        /// </summary>
        public string Route { get; set; }

        /// <summary>
        /// File path for the bundler to inject
        /// </summary>
        public string FilePath { get; set; }

        public BundlerRoute(string route, string filePath)
        {
            this.Route = route;
            this.FilePath = filePath;
        }

        public bool Matches(string url)
        {
            return url == this.Route;
        }
    }

    public class BundlerRegexRoute : IBundlerRoute
    {
        public Regex Route { get; set; }

        public string FilePath { get; set; }

        public BundlerRegexRoute(Regex route, string filePath)
        {
            this.Route = route;
            this.FilePath = filePath;
        }

        public bool Matches(string url)
        {
            return this.Route.Match(url).Success;
        }
    }
}
