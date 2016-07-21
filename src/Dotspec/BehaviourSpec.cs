using System;

namespace Dotspec
{
    public class BehaviourSpec<TSubject> : IBehaviourSpec<TSubject>
        where TSubject : class
    {
        private Action Precondition { get; }

        private ISpecFactory<TSubject> SpecFactory { get; }

        public BehaviourSpec(Action precondition, ISpecFactory<TSubject> specFactory)
        {
            if (precondition == null) throw new ArgumentNullException(nameof(precondition));
            if (specFactory == null) throw new ArgumentNullException(nameof(specFactory));

            Precondition = precondition;
            SpecFactory = specFactory;
        }

        public IAssertionSpec<TSubject> When(Action<TSubject> behaviour)
        {
            // Includes Precondition before passing along to create the
            // AssertionSpec instance to continue the specification.
            Action<TSubject> bundledBehaviour = subject =>
            {
                Precondition();
                behaviour(subject);
            };

            return SpecFactory.CreateAssertionSpec(bundledBehaviour);
        }
    }
}