using System;

namespace Dotspec
{
    public class ResultAssertionSpec<TSubject, TResult> : IResultAssertionSpec<TSubject, TResult>
        where TSubject : class
    {
        private Action Precondition { get; }
        private Func<TSubject, TResult> BehaviourSpec { get; }
        private ISpecFactory<TSubject> SpecFactory { get; }

        public ResultAssertionSpec(Action precondition, Func<TSubject, TResult> behaviourSpec, ISpecFactory<TSubject> specFactory)
        {
            if (precondition == null) throw new ArgumentNullException(nameof(precondition));
            if (behaviourSpec == null) throw new ArgumentNullException(nameof(behaviourSpec));
            if (specFactory == null) throw new ArgumentNullException(nameof(specFactory));

            Precondition = precondition;
            BehaviourSpec = behaviourSpec;
            SpecFactory = specFactory;
        }

        public IAssertable<TSubject> Then(Action<TSubject> assertion)
        {
            if (assertion == null) throw new ArgumentNullException(nameof(assertion));

            return SpecFactory.CreateAssertable(Precondition, BehaviourSpec, (subject, _) => assertion(subject));
        }

        public IAssertable<TSubject> Then(Action<TSubject, TResult> assertion)
        {
            if (assertion == null) throw new ArgumentNullException(nameof(assertion));

            return SpecFactory.CreateAssertable(Precondition, BehaviourSpec, assertion);
        }
    }
}