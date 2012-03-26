using System;
using System.Collections.Generic;
using DowJones.Tools.Ajax.HeadlineList;
using DowJones.Utilities.Formatters;
using Factiva.Gateway.Messages.Trigger.V1_1;
using Factiva.Gateway.Messages.Trigger.V2_0;

namespace DowJones.Tools.Ajax.TriggerList
{
// ReSharper disable InconsistentNaming
    /// <summary>
    /// 
    /// </summary>
    public class DetectionInfo
    {
        public int startIndex;
        public int endIndex;
        public string value;
    }

    /// <summary>
    /// 
    /// </summary>
    public class PropertyInfo
    {
        public string code;
        public string displayName;
        public string type;
        public string externalUri;
    }

    /// <summary>
    /// 
    /// </summary>
    public class GenericPropertyInfo
    {
        public string displayName;
        public string value;
        public bool isMostRecent;
    }

    /// <summary>
    /// 
    /// </summary>
    public enum CategoryType
    {
        Master,
        Instance
    }

    public class DetectionPhrase
    {
        private int __startIndex;

        public int StartIndex
        {
            get { return __startIndex; }
            set { __startIndex = value; }
        }

        private int __endIndex;


        public int EndIndex
        {
            get { return __endIndex; }
            set { __endIndex = value; }
        }

        private string __value;

        public string Value
        {
            get { return __value; }
            set { __value = value; }
        }
    }
    /// <summary>
    /// 
    /// </summary>
    public class TriggerInfo
    {

        /// <summary>
        /// 
        /// </summary>
        public long id;

        /// <summary>
        /// 
        /// </summary> 
        public string externalUri;

        /// <summary>
        /// 
        /// </summary>
        public bool selected;

        /// <summary>
        /// 
        /// </summary>
        public WholeNumber index;

        /// <summary>
        /// 
        /// </summary>
        public string displayName;

        /// <summary>
        /// 
        /// </summary>
        public string confidence;

        /// <summary>
        /// 
        /// </summary>
        public DateTime lowerDate;

        /// <summary>
        /// 
        /// </summary>
        public string lowerDateDescriptor;

        /// <summary>
        /// 
        /// </summary>
        public DateTime upperDate;

        /// <summary>
        /// 
        /// </summary>
        public string upperDateDescriptor;

        /// <summary>
        /// 
        /// </summary>
        public DateTime createDateTime;

        /// <summary>
        /// 
        /// </summary>
        public string createDateTimeDescriptor;

        /// <summary>
        /// 
        /// </summary>
        public DateTime createDate;

        /// <summary>
        /// 
        /// </summary>
        public string createDateDescriptor;

        /// <summary>
        /// 
        /// </summary>
        public DateTime modifiedDateTime;

        /// <summary>
        /// 
        /// </summary>
        public string modifiedDateTimeDescriptor;

        /// <summary>
        /// 
        /// </summary>
        public DateTime modifiedDate;

        /// <summary>
        /// 
        /// </summary>
        public string modifiedDateDescriptor;

        /// <summary>
        /// 
        /// </summary>
        public string typeCode;

        /// <summary>
        /// 
        /// </summary>
        public CategoryType category;

        /// <summary>
        /// 
        /// </summary>
        public string categoryDescriptor;

        /// <summary>
        /// 
        /// </summary>
        public HeadlineListDataResult masterHeadlineDataResult;

        /// <summary>
        /// 
        /// </summary>
        public List<DetectionPhrase> detectionPhrase;

        /// <summary>
        /// 
        /// </summary>
        public List<PropertyInfo> companies;

        /// <summary>
        /// 
        /// </summary>
        public List<PropertyInfo> executives;

        /// <summary>
        /// 
        /// </summary>
        public List<GenericPropertyInfo> additionalDetails;
    }
   
}
