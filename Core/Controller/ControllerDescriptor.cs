using System;
using System.Reflection;
using System.Text.RegularExpressions;
using WebServer.Core.DI;
using WebServer.Core.Http;

namespace WebServer.Core.Controllers
{
    public class ControllerDescriptor
    {
        public Type ControllerType { get; set; }
        public MethodInfo ActionMethod { get; set; }
        public string HttpMethod { get; set; }
        public string RoutePath { get; set; }
        public Regex RoutePathRegex { get; set; }
        public HttpResponse InvokeController(ServiceProvider serviceProvider)
        {
            
            var ctor = ControllerType.GetConstructors().First();
            var parameters = ctor.GetParameters()
                .Select(p => serviceProvider.GetService(p.ParameterType))
                .ToArray();
            var implementation=Activator.CreateInstance(ControllerType, parameters);
            var result = ActionMethod.Invoke(implementation,null);
            return new HttpResponse(result);
        }
    }
}
