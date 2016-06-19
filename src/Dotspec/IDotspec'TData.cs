namespace Dotspec
{
    public interface IDotspec<TSubject, TData> : IPreconditionSpec<TSubject>, IAdditionalPreconditionSpec<TSubject, TData>, IAssertionSpec<TSubject, TData>
        where TSubject : class
    {

    }
}