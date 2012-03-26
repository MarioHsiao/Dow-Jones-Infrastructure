namespace DowJones.Tools.Ajax
{
    public interface IAjaxResponseDelegate
    {
        long ReturnCode { get; set; }
        long ElapsedTime { get; set; }
        string StatusMessage { get; set; }
    }

    public interface IAjaxDelegate
    {
        
    }

    public interface IAjaxRequestDelegate
    {
        
    }
}