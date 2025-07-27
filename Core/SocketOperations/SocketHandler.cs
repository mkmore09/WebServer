using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using WebServer.Core.DI;
using WebServer.Core.Http;
using WebServer.Core.Middleware;
using WebServer.Core.Utils;

namespace WebServer.Core.SocketOperations
{
    public class SocketHandler
    {
        public MiddlewareBuilder.RequestDelegate requestDelegate;
        public ServiceProvider serviceProvider;
        private readonly TcpListener _listener;
        private bool _isRunning;
        public SocketHandler(int port, MiddlewareBuilder.RequestDelegate requestDelegate,ServiceProvider serviceProvider)
        {
            _listener = new TcpListener(IPAddress.Any, port);
            this.requestDelegate = requestDelegate;
            this.serviceProvider = serviceProvider;
        }
        public void Start()
        {
            _isRunning = true;
            _listener.Start();
            Console.WriteLine("Socket server running");
            while (true)
            {
                if (_isRunning)
                {
                    var clinetScoket = _listener.AcceptTcpClient();
                    Task.Run(() => handle(clinetScoket));
                }
            }
        }
        public void handle(TcpClient tcpClient)
        {
            try
            {
                var stream = tcpClient.GetStream();
                var requestHandler = new HttpRequestHandler();
                HttpRequest request = HttpRequest.Parse(requestHandler.ReadHttpRequest(stream));
                HttpContext httpContext = new HttpContext(serviceProvider.CreateScope(),request);
                requestDelegate(httpContext);
                requestHandler.SendHttpResponse(stream, httpContext);
                stream.Close();
                tcpClient.Close();
            }
            catch (Exception ex) 
            { 
                Console.WriteLine(ex.Message); }

        }
    }
}
