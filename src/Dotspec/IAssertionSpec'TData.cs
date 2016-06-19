using System;

namespace Dotspec
{
    public interface IAssertionSpec<TSubject, TData> : IAssertionSpec<TSubject>
        where TSubject : class
    {
        void Assert(Func<TData, TSubject> subjectConstructor);

        void Assert<TException>(Func<TData, TSubject> subjectConstructor, string errorMessage)
            where TException : Exception;
    }
}