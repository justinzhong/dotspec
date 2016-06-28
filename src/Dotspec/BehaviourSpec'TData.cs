using System;

namespace Dotspec
{
    public class BehaviourSpec<TSubject, TData> : IBehaviourSpec<TSubject, TData>
        where TSubject : class
    {
        private Func<TData> Precondition { get; }

        public BehaviourSpec(Func<TData> precondition)
        {
            if (precondition == null) throw new ArgumentNullException(nameof(precondition));

            Precondition = precondition;
        }

        public IAssertionSpec<TSubject, TData> When(Action<TSubject, TData> behaviour)
        {
            Action<TSubject, TData> behaviourSpec = (subject, data) =>
            {
                behaviour(subject, data);
            };

            return new AssertionSpec<TSubject, TData>(behaviourSpec, Precondition);
        }
    }
}