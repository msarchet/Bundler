Bundler
=======

OWIN Middleware for making use of bundling pipelines


Quick Start
===========

The easiest way to get started using BundlerMiddleware is to use the BundlerMiddleware.System.Web package

    Install-Package BundlerMiddleware.System.Web
    

Add the following line to your Startup class

    app.UseBundlerMiddleware()
    

Add a route pointing to the file you want to inject into to your global.asax

    
    BundlerRoute.Routes.Add(BundlerRoute.Routes.BundlerRouteFromVirtualPath("/", "~/index.html"));
    

Then if you have a bundle of ~/bundles/scripts

  <html>
  
    <body>
      !!scripts:~/bundles/scripts!!
    </body>
  </html>
  
And that's it
