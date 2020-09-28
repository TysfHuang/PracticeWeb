using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace PracticeWeb.WebUI
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: null,
                url: "{controller}/{action}/productID={productID}",
                defaults: new
                {
                    controller = "Home",
                    action = "ProductDetail",
                }
            );

            routes.MapRoute(
                name: null,
                url: "{category}",
                defaults: new
                {
                    controller = "Home",
                    action = "Index",
                }
            );

            routes.MapRoute(
                name: null,
                url: "",
                defaults: new
                {
                    controller = "Home",
                    action = "Index"
                }
            );

            routes.MapRoute(
                name: null,
                url: "{controller}/{action}",
                defaults: new
                {
                    controller = "Home"
                }
            );

            routes.MapRoute(
                name: null,
                url: "{controller}/{action}",
                defaults: new
                {
                    controller = "Home",
                    action = "Index",
                }
            );
        }
    }
}
