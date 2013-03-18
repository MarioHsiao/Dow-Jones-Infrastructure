using System.IO;
using System.Text;
using System.Web;

namespace DowJones.Web.Mvc.Diagnostics.Fakes
{
    public class FakeHttpResponse : HttpResponseBase
    {
        private StringBuilder _sb;
        private StringWriter _sw;

        public FakeHttpResponse()
        {
            _sb = new StringBuilder();
            _sw = new StringWriter(_sb);
        }


        public override void Write(string s)
        {
            
            _sb.Append(s);
        }

        public override TextWriter Output
        {
            get
            {
                return _sw;
            }
        }


        public override string ToString()
        {
            return _sb.ToString();
        }


    }
}
