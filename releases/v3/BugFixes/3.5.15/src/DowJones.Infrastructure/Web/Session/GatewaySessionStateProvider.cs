using System;
using System.IO;
using System.Web;
using System.Web.SessionState;
using DowJones.DependencyInjection;
using DowJones.Exceptions;
using DowJones.Infrastructure.Common;
using DowJones.Properties;
using DowJones.Session;
using Factiva.Gateway.Messages.Cache.SessionCache.V1_0;
using Factiva.Gateway.Services.V1_0;
using Factiva.Gateway.V1_0;
using log4net;

namespace DowJones.Web.Session
{
    /// <summary>
    /// 
    /// Implements an ASP.NET Session State provider that
    /// stores and retrieves user session data from the 
    /// Gateway Session Cache
    /// </summary>
    public class GatewaySessionStateProvider : SessionStateStoreProviderBase
    {
        private const CacheScope UserSessionCacheScope = CacheScope.Session;
        private const string SaveSessionErrorMessage = "Error saving user session";
        private const string RemoveItemErrorMessage = "Error removing cached item";
        private const string RetrieveSessionErrorMessage = "Error retrieving Session data";

        private static readonly ILog Log = LogManager.GetLogger(typeof (GatewaySessionStateProvider));

        public event EventHandler OnSaveError
        {
            add { _onSaveError += value; }
            remove { _onSaveError -= value; }
        }
        private event EventHandler _onSaveError;

        protected internal IControlData ControlData
        {
            get
            {
                // Use ServiceLocator to get the Control Data for the current request.
                // This cannot be injected because the Session Provider instance lives
                // at the application level (one per application)
                return _controlData ?? ServiceLocator.Resolve<IControlData>();
            }
            set { _controlData = value; }
        }

        private IControlData _controlData;

        public bool EnableAsync { get; set; }

        protected internal string SessionKey
        {
            get { return string.Format("{0}_WebSession", Product.CacheApplication); }
        }

        protected internal Product Product
        {
            get { return _product ?? ServiceLocator.Resolve<Product>(); }
            set { _product = value; }
        }
        private Product _product;

        public int Timeout
        {
            get { return Settings.Default.UserSessionCacheExpiryInMinutes; }
        }


        public override SessionStateStoreData CreateNewStoreData(HttpContext context, int timeout)
        {
            return new SessionStateStoreData(new SessionStateItemCollection(), GetHttpStaticObjectsCollection(context), timeout);
        }

        public override void CreateUninitializedItem(HttpContext context, string id, int timeout)
        {
            /* Intentially empty */
        }

        public override void Dispose()
        {
            /* Intentially empty */
        }

        public override void EndRequest(HttpContext context)
        {
            /* Intentially empty */
        }

        public override SessionStateStoreData GetItem(HttpContext context, string id, out bool locked, out TimeSpan lockAge, out object lockId, out SessionStateActions actions)
        {
            actions = SessionStateActions.None;
            lockAge = TimeSpan.MinValue;
            locked = false;
            lockId = null;

            var request = new GetItemRequest
                              {
                                  Key = SessionKey,
                                  Scope = UserSessionCacheScope,
                              };

            var response = SessionCacheService.GetItem(ControlDataManager.Convert(ControlData), request);

            HandleGatewayResponse(response, RetrieveSessionErrorMessage);

            // If there was any problem retrieving the session, create and return a new one
            if (response.rc != 0)
            {
                return CreateNewStoreData(context, Timeout);
            }

            var items = DeserializeSessionData(response.ObjectResponse.Item.StringData);

            return new SessionStateStoreData(items, GetHttpStaticObjectsCollection(context), 0);
        }

        public override SessionStateStoreData GetItemExclusive(HttpContext context, string id, out bool locked, out TimeSpan lockAge, out object lockId, out SessionStateActions actions)
        {
            return GetItem(context, id, out locked, out lockAge, out lockId, out actions);
        }

        public override void InitializeRequest(HttpContext context)
        {
            /* Intentially empty */
        }

        public override void ReleaseItemExclusive(HttpContext context, string id, object lockId)
        {
            /* Intentially empty */
        }

        public override void RemoveItem(HttpContext context, string id, object lockId, SessionStateStoreData item)
        {
            var request = new DeleteItemRequest
                              {
                                  Key = SessionKey,
                                  Scope = UserSessionCacheScope,
                              };

            var response = SessionCacheService.DeleteItem(ControlDataManager.Convert(ControlData), request);
            HandleGatewayResponse(response, RemoveItemErrorMessage);
        }

        public override void ResetItemTimeout(HttpContext context, string id)
        {
            /* Intentially empty */
        }

        public override void SetAndReleaseItemExclusive(HttpContext context, string id, SessionStateStoreData item, object lockId, bool newItem)
        {
            var sessionItems = item.Items as SessionStateItemCollection;

            if (sessionItems == null || sessionItems.Count == 0)
                return;

            var serializedSessionData = SerializeSessionData(sessionItems);

            var request = new StoreItemRequest
                              {
                                  Item = new CacheItem
                                             {
                                                 ExpirationPolicy = ExpirationPolicy.Sliding,
                                                 ExpiryInterval = (uint)Timeout,
                                                 Key = SessionKey,
                                                 Scope = UserSessionCacheScope,
                                                 StringData = serializedSessionData,
                                             }
                              };

            if(EnableAsync)
            {
                SessionCacheService.BeginStoreItem(ControlDataManager.Convert(ControlData), request,
                    result => HandleAsyncGatewayResponse(result, SaveSessionErrorMessage), null);
            }
            else
            {
                var response = SessionCacheService.StoreItem(ControlDataManager.Convert(ControlData), request);
                HandleGatewayResponse(response, SaveSessionErrorMessage);
            }
        }

        public override bool SetItemExpireCallback(SessionStateItemExpireCallback expireCallback)
        {
            throw new NotImplementedException();
        }


        private static SessionStateItemCollection DeserializeSessionData(string sessionData)
        {
            try
            {
                using (var stream = new MemoryStream(Convert.FromBase64String(sessionData)))
                {
                    var sessionItems = new SessionStateItemCollection();

                    if (stream.Length > 0)
                    {
                        using(var reader = new BinaryReader(stream))
                            sessionItems = SessionStateItemCollection.Deserialize(reader);
                    }

                    return sessionItems;
                }
            }
            catch (Exception ex)
            {
                // TODO: Should this not swallow and rethrow instead?
                Log.Warn("Error deserializing user session", ex);
                return new SessionStateItemCollection();
            }
        }

        private static HttpStaticObjectsCollection GetHttpStaticObjectsCollection(HttpContext context)
        {
            var httpStaticObjectsCollection = (context == null) ? new HttpStaticObjectsCollection() : SessionStateUtility.GetSessionStaticObjects(context);

            return httpStaticObjectsCollection;
        }

        private void HandleAsyncGatewayResponse(IAsyncResult result, string message)
        {
            HandleGatewayResponse(result as ServiceResponse, message);
        }

        private void HandleGatewayResponse(ServiceResponse response, string message)
        {
            if (response.rc == 0)
                return;

            Log.Warn(message, new DowJonesUtilitiesException(response.rc));
                
            if (_onSaveError != null)
                _onSaveError(this, EventArgs.Empty);
        }

        private static string SerializeSessionData(SessionStateItemCollection sessionItems)
        {
            if (sessionItems == null)
                return string.Empty;

            using(var ms = new MemoryStream())
            using(var writer = new BinaryWriter(ms))
            {
                sessionItems.Serialize(writer);

                writer.Close();

                return Convert.ToBase64String(ms.ToArray());
            }
        }
    }
}
