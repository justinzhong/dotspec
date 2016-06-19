using System;

namespace Dotspec
{
    public interface IAdditionalPreconditionSpec<TSubject> : IBehaviourSpec<TSubject>
        where TSubject : class
    {
        IAdditionalPreconditionSpec<TSubject> And(Action precondition);
    }

    public interface IAdditionalPreconditionSpec<TSubject, TData> : IAdditionalPreconditionSpec<TSubject>, IBehaviourSpec<TSubject, TData>
        where TSubject : class
    {
        IAdditionalPreconditionSpec<TSubject, TData> And(Action<TData> precondition);
    }
}
