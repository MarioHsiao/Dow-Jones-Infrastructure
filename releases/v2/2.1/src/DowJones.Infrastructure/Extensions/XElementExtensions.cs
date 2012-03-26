using System.Xml.Linq;

namespace DowJones.Extensions
{
    public static class XElementExtensions
    {

        public static string GetAttributeValue(this XElement node, string attributeName, string defaultValue = null)
        {
            if (node == null)
                return null;

            var attribute = node.Attribute(attributeName);

            if(attribute == null)
                return null;

            return attribute.Value;
        }

        public static string GetElementValue(this XElement node, string elementName, string defaultValue = null)
        {
            if (node == null)
                return null;

            var element = node.Element(elementName);

            if(element == null)
                return null;

            return element.Value;
        }

    }
}
