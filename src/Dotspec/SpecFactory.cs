using System;

namespace Dotspec
{
    public class SpecFactory<TSubject> : ISpecFactory<TSubject>
        where TSubject : class
    {
        public IActSpec<TSubject> CreateBehaviourSpec(Action precondition)
        {
            if (precondition == null) throw new ArgumentNullException(nameof(precondition));

            return new ActSpec<TSubject>(precondition, this);
        }

        public IActSpec<TSubject, TData> CreateBehaviourSpec<TData>(Func<TData> precondition)
        {
            if (precondition == null) throw new ArgumentNullException(nameof(precondition));

            return new ActSpec<TSubject, TData>(precondition, this);
        }

        public IAssertionSpec<TSubject> CreateAssertionSpec(Action precondition, Action<TSubject> behaviour)
        {
            if (precondition == null) throw new ArgumentNullException(nameof(precondition));
            if (behaviour == null) throw new ArgumentNullException(nameof(behaviour));

            return new AssertionSpec<TSubject>(precondition, behaviour, this);
        }

        public IAssertionSpec<TSubject, TData> CreateAssertionSpec<TData>(Func<TData> precondition, Action<TSubject, TData> behaviour)
        {
            if (precondition == null) throw new ArgumentNullException(nameof(precondition));
            if (behaviour == null) throw new ArgumentNullException(nameof(behaviour));

            return new AssertionSpec<TSubject, TData>(precondition, behaviour, this);
        }

        public IResultAssertionSpec<TSubject, TResult> CreateResultAssertionSpec<TResult>(Action precondition, Func<TSubject, TResult> behaviour)
        {
            if (precondition == null) throw new ArgumentNullException(nameof(precondition));
            if (behaviour == null) throw new ArgumentNullException(nameof(behaviour));

            return new ResultAssertionSpec<TSubject, TResult>(precondition, behaviour, this);
        }

        public ISubjectSpec<TSubject> CreateAssertable(Action precondition, Action<TSubject> behaviour, Action<TSubject> assertion)
        {
            if (precondition == null) throw new ArgumentNullException(nameof(precondition));
            if (behaviour == null) throw new ArgumentNullException(nameof(behaviour));
            if (assertion == null) throw new ArgumentNullException(nameof(assertion));

            return new SubjectSpec<TSubject>(precondition, behaviour, assertion);
        }

        public ISubjectSpec<TSubject> CreateAssertable<TResult>(Action precondition, Func<TSubject, TResult> behaviour, Action<TSubject, TResult> assertion)
        {
            if (precondition == null) throw new ArgumentNullException(nameof(precondition));
            if (behaviour == null) throw new ArgumentNullException(nameof(behaviour));
            if (assertion == null) throw new ArgumentNullException(nameof(assertion));

            return new SubjectResultSpec<TSubject, TResult>(precondition, behaviour, assertion);
        }

        public ISubjectSpec<TSubject, TData> CreateAssertable<TData>(Func<TData> precondition, Action<TSubject, TData> behaviour, Action<TSubject, TData> assertion)
        {
            if (precondition == null) throw new ArgumentNullException(nameof(precondition));
            if (behaviour == null) throw new ArgumentNullException(nameof(behaviour));
            if (assertion == null) throw new ArgumentNullException(nameof(assertion));

            return new SubjectSpec<TSubject, TData>(precondition, behaviour, assertion);
        }

        public ISubjectSpec<TSubject, TData> CreateSubjectSpec<TData>(Func<TData> precondition, Action<TSubject, TData> behaviour, Action<TData> assertion)
        {
            if (precondition == null) throw new ArgumentNullException(nameof(precondition));
            if (behaviour == null) throw new ArgumentNullException(nameof(behaviour));
            if (assertion == null) throw new ArgumentNullException(nameof(assertion));

            return new SubjectSpec<TSubject, TData>(precondition, behaviour, (_, data) => assertion(data));
        }
    }
}