using System.Collections.Generic;
using DowJones.DependencyInjection;
using DowJones.Globalization;
using DowJones.Managers.Search.CodedNewsQueries;
using DowJones.Utilities.Search.Core;
using Factiva.Gateway.Messages.Track.V1_0;
using Newtonsoft.Json;

namespace DowJones.Web.Mvc.UI.Components.SimpleAlert
{
    public class SimpleAlertData
    {
        private readonly IResourceTextManager _resources;

        private DocumentFormat _emailFormat = DocumentFormat.TextPlain;
        private DeliveryTimes _deliveryTime = Factiva.Gateway.Messages.Track.V1_0.DeliveryTimes.None;
        private RemoveDuplicate _duplicate = RemoveDuplicate.None;
        private List<Option> _deliveryTimes;
        private List<Option> _emailFormats;
        private List<Option> _duplicates;
        private List<Option> _resultsDisplayFormats;

        /// <summary>
        /// Gets or sets the flag determining whether the alert 
        /// highlighting feature is enabled
        /// </summary>
        [JsonProperty("highlightFieldEnabled")]
        public bool HighlightFieldEnabled { get; set; }

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
                    _emailFormats.Add(new Option { Id = ((int)DocumentFormat.TextPlain).ToString(), Name = _resources.GetString("plainText") });
                    _emailFormats.Add(new Option { Id = ((int)DocumentFormat.TextHtml).ToString(), Name = _resources.GetString("html") });
                }
                return _emailFormats;
            }
            set { _emailFormats = value; }
        }

        /// <summary>
        /// Gets or Sets the email formats
        /// </summary>
        [JsonProperty("resultsDisplayFormats")]
        public List<Option> ResultsDisplayFormats
        {
            get
            {
                if (_resultsDisplayFormats == null)
                {
                    _resultsDisplayFormats = new List<Option>();
                    _resultsDisplayFormats.Add(new Option { Id = ((int)ResultsDisplayFormat.HeadlinesContextual).ToString(), Name = _resources.GetString("headlinesContextual") });
                    _resultsDisplayFormats.Add(new Option { Id = ((int)ResultsDisplayFormat.HeadlinesTraditional).ToString(), Name = _resources.GetString("headlinesTraditional") });
                    _resultsDisplayFormats.Add(new Option { Id = ((int)ResultsDisplayFormat.FullText).ToString(), Name = _resources.GetString("fulltextDocs") });
                    _resultsDisplayFormats.Add(new Option { Id = ((int)ResultsDisplayFormat.FullTextIndexing).ToString(), Name = _resources.GetString("fulltextDocsIdx") });
                }
                return _resultsDisplayFormats;
            }
            set { _resultsDisplayFormats = value; }
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
                    _deliveryTimes.Add(new Option { Id = ((int)Factiva.Gateway.Messages.Track.V1_0.DeliveryTimes.None).ToString(), Name = _resources.GetString("none") });
                    _deliveryTimes.Add(new Option { Id = ((int)Factiva.Gateway.Messages.Track.V1_0.DeliveryTimes.EarlyMorning).ToString(), Name = _resources.GetString("earlymorning") });
                    _deliveryTimes.Add(new Option { Id = ((int)Factiva.Gateway.Messages.Track.V1_0.DeliveryTimes.Morning).ToString(), Name = _resources.GetString("morning") });
                    _deliveryTimes.Add(new Option { Id = ((int)Factiva.Gateway.Messages.Track.V1_0.DeliveryTimes.Afternoon).ToString(), Name = _resources.GetString("afternoon") });
                    _deliveryTimes.Add(new Option { Id = ((int)Factiva.Gateway.Messages.Track.V1_0.DeliveryTimes.Both).ToString(), Name = _resources.GetString("mornAndAft") });
                    _deliveryTimes.Add(new Option { Id = ((int)Factiva.Gateway.Messages.Track.V1_0.DeliveryTimes.Continuous).ToString(), Name = _resources.GetString("continuous") });
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
                    _duplicates.Add(new Option { Id = ((int)RemoveDuplicate.None).ToString(), Name = _resources.GetString("offRefresh") });
                    _duplicates.Add(new Option { Id = ((int)RemoveDuplicate.High).ToString(), Name = _resources.GetString("virtualyIdenticalRefresh") });
                    _duplicates.Add(new Option { Id = ((int)RemoveDuplicate.Medium).ToString(), Name = _resources.GetString("similarRefresh") });
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

        /// <summary>
        /// Get or Sets the include social media checkbox
        /// </summary>
        [JsonProperty("includeSocialMedia")]
        public bool IncludeSocialMedia { get; set; }

        public SimpleAlertData(IResourceTextManager resources = null)
        {
            _resources = resources ?? ServiceLocator.Resolve<IResourceTextManager>();
        }
    }
}