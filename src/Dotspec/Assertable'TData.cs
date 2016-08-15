using System;

namespace Dotspec
{
    public class Assertable<TSubject, TData> : IAssertable<TSubject, TData>
        where TSubject : class
    {
        private Func<TData> Precondition { get; }
        private Action<TSubject, TData> Behaviour { get; }
        private Action<TSubject, TData> Assertion { get; }

        public Assertable(Func<TData> precondition, Action<TSubject, TData> behaviour, Action<TSubject, TData> assertion)
        {
            if (precondition == null) throw new ArgumentNullException(nameof(precondition));
            if (assertion == null) throw new ArgumentNullException(nameof(assertion));
            if (behaviour == null) throw new ArgumentNullException(nameof(behaviour));

            Precondition = precondition;
            Assertion = assertion;
            Behaviour = behaviour;
        }

        public void Assert(Func<TData, TSubject> subjectConstructor)
        {
            if (subjectConstructor == null) throw new ArgumentNullException(nameof(subjectConstructor));

            var data = Precondition();
            var subject = subjectConstructor(data);

            Behaviour(subject, data);
            Assertion(subject, data);
        }
    }
}