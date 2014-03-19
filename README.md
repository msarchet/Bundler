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
    

Then if you have a bundle of `~/bundles/scripts` or styles `~/styles/main`

  <html>
  	<head>
  		!!styles:~/styles/main`
  	</head>
    <body>
      !!scripts:~/bundles/scripts!!
    </body>
  </html>
  
And that's it!

Bundler uses `!!` as syntax to look for `scripts:<bundlename>` or `styles:<bundlename>` and replaces them with the appropriate script or link tags. Since the `BundlerMiddleware.System.Web` uses the standard `System.Web.Optimization` resolvers it will insert the proper tags based on your environments debug mode.


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

at the place that you want the markdown content inserted for each route.

An example template could look like

	<!doctype html>
		<head>
			<tile>Title</title>
			!!styles:~/styles/main!!
		</head>
		<body>
			<h1><a href="/home">Home</a></h1>
			!!block content!!
			!!scripts:~/bundles/main!!
		</body>
	</html>
	
Currently templating is only provided with the markdown package. It will be moved into core and be useable throughout any Bundler middleware.

Example Project
===============

There is now a test site in this repository that shows code for using most of the current features.

[Test Project](https://github.com/msarchet/Bundler/tree/master/BundlerTestSite)

More Information
================

http://msarchet.com/using-the-asp-dot-net-bundling-pipeline-with-owin/

Roadmap
=======

This is a rough listing of planned features:

- Fluent route declarations
- Namespaced routes
- Templates everywhere
- Support for custom titles in templates, by convention
- Supoprt for nested templates
