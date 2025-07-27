using System.Text.RegularExpressions;
using WebServer.Core.Controllers;
using WebServer.Core.DI;

namespace WebServer.Core.Http
{
    public class HttpRequest
    {
        public string Method { get; set; }
        public string Path { get; set; }
        public string QueryString { get; set; }
        public string HttpVersion { get; set; }
        public ControllerDescriptor controllerDescriptor { get; set; }  
        public Dictionary<string, string> Headers { get; set; } = new();
        public Dictionary<string, string> Cookies { get; set; } = new();
        public Dictionary<string, string> QueryParameters { get; set; } = new();
        public Dictionary<string, string> RouteParameters { get; set; } = new();
        
        public string Body { get; set; }
        
        public static HttpRequest Parse(string rawRequest)
        {
            var request = new HttpRequest();

            var sections = rawRequest.Split(new[] { "\r\n\r\n" }, 2, StringSplitOptions.None);
            string headerSection = sections[0];
            string bodySection = sections.Length > 1 ? sections[1] : "";

            var lines = headerSection.Split(new[] { "\r\n" }, StringSplitOptions.None);
            var requestLineParts = lines[0].Split(' ');

            request.Method = requestLineParts[0];
            var fullPath = requestLineParts[1];
            request.HttpVersion = requestLineParts[2];

            
            var pathParts = fullPath.Split('?', 2);
            request.Path = pathParts[0];
            request.QueryString = pathParts.Length > 1 ? pathParts[1] : "";

            
            for (int i = 1; i < lines.Length; i++)
            {
                var headerParts = lines[i].Split(new[] { ": " }, 2, StringSplitOptions.None);
                if (headerParts.Length == 2)
                {
                    request.Headers[headerParts[0]] = headerParts[1];
                }
            }

           
            if (request.Headers.TryGetValue("Cookie", out string cookieHeader))
            {
                var cookies = cookieHeader.Split("; ");
                foreach (var cookie in cookies)
                {
                    var kv = cookie.Split('=');
                    if (kv.Length == 2)
                        request.Cookies[kv[0]] = kv[1];
                }
            }

            
            if (!string.IsNullOrEmpty(request.QueryString))
            {
                var pairs = request.QueryString.Split('&');
                foreach (var pair in pairs)
                {
                    var kv = pair.Split('=');
                    if (kv.Length == 2)
                        request.QueryParameters[kv[0]] = Uri.UnescapeDataString(kv[1]);
                }
            }

            
            request.Body = bodySection;

            return request;
        }
    }


}