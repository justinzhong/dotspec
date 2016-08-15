using System;

namespace Dotspec
{
    public class AssertionSpec<TSubject, TData> : IAssertionSpec<TSubject, TData>
        where TSubject : class
    {
        private Func<TData> Precondition { get; }
        private Action<TSubject, TData> Behaviour { get; }
        private ISpecFactory<TSubject> SpecFactory { get; }

        public AssertionSpec(Func<TData> precondition, Action<TSubject, TData> behaviour, ISpecFactory<TSubject> specFactory)
        {
            if (precondition == null) throw new ArgumentNullException(nameof(precondition));
            if (behaviour == null) throw new ArgumentNullException(nameof(behaviour));
            if (specFactory == null) throw new ArgumentNullException(nameof(specFactory));

            Precondition = precondition;
            Behaviour = behaviour;
            SpecFactory = specFactory;
        }

        public IAssertable<TSubject, TData> Then(Action<TSubject, TData> assertion)
        {
            if (assertion == null) throw new ArgumentNullException(nameof(assertion));

            return SpecFactory.CreateAssertable(Precondition, Behaviour, assertion);
        }
    }
}