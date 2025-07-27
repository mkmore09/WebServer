using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebServer.Core.Attributes;

namespace WebServer
{
    [ControllerAttribute]
    public class DummyController
    {
        public DummyController()
        {

        }
        [HttpGet("/Hello")]
        public string DummyMethod()
        {
            return "hello noobs";
        }
    }
}
