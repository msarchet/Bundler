﻿using System;
using System.Threading.Tasks;
using Microsoft.Owin;
using Owin;
using BundlerMiddleware;

[assembly: OwinStartup(typeof(BundlerTestSite.Startup))]

namespace BundlerTestSite
{
    using BundlerMiddleware;

    public class Startup
    {
        public static BundlerRouteTable MarkdownRoutes = new BundlerRouteTable();
        public static BundlerRouteTable MarkdownRoutesWithTemplate = new BundlerRouteTable();
        public void Configuration(IAppBuilder app)
        {
            app.UseBundlerMiddlewareForIIS();
            app.UseBundlerMarkdown(MarkdownRoutes);
            app.UseBundlerMarkdownWithTemplate("~/markdown/markdowntemplate.html", MarkdownRoutesWithTemplate);
        }
    }
}
