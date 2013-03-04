﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DowJones.Loggers;
using log4net;
using DowJones.Managers.Abstract;
using Factiva.Gateway.Messages.Assets.Lists.V1_0;
using Factiva.Gateway.Services.V1_0;
using Factiva.Gateway.V1_0;
using Factiva.Gateway.Utils.V1_0;
using DowJones.Extensions;
using DowJones.Session;
using FactivaControlData = Factiva.Gateway.Utils.V1_0.ControlData; //DowJones.Session.ControlData ambigous with Factiva.Gateway.Utils.V1_0, giving an alias.

namespace DowJones.Managers.SocialMedia.Config
{
    public class PAMSocialMediaIndustryProvider : AbstractAggregationManager, ISocialMediaIndustryProvider
    {
        IDictionary<string, string> _mappings;
        public PAMSocialMediaIndustryProvider(IControlData controlData, ITransactionTimer transactionTimer)
            : base(controlData, transactionTimer)
        {
            _mappings = LoadMappings();
        }

        private IDictionary<string, string> LoadMappings()
        {
            GetListsDetailsListResponse socialmediaresponse = GetListsDetailsListResponse();
            _mappings = new Dictionary<string, string>();
            if (socialmediaresponse != null){
                foreach (var listdetailitem in socialmediaresponse.ListDetailsItems)
                {
                    foreach (var item in listdetailitem.List.Items)
                    {
                        var channelmapitem = (SocialMediaChannelMappingListItem)item;
                        if (channelmapitem != null) {
                            _mappings.Add(channelmapitem.ChannelMap.Code , channelmapitem.ChannelMap.Channel);
                        }
                    }
                }
            }
            return _mappings;
        }
        
        private GetListsDetailsListResponse GetListsDetailsListResponse()
        {
            //Create a request based on the type.
            GetListsDetailsListRequest socialMediaListRequest = new GetListsDetailsListRequest();
            socialMediaListRequest.ListTypes.Add(ListType.SocialMediaChannelMappingList);
            return Process<GetListsDetailsListResponse>(socialMediaListRequest);
        } 

        public string GetChannelFromIndustryCode(string industrycode)
        {
            var code = industrycode.ToLower();
            if (!(_mappings.ContainsKey(code)))
                throw new ArgumentOutOfRangeException("code", industrycode, "Industry code not found.");
            return _mappings[code];    
        }

        protected override ILog Log
        {
            get { return LogManager.GetLogger("SocialMediaIndustryPAMProvider"); }
        }

    }
}
