using System;
using DowJones.Utilities.OperationalData.EntryPoint;

namespace DowJones.Utilities.OperationalData.Article
{
    public class EmailFullTextOperationalData : AbstractOperationalData
    {
        /// <summary>
        /// Gets or sets the article origin (from where the article was viewed). This is required.
        /// </summary>
        /// <value>The article origin.</value>
        public OriginType Origin
        {
            get
            {
                return EnumMapper.MapStringToOriginType(Get(ODSConstants.KEY_ORIGIN));
            }
            set
            {
                Add(ODSConstants.KEY_ORIGIN, EnumMapper.MapOriginTypeToString(value));
            }
        }

        /// <summary>
        /// Gets or sets the article origin details (from where the article was viewed).
        /// </summary>
        /// <value>The article origin details.</value>
        public string OriginDetails
        {
            get { return Get(ODSConstants.KEY_ORIGIN_ADDITIONAL); }
            set { Add(ODSConstants.KEY_ORIGIN_ADDITIONAL, value); }
        }

        /// <summary>
        /// Gets or sets the article destination details).
        /// </summary>
        /// <value>The article destination details.</value>
        public DestDetailsOption DestDetails
        {
            get { return (DestDetailsOption) Enum.Parse(typeof (DestDetailsOption), Get(ODSConstants.KEY_DEST_DETAIL)); }  
            set { Add(ODSConstants.KEY_DEST_DETAIL, value.ToString()); }
        }

        /// <summary>
        /// Gets or sets the source code. This is required.
        /// </summary>
        /// <value>The source code.</value>
        public string SourceCode
        {
            get { return Get(ODSConstants.KEY_SOURCE_CODE); }
            set { Add(ODSConstants.KEY_SOURCE_CODE, value);
            }
        }

        /// Gets or sets the article view destination (in product or newsletter html). This is auto-populated by ODS program.
        /// </summary>
        /// <value>The article view destination.</value>
        public ViewDestination Destination
        {
            get
            {
                return EnumMapper.MapStringToViewDestination(Get(ODSConstants.KEY_VIEW_DESTINATION));
            }
            set
            {
                Add(ODSConstants.KEY_VIEW_DESTINATION, EnumMapper.MapViewDestinationToString(value));
            }
        }

        /// <summary>
        /// Gets or sets the article display format. This is auto-populated by ODS program.
        /// </summary>
        /// <value>The article display format.</value>
        public DisplayFormatType DisplayFormat
        {
            get
            {
                return EnumMapper.MapStringToDisplayFormatType(Get(ODSConstants.KEY_DISPLAY_FORMAT));
            }
            set
            {
                Add(ODSConstants.KEY_DISPLAY_FORMAT, EnumMapper.MapDisplayFormatTypeToString(value));
            }
        }

        /// <summary>
        /// Gets or sets the flag if the search successful. This is auto-populated by ODS program.
        /// </summary>
        /// <value>Is the search successful.</value>
        public bool IsSuccessful
        {
            get
            {
                string v = Get(ODSConstants.KEY_IS_SUCCESSFUL);
                return (v != null && v == "S");
            }
            set
            {
                string v = "F";
                if (value)
                {
                    v = "S";
                }
                Add(ODSConstants.KEY_IS_SUCCESSFUL, v);
            }
        }
    }
}