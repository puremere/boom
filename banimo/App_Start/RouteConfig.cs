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
               defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional },
               constraints: new { controller = "Home|Admin|Connection|CustomerLogin|Error|app|base|Core|Partner" }

           );

            routes.MapRoute(
              name: "Default1",
              url: "{catname}/{subcatName}/{subcat2Name}",
              defaults: new { controller = "Home", action = "subMenuNew", subcatName = UrlParameter.Optional, subcat2Name = UrlParameter.Optional },
              constraints: new { catname = "digital|projection|history|store-35|mobile accessory|phone|tablet|mobile accessory|headphone|speaker|electronic equipment" +
              "" }
            );

            routes.MapRoute(
             name: "DisplayBrand",
             url: "brands/{name}",
             defaults: new { controller = "Home", action = "brandMenu", id = UrlParameter.Optional }
             //constraints: new { name = "aukey|anker|baseus|beyond|jbl|ldnio|lepow|sony" }
             );

            routes.MapRoute(
            name: "DisplayKombak",
            url: "{name}/{id}",
            defaults: new { controller = "Home", action = "productDetail", id = UrlParameter.Optional }
            );






        }
    }
}