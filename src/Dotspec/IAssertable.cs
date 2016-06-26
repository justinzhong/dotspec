using System;

namespace Dotspec
{
    public interface IAssertable
    {
        void Assert();
    }

    public interface IAssertable<TSubject>
        where TSubject : class
    {
        void Assert(TSubject subject);

        //void Assert<TException>(TSubject subject, string exceptionMessage)
        //    where TException : Exception;
    }
}
