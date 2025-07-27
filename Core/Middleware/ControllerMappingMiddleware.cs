using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebServer.Core.Controllers;
using WebServer.Core.Http;

namespace WebServer.Core.Middleware
{
    public static class ControllerMappingMiddleware
    {
        public static Func<HttpContext, MiddlewareBuilder.RequestDelegate, Task> Create(RouteTable registry)
        {
            return async (context, next) =>
            {
                var controllerDescripter = registry.Match(context.Request.Path);
                if (controllerDescripter != null)
                {
                    context.Request.controllerDescriptor = controllerDescripter;
                }

                await next(context);
            };
        }
    }

}
