

namespace DowJones.Token.Encryption
{
    public interface ITokenParser
    {
        string Encrypt();
        void Decrypt(string str);
    }
}
 