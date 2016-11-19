using System;

namespace Dotspec
{
    public interface ISpecFactory<TSubject>
        where TSubject : class
    {
        IActSpec<TSubject> CreateBehaviourSpec(Action precondition);

        IActSpec<TSubject, TData> CreateBehaviourSpec<TData>(Func<TData> precondition);

        IAssertionSpec<TSubject> CreateAssertionSpec(Action precondition, Action<TSubject> behaviour);

        IAssertionSpec<TSubject, TData> CreateAssertionSpec<TData>(Func<TData> precondition, Action<TSubject, TData> behaviour);

        IResultAssertionSpec<TSubject, TResult> CreateResultAssertionSpec<TResult>(Action precondition, Func<TSubject, TResult> behaviour);

        ISubjectSpec<TSubject> CreateAssertable(Action precondition, Action<TSubject> behaviour, Action<TSubject> assertion);

        ISubjectSpec<TSubject> CreateAssertable<TResult>(Action precondition, Func<TSubject, TResult> behaviour, Action<TSubject, TResult> assertion);

        ISubjectSpec<TSubject, TData> CreateAssertable<TData>(Func<TData> precondition, Action<TSubject, TData> behaviour, Action<TSubject, TData> assertion);

        ISubjectSpec<TSubject, TData> CreateSubjectSpec<TData>(Func<TData> precondition, Action<TSubject, TData> behaviour, Action<TData> assertion);
    }
}