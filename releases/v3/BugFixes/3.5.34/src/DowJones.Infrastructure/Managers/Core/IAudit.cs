namespace DowJones.Managers.Core
{
    public interface IAudit
    {
        long ElapsedTime { get; set; }

        long ReturnCode { get; set; }
    }
}