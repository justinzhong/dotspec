using System;

namespace Dotspec
{
    public interface ISpecFactory<TSubject>
        where TSubject : class
    {
        IBehaviourSpec<TSubject> CreateBehaviourSpec(Action precondition);

        IBehaviourSpec<TSubject, TData> CreateBehaviourSpec<TData>(Func<TData> precondition);

        IAssertionSpec<TSubject> CreateAssertionSpec(Action precondition, Action<TSubject> behaviour);

        IAssertionSpec<TSubject, TData> CreateAssertionSpec<TData>(Func<TData> precondition, Action<TSubject, TData> behaviour);

        IAssertable<TSubject> CreateAssertable(Action precondition, Action<TSubject> behaviour, Action<TSubject> assertion);

        IAssertable<TSubject, TData> CreateAssertable<TData>(Func<TData> precondition, Action<TSubject, TData> behaviour, Action<TSubject, TData> assertion);
    }
}