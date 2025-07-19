using System;
using System.Linq;
using System.Reflection;
using WebServer.Core.Attributes;

namespace WebServer.Core.Controllers
{
    public class ControllerDiscovery
    {
        private readonly RouteTable _routeTable;

        public ControllerDiscovery(RouteTable routeTable)
        {
            _routeTable = routeTable;
        }

        public void DiscoverControllers()
        {
            var allTypes = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(a => a.GetTypes());

            foreach (var type in allTypes)
            {
                if (type.GetCustomAttribute<ControllerAttribute>() == null)
                    continue;

                foreach (var method in type.GetMethods())
                {
                    var getAttr = method.GetCustomAttribute<HttpGetAttribute>();
                    var postAttr = method.GetCustomAttribute<HttpPostAttribute>();
                    var deleteAttr = method.GetCustomAttribute<HttpDeleteAttribute>();
                    var putAttr = method.GetCustomAttribute<HttpPutAttribute>();

                    if (getAttr != null)
                    {
                        _routeTable.Routes.Add(new ControllerDescriptor
                        {
                            ControllerType = type,
                            ActionMethod = method,
                            HttpMethod = "GET",
                            RoutePath = getAttr.Path
                        });
                    }
                    if (postAttr != null)
                    {
                        _routeTable.Routes.Add(new ControllerDescriptor
                        {
                            ControllerType = type,
                            ActionMethod = method,
                            HttpMethod = "POST",
                            RoutePath = postAttr.Path
                        });
                    }
                    if (deleteAttr != null)
                    {
                        _routeTable.Routes.Add(new ControllerDescriptor
                        {
                            ControllerType = type,
                            ActionMethod = method,
                            HttpMethod = "POST",
                            RoutePath = postAttr.Path
                        });
                    }
                    if (putAttr != null)
                    {
                        _routeTable.Routes.Add(new ControllerDescriptor
                        {
                            ControllerType = type,
                            ActionMethod = method,
                            HttpMethod = "POST",
                            RoutePath = postAttr.Path
                        });
                    }
                }
            }
        }
    }
}
