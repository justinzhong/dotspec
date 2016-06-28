using System;

namespace Dotspec
{
    public interface IAssertionSpec<TSubject, TData>
        where TSubject : class
    {
        IAssertable<TSubject> Then(Action<TData> assertion);
    }
}