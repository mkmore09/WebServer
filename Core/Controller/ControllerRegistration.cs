using System;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using WebServer.Core.Attributes;

namespace WebServer.Core.Controllers
{
    public class ControllerDiscovery
    {
        private readonly RouteTable _routeTable;

        public ControllerDiscovery(RouteTable routeTable)
        {
            _routeTable = routeTable;
            DiscoverControllers();
        }
        public static Regex BuildRegex(string routeTemplate)
        {

            string pattern = Regex.Replace(routeTemplate, @"\{(\w+)\}", match =>
            {
                string paramName = match.Groups[1].Value;
                return $"(?<{paramName}>[^/?]+)";
            });
            pattern = $"^{pattern}/?$";

            return new Regex(pattern, RegexOptions.Compiled);
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
                        var regex=BuildRegex(getAttr.Path);
                        _routeTable.Routes.Add(regex,new ControllerDescriptor
                        {
                            ControllerType = type,
                            ActionMethod = method,
                            HttpMethod = "GET",
                            RoutePath = getAttr.Path,
                            RoutePathRegex = regex,
                        });
                    }
                    if (postAttr != null)
                    {
                        var regex = BuildRegex(postAttr.Path);
                        _routeTable.Routes.Add(regex, new ControllerDescriptor
                        {
                            ControllerType = type,
                            ActionMethod = method,
                            HttpMethod = "POST",
                            RoutePath = postAttr.Path,
                            RoutePathRegex = regex,
                        });
                    }
                    if (deleteAttr != null)
                    {
                        var regex = BuildRegex(deleteAttr.Path);
                        _routeTable.Routes.Add(regex, new ControllerDescriptor
                        {
                            ControllerType = type,
                            ActionMethod = method,
                            HttpMethod = "DELETE",
                            RoutePath = deleteAttr.Path,
                            RoutePathRegex = regex,
                        });
                    }
                    if (putAttr != null)
                    {
                        var regex = BuildRegex(putAttr.Path);
                        _routeTable.Routes.Add(regex, new ControllerDescriptor
                        {
                            ControllerType = type,
                            ActionMethod = method,
                            HttpMethod = "PUT",
                            RoutePath = putAttr.Path,
                            RoutePathRegex = regex,
                        });
                    }
                }
            }
        }
    }
}
