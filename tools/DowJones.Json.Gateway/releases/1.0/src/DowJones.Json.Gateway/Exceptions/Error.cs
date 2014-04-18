namespace DowJones.Json.Gateway.Exceptions
{
    public class Error
    {
        public Error()
        {
            Code = -1;
        }

        public long Code { get; set; }

        public string Message { get; set; }
    }
}