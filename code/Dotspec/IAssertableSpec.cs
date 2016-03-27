namespace Dotspec
{
    public interface IAssertableSpec<TSubject>
        where TSubject : class
    {
        void Assert(TSubject subject);
    }
}
