namespace Dotspec
{
    public interface ISubjectSpec<TSubject>
        where TSubject : class
    {
        void For(TSubject subject);

        //void Assert<TException>(TSubject subject, string exceptionMessage)
        //    where TException : Exception;
    }
}
