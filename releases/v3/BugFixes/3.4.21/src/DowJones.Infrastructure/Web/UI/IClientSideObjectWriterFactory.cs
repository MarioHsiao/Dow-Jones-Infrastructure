using System.IO;

namespace DowJones.Web.UI
{
    public interface IClientSideObjectWriterFactory
    {
        IClientSideObjectWriter Create(string id, string type, TextWriter textWriter);
    }
}