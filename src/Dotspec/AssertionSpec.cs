using System;

namespace Dotspec
{
    public class AssertionSpec<TSubject> : IAssertionSpec<TSubject>
        where TSubject : class
    {
        private Action<TSubject> BehaviourSpec { get; }

        public AssertionSpec(Action<TSubject> behaviourSpec)
        {
            if (behaviourSpec == null) throw new ArgumentNullException(nameof(behaviourSpec));

            BehaviourSpec = behaviourSpec;
        }

        public IAssertable<TSubject> Then(Action assertion)
        {
            Action<TSubject> assertionSpec = subject =>
            {
                BehaviourSpec(subject);
                assertion();
            };

            return new Assertable<TSubject>(assertionSpec);
        }
    }
}