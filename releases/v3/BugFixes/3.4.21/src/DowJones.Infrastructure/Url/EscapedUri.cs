
namespace DowJones.Url
{
    public class EscapedUri : System.Uri
    {
        private readonly string _escapedUriStrng;
        
        public EscapedUri(string escapedUriString) : base(escapedUriString)
        {
            _escapedUriStrng = escapedUriString;
        }

        public override string ToString()
        {
            if (!string.IsNullOrEmpty(_escapedUriStrng) && !string.IsNullOrEmpty(_escapedUriStrng.Trim()))
                return _escapedUriStrng;
            return base.ToString();
        }
    }
}
