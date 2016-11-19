using System;

namespace Dotspec
{
    public interface IResultAssertionSpec<TSubject, TResult> : IAssertionSpec<TSubject>
        where TSubject : class
    {
        ISubjectSpec<TSubject> Then(Action<TSubject, TResult> assertion);
    }
}