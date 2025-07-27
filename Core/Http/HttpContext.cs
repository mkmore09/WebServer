using WebServer.Core.DI;

namespace WebServer.Core.Http
{
    public class HttpContext
    {
        public HttpContext(ServiceProvider serviceProvider,HttpRequest httpRequest) { 
            this.serviceProvider = serviceProvider;
            this.Request = httpRequest;
        }
        public HttpRequest Request { get; set; }
        public HttpResponse Response { get; set; }

        public ServiceProvider serviceProvider {  get; set; }
    }
}
