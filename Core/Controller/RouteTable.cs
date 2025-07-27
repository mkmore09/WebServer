using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace WebServer.Core.Controllers
{
    public class RouteTable
    {
        public Dictionary<Regex,ControllerDescriptor> Routes { get; } = new();
        public ControllerDescriptor Match(String url)
        {
            return Routes.FirstOrDefault(i=>i.Key.IsMatch(url)).Value;
        }
    }
}
