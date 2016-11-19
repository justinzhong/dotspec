using System;

namespace Dotspec
{
    public class SubjectResultSpec<TSubject, TResult> : ISubjectSpec<TSubject>
        where TSubject : class
    {
        private Action Precondition { get; }
        private Func<TSubject, TResult> BehaviourSpec { get; }
        private Action<TSubject, TResult> AssertionSpec { get; }

        public SubjectResultSpec(Action precondition, Func<TSubject, TResult> behaviourSpec, Action<TSubject, TResult> assertionSpec)
        {
            if (precondition == null) throw new ArgumentNullException(nameof(precondition));
            if (behaviourSpec == null) throw new ArgumentNullException(nameof(behaviourSpec));
            if (assertionSpec == null) throw new ArgumentNullException(nameof(assertionSpec));

            Precondition = precondition;
            BehaviourSpec = behaviourSpec;
            AssertionSpec = assertionSpec;
        }

        public void For(TSubject subject)
        {
            if (subject == null) throw new ArgumentNullException(nameof(subject));

            Precondition();
            var result = BehaviourSpec(subject);
            AssertionSpec(subject, result);
        }
    }

}