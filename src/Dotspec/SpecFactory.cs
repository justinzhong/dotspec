using System;

namespace Dotspec
{
    public class SpecFactory<TSubject> : ISpecFactory<TSubject>
        where TSubject : class
    {
        public IBehaviourSpec<TSubject> CreateBehaviourSpec(Action precondition)
        {
            if (precondition == null) throw new ArgumentNullException(nameof(precondition));

            return new BehaviourSpec<TSubject>(precondition, this);
        }

        public IBehaviourSpec<TSubject, TData> CreateBehaviourSpec<TData>(Func<TData> precondition)
        {
            if (precondition == null) throw new ArgumentNullException(nameof(precondition));

            return new BehaviourSpec<TSubject, TData>(precondition, this);
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

        public IAssertable<TSubject> CreateAssertable(Action precondition, Action<TSubject> behaviour, Action<TSubject> assertion)
        {
            if (precondition == null) throw new ArgumentNullException(nameof(precondition));
            if (behaviour == null) throw new ArgumentNullException(nameof(behaviour));
            if (assertion == null) throw new ArgumentNullException(nameof(assertion));

            return new Assertable<TSubject>(precondition, behaviour, assertion);
        }

        public IAssertable<TSubject, TData> CreateAssertable<TData>(Func<TData> precondition, Action<TSubject, TData> behaviour, Action<TSubject, TData> assertion)
        {
            if (precondition == null) throw new ArgumentNullException(nameof(precondition));
            if (behaviour == null) throw new ArgumentNullException(nameof(behaviour));
            if (assertion == null) throw new ArgumentNullException(nameof(assertion));

            return new Assertable<TSubject, TData>(precondition, behaviour, assertion);
        }
    }
}