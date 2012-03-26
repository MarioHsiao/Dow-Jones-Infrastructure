using System;
using Newtonsoft.Json;
using DowJones.Web.Mvc.UI.Components.Common.Types;
using DowJones.Web.Mvc.UI.Components.HeadlineList;
using System.Collections.Generic;
using Factiva.Gateway.Messages.Track.V1_0;
using Track = Factiva.Gateway.Messages.Track.V1_0;
using DowJones.Utilities.Search.Core;
using DowJones.Managers.Search.CodedNewsQueries;
using DowJones.Globalization;

namespace DowJones.Web.Mvc.UI.Components.Models
{
    public class Option
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("optGroup")]
        public bool OptGroup { get; set; }

        [JsonProperty("selected")]
        public bool Selected { get; set; }
    }

    public class SimpleAlertData
    {
        #region ..:: Private Members ::..
        ResourceTextManager _rt = ResourceTextManager.Instance;
        DocumentFormat _emailFormat = DocumentFormat.TextPlain;
        DeliveryTimes _deliveryTime = Track.DeliveryTimes.None;
        RemoveDuplicate _duplicate = RemoveDuplicate.None;
        List<Option> _deliveryTimes = null;
        List<Option> _emailFormats = null;
        List<Option> _duplicates = null;
        #endregion

        #region ..:: Public Members ::..

        /// <summary>
        /// Gets or sets the Source List.
        /// </summary>
        [JsonProperty("sourceList")]
        public List<Option> SourceList { get; set; }

        /// <summary>
        /// Gets or sets the selected Source value.
        /// </summary>
        [JsonProperty("selectedSource")]
        public string SelectedSource { get; set; }

        /// <summary>
        /// Gets or sets the selected Source Description.
        /// </summary>
        [JsonProperty("selectedSourceDesc")]
        public string SelectedSourceDesc { get; set; }

        /// <summary>
        /// Gets or sets the SearchText.
        /// </summary>
        [JsonProperty("searchText")]
        public string SearchText { get; set; }

        /// <summary>
        /// Gets or sets the Email Address.
        /// </summary>
        [JsonProperty("emailAddress")]
        public string EmailAddress { get; set; }

        /// <summary>
        /// Gets or Sets the email formats
        /// </summary>
        [JsonProperty("emailFormats")]
        public List<Option> EmailFormats
        {
            get
            {
                if (_emailFormats == null)
                {
                    _emailFormats = new List<Option>();
                    _emailFormats.Add(new Option { Id = ((int)DocumentFormat.TextPlain).ToString(), Name = _rt.GetString("plainText") });
                    _emailFormats.Add(new Option { Id = ((int)DocumentFormat.TextHtml).ToString(), Name = _rt.GetString("html") });
                }
                return _emailFormats;
            }
            set { _emailFormats = value; }
        }

        /// <summary>
        /// Gets or Sets the selected EmailFormat
        /// </summary>
        [JsonProperty("selectedEmailFormat")]
        public DocumentFormat SelectedEmailFormat
        {
            get { return _emailFormat; }
            set { _emailFormat = value; }
        }

        /// <summary>
        /// Gets the Delivery Time
        /// </summary>
        [JsonProperty("deliveryTimes")]
        public List<Option> DeliveryTimes
        {
            get
            {
                if (_deliveryTimes == null)
                {
                    _deliveryTimes = new List<Option>();
                    _deliveryTimes.Add(new Option { Id = ((int)Track.DeliveryTimes.None).ToString(), Name = _rt.GetString("none") });
                    _deliveryTimes.Add(new Option { Id = ((int)Track.DeliveryTimes.Morning).ToString(), Name = _rt.GetString("morning") });
                    _deliveryTimes.Add(new Option { Id = ((int)Track.DeliveryTimes.Afternoon).ToString(), Name = _rt.GetString("afternoon") });
                    _deliveryTimes.Add(new Option { Id = ((int)Track.DeliveryTimes.Both).ToString(), Name = _rt.GetString("mornAndAft") });
                    _deliveryTimes.Add(new Option { Id = ((int)Track.DeliveryTimes.Continuous).ToString(), Name = _rt.GetString("continuous") });
                }
                return _deliveryTimes;
            }
            set { _deliveryTimes = value; }
        }

        /// <summary>
        /// Gets or Sets the selected DeliveryTime
        /// </summary>
        [JsonProperty("selectedDeliveryTime")]
        public DeliveryTimes SelectedDeliveryTime
        {
            get { return _deliveryTime; }
            set { _deliveryTime = value; }
        }

        /// <summary>
        /// Gets the Delivery Time
        /// </summary>
        [JsonProperty("duplicates")]
        public List<Option> Duplicates
        {
            get
            {
                if (_duplicates == null)
                {
                    _duplicates = new List<Option>();
                    _duplicates.Add(new Option { Id = ((int)RemoveDuplicate.None).ToString(), Name = _rt.GetString("offRefresh") });
                    _duplicates.Add(new Option { Id = ((int)RemoveDuplicate.High).ToString(), Name = _rt.GetString("virtualyIdenticalRefresh") });
                    _duplicates.Add(new Option { Id = ((int)RemoveDuplicate.Medium).ToString(), Name = _rt.GetString("similarRefresh") });
                }
                return _duplicates;
            }
            set { _duplicates = value; }
        }

        /// <summary>
        /// Gets or Sets the selected DeliveryTime
        /// </summary>
        [JsonProperty("selectedDuplicate")]
        public RemoveDuplicate SelectedDuplicate
        {
            get { return _duplicate; }
            set { _duplicate = value; }
        }

        /// <summary>
        /// Gets or Sets the news filter
        /// </summary>
        [JsonProperty("newsFilter")]
        public List<NewsFilterCollection> NewsFilter { get; set; }

        /// <summary>
        /// Gets or Sets the alert name
        /// </summary>
        [JsonProperty("alertName")]
        public string AlertName { get; set; }

        #endregion
    }

    public class SimpleAlertModel : ViewComponentModel
    {
        #region ..:: Private Members ::..
        ResourceTextManager _rt = ResourceTextManager.Instance;
        SimpleAlertData _data = null;
        #endregion

        #region ..:: Client Properties ::..

        /// <summary>
        /// Gets or sets the selected Source index.
        /// </summary>
        [ClientProperty("title")]
        public string Title { get; set; }

        #endregion

        #region ..:: Client Data ::..

        [ClientData]
        public SimpleAlertData Data
        {
            get
            {
                if (_data == null)
                {
                    _data = new SimpleAlertData();
                }
                else
                {
                    SimpleAlertData tempData = new SimpleAlertData();
                    if (_data.EmailFormats == null)
                    {
                        _data.EmailFormats = tempData.EmailFormats;
                        _data.SelectedEmailFormat = tempData.SelectedEmailFormat;
                    }

                    if (_data.Duplicates == null)
                    {
                        _data.Duplicates = tempData.Duplicates;
                        _data.SelectedDuplicate = tempData.SelectedDuplicate;
                    }

                    if (_data.DeliveryTimes == null)
                    {
                        _data.DeliveryTimes = tempData.DeliveryTimes;
                        _data.SelectedDeliveryTime = tempData.SelectedDeliveryTime;
                    }
                }

                return _data;
            }
            set
            {
                _data = value;
            }
        }

        #endregion

        #region ..:: Client Event Handlers ::..

        /// <summary>
        /// Gets or sets the client side OnSave event handler.
        /// </summary>
        [ClientEventHandler("dj.SimpleAlert.save")]
        public string OnSave { get; set; }

        #endregion

        #region ..:: Constructor ::..

        public SimpleAlertModel()
        {

        }

        #endregion
    }
}
