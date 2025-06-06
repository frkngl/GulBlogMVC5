﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace GülBlogMVC5
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Home",
                url: "",
                defaults: new { controller = "Home", action = "Index" }
            );

            routes.MapRoute(
                name: "Blogs",
                url: "bloglar",
                defaults: new { controller = "Blogs", action = "Index" }
            );

            routes.MapRoute(
                name: "Contact",
                url: "iletisim",
                defaults: new { controller = "Contact", action = "Index" }
            );

            routes.MapRoute(
                name: "useCategory",
                url: "kategori/{slug}",
                defaults: new { controller = "Blogs", action = "useCategory" }
            );

            routes.MapRoute(
                name: "Detail",
                url: "blog/{slug}",
                defaults: new { controller = "Blogs", action = "Detail" }
            );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
