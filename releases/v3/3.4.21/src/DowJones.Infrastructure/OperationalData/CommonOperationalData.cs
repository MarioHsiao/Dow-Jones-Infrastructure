using System;
using System.Collections.Generic;
using System.Web;

namespace DowJones.OperationalData
{
    public class CommonOperationalData : UserInterfaceAndDeviceOperationalData
    {
        private string _actionType;
        private string _entryPoint;
        
        /// <summary>
        /// This is required
        /// </summary>
        public string EntryPoint
        {
            get { return _entryPoint; }
            set
            {
                _entryPoint = value;
            }
        }

        public string ActionType
        {
            get { return _actionType; }
            set
            {
                string tempValue = value;
                tempValue = ValidateAndCleanActionType(tempValue);
                _actionType = tempValue;
            }
        }

        /// <summary>
        /// UI Hack/Validation to avoid passing invalid action type
        /// </summary>
        /// <param name="rawActionType"></param>
        /// <returns></returns>
        private static string ValidateAndCleanActionType(string rawActionType)
        {
            //This should not happen but incase!
            if (String.IsNullOrEmpty(rawActionType))
            {
                return null;
            }
            rawActionType = rawActionType.Trim();
            rawActionType = HttpUtility.UrlDecode(rawActionType);
            if (rawActionType.IndexOf(',') != -1)
            {
                string[] actionList = rawActionType.Split(',');
                foreach (string action in actionList)
                {
                    if (String.IsNullOrEmpty(action))
                        continue;
                    rawActionType = action;
                    break;
                }
            }
            return rawActionType;
        }

        public CommonOperationalData()
        {
            
        }

        protected CommonOperationalData(IDictionary<string, string> list)
            : base(list)
        {
        }

        #region IBaseOperationalData Members

        public override IDictionary<string, string> GetKeyValues
        {
            get
            {
                Dictionary<string, string> tempList = new Dictionary<string, string>();
                if (_entryPoint != null)
                {
                    tempList.Add(ODSConstants.ODS_PREFIX_FOR_KEY + "EP", _entryPoint.ToUpper());
                }
                if (_actionType != null)
                {
                    tempList.Add(ODSConstants.ODS_PREFIX_FOR_KEY + "ACTION", _actionType.ToUpper());
                }

                IEnumerator<KeyValuePair<string, string>> enumerator = base.GetKeyValues.GetEnumerator();
                while (enumerator.MoveNext())
                {
                    tempList.Add(enumerator.Current.Key, enumerator.Current.Value);
                }
                return tempList;
            }
        }

        #endregion
    }
}