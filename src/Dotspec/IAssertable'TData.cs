using System;

namespace Dotspec
{
    public interface IAssertable<TSubject, TData>
        where TSubject : class
    {
        void Assert(Func<TData, TSubject> subjectConstructor);
    }
}