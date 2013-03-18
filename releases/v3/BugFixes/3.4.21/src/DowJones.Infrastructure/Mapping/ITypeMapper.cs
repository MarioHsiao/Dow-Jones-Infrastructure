namespace DowJones.Mapping
{
    public interface ITypeMapper
    {
        object Map(object source);
    }

    public interface ITypeMapper<in TSource, out TTarget> : ITypeMapper
    {
        TTarget Map(TSource source);
    }
}