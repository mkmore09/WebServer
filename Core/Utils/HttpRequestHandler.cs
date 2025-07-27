using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using WebServer.Core.Http;

namespace WebServer.Core.Utils
{
    internal class HttpRequestHandler
    {
        public void SendHttpResponse(NetworkStream stream,HttpContext httpContent)
        {
            string responseBody = "<html><body><h1>"+httpContent.Response.Response+"</h1></body></html>";

            string httpResponse =
                "HTTP/1.1 200 OK\r\n" +
                "Content-Type: text/html; charset=UTF-8\r\n" +
                $"Content-Length: {Encoding.UTF8.GetByteCount(responseBody)}\r\n" +
                "Connection: close\r\n" +
                "\r\n" +
                responseBody;

            byte[] responseBytes = Encoding.UTF8.GetBytes(httpResponse);
            stream.Write(responseBytes, 0, responseBytes.Length);
            stream.Flush();
        }
        public  string ReadHttpRequest(NetworkStream stream)
        {
            var reader = new StreamReader(stream, Encoding.UTF8, leaveOpen: true);
            var requestLines = new List<string>();

            string line;
            while (!string.IsNullOrEmpty(line = reader.ReadLine()))
            {
                requestLines.Add(line);
            }

            var contentLengthHeader = requestLines
                .FirstOrDefault(h => h.StartsWith("Content-Length:", StringComparison.OrdinalIgnoreCase));

            int contentLength = 0;
            if (contentLengthHeader != null)
            {
                contentLength = int.Parse(contentLengthHeader.Split(":")[1].Trim());
            }

            char[] bodyBuffer = new char[contentLength];
            reader.Read(bodyBuffer, 0, contentLength);
            string body = new string(bodyBuffer);

            return string.Join("\r\n", requestLines) + "\r\n\r\n" + body;

        }
    }
}
