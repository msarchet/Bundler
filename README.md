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

    
    BundlerRoute.Routes.FromVirtualPath("/", "~/index.html"));
    

Then if you have a bundle of ~/bundles/scripts

  <html>
  
    <body>
      !!scripts:~/bundles/scripts!!
    </body>
  </html>
  
And that's it


Markdown Support
================

You can now achieve markdown conversion

    Install-Package BundlerMiddleware.Markdown
    
For now you need to create a seperate route table for using the markdown and then

    app.UseBundlerMarkdown(MarkdownRoutes);
    
If you want to use a template for your markdown you can do the following

    app.UseBundlerMarkdownWithTemplate("template.html", MarkdownRoutes);
    
Where the template has 

    !!block content!!

at the place that you want the markdown content inserted for each route

[Test Project](https://github.com/msarchet/Bundler/tree/master/BundlerTestSite)

Example Project
===============

There is now a test site in this repository that shows code for using most of the current features.



More Information
================

http://msarchet.com/using-the-asp-dot-net-bundling-pipeline-with-owin/
