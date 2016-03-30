using System;

namespace Dotspec
{
    public class Spec<TSubject> : SpecBase<TSubject>, IAssertableSpec<TSubject>
        where TSubject : class
    {
        public Spec(string scenario, EventHandler<TSubject> assertionCallback) : base(scenario)
        {
            RegisterAssertionCallback(assertionCallback);
        }

        public Spec<TSubject> And(Action<TSubject> assertion)
        {
            RegisterAssertionCallback((_, subject) => assertion(subject));

            return this;
        }

        public void Assert(TSubject subject)
        {
            if (subject == null) throw new ArgumentNullException("subject");

            // Invokes all previously registered assertion callbacks.
            OnAssert(this, subject);
        }
    }
}
