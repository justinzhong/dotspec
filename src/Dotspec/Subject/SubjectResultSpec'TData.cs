using System;

namespace Dotspec
{
    public class SubjectResultSpec<TSubject, TData, TResult> : ISubjectConstructorSpec<TSubject, TData>
        where TSubject : class
    {
        private Func<TData> Precondition { get; }
        private Func<TSubject, TData, TResult> BehaviourSpec { get; }
        private Action<TSubject, TData, TResult> AssertionSpec { get; }

        public SubjectResultSpec(Func<TData> precondition, Func<TSubject, TData, TResult> behaviourSpec, Action<TSubject, TData, TResult> assertionSpec)
        {
            if (precondition == null) throw new ArgumentNullException(nameof(precondition));
            if (behaviourSpec == null) throw new ArgumentNullException(nameof(behaviourSpec));
            if (assertionSpec == null) throw new ArgumentNullException(nameof(assertionSpec));

            Precondition = precondition;
            BehaviourSpec = behaviourSpec;
            AssertionSpec = assertionSpec;
        }

        public void For(Func<TData, TSubject> subjectConstructor)
        {
            if (subjectConstructor == null) throw new ArgumentNullException(nameof(subjectConstructor));

            var data = Precondition();
            var subject = subjectConstructor(data);
            var result = BehaviourSpec(subject, data);
            AssertionSpec(subject, data, result);
        }
    }
}