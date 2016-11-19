using System;

namespace Dotspec
{
    public interface IAssertionSpec<TSubject, TData>
        where TSubject : class
    {
        ISubjectSpec<TSubject, TData> Then(Action<TData> assertion);

        ISubjectSpec<TSubject, TData> Then(Action<TSubject, TData> assertion);
    }
}