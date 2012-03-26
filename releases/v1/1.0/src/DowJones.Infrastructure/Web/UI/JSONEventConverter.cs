using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DowJones.Web.UI
{
    public class JSONEventConverter
    {

        /// <summary>
        /// Builds a dictionary of name/value pairs
        /// </summary>
        /// <param name="obj">The object to serialize. </param>
        /// <param name="serializer">The object that is responsible for the serialization. </param>
        /// <returns>An object that contains key/value pairs that represent the object’s data. </returns>
        /// <exception cref="InvalidOperationException"><paramref name="obj"/> must be of the <see cref="IDictionary<string, string>"/> type</exception>
        public static string Serialize(IDictionary<string, string> jsonEvents)
        {
            Guard.IsNotNull(jsonEvents, "jsonEvents");

            IEnumerable<KeyValuePair<string, string>> validEventHandlers = 
                jsonEvents
                    .Where(handler => !string.IsNullOrWhiteSpace(handler.Key))
                    .Where(handler => !string.IsNullOrWhiteSpace(handler.Value));

            if (!validEventHandlers.Any())
                return "{ }";

            StringBuilder sb = new StringBuilder();
            sb.Append("{");

            foreach (var item in validEventHandlers)
            {
                sb.AppendFormat("\"{0}\":{1},", item.Key, item.Value);
            }

            // strip out the last comma
            sb.Remove(sb.Length - 1, 1);

            sb.Append("}");
            
            return sb.ToString();
        }
    }
}
