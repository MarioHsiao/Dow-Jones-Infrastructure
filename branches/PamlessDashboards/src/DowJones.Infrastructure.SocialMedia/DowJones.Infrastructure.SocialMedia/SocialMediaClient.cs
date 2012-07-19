// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SocialMediaClient.cs" company="Dow Jones">
//   2011 Dow Jones Factiva
// </copyright>
// <summary>
//   A client for the Social Media service.
//   Use this client as a façade to send messages
//   through this service.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Diagnostics;
using DowJones.Infrastructure.Converters;
using DowJones.Infrastructure.Models.SocialMedia;
using DowJones.Infrastructure.SocialMedia.Serializers;
using Hammock;
using Newtonsoft.Json;
using DowJones.SocialMedia.Responses;

namespace DowJones.Infrastructure.SocialMedia
{

    /// <summary>
    /// A client for the Sulia service. 
    ///   Use this client as a façade to send messages
    ///   through this service.
    /// </summary>
    public partial class SocialMediaClient
    {
        #region Constants and Fields

        /// <summary>
        /// The serializer.
        /// </summary>
        private static readonly SocialMediaSerializer Serializer;

        /// <summary>
        /// The settings.
        /// </summary>
        private static readonly JsonSerializerSettings Settings;

        /// <summary>
        /// The client.
        /// </summary>
        private readonly RestClient client;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        ///   Initializes static members of the <see cref = "SocialMediaClient" /> class.
        /// </summary>
        static SocialMediaClient()
        {
            Settings = new JsonSerializerSettings
                {
                    MissingMemberHandling = MissingMemberHandling.Ignore,
                    NullValueHandling = NullValueHandling.Ignore,
                    DefaultValueHandling = DefaultValueHandling.Include,
                    ContractResolver = new JsonConventionResolver(),    
                };

            Settings.Converters.Add(new UnicodeJsonStringConverter());
            Settings.Converters.Add(new JsonDateTimeConverter());
            Settings.Converters.Add(new JsonGeoConverter());
            Settings.Converters.Add(new JsonBooleanConverter());
            Settings.Converters.Add(new WordCloudConverter());
            Settings.Converters.Add(new TalksAboutConverter());

            Serializer = new SocialMediaSerializer(Settings);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SocialMediaClient"/> class.
        /// </summary>
        /// <param name="serverToken">
        /// The Server token.
        /// </param>
        public SocialMediaClient(string serverToken = "23e87a0a9af35594288300bb223f49d25f6137239e912beef958e297")
        {
            ServerToken = serverToken;
            client = new RestClient { Authority = "http://api.tlists.com/api/v2/" };
        }

        #endregion

        #region Properties

        /// <summary>
        ///   Gets or sets the REST API endpoint by specifying your own address, if necessary.
        /// </summary>
        public string Authority
        {
            get
            {
                return client.Authority;
            }

            set
            {
                client.Authority = value;
            }
        }

        /// <summary>
        ///   Gets the API associated with the Sulia Service.
        /// </summary>
        /// <value>The server token.</value>
        public string ServerToken { get; private set; }

        #endregion

        #region Public Methods

        #endregion

        #region Methods

        /// <summary>
        /// Processes the specified request.
        /// </summary>
        /// <param name="response">The request.</param>
        /// <param name="action">The action.</param>
        private static void Process(ISocialMediaResponse response, Action action)
        {
            var stopwatch = Stopwatch.StartNew();
            var transaction = new Transaction(response.GetType().FullName);
            try
            {
                action();
            }
            catch (Exception ex)
            {
                response.Status = Status.Unknown;
                response.Message = ex.Message;
            }
            finally
            {
                stopwatch.Stop();
                transaction.ElapsedTime = stopwatch.ElapsedMilliseconds;
                response.Audit.TransactionCollection.Add(transaction);
            }
        }

        /// <summary>
        /// Tries the get social media request.
        /// </summary>
        /// <typeparam name="T">A Type that implements the ISocialMediaResponse interface.</typeparam>
        /// <param name="mediaResponse">The media response.</param>
        /// <param name="request">The request.</param>
        /// <param name="response">The response.</param>
        /// <returns>
        /// An object of Type T.
        /// </returns>
        private T TryGetSuliaResponse<T>(ISocialMediaResponse mediaResponse, RestRequest request, out RestResponseBase response) where T : class, ISocialMediaResponse, new()
        {
            var result = (T)mediaResponse;
            response = client.Request(request);

            switch ((int)response.StatusCode)
            {
                case 200: 
                    // Success
                case 304: 
                    // Not Modified: There was no new data to return
                    try
                    {
                        result = JsonConvert.DeserializeObject<T>( response.Content, Settings );
                    }
                    catch( JsonReaderException jre )
                    {
                        result.Status = Status.DeserializationError;
                        result.Message = jre.StackTrace;
                    }
                    catch( JsonSerializationException jse )
                    {
                        result.Status = Status.DeserializationError;
                        result.Message = jse.StackTrace;
                    }

                    break;
                case 400: 
                    // Bad Request: The request was invalid. An accompanying error message will explain why.
                case 401: 
                    // Not Authorized: Authentication credentials were missing or incorrect.
                case 403:
                    // Forbidden: The request is understood, but it has been refused.  An accompanying error message will explain why.
                case 404:
                    // Not Found: The URI requested is invalid or the resource requested, such as a user, does not exists.   
                case 406:
                    // Not Acceptable: Returned by the Search API when an invalid format is specified in the request.
                    try
                    {
                        result.Status = Status.UserError;
                        result.Message = JsonConvert.DeserializeObject<string>(response.Content, Settings);
                    }
                    catch (JsonReaderException)
                    {
                        result.Message = "unable to read error message";
                    }
                    catch(JsonSerializationException)
                    {
                        result.Message = "unable to read error message";
                    }

                    break;
                case 500:
                    // Internal Server Error: Something is broken.  Please contact TLists support if this error persists.
                case 503:
                    // Service Unavailable: The TLists servers are up, but overloaded with requests. Try again later.
                    result.Status = Status.ServerError;
                    break;
            }

            return result;
        }

        /// <summary>
        /// Tries the get social media request.
        /// </summary>
        /// <typeparam name="T">A Type that implements the ISocialMediaResponse interface.</typeparam>
        /// <param name="request">The request.</param>
        /// <returns>An object of Type T.</returns>
        private T TryGetSuliaResponseImplementation<T>(RestRequest request) where T : class, ISocialMediaResponse, new()
        {
            var socialMediaResponse = new T();
            T result = default(T);
            Process(
                socialMediaResponse, 
                () =>
                    {
                        RestResponseBase response;
                        result = TryGetSuliaResponse<T>(socialMediaResponse, request, out response) 
                            ?? new T 
                            {
                              Status = Status.Unknown,
                              Message = response.StatusDescription
                            };      
                    });

            return result;
        }                             
         
        /// <summary>
        /// The set social media meta.
        /// </summary>
        /// <param name="request">The request.</param>
        private void SetSocialMediaMeta(RestBase request)
        {
            request.AddHeader("Accept", "application/json");
            request.AddHeader("Content-Type", "application/json; charset=utf-8");
            request.AddHeader("User-Agent", "Dow Jones Factiva");
            request.AddParameter("app_key", ServerToken);
        }
        #endregion
    }
}