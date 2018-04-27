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
				name: "Client",
				url: "Client",
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
