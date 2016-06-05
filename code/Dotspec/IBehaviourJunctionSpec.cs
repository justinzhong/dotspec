using System;

namespace Dotspec
{
    public interface IBehaviourJunctionSpec<TSubject>
        where TSubject : class
    {
        IBehaviourJunctionSpec<TSubject> And(Action precondition);

        IBehaviourJunctionSpec<TSubject> And(Action<TSubject> precondition);

        IAssertionSpec<TSubject> When(Action<TSubject> behaviour);
    }
}
