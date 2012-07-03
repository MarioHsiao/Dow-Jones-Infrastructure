using System;
using DowJones.Globalization;

namespace DowJones.Token
{
    public interface ITokenRegistry
    {
        string Get(string tokenName);
        string Get(Enum value);
        string GetErrorMessage(int errorNumber);
        string GetErrorMessage(string errorNumber);
    }

    public class TokenRegistry : ITokenRegistry
    {
        private readonly IResourceTextManager _resourceManager;
        private readonly EnumTokenResolver _enumResolver;


        public TokenRegistry(IResourceTextManager resourceManager, EnumTokenResolver enumResolver)
        {
            _resourceManager = resourceManager;
            _enumResolver = enumResolver;
        }


        public string Get(string tokenName)
        {
            return _resourceManager.GetString(tokenName);
        }

        public string GetErrorMessage( int errorNumber)
        {
            return _resourceManager.GetErrorMessage(errorNumber.ToString());
        }

        public string GetErrorMessage( string errorNumber )
        {
            return _resourceManager.GetErrorMessage(errorNumber);
        }

        public string Get(Enum value)
        {
            if(value == null)
                return null;

            string tokenName = _enumResolver.GetTokenName(value);
            return Get(tokenName);
        }
    }
}
