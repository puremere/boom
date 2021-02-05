using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace banimo
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            // BotDetect requests must not be routed
            routes.IgnoreRoute("{*botdetect}",
            new { botdetect = @"(.*)BotDetectCaptcha\.ashx" });


            routes.IgnoreRoute("{*botdetect}",
               new { botdetect = @"(.*)BotDetectCaptcha\.ashx" });
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
              
            );
            routes.MapRoute(
               name: "Default1",
               url: "{action}/{id}",
               defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
              
           );
           
        }
    }
}