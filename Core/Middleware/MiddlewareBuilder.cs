
using WebServer.Core.Http;

namespace WebServer.Core.Middleware
{
    public class MiddlewareBuilder
    {
        public delegate Task RequestDelegate(HttpContext context);

        private readonly List<Func<RequestDelegate, RequestDelegate>> _components = new();

        public MiddlewareBuilder Use(Func<HttpContext, RequestDelegate, Task> middleware)
        {
            _components.Add(next => context => middleware(context, next));
            return this;
        }

        public RequestDelegate Build()
        {
            RequestDelegate app = context => Task.CompletedTask;

            foreach (var component in _components.Reverse<Func<RequestDelegate, RequestDelegate>>())
            {
                app = component(app);
            }

            return app;
        }
    }
}
