using System;

namespace Dotspec
{
    public interface IAssertionSpec<TSubject>
        where TSubject : class
    {
        void Assert(TSubject subject);

        void Assert<TException>(TSubject subject, string exceptionMessage)
            where TException : Exception;
    }
}
