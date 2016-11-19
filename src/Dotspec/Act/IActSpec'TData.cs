using System;

namespace Dotspec
{
    public interface IActSpec<TSubject, TData>
        where TSubject : class
    {
        IAssertionSpec<TSubject, TData> When(Action<TSubject, TData> behaviour);
    }
}