using System.Collections.Generic;
using System.Globalization;
using System.Web.Http;

namespace DowJones.Json.Gateway.Mvc.Controllers
{
    public class DataController : ApiController
    {
        // GET api/data
        public IEnumerable<string> Get()
        {
            return new [] { "value1", "value2" };
        }

        // GET api/data/5
        public string Get(int id)
        {
            return id.ToString(CultureInfo.InvariantCulture);
        }

        // POST api/data
        public void Post([FromBody]string value)
        {
        }

        // PUT api/data/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/data/5
        public void Delete(int id)
        {
        }
    }
}
