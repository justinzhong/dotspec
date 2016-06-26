using System;

namespace Dotspec
{
    public class BehaviourSpec<TSubject> : IBehaviourSpec<TSubject>
        where TSubject : class
    {
        private Action Precondition { get; }

        public BehaviourSpec(Action precondition)
        {
            if (precondition == null) throw new ArgumentNullException(nameof(precondition));

            Precondition = precondition;
        }

        public IAssertionSpec<TSubject> When(Action<TSubject> behaviour)
        {
            Action<TSubject> behaviourSpec = subject =>
            {
                Precondition();
                behaviour(subject);
            };

            return new AssertionSpec<TSubject>(behaviourSpec);
        }
    }
}