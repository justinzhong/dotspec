using System;

namespace Dotspec
{
    public interface IAssertionSpec<TSubject>
        where TSubject : class
    {
        ISubjectSpec<TSubject> Then(Action<TSubject> assertion);
    }
}
