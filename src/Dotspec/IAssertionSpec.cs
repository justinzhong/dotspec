using System;

namespace Dotspec
{
    public interface IAssertionSpec<TSubject>
        where TSubject : class
    {
        IAssertable<TSubject> Then(Action assertion);
    }
}
