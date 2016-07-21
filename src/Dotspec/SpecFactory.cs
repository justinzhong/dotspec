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

        public IAssertionSpec<TSubject> CreateAssertionSpec(Action<TSubject> behaviour)
        {
            if (behaviour == null) throw new ArgumentNullException(nameof(behaviour));

            return new AssertionSpec<TSubject>(behaviour);
        }

        public IAssertionSpec<TSubject, TData> CreateAssertionSpec<TData>(Action<TSubject, TData> behaviour, Func<TData> precondition)
        {
            if (behaviour == null) throw new ArgumentNullException(nameof(behaviour));
            if (precondition == null) throw new ArgumentNullException(nameof(precondition));

            return new AssertionSpec<TSubject, TData>(behaviour, precondition);
        }
    }
}