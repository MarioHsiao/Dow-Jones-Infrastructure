using System;
using System.Diagnostics;
using DowJones.Infrastructure;
using DowJones.Infrastructure.Converters;
using DowJones.Infrastructure.Models.SocialMedia;
using DowJones.Managers.Abstract;
using Newtonsoft.Json;

namespace DowJones.Managers.SocialMedia
{

    using Config;
    using Responses;
    using Hammock;
    using Serializers;

    /// <summary>
    /// The Service for interacting with TweetRiver API
    /// </summary>
    public class SocialMediaService : IExternalService
    {
        
        /// <summary>
        /// The settings.
        /// </summary>
        private  readonly JsonSerializerSettings _settings;


        private readonly ISocialMediaProvider _socialMediaProvider;


        /// <summary>
        /// Gets the default json serializer settings.
        /// </summary>
        private static JsonSerializerSettings GetDefaultJsonSerializerSettings()
        {
            var settings = new JsonSerializerSettings
                           {
                               MissingMemberHandling = MissingMemberHandling.Ignore,
                               NullValueHandling = NullValueHandling.Ignore,
                               DefaultValueHandling = DefaultValueHandling.Include,
                               ContractResolver = new JsonConventionResolver(),
                           };

            settings.Converters.Add(new UnicodeJsonStringConverter());
            settings.Converters.Add(new JsonDateTimeConverter());
            settings.Converters.Add(new JsonGeoConverter());
            settings.Converters.Add(new JsonBooleanConverter());
            settings.Converters.Add(new WordCloudConverter());
            settings.Converters.Add(new TalksAboutConverter());

            return settings;
        }


        /// <summary>
        /// The mapping between industry code and channel name
        /// </summary>
        private readonly IndustryChannelMap IndustryChannelMap;


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
        private T TryGetSocialMediaResponse<T>(ISocialMediaResponse mediaResponse, RestRequest request, out RestResponseBase response)
            where T : class, ISocialMediaResponse, new()
        {
            var result = (T)mediaResponse;
            response = _socialMediaProvider.Client.Request(request);

            switch ((int)response.StatusCode)
            {
                case 200:
                // Success
                case 304:
                    // Not Modified: There was no new data to return
                    try
                    {
                        result = JsonConvert.DeserializeObject<T>(response.Content, _settings);
                    }
                    catch (JsonReaderException jre)
                    {
                        result.Status = Status.DeserializationError;
                        result.Message = jre.StackTrace;
                    }
                    catch (JsonSerializationException jse)
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
                        result.Message = JsonConvert.DeserializeObject<string>(response.Content, _settings);
                    }
                    catch (JsonReaderException)
                    {
                        result.Message = "unable to read error message";
                    }
                    catch (JsonSerializationException)
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
        private T TryGetSocialMediaResponseImplementation<T>(RestRequest request)
             where T : class, ISocialMediaResponse, new()
        {
            var socialMediaResponse = new T();
            T result = default(T);
            Process(
                socialMediaResponse,
                () =>
                {
                    RestResponseBase response;
                    result = TryGetSocialMediaResponse<T>(socialMediaResponse, request, out response)
                        ?? new T
                        {
                            Status = Status.Unknown,
                            Message = response.StatusDescription
                        };
                });

            return result;
        }

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
        /// Initializes members of the <see cref="SocialMediaService"/> class.
        /// </summary>
        public SocialMediaService(ISocialMediaProvider socialMediaProvider)
        {
            _settings = GetDefaultJsonSerializerSettings();
            IndustryChannelMap = new IndustryChannelMap();
            _socialMediaProvider = socialMediaProvider;
        }


        /// <summary>
        /// Gets the tweets by channel.
        /// </summary>
        /// <param name="channel">The channel.</param>
        /// <param name="requestOptions">The request options</param>
        /// <returns></returns>
        public GetTweetsByChannelResponse GetTweetsByChannel(string channel, RequestOptions requestOptions = null)
        {
            Guard.IsNotNullOrEmpty(channel, "channel");
            if (requestOptions == null)
                requestOptions = new RequestOptions();

            Guard.IsNotZeroOrNegative(requestOptions.Limit, "limit");

            var request = _socialMediaProvider.GetSocialMediaRequest(channel, requestOptions);
            var response = TryGetSocialMediaResponseImplementation<GetTweetsByChannelResponse>(request);
            return response;
        }

        public GetTweetsByChannelResponse GetTweetsByIndustry(string industry, RequestOptions requestOptions = null)
        {
            Guard.IsNotNullOrEmpty(industry, "industry");

            if (requestOptions == null)
                requestOptions = new RequestOptions();

            Guard.IsNotZeroOrNegative(requestOptions.Limit, "limit");

            var channel = IndustryChannelMap.GetChannelFromIndustryCode(industry);
            return GetTweetsByChannel(channel, requestOptions);
        }

        public GetExpertsByIndustryResponse GetExpertsByIndustry(string industry, RequestOptions requestOptions = null)
        {
            Guard.IsNotNullOrEmpty(industry, "industry");

            if (requestOptions == null)
                requestOptions = new RequestOptions
                                     {
                                         QueryType = QueryType.Experts
                                     };

            Guard.IsNotZeroOrNegative(requestOptions.Limit, "limit");

            var channel = IndustryChannelMap.GetChannelFromIndustryCode(industry);
            var request = _socialMediaProvider.GetSocialMediaRequest(channel, requestOptions);
            var response = TryGetSocialMediaResponseImplementation<GetExpertsByIndustryResponse>(request);

            return response;
        }
    }
}
