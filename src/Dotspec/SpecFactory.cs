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

            return new AssertionSpec<TSubject>(precondition, behaviour);
        }

        public IAssertionSpec<TSubject, TData> CreateAssertionSpec<TData>(Func<TData> precondition, Action<TSubject, TData> behaviour)
        {
            if (precondition == null) throw new ArgumentNullException(nameof(precondition));
            if (behaviour == null) throw new ArgumentNullException(nameof(behaviour));

            return new AssertionSpec<TSubject, TData>(behaviour, precondition);
        }
    }
}