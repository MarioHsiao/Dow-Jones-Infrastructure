namespace DowJones.Assemblers
{
    public interface IAssembler<out TTarget, in TSource>
    {
        TTarget Convert(TSource source);
    }
}

