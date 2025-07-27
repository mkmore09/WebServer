namespace WebServer.Core.Http
{ 
    public class HttpResponse
    {
        public HttpResponse(object response)
        {
            Response = response;
        }
        public object Response { get; set; }
    }
}