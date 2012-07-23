namespace DowJones.Web
{
    public enum ClientResourceProcessorOrder : short
    {
        First = -1,
        Unspecified = 0,
        Last = 1,
    }

    public enum ClientResourceProcessorKind : short
    {
        Populator = -2,
        Preprocessor = -1,
        Unspecified = 0,
        Postprocessor = 1,
    }


    public interface IClientResourceProcessor
    {
        ClientResourceProcessorOrder? Order { get; }

        ClientResourceProcessorKind ProcessorKind { get; }
        
        void Process(ProcessedClientResource resource);
    }
}