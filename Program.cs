using System.Net;
using System.Net.Sockets;
using System.Text;
using WebServer.Core.DI;
using WebServer.Core.Middleware;
using WebServer.Core.SocketOperations;

class Program
{
    static void Main(string[] args)
    {
        SocketHandler socketHandler = new SocketHandler(8080);
        socketHandler.Start();


    }
}
