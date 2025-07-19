using System.Collections.Generic;

namespace WebServer.Core.Controllers
{
    public class RouteTable
    {
        public List<ControllerDescriptor> Routes { get; } = new();
    }
}
