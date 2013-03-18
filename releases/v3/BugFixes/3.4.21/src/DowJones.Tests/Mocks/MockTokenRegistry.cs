using System;
using DowJones.Token;

namespace DowJones.Mocks
{
    public class MockTokenRegistry : ITokenRegistry
    {
        public string Get(string tokenName)
        {
            return string.Format("{{{0}}}", tokenName);
        }

        public string Get(Enum value)
        {
            return Get(value.ToString());
        }

        public string GetErrorMessage(int value)
        {
            return Get( string.Concat( "ErrorFor", value ) );     
        }

        public string GetErrorMessage(string value)
        {
            return Get( string.Concat( "ErrorFor", value ) );
        }
    }
}
