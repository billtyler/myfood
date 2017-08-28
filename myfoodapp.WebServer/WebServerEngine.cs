using Restup.Webserver.File;
using Restup.Webserver.Http;
using Restup.Webserver.Rest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace myfoodapp.WebServer
{
    public class WebServerEngine
    {
        public async Task Run()
        {
            var restRouteHandler = new RestRouteHandler();
            restRouteHandler.RegisterController<HomeController>();

            var configuration = new HttpServerConfiguration()
              .ListenOnPort(5000)
              .RegisterRoute("api", restRouteHandler)
              .RegisterRoute(new StaticFileRouteHandler(@"myfoodapp.WebServer\Web"))
              .EnableCors();

            var httpServer = new HttpServer(configuration);
            await httpServer.StartServerAsync();
        }
    }
}
