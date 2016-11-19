using System;

namespace Dotspec
{
    public interface IActSpec<TSubject>
        where TSubject : class
    {
        IAssertionSpec<TSubject> When(Action<TSubject> behaviour);

        IResultAssertionSpec<TSubject, TResult> When<TResult>(Func<TSubject, TResult> behaviour);
    }
}
