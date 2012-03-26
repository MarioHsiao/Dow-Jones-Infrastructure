// -----------------------------------------------------------------0---------------------------------------------------
// <copyright file="SocialMediaSerializer.cs" company="Dow Jones">
//   
// </copyright>
// <summary>
//   TODO: Update summary.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Dynamic;
using System.IO;
using System.Text;    
using Hammock;
using Hammock.Serialization;     
using Newtonsoft.Json;

namespace DowJones.Infrastructure.SocialMedia.Serializers
{
    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    internal class SocialMediaSerializer : ISerializer, IDeserializer
    {
        /// <summary>
        /// </summary>
        private readonly JsonSerializer serializer;

        /// <summary>
        /// Initializes a new instance of the <see cref="SocialMediaSerializer"/> class.
        /// </summary>
        /// <param name="settings">
        /// The settings.
        /// </param>
        public SocialMediaSerializer(JsonSerializerSettings settings)
        {
            this.serializer = new JsonSerializer
                              {                                 
                                  ConstructorHandling = settings.ConstructorHandling,
                                  ContractResolver = settings.ContractResolver,
                                  ObjectCreationHandling = settings.ObjectCreationHandling,
                                  MissingMemberHandling = settings.MissingMemberHandling,
                                  DefaultValueHandling = settings.DefaultValueHandling,
                                  NullValueHandling = settings.NullValueHandling
                              };

            foreach (var converter in settings.Converters)
            {
                this.serializer.Converters.Add(converter);
            }
        }

        public virtual object Deserialize(string content, Type type)
        {
            using (var stringReader = new StringReader(content))
            {
                using (var jsonTextReader = new JsonTextReader(stringReader))
                {
                    return this.serializer.Deserialize(jsonTextReader, type);
                }
            }
        }

        public virtual T Deserialize<T>(string content)
        {
            using (var stringReader = new StringReader(content))
            {
                using (var jsonTextReader = new JsonTextReader(stringReader))
                {
                    return this.serializer.Deserialize<T>(jsonTextReader);
                }
            }
        }

        public virtual string Serialize(object instance, Type type)
        {
            using (var stringWriter = new StringWriter())
            {
                using (var jsonTextWriter = new JsonTextWriter(stringWriter))
                {
                    jsonTextWriter.Formatting = Formatting.Indented;
                    jsonTextWriter.QuoteChar = '"';

                    this.serializer.Serialize(jsonTextWriter, instance);

                    var result = stringWriter.ToString();
                    return result;
                }
            }
        }

        public virtual string ContentType
        {
            get { return "application/json"; }
        }

        public Encoding ContentEncoding
        {
            get { return Encoding.UTF8; }
        }

        #region Implementation of IDeserializer

        /// <summary>
        /// Deserializes the specified response.
        /// </summary>
        /// <param name="response">The response.</param>
        /// <param name="type">The type.</param>
        /// <returns></returns>
        public virtual object Deserialize(RestResponse response, Type type)
        {
            return this.Deserialize(response.Content, type);
        }

        /// <summary>
        /// Deserializes the specified response.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="response">The response.</param>
        /// <returns></returns>
        public virtual T Deserialize<T>(RestResponse<T> response)
        {
            return Deserialize<T>(response.Content);
        }

        /// <summary>
        /// </summary>
        /// <param name="response">
        /// The response.
        /// </param>
        /// <returns>
        /// </returns>
        /// <exception cref="NotImplementedException">
        /// </exception>
        public dynamic DeserializeDynamic(RestResponse<dynamic> response)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Deserializes the dynamic.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="response">The response.</param>
        /// <returns></returns>
        public virtual dynamic DeserializeDynamic<T>(RestResponse<T> response) where T : DynamicObject
        {
            return Deserialize<T>(response.Content);
        }

        #endregion
    }
}