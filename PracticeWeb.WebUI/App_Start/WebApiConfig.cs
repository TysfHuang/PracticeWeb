using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace PracticeWeb.WebUI
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            config.MapHttpAttributeRoutes();

            //config.Routes.MapHttpRoute(
            //    name: "",
            //    routeTemplate: "api/{controller}/productID={productID}"
            //);

            //config.Routes.MapHttpRoute(
            //    name: "",
            //    routeTemplate: "api/{controller}/{cate}/{page}",
            //    defaults: new { cate = "ALL", page = 1 }
            //);

            config.Routes.MapHttpRoute(
                name: "",
                routeTemplate: "api/{controller}/{category}/{page}/{searchString}",
                defaults: new { category = "ALL", page = 1, searchString = "" }
            );

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }
    }
}
