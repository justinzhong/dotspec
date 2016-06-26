using System;

namespace Dotspec
{
    public class SpecFactory<TSubject> : ISpecFactory<TSubject>
        where TSubject : class
    {
        private Func<Action, IBehaviourSpec<TSubject>> BehaviourSpecConstructor { get; }

        public SpecFactory(Func<Action, IBehaviourSpec<TSubject>> behaviourSpecConstructor)
        {
            if (behaviourSpecConstructor == null) throw new ArgumentNullException(nameof(behaviourSpecConstructor));

            BehaviourSpecConstructor = behaviourSpecConstructor;
        }

        public IBehaviourSpec<TSubject> CreateBehaviourSpec(Action precondition)
        {
            if (precondition == null) throw new ArgumentNullException(nameof(precondition));

            return BehaviourSpecConstructor(precondition);
        }
    }
}