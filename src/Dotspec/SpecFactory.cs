using System;

namespace Dotspec
{
    public class SpecFactory<TSubject> : ISpecFactory<TSubject>
        where TSubject : class
    {
        public IBehaviourSpec<TSubject> CreateBehaviourSpec(Action precondition)
        {
            if (precondition == null) throw new ArgumentNullException(nameof(precondition));

            return new BehaviourSpec<TSubject>(precondition);
        }

        public IBehaviourSpec<TSubject, TData> CreateBehaviourSpec<TData>(Func<TData> precondition)
        {
            if (precondition == null) throw new ArgumentNullException(nameof(precondition));

            return new BehaviourSpec<TSubject, TData>(precondition);
        }
    }
}