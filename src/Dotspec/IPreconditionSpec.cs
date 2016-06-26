using System;

namespace Dotspec
{
    public interface IPreconditionSpec<TSubject>
        where TSubject : class
    {
        IBehaviourSpec<TSubject> Given(Action precondition);
    }
}
