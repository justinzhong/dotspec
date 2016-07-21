using System;

namespace Dotspec
{
    public class AssertionSpec<TSubject, TData> : IAssertionSpec<TSubject, TData>
        where TSubject : class
    {
        private Action<TSubject, TData> Behaviour { get; }
        private Func<TData> Precondition { get; }

        public AssertionSpec(Action<TSubject, TData> behaviour, Func<TData> precondition)
        {
            if (behaviour == null) throw new ArgumentNullException(nameof(behaviour));
            if (precondition == null) throw new ArgumentNullException(nameof(precondition));

            Behaviour = behaviour;
            Precondition = precondition;
        }

        public IAssertable<TSubject, TData> Then(Action<TData> assertion)
        {
            Action<TSubject, TData> assertionSpec = (subject, data) =>
            {
                Behaviour(subject, data);
                assertion(data);
            };

            return new Assertable<TSubject, TData>(assertionSpec, Precondition);
        }
    }
}