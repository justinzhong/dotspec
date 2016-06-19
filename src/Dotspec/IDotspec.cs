namespace Dotspec
{
    public interface IDotspec<TSubject> : IPreconditionSpec<TSubject>, IAdditionalPreconditionSpec<TSubject>, IAssertionSpec<TSubject>
        where TSubject : class
    {

    }
}
