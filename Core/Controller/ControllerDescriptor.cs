using System;
using System.Reflection;

namespace WebServer.Core.Controllers
{
    public class ControllerDescriptor
    {
        public Type ControllerType { get; set; }
        public MethodInfo ActionMethod { get; set; }
        public string HttpMethod { get; set; }
        public string RoutePath { get; set; }
    }
}
