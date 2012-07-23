// -----------------------------------------------------------------------
// <copyright file="AbstractMarketWatchService.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.ServiceModel;
using System.ServiceModel.Channels;
using DowJones.Managers.Abstract;
using DowJones.Properties;

namespace DowJones.Managers.MarketWatch.Core
{
    
    public abstract class AbstractMarketWatchService<TChannel> : IExternalService
    where TChannel : class
    {
        private const string HeaderNamespace = "http://service.dowjones.com/ws/2010/11/entitlement";
        private static readonly string HeaderValue = Settings.Default.ThunderBallEntitlementToken;
        private const string HeaderName = "EntitlementToken";
        private static readonly MessageHeader MessageHeader = MessageHeader.CreateHeader(HeaderName, HeaderNamespace, HeaderValue);

        public virtual void AddEntitlementToken(ClientBase<TChannel> client, Action processingFunction)
        {
            using (new OperationContextScope(client.InnerChannel))
            {
                OperationContext.Current.OutgoingMessageHeaders.Add(MessageHeader);
                processingFunction();
            }
        }
    }
}
