using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace HHMVC0522
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.MapRoute(
               name: "Home",
               url: "",
               defaults: new { controller = "Home2", action = "Index"}
           );
            routes.MapRoute(
               name: "PostDetail",
               url: "{controller}/{action}/{ID}",
               defaults: new { controller = "Home2", action = "PostDetail", ID = UrlParameter.Optional, Model=UrlParameter.Optional }
           );
            routes.MapRoute(
              name: "DeleteComment",
              url: "{controller}/{action}/{ID}",
              defaults: new { controller = "Home2", action = "DeleteComment", ID = UrlParameter.Optional }
          );
            
            routes.MapRoute(
               name: "Default",
               url: "{controller}/{action}/{id}",
               defaults: new { controller = "Home2", action = "Index", id = UrlParameter.Optional }
           );





        }
    }
}
