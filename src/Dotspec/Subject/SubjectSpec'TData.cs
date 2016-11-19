using System;

namespace Dotspec
{
    public class SubjectSpec<TSubject, TData> : ISubjectSpec<TSubject, TData>
        where TSubject : class
    {
        private Func<TData> Precondition { get; }
        private Action<TSubject, TData> Behaviour { get; }
        private Action<TSubject, TData> Assertion { get; }

        public SubjectSpec(Func<TData> precondition, Action<TSubject, TData> behaviour, Action<TSubject, TData> assertion)
        {
            if (precondition == null) throw new ArgumentNullException(nameof(precondition));
            if (assertion == null) throw new ArgumentNullException(nameof(assertion));
            if (behaviour == null) throw new ArgumentNullException(nameof(behaviour));

            Precondition = precondition;
            Assertion = assertion;
            Behaviour = behaviour;
        }

        public void For(TSubject subject)
        {
            if (subject == null) throw new ArgumentNullException(nameof(subject));

            var data = Precondition();

            Behaviour(subject, data);
            Assertion(subject, data);
        }

        public void For(Func<TData, TSubject> subjectConstructor)
        {
            if (subjectConstructor == null) throw new ArgumentNullException(nameof(subjectConstructor));

            var data = Precondition();
            var subject = subjectConstructor(data);

            Behaviour(subject, data);
            Assertion(subject, data);
        }
    }
}