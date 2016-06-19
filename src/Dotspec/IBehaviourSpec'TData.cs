using System;

namespace Dotspec
{
    public interface IBehaviourSpec<TSubject, TData> : IBehaviourSpec<TSubject>
        where TSubject : class
    {
        IAssertionSpec<TSubject, TData> When(Action<TSubject, TData> behaviour);
    }
}