using System;

namespace Dotspec
{
    public interface IPreconditionSpec<TSubject>
        where TSubject : class
    {
        IBehaviourJunctionSpec<TSubject> Given(Action precondition);

        IBehaviourJunctionSpec<TSubject> Given(Action<TSubject> precondition);
    }
}
