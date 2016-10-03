using System;

namespace Dotspec
{
    public interface IResultAssertable<TSubject, TData> where TSubject : class
    {
        void Assert(Func<TData, TSubject> subjectConstructor);
    }
}