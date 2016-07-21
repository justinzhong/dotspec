using System;

namespace Dotspec
{
    public class BehaviourSpec<TSubject, TData> : IBehaviourSpec<TSubject, TData>
        where TSubject : class
    {
        private Func<TData> Precondition { get; }
        private ISpecFactory<TSubject> SpecFactory { get; }

        public BehaviourSpec(Func<TData> precondition, ISpecFactory<TSubject> specFactory)
        {
            if (precondition == null) throw new ArgumentNullException(nameof(precondition));
            if (specFactory == null) throw new ArgumentNullException(nameof(specFactory));

            Precondition = precondition;
            SpecFactory = specFactory;
        }

        public IAssertionSpec<TSubject, TData> When(Action<TSubject, TData> behaviour)
        {
            if (behaviour == null) throw new ArgumentNullException(nameof(behaviour));

            return SpecFactory.CreateAssertionSpec(behaviour, Precondition);
        }
    }
}