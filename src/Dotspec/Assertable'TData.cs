using System;

namespace Dotspec
{
    public class Assertable<TSubject, TData> : IAssertable<TSubject, TData>
        where TSubject : class
    {
        private Action<TSubject, TData> AssertionSpec { get; }
        private Func<TData> Precondition { get; }

        public Assertable(Action<TSubject, TData> assertionSpec, Func<TData> precondition)
        {
            if (assertionSpec == null) throw new ArgumentNullException(nameof(assertionSpec));
            if (precondition == null) throw new ArgumentNullException(nameof(precondition));

            AssertionSpec = assertionSpec;
            Precondition = precondition;
        }

        public void Assert(Func<TData, TSubject> subjectConstructor)
        {
            if (subjectConstructor == null) throw new ArgumentNullException(nameof(subjectConstructor));

            var data = Precondition();
            var subject = subjectConstructor(data);

            AssertionSpec(subject, data);
        }
    }
}