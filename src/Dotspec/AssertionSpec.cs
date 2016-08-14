using System;

namespace Dotspec
{
    public class AssertionSpec<TSubject> : IAssertionSpec<TSubject>
        where TSubject : class
    {
        private Action Precondition { get; }
        private Action<TSubject> BehaviourSpec { get; }

        public AssertionSpec(Action precondition, Action<TSubject> behaviourSpec)
        {
            if (precondition == null) throw new ArgumentNullException(nameof(precondition));
            if (behaviourSpec == null) throw new ArgumentNullException(nameof(behaviourSpec));

            Precondition = precondition;
            BehaviourSpec = behaviourSpec;
        }

        public IAssertable<TSubject> Then(Action assertion)
        {
            Action<TSubject> assertionSpec = subject =>
            {
                Precondition();
                BehaviourSpec(subject);
                assertion();
            };

            return new Assertable<TSubject>(assertionSpec);
        }
    }
}