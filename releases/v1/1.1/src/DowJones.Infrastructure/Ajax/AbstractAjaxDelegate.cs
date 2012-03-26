namespace DowJones.Tools.Ajax
{
    public abstract class AbstractAjaxResponseDelegate : IAjaxResponseDelegate, IAjaxDelegate
    {
        #region IAjaxResponseDelegate Members

        public long ReturnCode { get; set; }

        public string StatusMessage { get; set; }

        public string ExceptionMessage { set; get; }

        public long ElapsedTime { get; set; }

        #endregion
    }
}
