using System;

namespace Dotspec
{
    public interface IResultAssertionSpec<TSubject, TResult> : IAssertionSpec<TSubject>
        where TSubject : class
    {
        IAssertable<TSubject> Then(Action<TSubject, TResult> assertion);
    }
}