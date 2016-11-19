using System;

namespace Dotspec
{
    public class AssertionSpec<TSubject> : IAssertionSpec<TSubject>
        where TSubject : class
    {
        private Action Precondition { get; }
        private Action<TSubject> BehaviourSpec { get; }
        private ISpecFactory<TSubject> SpecFactory { get; }

        public AssertionSpec(Action precondition, Action<TSubject> behaviourSpec, ISpecFactory<TSubject> specFactory)
        {
            if (precondition == null) throw new ArgumentNullException(nameof(precondition));
            if (behaviourSpec == null) throw new ArgumentNullException(nameof(behaviourSpec));
            if (specFactory == null) throw new ArgumentNullException(nameof(specFactory));

            Precondition = precondition;
            BehaviourSpec = behaviourSpec;
            SpecFactory = specFactory;
        }

        public ISubjectSpec<TSubject> Then(Action<TSubject> assertion)
        {
            if (assertion == null) throw new ArgumentNullException(nameof(assertion));

            return SpecFactory.CreateAssertable(Precondition, BehaviourSpec, assertion);
        }
    }
}