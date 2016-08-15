using System;

namespace Dotspec
{
    public class Assertable<TSubject> : IAssertable<TSubject>
        where TSubject : class
    {
        private Action Precondition { get; }
        private Action<TSubject> BehaviourSpec { get; }
        private ISpecFactory<TSubject> SpecFactory { get; }
        private Action<TSubject> AssertionSpec { get; }

        public Assertable(Action precondition, Action<TSubject> behaviourSpec, Action<TSubject> assertionSpec)
        {
            if (precondition == null) throw new ArgumentNullException(nameof(precondition));
            if (behaviourSpec == null) throw new ArgumentNullException(nameof(behaviourSpec));
            if (assertionSpec == null) throw new ArgumentNullException(nameof(assertionSpec));

            Precondition = precondition;
            BehaviourSpec = behaviourSpec;
            AssertionSpec = assertionSpec;
        }

        public void Assert(TSubject subject)
        {
            if (subject == null) throw new ArgumentNullException(nameof(subject));

            Precondition();
            BehaviourSpec(subject);
            AssertionSpec(subject);
        }
    }
}