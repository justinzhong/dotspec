using System;

namespace Dotspec
{
    public interface IAssertEventObserver<TSubject>
        where TSubject : class
    {
        event EventHandler<TSubject> AssertEvent;

        event EventHandler<TSubject> UnsubscribeEvent;
    }
}
