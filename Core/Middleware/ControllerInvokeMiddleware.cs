using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebServer.Core.Http;

namespace WebServer.Core.Middleware
{
    public class ControllerInvokeMiddleware
    {
        public static Func<HttpContext, MiddlewareBuilder.RequestDelegate, Task> Create()
        {
            return async (context, next) =>
            {
                if (context.serviceProvider != null)
                {
                    context.Response = context.Request.controllerDescriptor.InvokeController(context.serviceProvider);
                }

                await next(context);
            };
        }
    }
}
