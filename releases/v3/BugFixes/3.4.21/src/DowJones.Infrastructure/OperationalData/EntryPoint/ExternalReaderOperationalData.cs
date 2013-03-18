namespace DowJones.OperationalData.EntryPoint
{
    public class ExternalReaderOperationalData : AbstractOperationalData
    {
        /// <summary>
        /// Email address of the reader, not the bill to user
        /// </summary>
        public string Email
        {
            get { return Get(ODSConstants.KEY_EMAIL); }
            set { Add(ODSConstants.KEY_EMAIL, value); }
        }

        /// <summary>
        /// 
        /// </summary>
        public string FirstName
        {
            get { return Get(ODSConstants.KEY_FIRST_NAME); }
            set { Add(ODSConstants.KEY_FIRST_NAME, value); }
        }

        /// <summary>
        /// 
        /// </summary>
        public string LastName
        {
            get { return Get(ODSConstants.KEY_LAST_NAME); }
            set { Add(ODSConstants.KEY_LAST_NAME, value); }
        }

        /// <summary>
        /// 
        /// </summary>
        public string CompanyName
        {
            get { return Get(ODSConstants.KEY_COMPANY_NAME); }
            set { Add(ODSConstants.KEY_COMPANY_NAME, value); }
        }
    }
}