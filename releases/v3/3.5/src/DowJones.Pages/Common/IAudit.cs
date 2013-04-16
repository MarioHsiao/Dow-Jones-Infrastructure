namespace DowJones.Pages
{
    public interface IAudit
    {
        long ReturnCode { get; set; }

        long ElapsedTime { get; set; }
    }
}