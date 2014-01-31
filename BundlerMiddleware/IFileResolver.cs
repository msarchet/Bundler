namespace BundlerMiddleware
{
    using Microsoft.Owin;

    public interface IFileResolver
    {
        string GetFilePath(IOwinContext context, BundlerRoute route);
    }
}