using System.Collections.Generic;
using System.Web.Http;
using Nancy;

namespace DowJones.SyntaxHost.Modules
{
    public class TestController : ApiController
    {
        public IEnumerable<string> Get()
        {
            return new[] {"hello", "world"};
        }

        public string Get(int id)
        {
            return "hello world";
        }
    }

    public class HelloModule : NancyModule
    {
        public HelloModule()
        {
            Get["/hello"] = _ => "hello nancy";
        }
    }
}