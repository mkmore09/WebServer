using System;

namespace WebServer.Core.Attributes
{
    [AttributeUsage(AttributeTargets.Class)]
    public class ControllerAttribute : Attribute
    {
    }

    [AttributeUsage(AttributeTargets.Method)]
    public class HttpGetAttribute : Attribute
    {
        public string Path { get; }
        public HttpGetAttribute(string path)
        {
            Path = path;
        }
    }
    [AttributeUsage(AttributeTargets.Method)]
    public class HttpDeleteAttribute : Attribute
    {
        public string Path { get; }
        public HttpDeleteAttribute(string path)
        {
            Path = path;
        }
    }
    [AttributeUsage(AttributeTargets.Method)]
    public class HttpPutAttribute : Attribute
    {
        public string Path { get; }
        public HttpPutAttribute(string path)
        {
            Path = path;
        }
    }
    [AttributeUsage(AttributeTargets.Method)]
    public class HttpPostAttribute : Attribute
    {
        public string Path { get; }
        public HttpPostAttribute(string path)
        {
            Path = path;
        }
    }
}
