using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DowJones.Utilities.OperationalData;
using DowJones.Utilities.OperationalData.AssetActivity;

namespace DowJones.Utilities.OperationalData.ErrorData
{
    public class ODSErrorOperationalDataCapture : AbstractOperationalData
    {
        private BaseCommonRequestOperationalData _commonOperationalData;

        public string ErrorCode
        {
            get { return Get(ODSConstants.KEY_ERROR_CODE); }
            set { Add(ODSConstants.KEY_ERROR_CODE, value); }
        }
        public string ErrorDesc
        {
            get { return Get(ODSConstants.KEY_ERROR_DESC); }
            set { Add(ODSConstants.KEY_ERROR_DESC, value); }
        }
        public string StackTrace
        {
            get { return Get(ODSConstants.KEY_STACK_TRACE); }
            set { Add(ODSConstants.KEY_STACK_TRACE, value); }
        }
        public string OcpFunctionCode
        {
            get { return Get(ODSConstants.KEY_OCP_FUNCTION_CODE); }
            set { Add(ODSConstants.KEY_OCP_FUNCTION_CODE, value); }
        }
        public string AppFileName
        {
            get { return Get(ODSConstants.KEY_APP_FILE_NAME); }
            set { Add(ODSConstants.KEY_APP_FILE_NAME, value); }
        }
        public string AppFunctionName
        {
            get { return Get(ODSConstants.KEY_APP_FUNCTION_NAME); }
            set { Add(ODSConstants.KEY_APP_FUNCTION_NAME, value); }
        }
        public string AppFileLineNumber
        {
            get { return Get(ODSConstants.KEY_APP_FILE_LINE_NUMBER); }
            set { Add(ODSConstants.KEY_APP_FILE_LINE_NUMBER, value); }
        }
        public string RequestorIP
        {
            get { return Get(ODSConstants.KEY_REQUESTOR_IP); }
            set { Add(ODSConstants.KEY_REQUESTOR_IP, value); }
        }
        public string WebServerName
        {
            get { return Get(ODSConstants.KEY_WEB_SERVER_NAME); }
            set { Add(ODSConstants.KEY_WEB_SERVER_NAME, value); }
        }
        public string ReqDateTime
        {
            get { return Get(ODSConstants.KEY_REQ_DATE_TIME); }
            set { Add(ODSConstants.KEY_REQ_DATE_TIME, value); }
        }
        public string RespDateTime
        {
            get { return Get(ODSConstants.KEY_RESP_DATE_TIME); }
            set { Add(ODSConstants.KEY_RESP_DATE_TIME, value); }
        }
        public string ErrorLogTime
        {
            get { return Get(ODSConstants.KEY_ERROR_LOG_TIME); }
            set { Add(ODSConstants.KEY_ERROR_LOG_TIME, value); }
        }
        public string BackEndTranName
        {
            get { return Get(ODSConstants.KEY_BACK_END_TRAN_NAME); }
            set { Add(ODSConstants.KEY_BACK_END_TRAN_NAME, value); }
        }
        //public string BackEndServerIP
        //{
        //    get { return Get("BackEndServerIP"); }
        //    set { Add("BackEndServerIP", value); }
        //}
        public string GtwTranName
        {
            get { return Get(ODSConstants.KEY_GTW_TRAN_NAME); }
            set { Add(ODSConstants.KEY_GTW_TRAN_NAME, value); }
        }
        //public string ContentServerAddress
        //{
        //    get { return Get("ContentServerAddress"); }
        //    set { Add("ContentServerAddress", value); }
        //}
        //public string TransactionType
        //{
        //    get { return Get("TransactionType"); }
        //    set { Add("TransactionType", value); }
        //}
        public string GtwTranNameSpace
        {
            get { return Get(ODSConstants.KEY_GTW_TRAN_NAMESPACE); }
            set { Add(ODSConstants.KEY_GTW_TRAN_NAMESPACE, value); }
        }
        public string ReqCookiesCnt
        {
            get { return Get(ODSConstants.REQ_COOKIES_CNT); }
            set { Add(ODSConstants.REQ_COOKIES_CNT, value); }
        }
        public string ReqQueryStringCnt
        {
            get { return Get(ODSConstants.REQ_QUERY_STRING_CNT); }
            set { Add(ODSConstants.REQ_QUERY_STRING_CNT, value); }
        }
        public string FormCount
        {
            get { return Get(ODSConstants.FORM_CNT); }
            set { Add(ODSConstants.FORM_CNT, value); }
        }
        public string ODSTranName
        {
            get { return Get(ODSConstants.FCS_ODS_TRANS_NAME); }
            set { Add(ODSConstants.FCS_ODS_TRANS_NAME, value); }
        }
        public string AddnlInfo
        {
            get { return Get(ODSConstants.KEY_ADDNL_INFO); }
            set { Add(ODSConstants.KEY_ADDNL_INFO, value); }
        }
        public string UserAcctNum
        {
            get { return Get(ODSConstants.KEY_USER_ACCT_NUM); }
            set { Add(ODSConstants.KEY_USER_ACCT_NUM, value); }
        }
        /*Start: Infosys*/
        /*05/03/2010- Added FormMethod parameter*/
        public string FormMethod
        {
            get { return Get(ODSConstants.KEY_ODS_FORM_METHOD); }
            set { Add(ODSConstants.KEY_ODS_FORM_METHOD, value); }
        }
        /*End: Infosys*/
        public BaseCommonRequestOperationalData CommonOperationalData
        {
            get
            {
                if (_commonOperationalData == null)
                {
                    _commonOperationalData = new BaseCommonRequestOperationalData(List);
                }
                return _commonOperationalData;
            }
        }

        public ODSErrorOperationalDataCapture()
        {

        }
        protected ODSErrorOperationalDataCapture(IDictionary<string, string> list)
            : base(list)
        {
        }
    }
}
