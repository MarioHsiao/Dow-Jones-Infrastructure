namespace DowJones.Utilities.Managers.Core
{
    public interface IResourceTextManager
    {
        bool IsResourceAssemblyLoaded { get; }

        /// <summary>
        /// Creates an alias for an existing token
        /// </summary>
        /// <param name="existingToken">The existing token name to alias</param>
        /// <param name="alias">The alias (new token name) for the existing token</param>
        void Alias(string existingToken, string alias);

        string GetString(string name);

        /// <summary>
        /// Resolve the user-rea framework/gateway error message
        /// </summary>
        /// <param name="errorNumber"></param>
        /// <returns></returns>
        string GetErrorMessage(string errorNumber);

        /// <summary>
        /// Is the given token name an alias for another token?
        /// </summary>
        bool IsAlias(string tokenName);
    }
}