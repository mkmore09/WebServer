using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using WebServer.Core.Utils;

namespace WebServer.Core.SocketOperations
{
    public class SocketHandler
    {
        private readonly TcpListener _listener;
        private bool _isRunning;
        public SocketHandler(int port)
        {
            _listener = new TcpListener(IPAddress.Any, port);
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
                var request = new HttpRequestHandler();
                var a = request.ReadHttpRequest(stream);
                request.SendHttpResponse(stream);
                stream.Close();
                tcpClient.Close();
            }
            catch (Exception ex) 
            { 
                Console.WriteLine(ex.Message); }

        }
    }
}
