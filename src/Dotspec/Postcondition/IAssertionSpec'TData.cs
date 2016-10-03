using System;

namespace Dotspec
{
    public interface IAssertionSpec<TSubject, TData>
        where TSubject : class
    {
        IAssertable<TSubject, TData> Then(Action<TSubject, TData> assertion);
    }
}