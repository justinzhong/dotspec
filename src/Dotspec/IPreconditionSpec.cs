using System;

namespace Dotspec
{
    public interface IPreconditionSpec<TSubject>
        where TSubject : class
    {
        IAdditionalPreconditionSpec<TSubject> Given(Action precondition);

        IAdditionalPreconditionSpec<TSubject, TData> Given<TData>(Func<TData> precondition);
    }
}
