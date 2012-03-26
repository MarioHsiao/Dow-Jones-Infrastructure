

namespace DowJones.Utilities.TokenEncryption
{
    public interface ITokenParser
    {
        string Encrypt();
        void Decrypt(string str);
    }
}
 