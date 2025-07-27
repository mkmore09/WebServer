using System.Net;
using System.Net.Sockets;
using System.Text;
using WebServer.Core.Controllers;
using WebServer.Core.DI;
using WebServer.Core.Middleware;
using WebServer.Core.SocketOperations;

class Program
{
    static void Main(string[] args)
    {
        ServiceCollection services = new ServiceCollection();
        ServiceProvider serviceProvider = new ServiceProvider(services.GetDescriptors());
        
        RouteTable routeTable = new RouteTable();
        ControllerDiscovery controllerDiscovery = new ControllerDiscovery(routeTable);

        MiddlewareBuilder middlewareBuilder = new MiddlewareBuilder();
        middlewareBuilder.Use(ControllerMappingMiddleware.Create(routeTable));
        middlewareBuilder.Use(ControllerInvokeMiddleware.Create());
        
        SocketHandler socketHandler = new SocketHandler(8080,middlewareBuilder.Build(),serviceProvider);
        socketHandler.Start();


    }
}
