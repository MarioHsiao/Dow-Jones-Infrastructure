using DowJones.Utilities.Managers.Core;

namespace DowJones.Token
{
    public interface ITokenRegistry
    {
        string Get(string tokenName);
    }

    public class TokenRegistry : ITokenRegistry
    {
        private readonly ResourceTextManager _resourceManager;


        public TokenRegistry(ResourceTextManager resourceManager)
        {
            _resourceManager = resourceManager;
        }


        public string Get(string tokenName)
        {
            return _resourceManager.GetString(tokenName);
        }
    }
}
