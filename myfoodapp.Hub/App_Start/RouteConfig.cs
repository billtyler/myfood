using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;

namespace myfoodapp.Hub
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
			
				routes.MapRoute(
				name: "PioneerProductionSite",
				url: "PioneerProductionSite",
				defaults: new { controller = "ProductionUnits", action = "UserUnit" }
				);

			routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );

        }
    }
}
