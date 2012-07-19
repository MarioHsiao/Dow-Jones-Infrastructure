using System;
using System.Collections.Generic;
using System.Text;
using DowJones.OperationalData.EntryPoint;

namespace DowJones.OperationalData.Article
{
    public class BaseArticleOperationalData : AbstractOperationalData
    {
        private bool _originSpecified;
        private bool _successIndSpecified;

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
                _originSpecified = true;
            }
        }

        /// <summary>
        /// Gets or sets the auto completed term.
        /// </summary>
        /// <value>The auto completed term.</value>
        public AutoCompletedTerm AutoCompletedTerm
        {
            get
            {
                return EnumMapper.MapAutoCompletedTerm(Get(ODSConstants.KEY_VIEW_AUTO_COMPLETED_TERM));
            }
            set
            {
                Add(ODSConstants.KEY_VIEW_AUTO_COMPLETED_TERM, EnumMapper.MapAutoCompletedTermToString(value));
            }
        }



        /// <summary>
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
        /// Gets or sets the article view for post processing.
        /// </summary>
        /// <value>The article view for post processing.</value>
        public PostProcessingType PostProcessing
        {
            get
            {
                return EnumMapper.MapStringToPostProcessingType(Get(ODSConstants.KEY_POST_PROCESSING));
            }
            set
            {
                Add(ODSConstants.KEY_POST_PROCESSING, EnumMapper.MapPostProcessingTypeToString(value));
            }
        }

        /// <summary>
        /// Gets or sets the article view for post processing details.
        /// </summary>
        /// <value>The article view for post processing details.</value>
        public PostProcessingAdditional PostProcessingDetails
        {
            get
            {
                return EnumMapper.MapStringToPostProcessingAdditional(Get(ODSConstants.KEY_POST_PROCESSING_ADDITIONAL));
            }
            set
            {
                Add(ODSConstants.KEY_POST_PROCESSING_ADDITIONAL, EnumMapper.MapPostProcessingAdditionalToString(value));
            }
        }

        /// <summary>
        /// Gets or sets the article display format.
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
        /// Gets or sets the source code.
        /// </summary>
        /// <value>The source code.</value>
        public string[] SourceCode
        {
            get
            {
                string v = Get(ODSConstants.KEY_SOURCE_CODE);
                return v != null ? v.Split(new char[] { MultipleArtValueSpilter }) : null;
            }
            set
            {
                if (value != null)
                {
                    StringBuilder sb = new StringBuilder();
                    foreach (string v in value)
                    {
                        sb.Append(v).Append(MultipleArtValueSpilter);
                    }
                    if (sb.Length > 0)
                    {
                        sb.Remove(sb.Length - 1, 1);
                        Add(ODSConstants.KEY_SOURCE_CODE, sb.ToString());
                    }
                }
            }
        }
        
        /// <summary>
        /// Gets or sets the flag if the search successful. This is required.
        /// </summary>
        /// <value>Is the search successful.</value>
        public bool[] IsSuccessful
        {
            get
            {
                List<bool> list = new List<bool>();
                
                string v = Get(ODSConstants.KEY_IS_SUCCESSFUL);
                if (v == null)
                    return null;

                string[] savedValue = v.Split(new char[] { MultipleArtValueSpilter });
                if (savedValue.Length > 0)
                {
                    foreach(string str in savedValue)
                    {
                        if (str != null && v == "S")
                            list.Add(true);
                        else
                            list.Add(false);
                    }
                    return list.ToArray();
                }
                return null;
            }
            set
            {
                if (value != null)
                {
                    StringBuilder sb = new StringBuilder();
                    foreach (bool v in value)
                    {
                        string saveValue = "F";
                        if (v)
                        {
                            saveValue = "S";
                        }
                        sb.Append(saveValue).Append(MultipleArtValueSpilter);
                    }
                    if (sb.Length > 0)
                    {
                        sb.Remove(sb.Length - 1, 1);
                        Add(ODSConstants.KEY_IS_SUCCESSFUL, sb.ToString());
                        _successIndSpecified = true;
                    }
                }
            }
        }

        /// <summary>
        /// Gets or sets the error code.
        /// </summary>
        /// <value>The error code.</value>
        public string[] ErrorCode
        {
            get
            {
                string v = Get(ODSConstants.KEY_ERROR_CODE);
                return v != null ? v.Split(new char[] { MultipleArtValueSpilter }) : null;
            }
            set
            {
                if (value != null)
                {
                    StringBuilder sb = new StringBuilder();
                    foreach (string v in value)
                    {
                        sb.Append(v).Append(MultipleArtValueSpilter);
                    }
                    if (sb.Length > 0)
                    {
                        sb.Remove(sb.Length - 1, 1);
                        Add(ODSConstants.KEY_ERROR_CODE, sb.ToString());
                    }
                }
            }
        }
        
        public override IDictionary<string, string> GetKeyValues
        {
            get
            {
              if (!_originSpecified)
                {
                    throw new MissingFieldException("Origin value is not specified");
                }
                if (!_successIndSpecified)
                {
                    throw new MissingFieldException("Success or Failure indicator value is not specified");
                }

                return base.GetKeyValues;
            }
        }

    }
}